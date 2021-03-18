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

		//风
		[Header("风速")]
		public float WindSpeed = 1;
		[Header("风密度")]
		public float WindDensity = 0.3f;
		[Header("风强度")]
		public float WindStrenth = 1;

		[Header("(x, y)风向(zw)场景大小")]
		public Vector4 WindScale = new Vector4(-1, -1, 50, 50);

		void Start()
	    {
			int gradientNoiseID = Shader.PropertyToID("_GradientNoiseMap");
			Shader.SetGlobalTexture(gradientNoiseID, GradientNoise);
			int windSpeedID = Shader.PropertyToID("_WindSpeed");
			Shader.SetGlobalFloat(windSpeedID, WindSpeed);
			int windDensityID = Shader.PropertyToID("_WindDensity");
			Shader.SetGlobalFloat(windDensityID, WindDensity);
			int windStrenthID = Shader.PropertyToID("_WindStrenth");
			Shader.SetGlobalFloat(windStrenthID, WindStrenth);
			int windScaleID = Shader.PropertyToID("_WindScale");
			Shader.SetGlobalVector(windScaleID, WindScale);
		}


		private void Update()
		{
#if UNITY_EDITOR
			int windSpeedID = Shader.PropertyToID("_WindSpeed");
			Shader.SetGlobalFloat(windSpeedID, WindSpeed);
			int windDensityID = Shader.PropertyToID("_WindDensity");
			Shader.SetGlobalFloat(windDensityID, WindDensity);
			int windStrenthID = Shader.PropertyToID("_WindStrenth");
			Shader.SetGlobalFloat(windStrenthID, WindStrenth);
			int windScaleID = Shader.PropertyToID("_WindScale");
			Shader.SetGlobalVector(windScaleID, WindScale);
#endif

		}
		//void Update()
		//{

		//}
	}
}
