using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace CatlikePipeline
{
	[Serializable]
	public class ShadowSetting
	{
		public enum TextureSize
		{
			_256 = 256, _512 = 512, _1024 = 1024,
			_2048 = 2048, _4096 = 4096, _8192 = 8192
		}

		//Pencentage close Filtering（PCF）
		public enum FilterMode
		{
			PCF2x2, PCF3x3, PCF5x5, PCF7x7
		}

		public enum CascadeBlendMode
		{
			Hard, Soft, Dither
		}

		[Min(0.001f)]
		public float maxDistance = 100f;

		[Range(0.001f, 1f)]
		public float distanceFade = 0.1f;

		[System.Serializable]
		public class Directional
		{
			public TextureSize atlasSize;

			[HideInInspector]
			public int maxShadowedDirectionalLightCount = 4;

			[Range(1, 4)]
			public int CascadeCount = 1;

			[Range(0f, 1f)]
			public float cascadeRatio1, cascadeRatio2, cascadeRatio3;
			[HideInInspector]
			public Vector3 CascadeRatios => new Vector3(cascadeRatio1, cascadeRatio2, cascadeRatio3);

			[Range(0.001f, 1f)]
			public float cascadeFade;

			public FilterMode filter = FilterMode.PCF2x2;

			public CascadeBlendMode cascadeBlend = CascadeBlendMode.Hard;
		}

		public Directional directional = new Directional { atlasSize = TextureSize._1024 };
	}

	[Serializable]
	public class RenderSetting
	{
		public bool useDynamicBatching = true;
		public bool useGPUInstancing = true;
		public bool useSRPBatcher = true;

		[HideInInspector]
		public int maxPointLights = 4;
		[HideInInspector]
		public int maxDirectionalLights = 4;
		[HideInInspector]
		public int maxSpotLights = 4;

		public ShadowSetting shadowSetting = new ShadowSetting();

	}
}