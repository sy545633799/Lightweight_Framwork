using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace CatlikePipeline
{
	public class Lighting : RendererBase
	{
		private int maxPointLights;
		private int maxDirectionalLights;
		private int maxSpotLights;
		//平行光
		private int dLightCount= 0;
		private int dLightCountId;
		private Vector4[] DLightColors;
		private Vector4[] DLightDirections;
		//针对shader中的平行光参数，映射ID
		private int dLightDirId;
		private int dLightColorId;
		//点光源
		private int pLightCount = 0;
		private int pLightCountId;
		private Vector4[] PLightColors;
		private Vector4[] PLightPos;
		private int pLightPosId;
		private int pLightColorId;
		//聚光灯
		private int sLightCount = 0;
		private int sLightCountId;
		private Vector4[] SLightColors;
		private Vector4[] SLightDirections;
		private Vector4[] SLightPos;
		private int sLightPosId;
		private int sLightColorId;
		private int sLightDirId;

		public Lighting(RenderSetting renderSetting)
		{
			maxPointLights = renderSetting.maxPointLights;
			maxDirectionalLights = renderSetting.maxDirectionalLights;
			maxSpotLights = renderSetting.maxSpotLights;
			//将shader中需要的属性参数映射为ID，加速传参
			dLightCountId = Shader.PropertyToID("_DirectionalLightCount");
			DLightColors = new Vector4[maxDirectionalLights];
			DLightDirections = new Vector4[maxDirectionalLights];
			dLightDirId = Shader.PropertyToID("_DirectionalLightDirections");
			dLightColorId = Shader.PropertyToID("_DirectionalLightColors");
			//点光源
			pLightCountId = Shader.PropertyToID("_PointLightCount");
			PLightColors = new Vector4[maxPointLights];
			PLightPos = new Vector4[maxPointLights];
			pLightPosId = Shader.PropertyToID("_PointLightPosition");
			pLightColorId = Shader.PropertyToID("_PointLightColors");
			//聚光灯
			sLightCountId = Shader.PropertyToID("_SpotLightCount");
			SLightColors = new Vector4[maxSpotLights];
			SLightDirections = new Vector4[maxSpotLights];
			SLightPos = new Vector4[maxSpotLights];
			sLightPosId = Shader.PropertyToID("_SpotLightPosition");
			sLightColorId = Shader.PropertyToID("_SpotLightColors");
			sLightDirId = Shader.PropertyToID("_SpotLightDirections");
		}

		public override void HandleDirectional(VisibleLight light, int index, CullingResults cullingResults)
		{
			//获取灯光参数,平行光朝向即为灯光Z轴方向。矩阵第一到三列分别为xyz轴项，第四列为位置。
			Vector4 direction = light.localToWorldMatrix.GetColumn(2);
			//这边获取的灯光的finalColor是灯光颜色乘上强度之后的值，也正好是shader需要的值
			DLightColors[index] = light.finalColor;
			//灯光方向反向。默认管线中，unity提供的平行光方向也是灯光反向。光照计算决定
			DLightDirections[index] = -direction;
			//方向的第四个值(W值)为0，点为1.
			DLightDirections[index].w = 0;
			dLightCount = index + 1;
		}

		public override void HandlePoint(VisibleLight light, int index, CullingResults cullingResults)
		{
			PLightColors[index] = light.finalColor;
			//将点光源的距离设置塞到颜色的A通道
			PLightColors[index].w = light.range;
			//矩阵第4列为位置
			PLightPos[index] = light.localToWorldMatrix.GetColumn(3);
			pLightCount = index + 1;
		}

		public override void HandleSport(VisibleLight light, int index, CullingResults cullingResults)
		{
			SLightColors[index] = light.finalColor;
			//将聚光灯的距离设置塞到颜色的A通道
			SLightColors[index].w = light.range;
			//矩阵第三列为朝向，第四列为位置
			Vector4 lightpos = light.localToWorldMatrix.GetColumn(2);
			SLightDirections[index] = -lightpos;

			//外角弧度-unity中设置的角度为外角全角，我们之取半角进行计算
			float outerRad = Mathf.Deg2Rad * 0.5f * light.spotAngle;
			//外角弧度cos值和tan值
			float outerCos = Mathf.Cos(outerRad);
			float outerTan = Mathf.Tan(outerRad);
			//内角弧度计算-设定内角tan值为外角tan值的46/64
			float innerRad = Mathf.Atan(((46f / 64f) * outerTan));
			//内角弧度cos值
			float innerCos = Mathf.Cos(innerRad);
			SLightPos[index] = light.localToWorldMatrix.GetColumn(3);
			//角度计算用的cos(ro)与cos(ri) - cos(ro)分别存入方向与位置的w分量
			SLightDirections[index].w = outerCos;
			SLightPos[index].w = innerCos - outerCos;
			sLightCount = index + 1;
		}

		protected override void OnRender()
		{
			//平行光
			buffer.SetGlobalVectorArray(dLightDirId, DLightDirections);
			buffer.SetGlobalVectorArray(dLightColorId, DLightColors);
			//点光源
			buffer.SetGlobalVectorArray(pLightPosId, PLightPos);
			buffer.SetGlobalVectorArray(pLightColorId, PLightColors);
			//聚光灯
			buffer.SetGlobalVectorArray(sLightColorId, SLightColors);
			buffer.SetGlobalVectorArray(sLightPosId, SLightPos);
			buffer.SetGlobalVectorArray(sLightDirId, SLightDirections);
			//相机
			buffer.SetGlobalInt(dLightCountId, dLightCount);
			dLightCount = 0;
			buffer.SetGlobalInt(pLightCountId, pLightCount);
			pLightCount = 0;
			buffer.SetGlobalInt(sLightCountId, sLightCount);
			sLightCount = 0;

			ExecuteBufferAndClear();
		}
	}
}
