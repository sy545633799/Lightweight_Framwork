#ifndef CARTOON_H
#define CARTOON_H
#include "./Common.hlsl"

CBUFFER_START(UnityPerMaterial)
float4 _MainTex_ST;
half4 _BaseColor;
float _Cutout;
float4 _Sheet;
float _FrameRate;
CBUFFER_END

TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

v2f VERT(a2v i)
{
	v2f o;
    o.positionCS = TransformObjectToHClip(float4(LookAtCamera(i.positionOS.xyz),1));
	o.texcoord.xy = TRANSFORM_TEX(i.texcoord, _MainTex);
	return o;
}


half4 FRAG(v2f i) :SV_TARGET
{	
	float2 uv;
	uv.x = i.texcoord.x/_Sheet.x + frac(floor(_Time.y * _FrameRate) / _Sheet.x);
	uv.y = i.texcoord.y/_Sheet.y + 1 - frac(floor(_Time.y * _FrameRate /_Sheet.x) / _Sheet.y);
	return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
}

#endif