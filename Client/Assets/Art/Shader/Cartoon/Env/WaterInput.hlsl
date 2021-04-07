#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

CBUFFER_START(UnityPerMaterial)
float4 _AnimationParams;
//float4 _SlopeParams;
float4 _ShallowColor;
float4 _BaseColor;
float _Depth;
float _DepthExp;
//bump
float4 _BumpMap_ST;
float _BumpScale;
float _BumpTiling;
//Intersection
half _IntersectionLength;
half _IntersectionFalloff;
half _IntersectionTiling;
half _IntersectionRippleDist;
half _IntersectionRippleStrength;
half _IntersectionClipping;
float _IntersectionSpeed;
half _EdgeFade;
//Underwater
half _CausticsBrightness;
float _CausticsTiling;
half _CausticsDistortion;
half _RefractionStrength;

//spec
float _SunReflectionDistortion;
float _SunReflectionSize;
float _SunReflectionStrength;
CBUFFER_END

#define TIME ((_Time.y * _AnimationParams.z) * _AnimationParams.xy)
#define TIME_VERTEX ((TIME_VERTEX_OUTPUT * _AnimationParams.z) * _AnimationParams.xy)
SamplerState sampler_LinearRepeat;
#define Repeat sampler_LinearRepeat
SamplerState sampler_LinearClamp;
#define Clamp sampler_LinearClamp

TEXTURE2D(_IntersectionNoise); SAMPLER(sampler_IntersectionNoise);
TEXTURE2D(_CausticsTex);

float GetSlopeInverse(float3 normalWS)
{
	return saturate(pow(dot(float3(0, 1, 0), normalWS), 4));
}

float DepthDistance(float3 wPos, float3 viewPos, float3 normal)
{
	float3 dist = (wPos - viewPos);
	return ((dist.x * normal.x) + (dist.y * normal.y) + (dist.z * normal.z));
}

float SampleIntersection(float2 uv, float gradient, float2 time)
{
#if defined(_SHARP_INERSECTION)
	float sine = sin(time.y * 10 - (gradient * _IntersectionRippleDist)) * _IntersectionRippleStrength;
	float2 nUV = float2(uv.x, uv.y) * _IntersectionTiling;
	float noise = SAMPLE_TEXTURE2D(_IntersectionNoise, sampler_IntersectionNoise, nUV + time.xy).r;

	float dist = saturate(gradient / _IntersectionFalloff);
	noise = saturate((noise + sine) * dist + dist);
	float inter = step(_IntersectionClipping, noise);
#elif defined(_SMOOTH_INTERSECTION)
	float noise1 = SAMPLE_TEXTURE2D(_IntersectionNoise, sampler_IntersectionNoise, (float2(uv.x, uv.y) * _IntersectionTiling) + (time.xy)).r;
	float noise2 = SAMPLE_TEXTURE2D(_IntersectionNoise, sampler_IntersectionNoise, (float2(uv.x, uv.y) * (_IntersectionTiling * 1.5)) - (time.xy)).r;

	float dist = saturate(gradient / _IntersectionFalloff);
	float inter = saturate(noise1 + noise2 + dist) * dist;
#else
	float inter = 0;
#endif
	return saturate(inter);
}

float3 ReconstructViewPos(float4 screenPos, float3 positionWS, float eyeDepth)
{
	float3 camPos = _WorldSpaceCameraPos.xyz;
	float3 worldPos = eyeDepth * ((_WorldSpaceCameraPos - positionWS) / screenPos.w) - camPos;
	float3 perspWorldPos = -worldPos;

#if defined(ORTHOGRAPHIC_SUPPORT)
	return lerp(perspWorldPos, viewWorldPos, unity_OrthoParams.w);
#else
	return perspWorldPos;
#endif

}
float4 PackedUV(float2 sourceUV, float2 time, float2 flowmap, float speed)
{
#if _RIVER
	time *= flowmap;
	time.x = 0;
#endif

	float2 uv1 = sourceUV.xy + (time.xy * speed);
#ifndef _RIVER
	float2 uv2 = (sourceUV.xy * 0.5) + ((1 - time.xy) * speed * 0.5);
#else
	float2 uv2 = (sourceUV.xy * 0.5) + (time.xy * speed);
#endif

	return float4(uv1.xy, uv2.xy);
}

