#ifndef Character_H
#define Character_H
#include "../Core/Common.hlsl"

CBUFFER_START(UnityPerMaterial)
//clip
half _Cutoff;
//diffuse
float4 _BaseMap_ST;
half4 _BaseColor;
half _Rampthreshold;
half _BumpScale;
//spec
half4 _SpecColor;
float _SpecRange;
//rim
half4 _RimColor;
half _RimThreshold;
half _RimStrength;

//outline 
half4 _OutlineColor;
half _OutlineWidth;
half _Outlinethreshold;

CBUFFER_END

TEXTURE2D(_ShaderGradeMap); SAMPLER(sampler_ShaderGradeMap);
TEXTURE2D(_MaskMap); SAMPLER(sampler_MaskMap);


v2f CharacterPassVertex(a2v i)
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

half4 CharacterPassFragment(v2f i) :SV_TARGET
{
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);


	half4 tex = SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap,i.texcoord.xy);
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
	Light mylight = GetMainLight(shadowCoord);
	half4 lightColor = float4(mylight.color, 1);
	half3 lightDirWS = normalize(mylight.direction);
	half diffuse = (dot(lightDirWS, normalWS) * 0.5 + 0.5) * mylight.shadowAttenuation;
	diffuse = smoothstep(_Rampthreshold - 0.01, _Rampthreshold + 0.01, diffuse);
	float4 gradtex = SAMPLE_TEXTURE2D(_ShaderGradeMap, sampler_ShaderGradeMap, i.texcoord.xy);
	color = lerp(gradtex, tex, diffuse) * _BaseColor * lightColor;

#if defined(_USE_RIM) || defined(_USE_MASK)
	half3 viewDirWS = normalize(UnityWorldSpaceViewDir(positionWS));
#endif

#if defined(_USE_RIM)
	half rim = saturate(1 - dot(normalWS, viewDirWS));
	color += pow(rim, _RimThreshold) * _RimStrength * _RimColor;
#endif
	

#if defined(_USE_MASK)
	float4 mask = SAMPLE_TEXTURE2D(_MaskMap, sampler_MaskMap, i.texcoord.xy);
	half spe = saturate(dot(normalize(lightDirWS + viewDirWS), normalWS));
	half specular = pow(spe, _SpecRange) * _SpecColor;
	color = color * mask.r + (color * specular) * mask.g;
#endif

	color.rgb = MixFog(color.rgb, i.fogFactorAndVertexLight.x);

	return color;
}

#endif