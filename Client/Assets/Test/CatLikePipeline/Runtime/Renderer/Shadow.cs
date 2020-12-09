using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace CatlikePipeline
{
	public class Shadow : RendererBase
	{
		private ShadowSetting shadowSetting;

		struct ShadowedDirectionalLight
		{
			public int visibleLightIndex;
			public float slopeScaleBias;
			public float nearPlaneOffset;
		}

		//PCF百分比渐近过滤
		private static string[] directionalFilterKeywords = {"_DIRECTIONAL_PCF3","_DIRECTIONAL_PCF5","_DIRECTIONAL_PCF7"};
		//cascadeBlend
		private static string[] cascadeBlendKeywords = {"_CASCADE_BLEND_SOFT", "_CASCADE_BLEND_DITHER"};
		//shadowMask
		private static string[] shadowMaskKeywords = {"_SHADOW_MASK_ALWAYS", "_SHADOW_MASK_DISTANCE"};
		//所有阴影灯光信息(强度, 索引)
		private int allDirectinalLightShadowDataId;
		private Vector4[] allDirectionalLightShadowData;
		
		//shadowmap世界转灯光空间矩阵
		private int shadowmapMatricesId;
		private Matrix4x4[] shadowmapMatrices;
		//shadowmap图集
		private int dirShadowAtlasId;
		private int shadowAtlastSizeId;
		//有效的阴影灯光和数量
		private int shadowmapLightCount = 0;
		private ShadowedDirectionalLight[] shadowmapLights;
		//about 
		private int shadowDistanceFadeId;
		//abount cascade
		private int cascadeCountId;
		private int cascadeCullingSpheresId;
		private Vector4[] cascadeCullingSpheres;
		private int castedNormalBiasId;
		private float[] castedNormalBias;
		//pcf
		private int shadowPCFDataId;

		public Shadow(RenderSetting renderSetting)
		{
			shadowSetting = renderSetting.shadowSetting;

			//所有灯光的阴影信息
			allDirectinalLightShadowDataId = Shader.PropertyToID("_DirectionalLightShadowData");
			allDirectionalLightShadowData = new Vector4[renderSetting.maxDirectionalLights];
			//世界转灯光空间矩阵
			shadowmapMatricesId = Shader.PropertyToID("_DirectionalShadowMatrices");
			shadowmapMatrices = new Matrix4x4[shadowSetting.directional.maxShadowedDirectionalLightCount * shadowSetting.directional.CascadeCount];
			//shadowmap图集(通过DrawShadows自动设置?)
			dirShadowAtlasId = Shader.PropertyToID("_DirectionalShadowAtlas");
			shadowAtlastSizeId = Shader.PropertyToID("_ShadowAtlasSize");
			//cascadeCountId
			cascadeCountId = Shader.PropertyToID("_CascadeCount");
			cascadeCullingSpheresId = Shader.PropertyToID("_CascadeCullingSpheres");
			cascadeCullingSpheres = new Vector4[shadowSetting.directional.CascadeCount];
			//1.边缘消隐
			shadowDistanceFadeId = Shader.PropertyToID("_ShadowDistanceFade");
			//2.normalbias
			castedNormalBiasId = Shader.PropertyToID("_CastedNormalBias");
			castedNormalBias = new float[shadowSetting.directional.CascadeCount];
			//3.PCF
			shadowPCFDataId = Shader.PropertyToID("_ShadowPCFData");
			//4.


			//有效的阴影灯光数量
			shadowmapLights = new ShadowedDirectionalLight[shadowSetting.directional.maxShadowedDirectionalLightCount];
		}

		public override void HandleDirectional(VisibleLight light, int index, CullingResults results)
		{
			if (index == 0) shadowmapLightCount = 0;
			//判断如果不超数，灯光投影设置开，投影强度不为0且投影在视锥体内
			if (shadowmapLightCount < shadowSetting.directional.maxShadowedDirectionalLightCount &&
				light.light.shadows != LightShadows.None && light.light.shadowStrength > 0f &&
				results.GetShadowCasterBounds(index, out Bounds b))
			{
				//给投影灯组添加该投影灯
				shadowmapLights[shadowmapLightCount] = new ShadowedDirectionalLight{
					visibleLightIndex = index,
					slopeScaleBias = light.light.shadowBias,
					nearPlaneOffset = light.light.shadowNearPlane
				};
				//返回灯光的阴影强度和shadowmaId
				allDirectionalLightShadowData[index] = 
					new Vector3(light.light.shadowStrength, shadowmapLightCount++, light.light.shadowNormalBias);
			}
			else
				allDirectionalLightShadowData[index] = Vector2.zero;
		}

		/// <summary>
		/// 处理每一栈平行光的shadowmap
		/// </summary>
		/// <param name="index"></param>
		/// <param name="split"></param>
		/// <param name="tileSize"></param>
		void RenderDirectionalShadows(int index, int split, int tileSize)
		{

			ShadowedDirectionalLight light = shadowmapLights[index];
			//前两个参数为剪裁结果和投影光在有效光中的索引
			ShadowDrawingSettings shadowSettings = new ShadowDrawingSettings(cullingResults, light.visibleLightIndex);

			int cascadeCount = shadowSetting.directional.CascadeCount;
			Vector3 ratios = shadowSetting.directional.CascadeRatios;
			int tileOffset = index * cascadeCount;

			for (int i = 0; i < cascadeCount; i++)
			{
				//其中第二三四参数为级联索引、级联数和级联比率，我们暂时不考虑，直接传入0，1，vector3.zero即可。然后是每个分块的阴影图分辨率，阴影近裁面偏移。之后输出所需的3个参数。
				cullingResults.ComputeDirectionalShadowMatricesAndCullingPrimitives(light.visibleLightIndex, i, cascadeCount, ratios, tileSize, light.nearPlaneOffset, out Matrix4x4 viewMatrix, out Matrix4x4 projectionMatrix, out ShadowSplitData splitData);
				//Unity通过为每个级联创建一个裁切球来确定覆盖的区域, 裁切球的作用还可以用来确定从哪个级联进行采样
				if (index == 0)
				{
					Vector4 cullingSphere = splitData.cullingSphere;
					//一个像素对应的距离大小
					float texelSize = 2f * cullingSphere.w / tileSize;
					float filterSize = texelSize * ((float)shadowSetting.directional.filter + 1f);
					cullingSphere.w -= filterSize;
					cullingSphere.w *= cullingSphere.w;
					cascadeCullingSpheres[i] = cullingSphere;
					castedNormalBias[i] = filterSize * 1.4142136f;
				}


				//获取世界转灯光空间矩阵,用于采样阴影图
				int tileIndex = tileOffset + i;
				Matrix4x4 vp = projectionMatrix * viewMatrix;
				shadowmapMatrices[tileIndex] = vp.ConvertToAtlasMatrix(tileIndex, split, tileSize, out Rect viewport);
				//设置当前联级区域的viewport?
				buffer.SetViewport(viewport);
				//更改渲染阴影的灯光相机VP矩阵
				buffer.SetViewProjectionMatrices(viewMatrix, projectionMatrix);
				buffer.SetGlobalDepthBias(0, light.slopeScaleBias);
				ExecuteBufferAndClear();
				//最后一个参数设置为true，为使用shadowlayermask进行过滤
				shadowSettings.useRenderingLayerMaskTest = true;
				//还有一个参数为级联阴影数据。
				shadowSettings.splitData = splitData;
				context.DrawShadows(ref shadowSettings);
				buffer.SetGlobalDepthBias(0f, 0f);
			}
			
		}

		protected override void OnRender()
		{
			if (shadowmapLightCount == 0) return;
			int atlasSize = (int)shadowSetting.directional.atlasSize;
			buffer.GetTemporaryRT(dirShadowAtlasId, atlasSize, atlasSize, 32, FilterMode.Bilinear, RenderTextureFormat.Shadowmap);
			//通过调用SetRenderTarget缓冲区，标识渲染纹理以及应如何加载和存储其数据来完成此操作。我们不在乎它的初始状态，因为我们会立即清除它，因此我们将使用RenderBufferLoadAction.DontCare。纹理的目的是包含阴影数据，因此我们需要将其RenderBufferStoreAction.Store用作第三个参数。
			buffer.SetRenderTarget(dirShadowAtlasId, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
			//我们只需要深度信息，因此其他信息可以不用清除
			buffer.ClearRenderTarget(true, false, Color.clear);

			int tiles = shadowmapLightCount * shadowSetting.directional.CascadeCount;
			int split = tiles <= 1 ? 1 : (tiles <= 4 ? 2 : 4);
			int tileSize = atlasSize / split;

			//接第一步，获取全部投影平行灯并申请了ShadowAtlas之后，在投影灯中进行循环
			for (int i = 0; i < shadowmapLightCount; i++)
				RenderDirectionalShadows(i, split, tileSize);

			buffer.ReleaseTemporaryRT(dirShadowAtlasId);

			buffer.SetGlobalVector(shadowAtlastSizeId, new Vector4(atlasSize, 1f / atlasSize));
			//所有灯光的阴影信息
			buffer.SetGlobalVectorArray(allDirectinalLightShadowDataId, allDirectionalLightShadowData);
			//cascadeCount
			buffer.SetGlobalInt(cascadeCountId, shadowSetting.directional.CascadeCount);
			buffer.SetGlobalVectorArray(cascadeCullingSpheresId, cascadeCullingSpheres);
			//shadowmap矩阵(tilecount)
			buffer.SetGlobalMatrixArray(shadowmapMatricesId, shadowmapMatrices);
			//1.edge fade
			float f = 1f - shadowSetting.directional.cascadeFade;
			//请使用一除以这些数值，这样就可以避免在着色器中进行除法，因为乘法速度更快。
			Vector4 distanceFace = new Vector4(1f / shadowSetting.maxDistance, 1f / shadowSetting.distanceFade, 1f / (1f - f * f));
			buffer.SetGlobalVector(shadowDistanceFadeId, distanceFace);
			//2.normalbias
			buffer.SetGlobalFloatArray(castedNormalBiasId, castedNormalBias);
			//3.PCF
			SetKeywords(directionalFilterKeywords, (int)shadowSetting.directional.filter - 1);
			buffer.SetGlobalVector(shadowPCFDataId, new Vector4(atlasSize, 1f / atlasSize));
			//4.cascadeblend
			SetKeywords(cascadeBlendKeywords, (int)shadowSetting.directional.cascadeBlend - 1);

			ExecuteBufferAndClear();
			shadowmapLightCount = 0;
		}

	}
}