float3 SampleNormals(float2 uv, float2 time, float2 flowmap, float speed, float slope)
{
	float4 uvs = PackedUV(uv, time, flowmap, speed);
	float3 n1 = UnpackNormal(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, uvs.xy));
	float3 n2 = UnpackNormal(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, uvs.zw));

	float3 blendedNormals = BlendNormal(n1, n2);

#if _RIVER
	uvs = PackedUV(uv, time, flowmap, speed * _SlopeParams.y);
	uvs.xy = uvs.xy * float2(1, 1 - _SlopeParams.x);
	float3 n3 = UnpackNormal(SAMPLE_TEX(_BumpMap, sampler_BumpMap, uvs.xy));

	blendedNormals = lerp(n3, blendedNormals, slope);
#endif

	return blendedNormals;
}

float4 SunSpecular(Light light, float3 viewDir, float3 normalWS, float perturbation, float size, float intensity)
{
	float3 viewLightTerm = normalize(light.direction + (normalWS * perturbation) + viewDir);

	float NdotL = saturate(dot(viewLightTerm, float3(0, 1, 0)));

	half specSize = lerp(8196, 64, size);
	float specular = (pow(NdotL, specSize));
	specular *= (light.distanceAttenuation * light.shadowAttenuation);

	float3 specColor = specular * light.color * intensity;

	return float4(specColor, specSize);
}

float3 SampleCaustics(float3 depthPos, float tiling, float2 pixelOffset)
{
	//Sun projection coords
	//float4 lightSpaceUVs = mul(_MainLightWorldToShadow[0], float4(DepthPos.xyz, 1)) ;

	//Planar depth projection
	float2 causticUV = (depthPos.xz * tiling) * 0.5 + (pixelOffset.xy);
	float3 caustics = SAMPLE_TEXTURE2D(_CausticsTex, Repeat, causticUV).rgb;

	return caustics;
}

float4 ApplyLighting(SurfaceData surfaceData, InputData inputData)
{
	float4 finalColor = 0;

	Light mainLight = GetMainLight(inputData.shadowCoord);

	mainLight.shadowAttenuation = saturate(mainLight.shadowAttenuation);

	MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI, half4(0, 0, 0, 0));

	half3 attenuatedLightColor = mainLight.color * (mainLight.distanceAttenuation * mainLight.shadowAttenuation);
	half3 diffuseColor = inputData.bakedGI + LightingLambert(attenuatedLightColor, mainLight.direction, inputData.normalWS);
	half3 specularColor = 0;

#ifdef _ADDITIONAL_LIGHTS
	uint pixelLightCount = GetAdditionalLightsCount();
	for (uint lightIndex = 0u; lightIndex < pixelLightCount; ++lightIndex)
	{
		Light light = GetAdditionalLight(lightIndex, inputData.positionWS);
		half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
		diffuseColor += LightingLambert(attenuatedLightColor, light.direction, inputData.normalWS);
		specularColor += LightingSpecular(attenuatedLightColor, light.direction, inputData.normalWS, inputData.viewDirectionWS, half4(light.color.rgb, 0), surfaceData.smoothness * 0.1);
	}
#endif

#ifdef _ADDITIONAL_LIGHTS_VERTEX 
	diffuseColor += inputData.vertexLighting;
#endif

	//Emission holds sun specular
	finalColor.rgb = diffuseColor * surfaceData.albedo + surfaceData.emission + specularColor;
	finalColor.a = surfaceData.alpha;

	return finalColor;
}