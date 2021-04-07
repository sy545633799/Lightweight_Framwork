Shader "Cartoon/Env/Water"
{
	Properties
	{
		//[Header(Color)]
		[HDR][MainColor]_BaseColor("Deep Color", Color) = (0, 0.44, 0.62, 1)
		[HDR]_ShallowColor("Shallow Color", Color) = (0.1, 0.9, 0.89, 0.02)

		//bump
		[NoScaleOffset][Normal]_BumpMap("Normalmap", 2D) = "bump" {}
		_BumpScale("BumpScale", Range(0, 1)) = 1
		_BumpTiling("BumpTiling", Float) = 1
		_AnimationParams("XY=Direction, Z=Speed", Vector) = (1,1,1,0)
		//_SlopeParams("River Slope (X=Stretch) (Y=Speed)", Vector) = (0.5, 4, 0, 0)
		_EdgeFade("Edge Fade", Range(0, 99)) = 0.1
		_Depth("Color Depth", Range(0.01 , 8)) = 1
		_DepthExp("Color Blend", Range(0 , 1)) = 1
		

		[Header(Foam)]
		[NoScaleOffset]_IntersectionNoise("Intersection noise", 2D) = "white" {}
		_IntersectionColor("Color", Color) = (1,1,1,1)
		
		_IntersectionLength("Distance", Range(0.01 , 5)) = 2
		_IntersectionClipping("Cutoff", Range(0.01, 1)) = 0.5
		_IntersectionFalloff("Falloff", Range(0.01 , 1)) = 0.5
		_IntersectionTiling("Noise Tiling", float) = 0.2
		_IntersectionSpeed("Speed multiplier", float) = 0.1
		_IntersectionRippleDist("Ripple distance", float) = 32
		_IntersectionRippleStrength("Ripple Strength", Range(0 , 1)) = 0.5
		

		[Header(Sun Reflection)]
		_SunReflectionSize("Size", Range(0 , 1)) = 0.5
		_SunReflectionStrength("Strength", Float) = 10
		_SunReflectionDistortion("Distortion", Range(0 , 1)) = 0.49

		[Header(_Caustics)]
		[NoScaleOffset]_CausticsTex("Caustics Mask", 2D) = "black" {}
		_CausticsBrightness("Brightness", Float) = 2
		_CausticsTiling("Tiling", Float) = 0.5
		_CausticsDistortion("Distortion", Range(0 , 1)) = 0.1
		_RefractionStrength("_RefractionStrength", Range(0 , 1)) = 0.1
		// //[Header(Waves)]
		// [Toggle(_WAVES)] _WavesOn("_WAVES", Float) = 0
		// _WaveSpeed("Speed", Float) = 2
		// _WaveTint("Wave tint", Range(-0.1 , 0.1)) = 0
		// _WaveHeight("Height", Range(0 , 10)) = 0.25
		// _WaveNormalStr("Normal Strength", Range(0 , 6)) = 0.5
		// _WaveDistance("Distance", Range(0 , 1)) = 0.8
		// _WaveSteepness("Steepness", Range(0 , 1)) = 0.1
		// _WaveCount("Count", Range(1 , 5)) = 1
		// _WaveDirection("Direction", vector) = (1,1,1,1)

	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Transparent"}
		LOD 300

		Pass
		{
			Tags{"LightMode" = "UniversalForward" }

			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON

			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#define _SHARP_INERSECTION
			#define _SPECULAR_COLOR
			#define _REFRACTION
			#define _USE_CAUSTICS
			#include "./WaterInput.hlsl"
			#include "./WaterPass.hlsl"
			ENDHLSL
		}
		UsePass "Cartoon/Common/DepthOnly/DepthOnly"
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Transparent"}
		LOD 200

		Pass
		{
			Tags{"LightMode" = "UniversalForward" }

			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON

			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#define _SHARP_INERSECTION
			#define _SPECULAR_COLOR
			#include "./WaterInput.hlsl"
			#include "./WaterPass.hlsl"
			ENDHLSL
		}
		UsePass "Cartoon/Common/DepthOnly/DepthOnly"
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Transparent"}
		LOD 100

		Pass
		{
			Tags{"LightMode" = "UniversalForward" }

			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON

			#pragma multi_compile_fog
			#pragma multi_compile_instancing
			#include "./WaterInput.hlsl"
			#include "./WaterPass.hlsl"
			ENDHLSL
		}
		UsePass "Cartoon/Common/DepthOnly/DepthOnly"
	}
}
