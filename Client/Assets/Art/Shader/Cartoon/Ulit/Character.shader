Shader "Cartoon/Ulit/Character"
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
		[MainColor] _BaseColor("Color(a通道为是否有积雪)", Color) = (1,1,1,1)
		[MainTexture] _BaseMap("MainTex", 2D) = "white" {}
		//mask
		_MaskMap("Mask r(遮罩阴影) g(未定）b (高光通道)", 2D) = "white" {}

		[Header(Outline)]
		_OutlineWidth("描边宽度",Range(0,1)) = 0.626 
		_OutlineColor("描边颜色",COLOR) = (0.264,0.0137,0.0137,1)
		_OutlineScaledMaxDistance("描边距离系数",Range(0.0,100.0)) = 15.0
		[Header(Shadow)]
		_BrightSideColor("明部颜色",COLOR) = (1,1,1,1)
		_DarkSideColor("暗部颜色",COLOR) = (0.83,0.73,0.77,1)
		_Rampthreshold("明暗部系数调整",Range(0,1)) = 0.5
		_ShadowRange("阴影边界范围", Range(0, 0.2)) = 0
		_FaceShadowRange("脸部与阴影边界范围", Range(0.001, 1)) = 0.1
		[Header(Specular)]
		_Shininess("高光范围", Range(0,1)) = 1
		_SpecTrail("高光拖尾", Range(0,1)) = 0
		_Specular("高光强度", float) = 1
		_SpecularColor("高光颜色", COLOR) = (1.0,1.0,1.0,1)
		[Header(Rim)]
		_RimThreshold("范围宽度", Range(0,1)) = 0.893
		_RimPower("边缘正面影响参数", Range(0,1)) = 0
		_RimMin("边缘光最小值", Range(0,1)) = 0.161
		_RimMax("边缘光最大值", Range(0,1)) = 1
		[HDR]_RimColor("边缘光颜色", COLOR) = (1.0,1.0,1.0,1.0)

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

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _MASKMAP
			#pragma shader_feature _EMISSION
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ALPHABLEND_ON

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
			#define _USE_RIM 
			#include "./CharacterInput.hlsl"
			#include "./CharacterPass.hlsl"

			ENDHLSL
		}
		UsePass "Cartoon/Outline/Character/Outline"
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

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _MASKMAP
			#pragma shader_feature _EMISSION
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ALPHABLEND_ON

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

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _EMISSION
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ALPHABLEND_ON

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
		UsePass "Cartoon/DepthOnly/Character/DepthOnly"
	}


	SubShader
	{
		Tags{ "Queue" = "Geometry" }

		LOD 90

		UsePass "Common/Shadow/Default/ShadowCaster"

	}

	CustomEditor "Game.Editor.CharacterGUI"
}