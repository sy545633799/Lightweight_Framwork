#ifndef TETTRAIN_H
#define TETTRAIN_H
#include "./Common.hlsl"
CBUFFER_START(UnityPerMaterial)
TEXTURE2D(_Albedo0);
SAMPLER(sampler_Albedo0);
float4 _Albedo0_ST;
TEXTURE2D(_Albedo1);
float4 _Albedo1_ST;
TEXTURE2D(_Albedo2);
float4 _Albedo2_ST;
TEXTURE2D(_Normal0);
SAMPLER(sampler_Normal0);
float4 _Normal0_ST;
TEXTURE2D(_Normal1);
float4 _Normal1_ST;
TEXTURE2D(_Normal2);
float4 _Normal2_ST;
TEXTURE2D(_Mask);
float4 _Mask_ST;
SAMPLER(sampler_Mask);
half4 _Color;
half _Metallic0;
half _Metallic1;
half _Metallic2;
half _Hightthreshold;
CBUFFER_END


inline half3 Blend(half high1, half high2, half high3, half3 control)
{
	half3 blend;
	blend.r = high1 * control.r;
	blend.g = high2 * control.g;
	blend.b = high3 * control.b;

	half ma = max(blend.r, max(blend.g, blend.b));
	blend = max(blend - ma + _Hightthreshold, 0) * control;
	return blend / (blend.r + blend.g + blend.b);
}

v2f TerrainPassVertex(a2v i)
{	
	v2f o;
	UNITY_SETUP_INSTANCE_ID(o);
	UNITY_TRANSFER_INSTANCE_ID(i, o);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

	o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
	half3 positionWS = TransformObjectToWorld(i.positionOS.xyz);
#if defined(_NORMALMAP) || defined(_USE_TERRAIN)
	half4x4 tangentToWorld = CreateTangentToWorldPerVertexFantasy(i.normalOS, i.tangentOS, positionWS);
	o.tangentToWorld[0] = tangentToWorld[0];
	o.tangentToWorld[1] = tangentToWorld[1];
	o.tangentToWorld[2] = tangentToWorld[2];
#else
	o.normalWS = TransformObjectToWorldDir(i.normalOS);
	o.positionWS = positionWS;
#endif

	o.fogFactorAndVertexLight.r = ComputeFogFactor(o.positionCS.z);
#if defined(_ADDITIONAL_LIGHTS_VERTEX)
	o.fogFactorAndVertexLight.gba = VertexLighting(positionWS, TransformObjectToWorldDir(i.normalOS));
#endif

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
	o.shadowCoord = TransformWorldToShadowCoord(positionWS);
#endif

	o.texcoord.xy = TRANSFORM_TEX(i.texcoord, _Albedo0);
	o.texcoord.zw = TRANSFORM_TEX(i.texcoord, _Albedo1);
	o.texcoord2.xy = TRANSFORM_TEX(i.texcoord, _Albedo2);
	o.texcoord2.zw = TRANSFORM_TEX(i.texcoord, _Mask);

	return o;
}

half4 TerrainPassFragment(v2f i) : SV_Target
{
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

	half4 tex = SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap,i.texcoord.xy);
#if defined(_NORMALMAP)
	float4 nortex = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, i.texcoord.xy);
	half3 normalTangent = UnpackNormalScale(nortex, _BumpScale).xyz;
	half3 normalWS = normalize(mul(half3x3(i.tangentToWorld[0].xyz, i.tangentToWorld[1].xyz, i.tangentToWorld[2].xyz), normalTangent));
	half3 positionWS = half3(i.tangentToWorld[0].w, i.tangentToWorld[1].w, i.tangentToWorld[2].w);
#elif !defined(_USE_TERRAIN)
	half3 normalWS = i.normalWS;
	half3 positionWS = i.positionWS;
#endif

	float4 shadowCoord;
#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
	shadowCoord = i.shadowCoord;
#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
	shadowCoord = TransformWorldToShadowCoord(positionWS);
#else
	shadowCoord = float4(0, 0, 0, 0);
#endif
	/******************************************************以上貌似可以统一处理************************************************/

	half4 albedo0 = SAMPLE_TEXTURE2D(_Albedo0,sampler_Albedo0,i.texcoord.xy);
	half4 albedo1 = SAMPLE_TEXTURE2D(_Albedo1,sampler_Albedo0,i.texcoord.zw);
	half4 albedo2 = SAMPLE_TEXTURE2D(_Albedo2,sampler_Albedo0,i.texcoord2.xy);
	half4 normal0 = SAMPLE_TEXTURE2D(_Normal0,sampler_Normal0,i.texcoord.xy);
	half4 normal1 = SAMPLE_TEXTURE2D(_Normal1,sampler_Normal0,i.texcoord.zw);
	half4 normal2 = SAMPLE_TEXTURE2D(_Normal2,sampler_Normal0,i.texcoord2.xy);

	half4 mask = SAMPLE_TEXTURE2D(_Mask,sampler_Mask, i.texcoord2.zw);
	half3 blend = Blend(albedo0.a,albedo1.a,albedo2.a,mask.rgb);
	half3 albedo = (blend.x*albedo0 + blend.y*albedo1 + blend.z*albedo2).xyz;
	half4 bump = blend.x*normal0 + blend.y*normal1 + blend.z*normal2;
	half3 normalWS = normalize(mul(half3x3(i.tangentToWorld[0].xyz, i.tangentToWorld[1].xyz, i.tangentToWorld[2].xyz), bump.xyz));
	half3 positionWS = half3(i.tangentToWorld[0].w, i.tangentToWorld[1].w, i.tangentToWorld[2].w);
	float3 viewDirWS = normalize(UnityWorldSpaceViewDir(positionWS));

	half3 alpha = 1.0;
	half smoothness = tex.a;
	half metallic = dot(blend, half3(_Metallic0, _Metallic1, _Metallic2));
	half occlusion = 1.0;
	half3 specular = half3(0.0h, 0.0h, 0.0h);

#if defined(_EMISSION)
	half3 emission = SampleEmission(i.texcoord.xy, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap)).rgb;
#else
	half3 emission = 0;
#endif
	InputData inputData = GetInputData(i, positionWS, normalWS, viewDirWS);
	half4 color = UniversalFragmentPBR(inputData, albedo, metallic, specular, smoothness, occlusion, emission, alpha);
	color.rgb = MixFog(color.rgb, i.fogFactorAndVertexLight.x);

	return color;
}
#endif