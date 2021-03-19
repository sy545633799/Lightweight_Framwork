Shader "Cartoon/Common/Shadow"
{
	Properties
	{
		[Gamma]_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
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
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _ALPHATEST_ON
			#pragma multi_compile_instancing

			float4 _BaseMap_ST;
			half _Cutoff;
			
			#include "../Core/ShadowCaster.hlsl"
			ENDHLSL
		}
	}
}
