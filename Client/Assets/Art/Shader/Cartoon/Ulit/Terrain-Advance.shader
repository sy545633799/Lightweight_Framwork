Shader "Cartoon/Ulit/Terrain-Advance"
{
	Properties
	{
		[HideInInspector] _BlendMode("BlendMode", float) = 0
		[HideInInspector][Enum(Off, 0, On, 1)] _ZWrite("__zw", Float) = 1.0
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("__src", Float) = 1.0
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("__dst", Float) = 0.0
		[HideInInspector][Enum(UnityEngine.Rendering.CullMode)] _Cull("__cull", Float) = 2.0

		// used in fallback on old cards & base map
		[HideInInspector] _MainTex("BaseMap (RGB)", 2D) = "grey" {}
		_BaseColor("Main Color", Color) = (1,1,1,1)
		_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		[PowerSlider(5.0)] _Shininess("Shininess", Range(1, 100)) = 10

		[NoScaleOffset]_Diffuse("Diffuse Array", 2DArray) = "white" {}
		[NoScaleOffset]_Normal("Normal Array", 2DArray) = "bump" {}
		_Index("Index Map (RGBA)", 2D) = "white" {}
		_Control("Control Map (RGBA)", 2D) = "white" {}
		_MaxSubTexCount("Max Sub Texture Count", Int) = 8
		//_UVScale("Global UV Scales", VectorArray) = (45, 45, 0, 0)

		////emission
		//_EmissionColor("EmissionColor", Color) = (0,0,0)
		//_EmissionMap("Emission", 2D) = "white" {}

		//给depthOnlyPass用
		[HideInInspector] _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0
		[HideInInspector] _BaseMap("MainTex", 2D) = "white" {}
		[HideInInspector] _BaseColor("Color", Color) = (1,1,1,1)
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

		   #pragma shader_feature _NORMALMAP
		   #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
		   //目前URP只有逐顶点条件下的多光源
		   // #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
		   #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
		   #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
		   #pragma multi_compile _ _SHADOWS_SOFT
		   #pragma multi_compile _ LIGHTMAP_ON
		   #pragma multi_compile_fog

		   #define _NORMALMAP 1
		   #define _USE_PBR 1
		   #include "./Terrain-AdvanceInput.hlsl"
		   #include "./Terrain-AdvancePass.hlsl"
		   ENDHLSL 
	   }
		UsePass "Cartoon/Shadow/Terrain/ShadowCaster"
		UsePass "Cartoon/DepthOnly/Terrain/DepthOnly"
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

			#define _NORMALMAP 1
			#define _SPECULAR_COLOR 1
			#include "./Terrain-AdvanceInput.hlsl"
			#include "./Terrain-AdvancePass.hlsl"
			ENDHLSL
	   }
	   UsePass "Cartoon/Shadow/Terrain/ShadowCaster"
	   UsePass "Cartoon/DepthOnly/Terrain/DepthOnly"
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
			#include "./Terrain-AdvanceInput.hlsl"
		   #include "./Terrain-AdvancePass.hlsl"

		   ENDHLSL
	   }

	   UsePass "Cartoon/Shadow/Terrain/ShadowCaster"
	   UsePass "Cartoon/DepthOnly/Terrain/DepthOnly"
   }
}