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
		//public Texture2D[] lightmapColor;
		//public Texture2D[] lightmapDir;
		//public Texture2D[] shadowMask;
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
	}
}
