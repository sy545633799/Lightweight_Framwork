#ifndef TOONOUTLINE_H
#define TOONOUTLINE_H

#include "./Common.hlsl"

struct v2f2
{
	float4 positionCS : SV_POSITION;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

v2f2 vert(a2v v)
{
	v2f2 o;
	//UNITY_INITIALIZE_OUTPUT(v2f2, o);
	UNITY_SETUP_INSTANCE_ID(v);
	UNITY_TRANSFER_INSTANCE_ID(v, o);
//
//#if defined(_AnimSkin)
//	v.vertex = skin_pos(v);
//#elif defined(_AnimVertex)
//	v.vertex.xyz = anim_vert(_AnimInfo[(uint)UNITY_ACCESS_INSTANCED_PROP(_AnimID_arr, _AnimID)], vid);
//#endif

	o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
	float4 nearUpperRight = mul(unity_CameraInvProjection, float4(1, 1, UNITY_NEAR_CLIP_VALUE, _ProjectionParams.y));
	float aspect = abs(nearUpperRight.y / nearUpperRight.x);
	float3 viewNormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normalOS.xyz);
	float3 clipNormal = TransformWViewToHClip(viewNormal.xyz);
	float2 projectedNormal = normalize(clipNormal.xy);
	projectedNormal *= min(o.positionCS.w, _OutlineScaledMaxDistance);
	projectedNormal.x *= aspect;
	o.positionCS.xy += 0.01 * _OutlineWidth * projectedNormal.xy * saturate(1 - abs(normalize(viewNormal).z));

	return o;
}

half4 frag(v2f2 i) : SV_TARGET
{
	UNITY_SETUP_INSTANCE_ID(i);
	Light mainLight = GetMainLight();
	return half4(_OutlineColor.rgb * mainLight.color,1);
}

#endif