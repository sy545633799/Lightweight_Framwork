Shader "UI/UIHUD"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_Color("_Color",Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags{ "Queue" = "Geometry" }
		LOD 100
		Pass
		{
			Name "HUD"
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
			}
			Cull Off
			Lighting Off
			ZWrite Off
			ZTest Off
			Fog { Mode Off }
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			HLSLPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			
			struct appdata_t
			{
				float3 positionOS : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
				float2 uv2 : TEXCOORD1;
			};

			struct v2f
			{
				float4 positionCS : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			CBUFFER_START(UnityPerMaterial)
			TEXTURE2D(_MainTex);
			float4 _MainTex_ST;
			SAMPLER(sampler_MainTex);
			float4 _Color;
			CBUFFER_END


			v2f vert(appdata_t i)
			{
				v2f o;

				float2 offset = i.uv2;
				float4 viewPos = mul(UNITY_MATRIX_MV,float4(i.positionOS.xyz, 1)) + float4(offset.x, offset.y,0.0,0.0);
				o.positionCS = mul(UNITY_MATRIX_P, viewPos);

				o.uv = TRANSFORM_TEX(i.uv, _MainTex);

				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
				//col *= _Color;

				return col;
			}
				

			ENDHLSL
		}
	}
}
