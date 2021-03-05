
#ifndef DEPTHONLY_H
#define DEPTHONLY_H

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct a2v
{
	float4 positionOS   : POSITION;
	float2 texcoord     : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
	float2 texcoord     : TEXCOORD0;
	float4 positionCS   : SV_POSITION;
	UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO
};

v2f DepthOnlyVertex(a2v i)
{
	v2f o = (v2f)0;
	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

	o.texcoord.xy = TRANSFORM_TEX(i.texcoord, _BaseMap);
	o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
	return o;
}

half4 DepthOnlyFragment(v2f i) : SV_TARGET
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
	Alpha(SampleAlbedoAlpha(i.texcoord, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap)).a, _BaseColor, _Cutoff);
	return 0;
}

#endif