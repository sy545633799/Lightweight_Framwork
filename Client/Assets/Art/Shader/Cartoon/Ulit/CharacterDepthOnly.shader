Shader "Cartoon/DepthOnly/Character"
{
	Properties
	{
		_Color("Color Tint", Color) = (0.5,0.5,0.5)
		_MainTex("MainTex",2D) = "white"{}
	}

		SubShader
	{
		Tags{ "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
		LOD 100
		Pass
		{
			Name "DepthOnly"
			Tags{"LightMode" = "DepthOnly"}

			ZWrite On
			ColorMask 0
			Cull[_Cull]

			HLSLPROGRAM

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			#include "./CharacterInput.hlsl"
			#include "../Core/DepthOnly.hlsl"
			#pragma vertex DepthOnlyVertex
			#pragma fragment DepthOnlyFragment
			#pragma multi_compile_instancing

			#pragma shader_feature _ALPHATEST_ON

			ENDHLSL
		}
	}
}
