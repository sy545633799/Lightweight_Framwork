#ifndef Character_H
#define Character_H

#define REQUIRES_WORLD_SPACE_POS_INTERPOLATOR 1
#include "../Core/Common.hlsl"

v2f CharacterPassVertex(a2v i)
{
	v2f o = (v2f)0;
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_TRANSFER_INSTANCE_ID(i, o);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

	CommonInitV2F(i, o);
	o.texcoord.xy = TRANSFORM_TEX(i.texcoord, _BaseMap);

	return o;
}

half4 CharacterPassFragment(v2f i) :SV_TARGET
{
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
	WORLD_NORMAL_POSITION_VIEWDIR(i);

	half3 positionWS = i.positionWS;
	float3 viewDirWS = SafeNormalize(UnityWorldSpaceViewDir(positionWS));
	InputData inputData = GetInputData(i, positionWS, normalWS, viewDirWS, SH);

	half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.texcoord.xy);
	//half3 alpha = Alpha(albedo.a, _BaseColor, _Cutoff);
#if defined(_ALPHATEST_ON)
	half alpha = _BaseColor.a;
	clip(albedo.a - _Cutoff);
#elif defined(_ALPHABLEND_ON)
	half alpha = _BaseColor.a * albedo.a;
#else
	half alpha = _BaseColor.a;
#endif
	
	albedo.rgb *= _BaseColor.rgb;

#if defined(_EMISSION)
	half3 emission = SampleEmission(i.texcoord.xy, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
#else
	half3 emission = _EmissionColor.rgb;
#endif

#if defined(_MASKMAP)
	half4 mask = SAMPLE_TEXTURE2D(_MaskMap, sampler_MaskMap, i.texcoord.xy);
	half4 color = UniversalFragmentCartoon(inputData, albedo, mask, emission, alpha);
#else
	half4 color = UniversalFragmentCartoon(inputData, albedo, 0, emission, alpha);
#endif
	

	color.rgb = MixFog(color.rgb, i.fogFactorAndVertexLight.x);

	return color;
}

#endif