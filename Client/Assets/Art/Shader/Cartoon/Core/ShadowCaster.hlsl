#ifndef TOON_SHADOW_CASTER_PASS_INCLUDED
#define TOON_SHADOW_CASTER_PASS_INCLUDED

#include "./Common.hlsl"

v2f vert(a2v i)
{
	v2f o;
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_TRANSFER_INSTANCE_ID(i, o);

#if defined(_ALPHATEST_ON)
	o.texcoord.xy = TRANSFORM_TEX(i.texcoord.xy, _BaseMap);
#endif

	float3 positionWS = ApplyVertexTransform(i);
	Light MainLight = GetMainLight();
	float3 worldNormal = TransformObjectToWorldNormal(i.normalOS);
	o.positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, worldNormal, MainLight.direction));
	
	//urp管线中， opengl的近裁面为-1, 远裁面为1, UNITY_REVERSED_Z为1, 其他平台的近裁面为1, 远裁面为0, UNITY_REVERSED_Z为0
	//@catlike 用normalbias 修复阴影痤疮时，当渲染定向光的阴影投射器时，近平面尽可能地向前移动。这可以提高深度精度，但是这意味着不在摄像机视线范围内的阴影投射器可以终止在近平面的前面，这会导致它们在不应该被投射时被修剪。
	//通过在ShadowCasterPassVertex中将顶点位置固定到近平面来解决此问题，可以有效地展平位于近平面前面的阴影投射器，将它们变成粘在近平面上的花纹。我们通过获取剪辑空间Z和W坐标的最大值或定义UNITY_REVERSED_Z时的最小值来做到这一点。要将正确的符号用于W坐标，请乘以UNITY_NEAR_CLIP_VALUE
#if UNITY_REVERSED_Z
	o.positionCS.z = min(o.positionCS.z, o.positionCS.w * UNITY_NEAR_CLIP_VALUE);
#else
	o.positionCS.z = max(o.positionCS.z, o.positionCS.w * UNITY_NEAR_CLIP_VALUE);
#endif

	return o;
}

half4 frag(v2f i) :SV_Target
{
	UNITY_SETUP_INSTANCE_ID(i);
#ifdef _ALPHATEST_ON
		half alpha = SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap, i.texcoord).a;
		clip(alpha - _Cutoff);
#endif
	return 0;
}

#endif