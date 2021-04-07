Shader "Cartoon/Ulit/Character"
{
	Properties
	{
		[HideInInspector] _Mode("__blend", float) = 0
		[HideInInspector][Enum(Off, 0, On, 1)] _ZWrite("__zw", Float) = 1.0
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("__src", Float) = 1.0
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("__dst", Float) = 0.0
		[HideInInspector][Enum(UnityEngine.Rendering.CullMode)] _Cull("__cull", Float) = 2.0

		[Gamma]_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
		//diffuse
		[HDR][MainColor] _BaseColor("Color(a通道为是否有积雪)", Color) = (1,1,1,1)
		[MainTexture] _BaseMap("MainTex", 2D) = "white" {}
		//bump
		[NoScaleOffset][Normal]_BumpMap("Normalmap", 2D) = "bump" {}
		_BumpScale("BumpScale", Range(0, 1)) = 1
		//mask
		_MaskMap("Mask r(遮罩阴影) g(未定）b (高光通道)", 2D) = "white" {}

		[Header(Outline)]
		_OutlineWidth("_OutlineWidth",Range(0,1)) = 0.626 
		_OutlineColor("_OutlineColor",COLOR) = (0.264,0.0137,0.0137,1)
		_OutlineScaledMaxDistance("_OutlineScaledMaxDistance",Range(0.0,100.0)) = 15.0
		[Header(Shadow)]
		_BrightSideColor("_BrightSideColor",COLOR) = (1,1,1,1)
		_DarkSideColor("_DarkSideColor",COLOR) = (0.83,0.73,0.77,1)
		_Rampthreshold("_Rampthreshold",Range(0,1)) = 0.5
		_ShadowRange("_ShadowRange", Range(0, 0.2)) = 0
		_FaceShadowRange("_FaceShadowRange", Range(0.001, 1)) = 0.1
		[Header(Specular)]
		_Shininess("_Shininess", Range(0,1)) = 1
		_SpecTrail("_SpecTrail", Range(0,1)) = 0
		_Specular("_Specular", Float) = 1
		_SpecularColor("_SpecularColor", COLOR) = (1.0,1.0,1.0,1)
		[Header(Rim)]
		_RimThreshold("_RimThreshold", Range(0,1)) = 0.893
		_RimPower("_RimPower", Range(0,1)) = 0
		[HDR]_RimColor("_RimColor", COLOR) = (1.0,1.0,1.0,1.0)

		//emission
		[HDR]_EmissionColor("Color", Color) = (0,0,0)
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
			#pragma vertex CharacterPassVertex
			#pragma fragment CharacterPassFragment
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma shader_feature _MASKMAP
			#pragma shader_feature _EMISSION
			#pragma shader_feature _ALPHATEST_ON

			//目前URP只有逐顶点条件下的多光源
			 #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#define _USE_SPECULAR 
			#define _USE_RIM 
			#include "./CharacterInput.hlsl"
			#include "./CharacterPass.hlsl"

			ENDHLSL
		}
		UsePass "Cartoon/Outline/Character/Outline"
		UsePass "Cartoon/Shadow/Character/ShadowCaster"
		UsePass "Cartoon/DepthOnly/Character/DepthOnly" 
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
			#pragma vertex CharacterPassVertex
			#pragma fragment CharacterPassFragment
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma shader_feature _MASKMAP
			#pragma shader_feature _EMISSION
			#pragma shader_feature _ALPHATEST_ON

			//目前URP只有逐顶点条件下的多光源
			// #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#define _USE_SPECULAR 
			#include "./CharacterInput.hlsl"
			#include "./CharacterPass.hlsl"

			ENDHLSL
		}
		UsePass "Cartoon/Outline/Character/Outline"
		UsePass "Cartoon/Shadow/Character/ShadowCaster"
		UsePass "Cartoon/DepthOnly/Character/DepthOnly"
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
			#pragma vertex CharacterPassVertex
			#pragma fragment CharacterPassFragment
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma shader_feature _EMISSION
			#pragma shader_feature _ALPHATEST_ON

			//目前URP只有逐顶点条件下的多光源
			// #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#include "./CharacterInput.hlsl"
			#include "./CharacterPass.hlsl"

			ENDHLSL
		}
		UsePass "Cartoon/Outline/Character/Outline"
		UsePass "Cartoon/Shadow/Character/ShadowCaster"
		UsePass "Cartoon/DepthOnly/Character/DepthOnly"
	}

	CustomEditor "Game.Editor.CharacterGUI"
}