#ifndef TOON_SHADOW_CASTER_PASS_INCLUDED
#define TOON_SHADOW_CASTER_PASS_INCLUDED

#include "./Common.hlsl"

struct v2f2
{
	float4 positionCS : SV_POSITION;
#ifdef _ALPHATEST_ON
	half2 texcoord : TEXCOORD0;
#endif
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

v2f2 vert(a2v i)
{
	v2f2 o;
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_TRANSFER_INSTANCE_ID(i, o);


#ifdef _ALPHATEST_ON
	o.texcoord = TRANSFORM_TEX(i.texcoord.xy, _BaseMap);
#endif
	float3 worldPos = TransformObjectToWorld(i.positionOS.xyz);

	
	Light MainLight = GetMainLight();
	float3 worldNormal = TransformObjectToWorldNormal(i.normalOS);
	o.positionCS = TransformWorldToHClip(ApplyShadowBias(worldPos, worldNormal, MainLight.direction));
#if UNITY_REVERSED_Z
	o.positionCS.z = min(o.positionCS.z, o.positionCS.w * UNITY_NEAR_CLIP_VALUE);
#else
	o.positionCS.z = max(o.positionCS.z, o.positionCS.w * UNITY_NEAR_CLIP_VALUE);
#endif

	return o;
}

half4 frag(v2f2 i) :SV_Target
{
	UNITY_SETUP_INSTANCE_ID(i);
#ifdef _ALPHATEST_ON
		half alpha = SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap, i.texcoord).a;
		clip(alpha - _Cutoff);
#endif
	return 0;
}

#endif