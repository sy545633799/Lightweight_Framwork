
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/EntityLighting.hlsl"
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/ImageBasedLighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

CBUFFER_START(UnityPerMaterial)
//clip
half _Cutoff;
//diffuse
float4 _BaseMap_ST;
half4 _BaseColor;
//bump
float4 _BumpMap_ST;
float _BumpScale;
//outline
float _OutlineWidth;
float3 _OutlineColor;
float _OutlineScaledMaxDistance;
//cartoon
float _Rampthreshold;
half _ShadowRange;
half _FaceShadowRange;
half3 _BrightSideColor;
half3 _DarkSideColor;
half _Shininess;
half3 _SpecularColor;
half _SpecTrail;
half _Specular;
half _RimPower;
half _RimThreshold;
half3 _RimColor;
//emission
half4 _EmissionColor;

CBUFFER_END

TEXTURE2D(_MaskMap); SAMPLER(sampler_MaskMap);

half4 UniversalFragmentCartoon(InputData inputData, half3 albedo, half2 uv, half3 emission, half alpha)
{ 
	half3 color = albedo;  

	Light mainLight = GetMainLight(inputData.shadowCoord);
	half attenuatedLightColor = (mainLight.distanceAttenuation * mainLight.shadowAttenuation);
	half lightAtten = (dot(mainLight.direction, inputData.normalWS) * 0.5 + 0.5) * attenuatedLightColor;;
	lightAtten = (smoothstep(_Rampthreshold - _ShadowRange, _Rampthreshold + _ShadowRange, lightAtten));

#if defined(_MASKMAP)
	half4 mask = SAMPLE_TEXTURE2D(_MaskMap, sampler_MaskMap, uv);
	lightAtten = lerp(1, lightAtten, smoothstep(0.001, _FaceShadowRange, mask.r));//R通道做遮罩//考虑加算
#endif
	half3 rampColor = lightAtten * _BrightSideColor + (1 - lightAtten) *  _DarkSideColor;
	color.rgb *= rampColor;

//高光
#if defined(_USE_SPECULAR)
	half3 halfDir = normalize(mainLight.direction + inputData.viewDirectionWS);
	float specular = saturate(max(0, dot(halfDir, inputData.normalWS)));
	specular = smoothstep(_Shininess - _SpecTrail, _Shininess, specular)*_Specular;
#if defined(_MASKMAP)
	specular *= mask.b;
#endif
	half3 specularColor = specular * _SpecularColor;
	color.rgb += specularColor;
#endif

//边光
#if defined(_USE_RIM)
	half ndotv = max(0, dot(inputData.normalWS, inputData.viewDirectionWS));
	half rim = 1 - saturate(ndotv);
	rim = smoothstep(0, _RimThreshold, rim);
	half3 rimColor = rim * _RimPower * _RimColor;
	color.rgb += rimColor * lightAtten;
#endif

#ifdef _ADDITIONAL_LIGHTS_VERTEX
	color += inputData.vertexLighting;
#endif

	half3 finalColor = color * mainLight.color + emission;

	return half4(finalColor, alpha);
}

