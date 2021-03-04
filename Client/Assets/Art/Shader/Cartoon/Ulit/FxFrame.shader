Shader "Cartoon/Ulit/FxFrame"
{
	Properties
	{
		_Cutout ("Cutout", Range(0, 1)) = 0.65
		_MainTex("MainTex",2D) = "White"{}
		_BaseColor("BaseColor",Color) = (1,1,1,1)
        _Sheet("Sheet", Vector) = (1,1,1,1)
        _FrameRate("FrameRate", float) = 25
		
	}

	SubShader
	{
		Tags
		{
			"RenderPipeline" = "UniversalRenderPipeline"
			"RenderType" = "Transparent"
            "Queue" = "Transparent"
		}

		pass
		{
			Tags { "LightMode" = "UniversalForward" }

			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite OFF
			Cull Back

			HLSLPROGRAM
			#pragma vertex VERT
			#pragma fragment FRAG	

			#include "./FxFrame.hlsl"
			
			ENDHLSL
		}
	}
}
