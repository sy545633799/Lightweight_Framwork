#ifndef CUSTOM_FANTASY_GI_INCLUDED
#define CUSTOM_FANTASY_GI_INCLUDED


#if defined(_DIRECTIONAL_PCF3)
	#define DIRECTIONAL_FILTER_SAMPLES 4
	#define DIRECTIONAL_FILTER_SETUP SampleShadow_ComputeSamples_Tent_3x3
#elif defined(_DIRECTIONAL_PCF5)
	#define DIRECTIONAL_FILTER_SAMPLES 9
	#define DIRECTIONAL_FILTER_SETUP SampleShadow_ComputeSamples_Tent_5x5
#elif defined(_DIRECTIONAL_PCF7)
	#define DIRECTIONAL_FILTER_SAMPLES 16
	#define DIRECTIONAL_FILTER_SETUP SampleShadow_ComputeSamples_Tent_7x7
#endif 


float FadedShadowStrength (float distance, float scale, float fade) {
	return saturate((1.0 - distance * scale) * fade);
}

ShadowData GetShadowData(SurfaceData surface) {
	ShadowData data;
	//data.shadowMask.always = false;
	//data.shadowMask.distance = false;
	//data.shadowMask.shadows = 1.0;
	data.cascadeBlend = 1.0;

	//Unity在模型空间 和 世界空间中, 都是左手坐标系，而在观察空间中使用的是 右手坐标系, Z方向为负
	float distance = -TransformWorldToView(surface.position).z;
	//(1 - (d / m)) / f :d表面离相机距离，m是最大阴影距离，f是消隐范围，表示为最大距离的一部分
	//scale = (1 / m), f = (1 / f)
	data.strength = FadedShadowStrength(distance, _ShadowDistanceFade.x, _ShadowDistanceFade.y);

	int i ;
	for (i = 0; i < _CascadeCount; i++) {
		float4 sphere = _CascadeCullingSpheres[i];
		float distanceSqr = DistanceSquared(surface.position, sphere.xyz);
		// sphere.w为裁切球半径的平方
		if (distanceSqr < sphere.w) {
			float fade = FadedShadowStrength(distanceSqr, 1 / sphere.w, _ShadowDistanceFade.z);
			 //最后一个级联才做消隐处理(1 - (d^2 / r^2) / (1 - f^2))
			if (i == _CascadeCount - 1)
				data.strength *= fade;
#if defined(_CASCADE_BLEND_SOFT)
			else
				data.cascadeBlend = fade;
#endif
			break;
		}
	}
	
	//级联之外强度为0
	if (i == _CascadeCount)
		data.strength = 0;
#if defined(_CASCADE_BLEND_DITHER)
	//else if (data.cascadeBlend < surface.dither) {
	//	i += 1;
	//}
#endif
	data.cascadeIndex = i;
	//if(i == 0) data.strength = 0;
	return data; 
} 

float _CastedNormalBias[MAX_CASCADE_COUNT];
DirectionalShadowData GetDirectionalShadowData(int lightIndex, ShadowData shadowData)
{
	DirectionalShadowData data;
	data.strength = _DirectionalLightShadowData[lightIndex].x;
	data.tileIndex = _DirectionalLightShadowData[lightIndex].y * _CascadeCount + shadowData.cascadeIndex;
	data.normalBias = _DirectionalLightShadowData[lightIndex].z;
	//data.shadowMaskChannel = _DirectionalLightShadowData[lightIndex].w;
	return data;
}


float MixBakedAndRealtimeShadows(ShadowData shadowData, DirectionalShadowData directional, float shadow) 
{
	//float baked = GetBakedShadow(global.shadowMask, shadowMaskChannel);
	//if (global.shadowMask.always) {
	//	shadow = lerp(1.0, shadow, global.strength);
	//	shadow = min(baked, shadow);
	//	return lerp(1.0, shadow, strength);
	//}
	//if (global.shadowMask.distance) {
	//	shadow = lerp(baked, shadow, global.strength);
	//	return lerp(1.0, shadow, strength);
	//}
	return lerp(1.0, shadow, shadowData.strength * directional.strength);
}

