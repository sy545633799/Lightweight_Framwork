#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

CBUFFER_START(UnityPerMaterial)
half4 _SpecColor;
float _Shininess;
TEXTURE2D(_Splat0);
SAMPLER(sampler_Splat0);
float4 _Splat0_ST;
TEXTURE2D(_Splat1);
float4 _Splat1_ST;
TEXTURE2D(_Splat2);
float4 _Splat2_ST;
TEXTURE2D(_Splat3);
float4 _Splat3_ST;
TEXTURE2D(_Normal0);
SAMPLER(sampler_Normal0);
float4 _Normal0_ST;
TEXTURE2D(_Normal1);
SAMPLER(sampler_Normal1);
float4 _Normal1_ST;
TEXTURE2D(_Normal2);
SAMPLER(sampler_Normal2);
float4 _Normal2_ST;
TEXTURE2D(_Normal3);
SAMPLER(sampler_Normal3);
float4 _Normal3_ST;
TEXTURE2D(_Control);
float4 _Control_ST;
float4 _Control_TexelSize;
SAMPLER(sampler_Control);

half4 _BaseColor;
half _Metallic0;
half _Metallic1;
half _Metallic2;
half _Metallic3;
half _Smoothness0;
half _Smoothness1;
half _Smoothness2;
half _Smoothness3;
half _Hightthreshold;
half4 _EmissionColor;
//给DepthOnlyPass用
half _Cutoff;
float4 _BaseMap_ST;
CBUFFER_END