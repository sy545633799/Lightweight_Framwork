
#ifndef FXMAIN_H
#define FXMAIN_H
#include "./Common.hlsl"

v2f vert(a2v i)
{
	v2f o = (v2f)0;
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_TRANSFER_INSTANCE_ID(i, o);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

	o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
	o.fogFactorAndVertexLight.r = ComputeFogFactor(o.positionCS.z);

	o.texcoord.xy = i.texcoord.xy;
#ifdef _FLIPBOOKBLENDING_ON
	o.texcoord2.xy = i.texcoord.zw;
	o.texcoord2.z = i.texcoord2.x;
#endif

	o.texcoord3 = i.color;
#if defined(_SOFTPARTICLES_ON) || defined(_FADING_ON) || defined(_DISTORTION_ON)
	o.projectedPosition = ComputeScreenPos(vertexInput.positionCS);
#endif

	return o;
}

half4 frag(v2f i) : SV_Target
{
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

	float2 uv = i.texcoord.xy;
	float3 blendUv = float3(0, 0, 0);
#if defined(_FLIPBOOKBLENDING_ON)
	blendUv = i.texcoord2;
#endif

	float4 projectedPosition = float4(0,0,0,0);
#if defined(_SOFTPARTICLES_ON) || defined(_FADING_ON) || defined(_DISTORTION_ON)
	projectedPosition = i.projectedPosition;
#endif

	half4 albedo = SampleAlbedo(uv, blendUv, _BaseColor, i.texcoord3, projectedPosition, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
	half3 normalTS = SampleNormalTS(uv, blendUv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap));

#if defined (_DISTORTION_ON)
	albedo.rgb = Distortion(albedo, normalTS, _DistortionStrengthScaled, _DistortionBlend, projectedPosition);
#endif

#if defined(_EMISSION)
	half3 emission = BlendTexture(TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap), uv, blendUv).rgb * _EmissionColor.rgb;
#else
	half3 emission = half3(0, 0, 0);
#endif

	half3 result = albedo.rgb + emission;

	result = MixFog(result.rgb, i.fogFactorAndVertexLight.x);

	albedo.a = OutputAlpha(albedo.a, _Surface);

	return half4(result, albedo.a);
}


#endif