// ========================================================
// des：
// author: 
// time：2020-12-10 15:59:54
// version：1.0
// ========================================================

using UnityEngine;
using UnityEngine.Rendering;

namespace Game {
	[System.Serializable]
	public struct LightData
	{
		public Vector3 position;
		public Vector3 rotation;

		public Color color;
		public float intensity;
		public float bounceIntensity;
		public float range;

		public LightShadows shadows;
		public float shadowStrength;
		public LightShadowResolution shadowResolution;
		public float shadowNormalBias;
		public float shadowNearPlane;

		public LightRenderMode renderMode;
		public int cullingMask;

		public void SetLight(Light light)
		{
			light.transform.position = position;
			light.transform.eulerAngles = rotation;

			light.type = LightType.Directional;
			light.color = color;
			light.intensity = intensity;
			light.bounceIntensity = bounceIntensity;
			light.range = range;

			light.shadows = shadows;
			light.shadowStrength = shadowStrength;
			light.shadowResolution = shadowResolution;
			light.shadowNormalBias = shadowNormalBias;
			light.shadowNearPlane = shadowNearPlane;

			light.renderMode = LightRenderMode.ForcePixel;
			light.cullingMask = cullingMask;
		}
	}
}
