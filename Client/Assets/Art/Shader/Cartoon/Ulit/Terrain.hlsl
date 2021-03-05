#ifndef TETTRAIN_H
#define TETTRAIN_H

#define _USE_TEXCOORD2 1
#define _USE_TEXCOORD3 1
#define REQUIRES_WORLD_SPACE_POS_INTERPOLATOR 1

#include "../Core/Common.hlsl"

CBUFFER_START(UnityPerMaterial)
half4 _SpecColor;
float _Shininess;
TEXTURE2D(_Splat0);
SAMPLER(sampler_Splat0);
float4 _Splat0_ST;
TEXTURE2D(_Splat1);
float4 _Splat1_ST;
TEXTURE2D(_Splat2);
float4 _Splat2_ST;
TEXTURE2D(_Splat3);
float4 _Splat3_ST;
TEXTURE2D(_Normal0);
SAMPLER(sampler_Normal0);
float4 _Normal0_ST;
TEXTURE2D(_Normal1);
SAMPLER(sampler_Normal1);
float4 _Normal1_ST;
TEXTURE2D(_Normal2);
SAMPLER(sampler_Normal2);
float4 _Normal2_ST;
TEXTURE2D(_Normal3);
SAMPLER(sampler_Normal3);
float4 _Normal3_ST;
TEXTURE2D(_Control);
float4 _Control_ST;
float4 _Control_TexelSize;
SAMPLER(sampler_Control);

half4 _BaseColor;
half _Metallic0;
half _Metallic1;
half _Metallic2;
half _Metallic3;
half _Smoothness0;
half _Smoothness1;
half _Smoothness2;
half _Smoothness3;
half _Hightthreshold;
half4 _EmissionColor;
TEXTURE2D(_EmissionTex);
SAMPLER(sampler_EmissionTex);

CBUFFER_END

v2f TerrainPassVertex(a2v i)
{	
	v2f o = (v2f)0;
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_TRANSFER_INSTANCE_ID(i, o);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

	CommonInitV2F(i, o);
	o.texcoord.xy = TRANSFORM_TEX(i.texcoord, _Control);
#ifdef LIGHTMAP_ON
	o.texcoord.zw = i.lightmapUV.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif

	o.texcoord2.xy = TRANSFORM_TEX(i.texcoord, _Splat0);
	o.texcoord2.zw = TRANSFORM_TEX(i.texcoord, _Splat1);
	o.texcoord3.xy = TRANSFORM_TEX(i.texcoord, _Splat2);
	o.texcoord3.zw = TRANSFORM_TEX(i.texcoord, _Splat3);
	return o;
}

half4 TerrainPassFragment(v2f i) : SV_Target
{
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

	float2 splatUV = (i.texcoord.xy * (_Control_TexelSize.zw - 1.0f) + 0.5f) * _Control_TexelSize.xy;
	half4 splatControl = SAMPLE_TEXTURE2D(_Control, sampler_Control, splatUV);
	half4 albedo = 0.0h;

	albedo += SAMPLE_TEXTURE2D(_Splat0, sampler_Splat0, i.texcoord2.xy) * half4(splatControl.rrr, 1.0h);
	albedo += SAMPLE_TEXTURE2D(_Splat1, sampler_Splat0, i.texcoord2.zw) * half4(splatControl.ggg, 1.0h);
	albedo += SAMPLE_TEXTURE2D(_Splat2, sampler_Splat0, i.texcoord3.xy) * half4(splatControl.bbb, 1.0h);
	albedo += SAMPLE_TEXTURE2D(_Splat3, sampler_Splat0, i.texcoord3.zw) * half4(splatControl.aaa, 1.0h);

#ifdef _NORMALMAP
	half3 normalTS = 0.0f;
	normalTS += UnpackNormalScale(SAMPLE_TEXTURE2D(_Normal0, sampler_Normal0, i.texcoord2.xy), splatControl.r);
	normalTS += UnpackNormalScale(SAMPLE_TEXTURE2D(_Normal1, sampler_Normal1, i.texcoord2.zw), splatControl.g);
	normalTS += UnpackNormalScale(SAMPLE_TEXTURE2D(_Normal2, sampler_Normal2, i.texcoord3.xy), splatControl.b);
	normalTS += UnpackNormalScale(SAMPLE_TEXTURE2D(_Normal3, sampler_Normal3, i.texcoord3.zw), splatControl.a);
	normalTS = normalize(normalTS.xyz);
	float sgn = i.tangentWS.w;
	float3 bitangent = sgn * cross(i.normalWS.xyz, i.tangentWS.xyz);
	half3 normalWS = NormalizeNormalPerPixel(TransformTangentToWorld(normalTS, half3x3(i.tangentWS.xyz, bitangent.xyz, i.normalWS.xyz)));
	half3 SH = SampleSH(normalWS.xyz);
#else
	half3 normalWS = i.normalWS;
	half3 SH = i.vertexSH;
#endif

	half3 positionWS = i.positionWS;
	float3 viewDirWS = normalize(UnityWorldSpaceViewDir(positionWS));
	InputData inputData = GetInputData(i, positionWS, normalWS, viewDirWS, SH);

#if defined(_EMISSION)
	half3 emission = SampleEmission(i.texcoord.xy, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
#else
	half3 emission = _EmissionColor.rgb;
#endif
	
#if defined(_USE_PBR)
	half4 smoothness = dot(splatControl, half4(_Smoothness0, _Smoothness1, _Smoothness2, _Smoothness3));
	half metallic = dot(splatControl, half4(_Metallic0, _Metallic1, _Metallic2, _Metallic3));
	half4 color = UniversalFragmentPBR(inputData, albedo, metallic, half3(0.0h, 0.0h, 0.0h), smoothness, 1.0, emission, 1);
#else
	half4 color = UniversalFragmentBlinnPhong(inputData, albedo.rgb, albedo * _SpecColor, _Shininess, emission, 1);
#endif


	color.rgb = MixFog(color.rgb, i.fogFactorAndVertexLight.x);

	return half4(color.rgb, 1.0h);
}
#endif