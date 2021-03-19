
#ifndef DEPTHONLY_H
#define DEPTHONLY_H

#include "./Common.hlsl"

v2f DepthOnlyVertex(a2v i)
{
	v2f o;
	UNITY_SETUP_INSTANCE_ID(i); 
#if defined(_ALPHATEST_ON)
	o.texcoord.xy = TRANSFORM_TEX(i.texcoord, _BaseMap);
#endif
	o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
	return o;
}

half4 DepthOnlyFragment(v2f i) : SV_TARGET
{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

#if defined(_ALPHATEST_ON)
	half alpha = SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap, i.texcoord).a;
	clip(alpha - _Cutoff);
#elif defined(_ALPHBLEND_ON)
	clip(-1);	
#endif

	return 0;
}

#endif