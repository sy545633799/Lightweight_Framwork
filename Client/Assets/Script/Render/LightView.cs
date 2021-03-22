// ========================================================
// des：
// author: 
// time：2021-03-17 12:09:20
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	[ExecuteInEditMode]
	public class LightView : MonoBehaviour
	{
		public Texture2D GradientNoise;
		public Texture2D PerlinNoise;
		public Texture2D Distortion;

		//风
		[Header("风速")]
		public float WindSpeed = 1;
		[Header("风密度")]
		public float WindDensity = 0.3f;
		[Header("风强度")]
		public float WindStrenth = 1;

		[Header("(x, y)风向(zw)场景大小")]
		public Vector4 WindScale = new Vector4(-1, -1, 50, 50);

		[Range(0, 1)]
		[Header("人物影响范围")]
		public float PlayerRadius = 0.5f;
		[Range(0, 1)]
		[Header("人物影响强度")]
		public float PlayerStrength = 0.5f;

		int gradientNoiseID;
		int perlinNoiseID;
		int distortionID;

		int windSpeedID;
		int windDensityID;
		int windStrenthID;
		int windScaleID;
		int playerStrength;
		int playerRadius;

		void Start()
	    {
			gradientNoiseID = Shader.PropertyToID("_GradientNoiseMap");
			Shader.SetGlobalTexture("_GradientNoiseMap", GradientNoise);
			perlinNoiseID = Shader.PropertyToID("_PerlinNoiseMap");
			Shader.SetGlobalTexture("_PerlinNoiseMap", PerlinNoise);
			distortionID = Shader.PropertyToID("_DistortionMap");
			Shader.SetGlobalTexture("_DistortionMap", Distortion);
			windSpeedID = Shader.PropertyToID("_WindSpeed");
			windDensityID = Shader.PropertyToID("_WindDensity");
			windStrenthID = Shader.PropertyToID("_WindStrenth");
			windScaleID = Shader.PropertyToID("_WindScale");
			playerStrength = Shader.PropertyToID("_PlayerStrength");
			playerRadius = Shader.PropertyToID("_PlayerRadius");
			SetGlobal();
		}

#if UNITY_EDITOR
		private void Update()
		{
			SetGlobal();
		}
#endif

		private void SetGlobal()
		{
			Shader.SetGlobalFloat(windSpeedID, WindSpeed);
			Shader.SetGlobalFloat(windDensityID, WindDensity);
			Shader.SetGlobalFloat(windStrenthID, WindStrenth);
			Shader.SetGlobalVector(windScaleID, WindScale);
			Shader.SetGlobalFloat(playerRadius, PlayerRadius);
			Shader.SetGlobalFloat(playerStrength, PlayerStrength);
		}
	}
}
