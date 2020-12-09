using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace CatlikePipeline {

	public static class LightEx
	{
		//方法ReserveDirectionalShadows。主要用于获取投影的平行光及给材质传递阴影计算所需要的参数(灯光阴影强度和ID(主要用于采样shadowmapAtlas))
		//public static Vector2 ReserveDirectionalShadows(this Light light, int visibleLightIndex, CullingResults cullingResults)
		//{
		//	if (
		//			//判断如果不超数，灯光投影设置开，投影强度不为0且投影在视锥体内
		//			shadowedDirLightCount < maxShadowedDirectionalLightCount &&
		//			light.shadows != LightShadows.None && light.shadowStrength > 0f &&
		//			cullingResults.GetShadowCasterBounds(visibleLightIndex, out Bounds b)
		//		)
		//	{
		//		//给投影灯组添加该投影灯
		//		ShadowedDirectionalLights[shadowedDirLightCount] =
		//		new ShadowedDirectionalLight
		//		{
		//			visibleLightIndex = visibleLightIndex
		//		};
		//		return new Vector2(
		//				   //返回灯光的阴影强度和投影灯ID
		//				   light.shadowStrength, shadowedDirLightCount++
		//			   );
		//	}
		//	return Vector2.zero;
		//}
	}
}
