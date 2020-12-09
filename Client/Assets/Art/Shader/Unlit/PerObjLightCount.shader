Shader "Unlit/PerObjLightCount"
{
	Properties
	{}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100
		Pass
		{
			Name "ForwardLit"
			Tags{"LightMode" = "UniversalForward"}
			HLSLPROGRAM
			#pragma vertex LitPassVertex
			#pragma fragment MyLitPassFragment
			#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/LitForwardPass.hlsl"

			half4 MyLitPassFragment(Varyings input) : SV_Target
			{
				half4 col = half4(0.25,0.25,0.25,1);
				half addLightCount = GetAdditionalLightsCount();
				col.r += step(1.0, addLightCount / 8.0) * 0.25 +
					step(1.0, addLightCount / 7.0) * 0.25 +
					step(1.0, addLightCount / 6.0) * 0.25;
				col.b += step(1.0, addLightCount / 5.0) * 0.375 +
					step(1.0, addLightCount / 4.0) * 0.375;
				col.b *= step(addLightCount, 5.0);
				col.g += step(1.0, addLightCount / 3.0) * 0.25 +
					step(1.0, addLightCount / 2.0) * 0.25 +
					step(1.0, addLightCount) * 0.25;
				col.g *= step(addLightCount, 3.0);
				return col;
			}
			ENDHLSL
		}
	}
}