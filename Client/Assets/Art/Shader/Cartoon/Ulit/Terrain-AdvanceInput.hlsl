
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

CBUFFER_START(UnityPerMaterial)
half4 _BaseColor;
half4 _SpecColor;
half _Shininess;
half _Metallic;
half _Smoothness;
//给DepthOnlyPass用
half _Cutoff;
float4 _BaseMap_ST;
/*************************************************/
TEXTURE2D_ARRAY(_Diffuse);
SAMPLER(sampler_Diffuse);
TEXTURE2D_ARRAY(_Normal);
SAMPLER(sampler_Normal);
half4 _Splat0_ST;
half4 _Splat1_ST;
half4 _Splat2_ST;
half4 _Splat3_ST;
half4 _Splat4_ST;
half4 _Splat5_ST;
half4 _Splat6_ST;
half4 _Splat7_ST;
//Splash数量
half _MaxSubTexCount;
//索引图
TEXTURE2D(_Index);
SAMPLER(sampler_Index);
float4 _Index_ST;
//混合图
TEXTURE2D(_Control);
SAMPLER(sampler_Control);
float4 _Control_ST;
CBUFFER_END


uniform float4 _UV_STArray[8];