float4 _ShadowPCFData;
//biasPosition:用normalbias修正后的NDC坐标
//通过biasPosition采样阴影图中真正的阴影
//使用PCF(Pencentage close Filtering)修正 Aliasing
float GetFilterShadow(float3 biasPosition)
{
	float shadow =	0;
	float4 size = _ShadowAtlasSize.yyxx;
#if defined(_DIRECTIONAL_PCF3) || defined(_DIRECTIONAL_PCF5) || defined(_DIRECTIONAL_PCF7)
	
	int filterSamples = 0;
	#if defined(_DIRECTIONAL_PCF3)
		filterSamples = 4;
		real weights[4];
		real2 positions[4];
		SampleShadow_ComputeSamples_Tent_3x3(size, biasPosition.xy, weights, positions);
	#elif defined(_DIRECTIONAL_PCF5)
		filterSamples = 9;
		real weights[9];
		real2 positions[9];
		SampleShadow_ComputeSamples_Tent_5x5(size, biasPosition.xy, weights, positions);
	#elif defined(_DIRECTIONAL_PCF7)
		filterSamples = 16;
		real weights[16];
		real2 positions[16];
		SampleShadow_ComputeSamples_Tent_7x7(size, biasPosition.xy, weights, positions);
	#endif
	for (int i = 0; i < filterSamples; i++) 
	{
		shadow += weights[i] * SAMPLE_TEXTURE2D_SHADOW(_DirectionalShadowAtlas, SHADOW_SAMPLER, float3(positions[i].xy, biasPosition.z));
	}
#else
	shadow = SAMPLE_TEXTURE2D_SHADOW(_DirectionalShadowAtlas, SHADOW_SAMPLER, biasPosition);
#endif
	return shadow;
}


float GetDirectionalShadowAttenuation(DirectionalShadowData directional, ShadowData shadowData, SurfaceData surface, float3 lightDir)
{

	if (directional.strength * shadowData.strength <= 0.0) {
		//return GetBakedShadow(
		//	global.shadowMask, directional.shadowMaskChannel,
		//	abs(directional.strength)
		//);
		return 1;
	}
	//1.返回使用normalbias修正acne后的采样点//https://www.zhihu.com/question/49090321
	float3 normalBias = surface.normal * directional.normalBias * _CastedNormalBias[shadowData.cascadeIndex];
	float3 biasPosition = mul(_DirectionalShadowMatrices[directional.tileIndex], float4(surface.position + normalBias, 1.0)).xyz;
	float shadow = GetFilterShadow(biasPosition);
	if(shadowData.cascadeBlend < 1)
	{
		//采样下一个级联的阴影跟当前阴影做混合
		normalBias = surface.normal * (directional.normalBias * _CastedNormalBias[shadowData.cascadeIndex + 1]);
		biasPosition = mul(_DirectionalShadowMatrices[directional.tileIndex + 1], float4(surface.position + normalBias, 1.0)).xyz;
		shadow = lerp(GetFilterShadow(biasPosition), shadow, shadowData.cascadeBlend);
	}
	return MixBakedAndRealtimeShadows(shadowData, directional, shadow);
}

Light GetDirectionalLight(int index, SurfaceData surface, ShadowData shadowData) {
	Light light;
	light.color = _DirectionalLightColors[index].rgb;
	light.direction = _DirectionalLightDirections[index].xyz;
	DirectionalShadowData dirShadowData = GetDirectionalShadowData(index, shadowData);
	light.shadowAttenuation = GetDirectionalShadowAttenuation(dirShadowData, shadowData, surface, light.direction);
	return light;
}

GIData GetGIDataByIndex(SurfaceData o, int lightIndex)
{
	GIData gi;
	ShadowData data = GetShadowData(o);
	gi.light = GetDirectionalLight(lightIndex, o, data);
	return gi;
}


#endif

