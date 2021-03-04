Shader "Common/DepthOnly/Default"
{
	Properties
	{
		_Color("Color Tint", Color) = (0.5,0.5,0.5)
		_MainTex("MainTex",2D) = "white"{}
	}

	SubShader
	{
		Tags{ "Queue" = "Geometry" }
		LOD 100
		Pass
		{
			Name "DepthOnly"
			Tags{"LightMode" = "DepthOnly"}
		
			ZWrite On
			ColorMask 0
			Cull[_Cull]
		
			HLSLPROGRAM
				// Required to compile gles 2.0 with standard srp library
				#pragma prefer_hlslcc gles
				#pragma exclude_renderers d3d11_9x
				#pragma target 2.0
		
				#pragma vertex DepthOnlyVertex
				#pragma fragment DepthOnlyFragment
		
				// -------------------------------------
				// Material Keywords
				#pragma shader_feature _ALPHATEST_ON
				#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		
				//--------------------------------------
				// GPU Instancing
				#pragma multi_compile_instancing
		
				#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
				ENDHLSL
		}
	}
}
