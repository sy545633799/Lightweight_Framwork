Shader "Cartoon/Terrain"
{
	Properties
	{
		[HideInInspector] _BlendMode("BlendMode", float) = 0
		[HideInInspector][Enum(Off, 0, On, 1)] _ZWrite("__zw", Float) = 1.0
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("__src", Float) = 1.0
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("__dst", Float) = 0.0
		[HideInInspector][Enum(UnityEngine.Rendering.CullMode)] _Cull("__cull", Float) = 2.0

		_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		[PowerSlider(5.0)] _Shininess("Shininess", Range(1, 30)) = 10
		// set by terrain engine
	    _Control("Control (RGBA)", 2D) = "red" {}
	    _Splat0("Layer 0 (R)", 2D) = "grey" {}
	    _Normal0("Normal 0 (R)", 2D) = "bump" {}
		[Gamma] _Metallic0("Metallic 0", Range(0.0, 1.0)) = 0.0
		_Smoothness0("Smoothness 0", Range(0.0, 1.0)) = 0.5

		_Splat1("Layer 1 (G)", 2D) = "grey" {}
		_Normal1("Normal 1 (G)", 2D) = "bump" {}
		[Gamma] _Metallic1("Metallic 1", Range(0.0, 1.0)) = 0.0
		_Smoothness1("Smoothness 1", Range(0.0, 1.0)) = 0.5

		_Splat2("Layer 2 (B)", 2D) = "grey" {}
		_Normal2("Normal 2 (B)", 2D) = "bump" {}
		[Gamma] _Metallic2("Metallic 2", Range(0.0, 1.0)) = 0.0
		_Smoothness2("Smoothness 2", Range(0.0, 1.0)) = 0.5

		_Splat3("Layer 3 (A)", 2D) = "grey" {}
		_Normal3("Normal 3 (A)", 2D) = "bump" {}
	   [Gamma] _Metallic3("Metallic 3", Range(0.0, 1.0)) = 0.0
	    _Smoothness3("Smoothness 3", Range(0.0, 1.0)) = 0.5

		// used in fallback on old cards & base map
		[HideInInspector] _MainTex("BaseMap (RGB)", 2D) = "grey" {}
		[HideInInspector] _BaseColor("Main Color", Color) = (1,1,1,1)

		_Hightthreshold("_Hightthreshold",Range(0,1)) = 0.3
	
	}

		SubShader
	{
		Tags
		{
			"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"
		}

		LOD 100

		pass
		{
			Name "ForwardLit"
			Tags{"LightMode" = "UniversalForward"}

			Blend[_SrcBlend][_DstBlend]
			ZWrite[_ZWrite]
			Cull[_Cull]

			HLSLPROGRAM
			#pragma vertex TerrainPassVertex
			#pragma fragment TerrainPassFragment
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma shader_feature _ADDITIONAL_LIGHTS_VERTEX	
			//目前URP只有逐顶点条件下的多光源
			// #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#define _SPECULAR_COLOR 1
			#include "./Core/Terrain.hlsl"

			ENDHLSL
		}

		UsePass "Common/Shadow/Default/ShadowCaster"
	}

	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"
		}

		LOD 200

		pass
		{
			Name "ForwardLit"
			Tags{"LightMode" = "UniversalForward"}

			Blend[_SrcBlend][_DstBlend]
			ZWrite[_ZWrite]
			Cull[_Cull]

			HLSLPROGRAM
			#pragma vertex TerrainPassVertex
			#pragma fragment TerrainPassFragment
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma shader_feature _ADDITIONAL_LIGHTS_VERTEX	
			//目前URP只有逐顶点条件下的多光源
			// #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma shader_feature _NORMALMAP

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#define _SPECULAR_COLOR 1
			#include "./Core/Terrain.hlsl"

			ENDHLSL
		}

		UsePass "Common/Shadow/Default/ShadowCaster"
	}

	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"
		}
		LOD 300
		pass
		{
			Name "ForwardLit"
			Tags{"LightMode" = "UniversalForward"}
			Blend[_SrcBlend][_DstBlend]
			ZWrite[_ZWrite]
			Cull[_Cull]
			HLSLPROGRAM
			#pragma vertex TerrainPassVertex
			#pragma fragment TerrainPassFragment
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers 
			#pragma shader_feature _ADDITIONAL_LIGHTS_VERTEX	
			//目前URP只有逐顶点条件下的多光源
			// #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma shader_feature _NORMALMAP

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog
			
			#define _USE_PBR 1
			#include "./Core/Terrain.hlsl"
			ENDHLSL
		}
		UsePass "Common/Shadow/Default/ShadowCaster"
	}
}