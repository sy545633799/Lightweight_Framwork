#ifndef COMMON_H
#define COMMON_H

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
struct a2v
{
	float4 positionOS	: POSITION;
	float4 normalOS		: NORMAL;
	float2 texcoord		: TEXCOORD;
#if defined(_NORMALMAP)
	float4 tangentOS	: TANGENT;
#endif
#ifdef LIGHTMAP_ON
	float2 lightmapUV   : TEXCOORD1;
#endif
#if defined(_USE_VERTEX_COLOR)
	half4 color : COLOR;
#endif
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
	float4 positionCS				: SV_POSITION;
	half3 normalWS					: NORMAL;
#if defined(_USE_OUTPUT_COLOR)
	half4 color				: COLOR;
#endif
	float4 texcoord					: TEXCOORD0;

#if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
	half3 positionWS				: TEXCOORD1;
#endif

#if defined(_NORMALMAP)
	half4 tangentWS		: TEXCOORD2;
#else
	//如果没有NormalMap，球谐光在Vertex, 反之在Fragment采样，跟默认Terrain相同，跟默认Lit不同，注意
	half3 vertexSH                  : TEXCOORD2;
#endif

	half4 fogFactorAndVertexLight   : TEXCOORD3;
#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
	float4 shadowCoord              : TEXCOORD4;
#endif

#if defined(_USE_TEXCOORD2)
	float4 texcoord2				: TEXCOORD5;
#endif

#if defined(_USE_TEXCOORD3)
	float4 texcoord3				: TEXCOORD6;
#endif

#if defined(_USE_TEXCOORD4)
	float4 texcoord4				: TEXCOORD7;
#endif

#if defined(_USE_TEXCOORD5)
	float4 texcoord5				: TEXCOORD8;
#endif

	UNITY_VERTEX_INPUT_INSTANCE_ID
};


inline float3 UnityWorldSpaceViewDir(float3 worldPos) {
	return _WorldSpaceCameraPos - worldPos;
}

inline float3 UnityWorldSpaceLightDir(float3 worldPos) {
	return _MainLightPosition.xyz;
}

inline float3 ObjSpaceLightDir(Light l) {
	return TransformWorldToObjectDir(l.direction);
}

inline float3 ObjSpaceViewDir(in float4 vertex)
{
	float3 objSpaceCameraPos = TransformWorldToObject(_WorldSpaceCameraPos.xyz).xyz;
	return objSpaceCameraPos - vertex.xyz;
}


inline half3 LookAtCamera(half3 positionOS)
{
	// //1.视口空间转剪裁空间
	// //1) 如果乘以UNITY_MATRIX_M矩阵, 则面片必须正向放置, 好处是旋转90度后可以隐藏
	// float3 offset = mul((float3x3)UNITY_MATRIX_M,i.positionOS.xyz);//模型空间内的旋转缩放
	// //2) 模型空间坐标
	// float3 offset = i.positionOS.xyz;
	// float4 viewPos = mul(UNITY_MATRIX_MV,float4(0,0,0,1)) + float4(offset.x,offset.y,0.0,0.0);
    // o.positionCS = mul(UNITY_MATRIX_P, viewPos);//z轴朝向相机

	half3 cameraPos = TransformWorldToObject(_WorldSpaceCameraPos);
	//z轴指向相机
    half3 normalDir = normalize(cameraPos);
    half3 upDir = abs(normalDir.y) > 0.999f ? half3(0, 0, 1) : half3(0, 1, 0);
    half3 rightDir = normalize(cross(normalDir, upDir));
    upDir = normalize(cross(rightDir, normalDir));
    //用旋转矩阵对顶点进行偏移(实际上是按行排列的旋转矩阵左乘原始顶点 原始顶点->视口面向相机的顶点)
    float3 localPos = rightDir * positionOS.x + upDir * positionOS.y + normalDir * positionOS.z;
	
	return localPos;
}

inline void CommonInitV2F(in a2v i, inout v2f o)
{
	o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
	half3 positionWS = TransformObjectToWorld(i.positionOS.xyz);
#if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
	o.positionWS = positionWS;
#endif

#if defined(_NORMALMAP)
	VertexNormalInputs normalInput = GetVertexNormalInputs(i.normalOS, i.tangentOS);
	o.normalWS = normalInput.normalWS;
	real sign = i.tangentOS.w * GetOddNegativeScale();
	o.tangentWS = half4(normalInput.tangentWS.xyz, sign);
#else
	o.normalWS = TransformObjectToWorldDir(i.normalOS);
	o.vertexSH = SampleSH(o.normalWS.xyz);
#endif

	o.fogFactorAndVertexLight.r = ComputeFogFactor(o.positionCS.z);
#if defined(_ADDITIONAL_LIGHTS_VERTEX) 
	o.fogFactorAndVertexLight.gba = VertexLighting(positionWS, TransformObjectToWorldDir(i.normalOS));
#endif

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
	o.shadowCoord = TransformWorldToShadowCoord(positionWS);
#endif
}

#if defined(_NORMALMAP)
	#define WORLD_NORMAL_POSITION_VIEWDIR(i) \
		half3 normalTS = SampleNormal(i.texcoord.xy, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale); \
		float sgn = i.tangentWS.w; \
		float3 bitangent = sgn * cross(i.normalWS.xyz, i.tangentWS.xyz); \
		float3 normalWS = NormalizeNormalPerPixel(TransformTangentToWorld(normalTS, half3x3(i.tangentWS.xyz, bitangent.xyz, i.normalWS.xyz))); \
		half3 SH = SampleSH(normalWS.xyz);
#else
	#define WORLD_NORMAL_POSITION_VIEWDIR(i) \
		half3 normalWS = NormalizeNormalPerPixel(i.normalWS); \
		half3 SH = i.vertexSH;
#endif

InputData GetInputData(v2f i, half3 positionWS, half3 normalWS, half3 viewDirWS, float3 vertexSH)
{
	InputData inputData = (InputData)0;
#if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
	inputData.positionWS = positionWS;
#endif
	inputData.normalWS = normalWS;
	inputData.viewDirectionWS = SafeNormalize(viewDirWS);;

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
	inputData.shadowCoord = i.shadowCoord;
#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
	inputData.shadowCoord = TransformWorldToShadowCoord(positionWS);
#else
	inputData.shadowCoord = float4(0, 0, 0, 0);
#endif
	inputData.fogCoord = i.fogFactorAndVertexLight.x;
	inputData.vertexLighting = i.fogFactorAndVertexLight.yzw;
	inputData.bakedGI = SAMPLE_GI(i.texcoord.zw, vertexSH, normalWS);
	return inputData;
}


#endif