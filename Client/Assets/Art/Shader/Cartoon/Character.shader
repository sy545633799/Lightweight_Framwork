Shader "Cartoon/Character"
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
		_ShaderGradeMap("ShaderGradeMap", 2D) = "gray" {}
		_Rampthreshold("Rampthreshold", Range(0, 1)) = 0.5
		//bump
		[NoScaleOffset][Normal]_BumpMap("Normalmap", 2D) = "bump" {}
		_BumpScale("BumpScale", Range(0, 1)) = 1
		//spec
		_SpecRange("SpecRange", Range(10, 300)) = 100
		_SpecColor("SpecColor", Color) = (1,1,1,1)
		//rim
		_RimColor("RimColor", Color) = (1,1,1,1)
		_RimThreshold("RimThreshold", Range(0, 5)) = 1
		_RimStrength("RimStrength", Range(0, 1)) = 1
		//mask
		_MaskMap("Mask r(遮罩阴影) g（高光通道)", 2D) = "white" {}

		//outline
		_OutlineColor("_OutlineColor: 描边颜色",COLOR) = (0,0.0,0.0,1)
		_OutlineWidth("_OutlineWidth: 描边宽度",Range(0,1)) = 0.2
		_Outlinethreshold("_Outlinethreshold: 描边距离系数",Range(0,1)) = 0.5
		_OffsetFactor("_OffsetFactor",float) = -0.1
		_OffsetUnits("_OffsetUnits",float) = 0.1
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

			#pragma shader_feature _ADDITIONAL_LIGHTS_VERTEX	
			//目前URP只有逐顶点条件下的多光源
			// #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ALPHABLEND_ON

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#include "./Library/Character.hlsl"

			ENDHLSL
		}
		UsePass "Cartoon/Common/Outline/Outline"
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
			#pragma vertex CharacterPassVertex
			#pragma fragment CharacterPassFragment
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma shader_feature _ADDITIONAL_LIGHTS_VERTEX	
			//目前URP只有逐顶点条件下的多光源
			// #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ALPHABLEND_ON

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#pragma shader_feature _USE_RIM
			#define _USE_RIM 
			#include "./Library/Character.hlsl"

			ENDHLSL
		}
		UsePass "Cartoon/Common/Outline/Outline"
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
			#pragma vertex CharacterPassVertex
			#pragma fragment CharacterPassFragment
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma shader_feature _ADDITIONAL_LIGHTS_VERTEX	
			//目前URP只有逐顶点条件下的多光源
			// #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ALPHABLEND_ON

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#pragma shader_feature _USE_MASK
			#define _USE_MASK 
			#include "./Library/Character.hlsl"

			ENDHLSL
		}
		UsePass "Cartoon/Common/Outline/Outline"
		UsePass "Common/Shadow/Default/ShadowCaster"

	}
}