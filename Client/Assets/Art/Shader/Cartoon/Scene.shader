Shader "Cartoon/Scene"
{
	Properties
	{
		[HideInInspector] _BlendMode("BlendMode", float) = 0
		[HideInInspector][Enum(Off, 0, On, 1)] _ZWrite("__zw", Float) = 1.0
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("__src", Float) = 1.0
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("__dst", Float) = 0.0
		[HideInInspector][Enum(UnityEngine.Rendering.CullMode)] _Cull("__cull", Float) = 2.0

		_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
		//diffuse
		[MainColor] _BaseColor("Color", Color) = (1,1,1,1)
		[MainTexture] _BaseMap("MainTex", 2D) = "white" {}
		//bump
		[NoScaleOffset][Normal]_BumpMap("Normalmap", 2D) = "bump" {}
		_BumpScale("BumpScale", Range(0, 1)) = 1
		//spec
		_SpecRange("SpecRange", Range(10, 300)) = 100
		_SpecColor("SpecColor", Color) = (1,1,1,1)
		//pbr
		_PBRTex("Smooth(R), Metallic(G), AO(B)", 2D) = "gray" {}
		_Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
		_Metallic("Metallic", Range(0.0, 1.0)) = 0.0
		_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		//emission
		_EmissionColor("Color", Color) = (0,0,0)
		_EmissionMap("Emission", 2D) = "white" {}

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
			#pragma vertex ScenePassVertex
			#pragma fragment ScenePassFragment
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

			#pragma shader_feature _USE_SPECULAR
			#define _USE_SPECULAR

			#include "./Library/Scene.hlsl"
			
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
			#pragma vertex ScenePassVertex
			#pragma fragment ScenePassFragment
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

			#pragma shader_feature _USE_SPECULAR
			#define _USE_SPECULAR
			#include "./Library/Scene.hlsl"

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
			#pragma vertex ScenePassVertex
			#pragma fragment ScenePassFragment
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma shader_feature _ADDITIONAL_LIGHTS_VERTEX	
			//目前URP只有逐顶点条件下的多光源
			// #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _PBRMAP
			#pragma shader_feature _EMISSION
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ALPHABLEND_ON

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#pragma shader_feature _USE_PBR
			#define _USE_PBR 
			#include "./Library/Scene.hlsl"

			ENDHLSL
		}

		UsePass "Common/Shadow/Default/ShadowCaster"
	}
}