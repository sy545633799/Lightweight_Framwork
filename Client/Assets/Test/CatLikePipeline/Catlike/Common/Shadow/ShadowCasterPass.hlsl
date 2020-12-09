#ifndef CUSTOM_SHADOW_CASTER_PASS_INCLUDED
#define CUSTOM_SHADOW_CASTER_PASS_INCLUDED
#include "../Base/Common.hlsl"


Varyings ShadowCasterPassVertex (Attributes input) {
	Varyings output; 
	UNITY_SETUP_INSTANCE_ID(input);   
	UNITY_TRANSFER_INSTANCE_ID(input, output);
	float3 positionWS = TransformObjectToWorld(input.positionOS);
	output.positionCS = TransformWorldToHClip(positionWS);
	#if UNITY_REVERSED_Z
		output.positionCS.z = min(output.positionCS.z, output.positionCS.w * UNITY_NEAR_CLIP_VALUE);
	#else
		output.positionCS.z = max(output.positionCS.z, output.positionCS.w * UNITY_NEAR_CLIP_VALUE); 
	#endif
	output.baseUV = TransformBaseUV(input.baseUV);
	return output;
}

half4 ShadowCasterPassFragment (Varyings input) :SV_Target{
	UNITY_SETUP_INSTANCE_ID(input);
	ClipLOD(input.positionCS.xy, unity_LODFade.x); 

	float4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.baseUV);
	float4 baseColor = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseColor);
	float4 base = baseMap * baseColor;
	// #if defined(_SHADOWS_CLIP)
	float cutoff = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Cutoff);
	clip(base.a - cutoff);
	// #elif defined(_SHADOWS_DITHER) 
	// 	float dither = InterleavedGradientNoise(input.positionCS.xy, 0);
	// 	clip(base.a - dither);
	// #endif
	return 0;
}




#endif