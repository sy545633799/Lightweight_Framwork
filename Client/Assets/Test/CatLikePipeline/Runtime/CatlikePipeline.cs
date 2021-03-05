using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


namespace CatlikePipeline
{
	public class CatlikePipeline : RenderPipeline
	{
		private RenderSetting renderSetting;
		private Lighting lighting;
		private Shadow shadow;
		CommandBuffer buffer = new CommandBuffer{ name = "Render Camera" };

		public CatlikePipeline(RenderSetting renderSetting)
		{
			this.renderSetting = renderSetting;
			lighting = new Lighting(renderSetting);
			shadow = new Shadow(renderSetting);
			GraphicsSettings.useScriptableRenderPipelineBatching = this.renderSetting.useSRPBatcher;
			GraphicsSettings.lightsUseLinearIntensity = true;
		}

		/// <summary>
		/// 确保在场景中绘制, 必须剔除前使用
		/// </summary>
		private void PrepareForSceneWindow(Camera camera)
		{
#if UNITY_EDITOR
			if (camera.cameraType == CameraType.SceneView)
				ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
#endif
		}

		private void HandleLights(CullingResults cullingResults)
		{
			var lights = cullingResults.visibleLights;
			int dLightIndex = 0;
			int pLightIndex = 0;
			int sLightIndex = 0;
			foreach (var light in lights)
			{
				//判断灯光类型
				if (light.lightType == LightType.Directional)
				{
					if (dLightIndex < renderSetting.maxDirectionalLights)
					{
						lighting.HandleDirectional(light, dLightIndex, cullingResults);
						shadow.HandleDirectional(light, dLightIndex, cullingResults);
						dLightIndex++;
					}
				}

				if (light.lightType == LightType.Point)
				{
					if (pLightIndex < renderSetting.maxPointLights)
					{
						lighting.HandlePoint(light, pLightIndex, cullingResults);
						shadow.HandlePoint(light, pLightIndex, cullingResults);
						pLightIndex++;
					}
				}

				if (light.lightType == LightType.Spot)
				{
					if (sLightIndex < renderSetting.maxSpotLights)
					{
						lighting.HandleSport(light, sLightIndex, cullingResults);
						shadow.HandleSport(light, sLightIndex, cullingResults);
						sLightIndex++;
					}
				}
			}
		}

		/// <summary>
		/// 绘制相机Gizmos
		/// </summary>
		/// <param name="context"></param>
		/// <param name="camera"></param>
		private void DrawGizmos(ScriptableRenderContext context, Camera camera)
		{
#if UNITY_EDITOR
			if (UnityEditor.Handles.ShouldRenderGizmos())
			{
				context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
				context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
			}
#endif
		}

		/// <summary>
		/// 渲染场景物体
		/// </summary>
		private void DrawVisibleGeometry(ScriptableRenderContext context, Camera camera, CullingResults cullingResults)
		{
			//先渲染不透明物体
			//渲染时，会牵扯到渲染排序，所以先要进行一个相机的排序设置，这里Unity内置了一些默认的排序可以调用
			SortingSettings sortSet = new SortingSettings(camera) { criteria = SortingCriteria.CommonOpaque };
			//这边进行渲染的相关设置，需要指定渲染的shader的光照模式(就是这里，如果shader中没有标注LightMode的
			//话，使用该shader的物体就没法进行渲染了)和上面的排序设置两个参数
			DrawingSettings drawSet = new DrawingSettings(new ShaderTagId("BaseLit"), sortSet) {
				enableDynamicBatching = renderSetting.useDynamicBatching,
				enableInstancing = renderSetting.useGPUInstancing,
				perObjectData =
				PerObjectData.ReflectionProbes |
				PerObjectData.Lightmaps | PerObjectData.ShadowMask |
				PerObjectData.LightProbe | PerObjectData.OcclusionProbe |
				PerObjectData.LightProbeProxyVolume |
				PerObjectData.OcclusionProbeProxyVolume
			};
			//这边是指定渲染的种类(对应shader中的Rendertype)和相关Layer的设置(-1表示全部layer)
			FilteringSettings filtSet = new FilteringSettings(RenderQueueRange.opaque, -1);
			context.DrawRenderers(cullingResults, ref drawSet, ref filtSet);
			//再渲染天空盒
			context.DrawSkybox(camera);
			//最后渲染透明物体
		}


		/// <summary>
		/// 
		/// </summary>
		private void DrawUnsupportedShaders()
		{
			
		}

		protected override void Render(ScriptableRenderContext context, Camera[] cameras)
		{
			foreach (Camera camera in cameras)
			{
				//场景中绘制
				PrepareForSceneWindow(camera);
				//剔除
				if (!camera.TryGetCullingParameters(out var cullingParameters)) continue;
				cullingParameters.shadowDistance = Mathf.Min(renderSetting.shadowSetting.maxDistance, camera.farClipPlane);
				CullingResults cullResults = context.Cull(ref cullingParameters);
				
				//光照以及阴影设置
				HandleLights(cullResults);
				lighting.Render(context, camera, renderSetting, cullResults);
				shadow.Render(context, camera, renderSetting, cullResults);
				//设置当前相机为渲染目标(必须在光照设置之后?), 然后绘制场景物体

				context.SetupCameraProperties(camera);
				CameraClearFlags flags = camera.clearFlags;
				buffer.ClearRenderTarget(
					flags <= CameraClearFlags.Depth,
					flags == CameraClearFlags.Color,
					flags == CameraClearFlags.Color ?
						camera.backgroundColor.linear : Color.clear
				);
				context.ExecuteCommandBuffer(buffer);
				buffer.Clear();

				context.SetupCameraProperties(camera);
				DrawVisibleGeometry(context, camera, cullResults);
				DrawUnsupportedShaders(); 
				//绘制Gizmos
				DrawGizmos(context, camera);

				context.ExecuteCommandBuffer(buffer);
				buffer.Clear();
				//提交
				context.Submit();
			}
		}
	}
}