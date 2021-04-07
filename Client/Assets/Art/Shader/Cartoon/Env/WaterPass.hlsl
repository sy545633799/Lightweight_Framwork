
#define _NORMALMAP 1
#define _USE_TEXCOORD2 1
#define REQUIRES_WORLD_SPACE_POS_INTERPOLATOR 1
#include "../Core/Mathf.hlsl"
#include "../Core/Common.hlsl"

v2f vert(a2v i)
{
	v2f o = (v2f)0;
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_TRANSFER_INSTANCE_ID(i, o);
	//UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
	CommonInitV2F(i, o);

	o.texcoord.xy = i.texcoord;
#ifdef LIGHTMAP_ON
	o.texcoord.zw = i.lightmapUV.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif
	o.texcoord2 = ComputeScreenPos(o.positionCS);
	return o;
}

half4 frag(v2f i) : SV_Target
{
	UNITY_SETUP_INSTANCE_ID(i);

	float3 finalColor = 0;
	float alpha = 1;

	float4 screenPos = i.texcoord2;
	half3 positionWS = i.positionWS;
	float3 viewDirWS = SafeNormalize(UnityWorldSpaceViewDir(positionWS));
	float2 uv = positionWS.xz;
#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
	Light mainLight = GetMainLight(i.shadowCoord);
#else
	Light mainLight = GetMainLight();
#endif

	float3 waveNormal = SafeNormalize(i.normalWS);

#if defined(_NORMALMAP)
	float3 normalTS = SampleNormals(i.texcoord.xy * _BumpTiling * 1000, _Time.y, float2(1, 1), 0.2, 0);
	float sgn = i.tangentWS.w;
	float3 bitangent = sgn * cross(i.normalWS.xyz, i.tangentWS.xyz);
	float3 normalWS = SafeNormalize(TransformTangentToWorld(normalTS, half3x3(i.tangentWS.xyz, bitangent, i.normalWS.xyz)));
#else
	float3 normalTS = float3(0.5, 0.5, 1);
	float3 normalWS = waveNormal;
#endif

#if defined(_REFRACTION)
	float4 refractedScreenPos = screenPos.xyzw + (float4(normalWS.xy, 0, 0) * (_RefractionStrength * 0.1));
#endif

	float depth = SampleSceneDepth(screenPos / screenPos.w);
	float eyeDepth = LinearEyeDepth(depth, _ZBufferParams);
	float3 opaquePositionWS = ReconstructViewPos(screenPos, positionWS, eyeDepth);
	float normalSign = ceil(dot(viewDirWS, i.normalWS.xyz)) == 0 ? -1 : 1;
	float opaqueDist = DepthDistance(positionWS, opaquePositionWS, i.normalWS.xyz * normalSign);
	float absorptionDepth = saturate(lerp(opaqueDist / _Depth, 1 - (exp(-opaqueDist) / _Depth), _DepthExp));
	
	float4 baseColor = lerp(_ShallowColor, _BaseColor, absorptionDepth);
	finalColor.rgb = baseColor.rgb;
	float edgeFade = saturate(opaqueDist / (_EdgeFade * 0.01));
	alpha = baseColor.a * edgeFade;

#if defined(_SHARP_INERSECTION) || defined(_SMOOTH_INTERSECTION)
	float interSecGradient = 1 - saturate(exp(opaqueDist) / _IntersectionLength);
	float intersection = SampleIntersection(uv.xy, interSecGradient, TIME * _IntersectionSpeed);
	waveNormal = lerp(waveNormal, i.normalWS, intersection);
#else
	float intersection = 0;
#endif

#if defined(_SPECULAR_COLOR)
	half4 sunSpec = 0;
	float3 sunReflectionNormals = normalWS;
	sunSpec = SunSpecular(mainLight, viewDirWS, sunReflectionNormals, _SunReflectionDistortion, _SunReflectionSize, _SunReflectionStrength);
	sunSpec.rgb *= saturate(1 - intersection);
#else
	half4 sunSpec = 0;
#endif

#if defined(_USE_CAUSTICS)
	float3 caustics = SampleCaustics(opaquePositionWS, _CausticsTiling, normalWS.xy * _CausticsDistortion) * _CausticsBrightness;
	float causticsMask = absorptionDepth;
	causticsMask = saturate(causticsMask + intersection);
	finalColor = lerp(finalColor + caustics, finalColor, causticsMask);
#endif

#if defined(_SHARP_INERSECTION) || defined(_SMOOTH_INTERSECTION)
	finalColor.rgb = lerp(finalColor.rgb, 1, intersection);
#endif

	alpha = saturate(alpha + intersection);
	half normalMask = saturate((intersection));
	normalWS = lerp(waveNormal, normalWS, saturate(_BumpScale - normalMask));

	SurfaceData surf = (SurfaceData)0;
	surf.albedo = finalColor.rgb;
	surf.specular = 0;
	surf.metallic = 0;
	surf.smoothness = sunSpec.a;
	surf.normalTS = normalTS;
	surf.emission = sunSpec.rgb;
	surf.occlusion = 1;
	surf.alpha = alpha;

	InputData inputData;
	inputData.positionWS = positionWS;
	inputData.viewDirectionWS = viewDirWS;
	inputData.shadowCoord = 0;
	inputData.normalWS = normalWS;
	inputData.fogCoord = i.fogFactorAndVertexLight.x;

	inputData.vertexLighting = i.fogFactorAndVertexLight.yzw;
	inputData.bakedGI = 0;

	float4 color = ApplyLighting(surf, inputData);

#if defined(_REFRACTION)
	float3 refraction = SampleSceneColor(refractedScreenPos.xy / refractedScreenPos.w).rgb;
	color.rgb = lerp(refraction, color.rgb, alpha);
	alpha = edgeFade;
#endif
	color.rgb = MixFog(color.rgb, i.fogFactorAndVertexLight.x);
	color.a = alpha * saturate(alpha);

	return color;
}