Shader "Cartoon/Ulit/Scene"
{
	Properties
	{
		[HideInInspector] _Mode("__blend", float) = 0
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
		_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		[PowerSlider(5.0)] _Shininess("Shininess", Range(1, 30)) = 10
		//pbr
		_MetallicGlossMap("Metallic(R), (G未定), (B未定), Smooth(A)", 2D) = "gray" {}
		_Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
		_Metallic("Metallic", Range(0.0, 1.0)) = 0.0
		_OcclusionStrength("OcclusionStrength", Range(0.0, 1.0)) = 1.0
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
			#pragma shader_feature _METALLICSPECGLOSSMAP
			#pragma shader_feature _EMISSION
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ALPHABLEND_ON

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#define _USE_PBR
			#define _ADDITIONAL_LIGHTS_VERTEX

			#include "./Scene.hlsl"

			ENDHLSL
		}



		UsePass "Common/DepthOnly/Default/DepthOnly"
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

			#define _SPECULAR_COLOR
			#include "./Scene.hlsl"

			ENDHLSL
		}

		UsePass "Common/DepthOnly/Default/DepthOnly"
		UsePass "Common/Shadow/Default/ShadowCaster"
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
			//#pragma shader_feature _NORMALMAP
			//#pragma shader_feature _EMISSION
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ALPHABLEND_ON

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#define _SPECULAR_COLOR
			#include "./Scene.hlsl"

			ENDHLSL
		}

		UsePass "Common/DepthOnly/Default/DepthOnly"
		UsePass "Common/Shadow/Default/ShadowCaster"
	}
	CustomEditor "Game.Editor.SceneGUI"
}