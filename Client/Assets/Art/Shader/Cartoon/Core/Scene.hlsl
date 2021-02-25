#ifndef CARTOON_H
#define CARTOON_H
#include "./Common.hlsl"

CBUFFER_START(UnityPerMaterial)
//clip
half _Cutoff;
//diffuse
float4 _BaseMap_ST;
half4 _BaseColor;
//bump
float4 _BumpMap_ST;
float _BumpScale;
//spec
half4 _SpecColor;
float _SpecRange;
//pbr(如果没有PBRTEX，则手动输入_Smoothness和_Metallic)
half _Smoothness;
//如果有PBRTEX, 则没有这个选项
half _Metallic;
half _OcclusionStrength;
//emission
half4 _EmissionColor;
CBUFFER_END

TEXTURE2D(_PBRTex); SAMPLER(sampler_PBRTex);
TEXTURE2D(_EmissionTex); SAMPLER(sampler_EmissionTex);

v2f ScenePassVertex(a2v i)
{
	v2f o = (v2f)0;
	UNITY_SETUP_INSTANCE_ID(o);
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

	InputData inputData = GetInputData(i, positionWS, normalWS, viewDirWS, SH);

	half4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.texcoord.xy);
#if defined(_ALPHATEST_ON)
	clip(tex.a - _Cutoff);
#endif
	half3 alpha = Alpha(tex.a, _BaseColor, _Cutoff);
	half3 albedo = tex.rgb * _BaseColor.rgb;
	
#if defined(_USE_PBR)
	//PBR
#if defined(_PBRMAP)
	half4 pbr = SAMPLE_TEXTURE2D(_PBRTex, sampler_PBRTex, i.texcoord.xy);
	half smoothness = pbr.r;
	half metallic = pbr.g;
#if defined(SHADER_API_GLES)
	half occlusion = pbr.b;
#else
	half occlusion = LerpWhiteTo(pbr.b, _OcclusionStrength);
#endif
#else
	half smoothness = _Smoothness;
	half metallic = _Metallic;
	half occlusion = 1.0;
#endif
	half3 specular = half3(0.0h, 0.0h, 0.0h);

#if defined(_EMISSION)
	half3 emission = SampleEmission(i.texcoord.xy, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap)).rgb;
#else
	half3 emission = 0;
#endif
	half4 color = UniversalFragmentPBR(inputData, albedo, metallic, specular, smoothness, occlusion, emission, alpha);
#else
	//BlinnPhong
#if defined(_EMISSION)
	half3 emission = SampleEmission(i.texcoord.xy, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
#else
	half3 emission = 0;
#endif
	half4 color = UniversalFragmentBlinnPhong(inputData, _BaseColor.rgb, _SpecColor, _SpecRange, emission, alpha);
#endif
	
	color.rgb = MixFog(color.rgb, i.fogFactorAndVertexLight.x);

	return color;
}

#endif