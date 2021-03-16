#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

CBUFFER_START(UnityPerMaterial)
//clip
half _Cutoff;
//diffuse
float4 _BaseMap_ST;
half4 _BaseColor;
//bump
float4 _BumpMap_ST;
float _BumpScale;
//spec
half4 _SpecColor;
float _Shininess;
//pbr(如果没有PBRTEX，则手动输入_Smoothness和_Metallic)
half _Smoothness;
//如果有PBRTEX, 则没有这个选项
half _Metallic;
half _OcclusionStrength;
//emission
half4 _EmissionColor;

float3 _LightDirection;
float4 _WaveParams;

CBUFFER_END