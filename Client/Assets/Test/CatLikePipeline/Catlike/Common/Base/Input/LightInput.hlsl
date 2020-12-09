#ifndef CUSTOM_LIGHT_INCLUDED
#define CUSTOM_LIGHT_INCLUDED

#define MAX_DIRECTIONAL_LIGHT_COUNT 4
#define MAX_POINT_LIGHT_COUNT 4
#define MAX_SPOT_LIGHT_COUNT 4

CBUFFER_START(_CustomLight)
int _DirectionalLightCount;
int _PointLightCount;
int _SpotLightCount;

float4 _DirectionalLightColors[MAX_DIRECTIONAL_LIGHT_COUNT];
float4 _DirectionalLightDirections[MAX_DIRECTIONAL_LIGHT_COUNT];

float4 _PointLightPosition[MAX_POINT_LIGHT_COUNT];
float4 _PointLightColors[MAX_POINT_LIGHT_COUNT];

float4 _SpotLightPosition[MAX_SPOT_LIGHT_COUNT];
float4 _SpotLightDirections[MAX_SPOT_LIGHT_COUNT];
float4 _SpotLightColors[MAX_SPOT_LIGHT_COUNT];
  
CBUFFER_END 

#endif