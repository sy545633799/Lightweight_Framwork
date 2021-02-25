Shader "Cartoon/Terrain"
{
	Properties
	{
		[HideInInspector] _BlendMode("BlendMode", float) = 0
		[HideInInspector][Enum(Off, 0, On, 1)] _ZWrite("__zw", Float) = 1.0
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("__src", Float) = 1.0
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("__dst", Float) = 0.0
		[HideInInspector][Enum(UnityEngine.Rendering.CullMode)] _Cull("__cull", Float) = 2.0

		_Color("Color", Color) = (1, 1, 1, 1)
		_Albedo0("Albedo0", 2D) = "white" {}
		_Normal0("Normal0", 2D) = "bump" {}
		_Metallic0("__Metallic0",Range(0,1)) = 0.1
		[Space]
		_Albedo1("Albedo1", 2D) = "white" {}
		_Normal1("Normal1", 2D) = "bump" {}
		_Metallic1("__Metallic1",Range(0,1)) = 0.1
		[Space]
		_Albedo2("Albedo2", 2D) = "white" {}
		_Normal2("Normal2", 2D) = "bump" {}
		_Metallic2("__Metallic2",Range(0,1)) = 0.1
		_Hightthreshold("_Hightthreshold",Range(0,1)) = 0.3
		_Mask("_Mask",2D) = "white"{}

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
			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _EMISSION
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ALPHABLEND_ON

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#pragma shader_feature _USE_NORMAL
			#define _USE_NORMAL
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
			#pragma shader_feature _EMISSION
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ALPHABLEND_ON

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#pragma shader_feature _USE_NORMAL
			#define _USE_NORMAL
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
			#pragma shader_feature _PBRMAP
			#pragma shader_feature _EMISSION
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature 
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog

			#pragma shader_feature _USE_NORMAL
			#define _USE_NORMAL
			#include "./Core/Terrain.hlsl"
			ENDHLSL
		}
		UsePass "Common/Shadow/Default/ShadowCaster"
	}
}