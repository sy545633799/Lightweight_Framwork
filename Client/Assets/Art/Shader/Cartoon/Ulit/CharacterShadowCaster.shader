﻿Shader "Cartoon/Shadow/Character"
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
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "./CharacterInput.hlsl"
			#include "../Core/ShadowCaster.hlsl"
			ENDHLSL
		}
	}
}
