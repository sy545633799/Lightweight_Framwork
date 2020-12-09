Shader "Catlike/Common/Shadow/Default"
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
			Name "ShadowCaster"
			Tags{"LightMode" = "ShadowCaster"} 
			ColorMask 0
			HLSLPROGRAM
			#pragma target 3.5
			#pragma shader_feature _CLIPPING
			#pragma vertex ShadowCasterPassVertex
			#pragma fragment ShadowCasterPassFragment
			#pragma multi_compile_instancing
			#include "ShadowCasterPass.hlsl"
			ENDHLSL
		}
	}
}
