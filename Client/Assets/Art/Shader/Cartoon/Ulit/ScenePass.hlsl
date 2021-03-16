#ifndef CARTOON_H
#define CARTOON_H

#define REQUIRES_WORLD_SPACE_POS_INTERPOLATOR 1

#include "../Core/Common.hlsl"

TEXTURE2D(_MetallicGlossMap);
SAMPLER(sampler_MetallicGlossMap);
TEXTURE2D(_EmissionTex);
SAMPLER(sampler_EmissionTex);

v2f ScenePassVertex(a2v i)
{
	v2f o = (v2f)0;
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_TRANSFER_INSTANCE_ID(i, o);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
	
	CommonInitV2F(i, o);
	o.texcoord.xy = TRANSFORM_TEX(i.texcoord, _BaseMap);
#ifdef LIGHTMAP_ON
	o.texcoord.zw = i.lightmapUV.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif

	return o;
}

half4 ScenePassFragment(v2f i) :SV_TARGET
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

#if defined(_METALLICSPECGLOSSMAP)
	half4 pbr = SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, i.texcoord.xy);
#endif


#if defined(_USE_PBR)
#if defined(_METALLICSPECGLOSSMAP)
	
	half smoothness = pbr.a * _Smoothness;
	half metallic = pbr.r;
//#if defined(SHADER_API_GLES)
//	half occlusion = pbr.b;
//#else
//	half occlusion = LerpWhiteTo(pbr.b, _OcclusionStrength);
//#endif
	half occlusion = 1.0;
#else
	half smoothness = _Smoothness;
	half metallic = _Metallic;
	half occlusion = 1.0;
#endif
	half4 color = UniversalFragmentPBR(inputData, albedo.rgb, metallic, half3(0.0h, 0.0h, 0.0h), smoothness, occlusion, emission, alpha);
#else
	half4 color = UniversalFragmentBlinnPhong(inputData, albedo.rgb, albedo * _SpecColor, _Shininess, emission, alpha);
#endif
	
	color.rgb = MixFog(color.rgb, i.fogFactorAndVertexLight.x);

	return color;
}


#endif