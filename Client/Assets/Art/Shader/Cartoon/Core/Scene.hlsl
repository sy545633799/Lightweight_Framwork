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
half _Metallic;
half _OcclusionStrength;
//emission
half4 _EmissionColor;
CBUFFER_END

TEXTURE2D(_PBRTex); SAMPLER(sampler_PBRTex);
TEXTURE2D(_EmissionTex); SAMPLER(sampler_EmissionTex);

v2f ScenePassVertex(a2v i)
{
	v2f o;
	UNITY_SETUP_INSTANCE_ID(o);
	UNITY_TRANSFER_INSTANCE_ID(i, o);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

	o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
	o.texcoord.xy = TRANSFORM_TEX(i.texcoord, _BaseMap);
	//o.texcoord.zw = TRANSFORM_TEX(i.texcoord, _BumpMap);
	half3 positionWS = TransformObjectToWorld(i.positionOS.xyz);
	
#if defined(_NORMALMAP)
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

	return o;
}

half4 ScenePassFragment(v2f i) :SV_TARGET
{
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
	
	half4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.texcoord.xy);
#if defined(_ALPHATEST_ON)
	clip(tex.a - _Cutoff);
#endif

	
#if defined(_NORMALMAP)
	float4 nortex = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, i.texcoord.xy);
	half3 normalTangent = UnpackNormalScale(nortex, _BumpScale).xyz;
	half3 normalWS = normalize(mul(half3x3(i.tangentToWorld[0].xyz, i.tangentToWorld[1].xyz, i.tangentToWorld[2].xyz), normalTangent));
	half3 positionWS = half3(i.tangentToWorld[0].w, i.tangentToWorld[1].w, i.tangentToWorld[2].w);
#else
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

	half4 color;
#if !defined(_USE_PBR)
	Light mylight = GetMainLight(shadowCoord);
	half4 lightColor = float4(mylight.color, 1);
	half3 lightDirWS = normalize(mylight.direction);
	half diffuse = saturate(dot(lightDirWS, normalWS)) * mylight.shadowAttenuation;
	color = tex * _BaseColor * diffuse * lightColor;
#endif

#if !defined(_USE_DIFFUSE)
	float3 viewDirWS = normalize(UnityWorldSpaceViewDir(positionWS));
#endif

#if defined(_USE_SPECULAR)
	float diffuse = dot(lightDirWS, normal) * 0.5 + 0.5;
	half spe = saturate(dot(normalize(lightDirWS + viewDirWS), normalWS));
	half specular = pow(spe, _SpecRange) * _SpecColor;
	color += tex * _BaseColor * specular * lightColor;
#endif
	
	
#if !defined(_USE_PBR)
#if defined(_EMISSION)
	half3 emission = SampleEmission(i.texcoord.xy, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
	color += half4(i.fogFactorAndVertexLight.gba + emission, 1);
#elif defined(_ADDITIONAL_LIGHTS_VERTEX)
	color += half4(i.fogFactorAndVertexLight.gba, 1);
#endif
#else
	half3 alpha = Alpha(tex.a, _BaseColor, _Cutoff);
	half3 albedo = tex.rgb * _BaseColor.rgb;
#if defined(_PBRMAP)
	half4 pbr = SAMPLE_TEXTURE2D(_PBRTex, sampler_PBRTex, i.texcoord.xy);
	half smoothness = pbr.r;
	half metallic = pbr.g;
	half occlusion = LerpWhiteTo(pbr.b, _OcclusionStrength);
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
	InputData inputData = GetInputData(i, positionWS, normalWS, viewDirWS);
	color = UniversalFragmentPBR(inputData, albedo, metallic, specular, smoothness, occlusion, emission, alpha);
#endif
	
	color.rgb = MixFog(color.rgb, i.fogFactorAndVertexLight.x);

	return color;
}

#endif