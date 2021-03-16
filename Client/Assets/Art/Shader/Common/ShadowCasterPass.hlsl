#ifndef CUSTOM_SHADOW_CASTER_PASS_INCLUDED
#define CUSTOM_SHADOW_CASTER_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

CBUFFER_START(UnityPerMaterial)
float4 _BaseMap_ST;
half _Cutoff;
float3 _LightDirection;
float4 _WaveParams;
CBUFFER_END

struct a2v
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
#ifdef _ALPHATEST_ON
	half2 uv : TEXCOORD0;
#endif
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
	float4 pos : SV_POSITION;
#ifdef _ALPHATEST_ON
	half2 uv : TEXCOORD0;
#endif
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

v2f vert(a2v  v)
{
	v2f o;
	UNITY_SETUP_INSTANCE_ID(v);
	UNITY_TRANSFER_INSTANCE_ID(v, o);
#ifdef _ALPHATEST_ON
	o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
#endif
	float3 worldPos = TransformObjectToWorld(v.vertex.xyz);

#ifdef VERTEXWAVE_ON
	worldPos.xz -= WaveGrass(worldPos, _WaveParams);
#endif

	float3 worldNormal = TransformObjectToWorldNormal(v.normal);
	o.pos = TransformWorldToHClip(ApplyShadowBias(worldPos, worldNormal, _LightDirection));

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
#ifdef _ALPHATEST_ON
	 	half alpha = SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap, i.uv).a;
	 	clip(alpha - _Cutoff);
#endif
	return 0;
}


#endif