// ========================================================
// des：
// author: 
// time：2020-12-10 12:32:39
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class SceneEnvAsset : ScriptableObject
	{
		/// <summary>
		/// 灯光烘焙信息
		/// </summary>
		//public LightProbes lightProbes;
		/// <summary>
		/// 灯光信息
		/// </summary>
		public LightData lightData;
		/// <summary>
		/// 光照贴图相关
		/// </summary>
		public Texture2D[] lightmapColor;
		public Texture2D[] lightmapDir;
		public Texture2D[] shadowMask;
		public CameraInfo CameraInfo;
		/// <summary>
		/// 环境光相关
		/// </summary>
		public UnityEngine.Rendering.AmbientMode ambientMode;
		public float ambientIntensity;
		public Color ambientLight;
		public Color ambientSkyColor;
		public Color ambientGroundColor;
		public Color ambientEquatorColor;
		/// <summary>
		/// 环境反射相关
		/// </summary>
		public Material skyBox;
		public UnityEngine.Rendering.DefaultReflectionMode reflectionMode;
		public Cubemap customReflection;
		public float reflectionIntensity;
		public int reflectionBounces;
		/// <summary>
		/// 雾效相关
		/// </summary>
		public bool fog;
		public Color fogColor;
		public FogMode fogMode;
		public float fogStartDistance;
		public float fogEndDistance;
		public float haloStrength;
		public float flareFadeSpeed;
		public float flareStrength;

		public void SetEnv()
		{
			////灯光烘焙信息
			//LightmapSettings.lightProbes = sceneInfo.lightProbes;
			////设置光照贴图
			//int l1 = (sceneInfo.lightmapDir == null) ? 0 : sceneInfo.lightmapDir.Length;
			//int l2 = (sceneInfo.lightmapColor == null) ? 0 : sceneInfo.lightmapColor.Length;
			//int l3 = (sceneInfo.shadowMask == null) ? 0 : sceneInfo.shadowMask.Length;
			//int l = (l1 < l2) ? l2 : l1;
			//l = (l < l3) ? l3 : l;
			//LightmapData[] lightmaps = null;
			//if (l > 0)
			//{
			//	lightmaps = new LightmapData[l];
			//	for (int i = 0; i < l; i++)
			//	{
			//		lightmaps[i] = new LightmapData();
			//		if (i < l1)
			//			lightmaps[i].lightmapDir = sceneInfo.lightmapDir[i];
			//		if (i < l2)
			//			lightmaps[i].lightmapColor = sceneInfo.lightmapColor[i];
			//		if (i < l3)
			//			lightmaps[i].shadowMask = sceneInfo.shadowMask[i];
			//	}
			//	LightmapSettings.lightmaps = lightmaps;
			//}

			//设置环境光
			RenderSettings.ambientMode = ambientMode;
			RenderSettings.ambientIntensity = ambientIntensity;
			RenderSettings.ambientLight = ambientLight;
			RenderSettings.ambientSkyColor = ambientSkyColor;
			RenderSettings.ambientGroundColor = ambientGroundColor;
			RenderSettings.ambientEquatorColor = ambientEquatorColor;

			//设置环境反射
			RenderSettings.skybox = skyBox;
			RenderSettings.defaultReflectionMode = reflectionMode;
			RenderSettings.customReflection = customReflection;
			RenderSettings.reflectionIntensity = reflectionIntensity;
			RenderSettings.reflectionBounces = reflectionBounces;

			//雾效
			RenderSettings.fog = fog;
			RenderSettings.fogColor = fogColor;
			RenderSettings.fogMode = fogMode;
			RenderSettings.fogStartDistance = fogStartDistance;
			RenderSettings.fogEndDistance = fogEndDistance;
			RenderSettings.haloStrength = haloStrength;
			RenderSettings.flareFadeSpeed = flareFadeSpeed;
			RenderSettings.flareStrength = flareStrength;
		}

	}
}
