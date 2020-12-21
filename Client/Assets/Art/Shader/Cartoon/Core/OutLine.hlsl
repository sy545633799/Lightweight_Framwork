#ifndef TOONOUTLINE_H
#define TOONOUTLINE_H

#include "Character.hlsl"

struct appdata {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float4 color : COLOR;//A通道控制描边强度
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct OutLineV2F
{
	float4 pos : SV_POSITION;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

OutLineV2F vert(appdata v) //to do
{
	OutLineV2F o;
	UNITY_SETUP_INSTANCE_ID(v);
	UNITY_TRANSFER_INSTANCE_ID(v, o);
	o.pos = TransformObjectToHClip(v.vertex.xyz);
	float3 viewNormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal.xyz);
	float3 ndcNormal = normalize(mul((float3x3)UNITY_MATRIX_P, viewNormal)) * o.pos.w;//将法线变换到NDC空间
	float4 nearUpperRight = mul(unity_CameraInvProjection, float4(1, 1, UNITY_NEAR_CLIP_VALUE, _ProjectionParams.y));//将近裁剪面右上角位置的顶点变换到观察空间
	float aspect = abs(nearUpperRight.y / nearUpperRight.x);//求得屏幕宽高比
	float2 scal = float2(ndcNormal.x*aspect, ndcNormal.y) * 0.01;
	scal = o.pos.z < _Outlinethreshold ? scal : 0;

	o.pos.xy += _OutlineWidth * scal;
	return o;
}

half4 frag(v2f i) : SV_Target
{
	UNITY_SETUP_INSTANCE_ID(i);
	return half4(_OutlineColor.rgb,1.0);
}
#endif