Shader "Cartoon/ShadowCaster/Scene"
{
	Properties
	{
		[MainTexture] _BaseMap("MainTex", 2D) = "white" {}
		_MetallicGlossMap("Metallic(R), AlphaTest(G), (B未定), Smooth(A)", 2D) = "gray" {}
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
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _METALLICSPECGLOSSMAP
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
		#include "./SceneInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
#if defined(_ALPHATEST_ON)
				half2 uv : TEXCOORD0;
#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 pos : SV_POSITION;
#if defined(_ALPHATEST_ON)
				half2 uv : TEXCOORD0;
#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			v2f vert(a2v v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
#if defined(_ALPHATEST_ON)
				o.uv = v.uv.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
#endif
				//float3 worldPos = TransformObjectToWorld(v.vertex.xyz);
				//float3 worldNormal = TransformObjectToWorldNormal(v.normal);
				//o.pos = TransformWorldToHClip(ApplyShadowBias(worldPos, worldNormal, _LightDirection));

				o.pos = TransformObjectToHClip(v.vertex.xyz);
			
			#if UNITY_REVERSED_Z
				o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
			#else
				o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
			#endif
			
				return o;
			}
			
			half4 frag(v2f i) :SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
#if defined(_METALLICSPECGLOSSMAP) && defined(_ALPHATEST_ON)
				half alpha = SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, i.uv).g;
				clip(alpha - _Cutoff);
				
#endif
				return 0;
			} 
			ENDHLSL
		}
	}
}
