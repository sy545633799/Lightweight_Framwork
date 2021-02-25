#ifndef COMMON_H
#define COMMON_H

//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
struct a2v
{
	float4 positionOS	: POSITION;
	float4 normalOS		: NORMAL;
	float4 tangentOS	: TANGENT;
	float2 texcoord		: TEXCOORD;
	float2 lightmapUV   : TEXCOORD1;
	
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
	float4 positionCS				: SV_POSITION;
	float3 normalWS					: NORMAL;
	float4 texcoord					: TEXCOORD0;
	float4 texcoord2				: TEXCOORD1;
	/*float4 texcoord3				: TEXCOORD2;
	*/
	
#if defined(_NORMALMAP)
	half4 tangentToWorld[3]		: TEXCOORD3;
#else
	half3 positionWS				:TEXCOORD3;
	//如果没有NormalMap，球谐光在Vertex, 反之在Fragment采样，跟默认Terrain相同，跟默认Lit不同，注意
	half3 vertexSH                  : TEXCOORD4;
#endif
	half4 fogFactorAndVertexLight   : TEXCOORD7;
#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
	float4 shadowCoord              : TEXCOORD8;
#endif
	UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO
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

half4x4 CreateTangentToWorldPerVertexFantasy(half3 normal, half3 tangent, half3 worldPos)
{
	//假设一个对象的scale设置为(-1, 1, 1)，这意味着它是镜像的.
	//在这种情况下，我们必须翻转副法线，来正确地镜像切线空间.
	//事实上，当奇数维数为负时，我们必须这样做.
	//UnityShaderVariables通过定义half4 unity_WorldTransformParams变量来帮助我们完成这个任务.
	//half sign = tangentSign * unity_WorldTransformParams.w;
	half3 binormal = cross(normal, tangent);
	return half4x4(
		half4(tangent.x, binormal.x, normal.x, worldPos.x),
		half4(tangent.y, binormal.y, normal.y, worldPos.y),
		half4(tangent.z, binormal.z, normal.z, worldPos.z),
		half4(tangent.z, binormal.z, normal.z, worldPos.z));
}

inline void CommonInitV2F(in a2v i, inout v2f o)
{
	o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
	half3 positionWS = TransformObjectToWorld(i.positionOS.xyz);
#if defined(_NORMALMAP)
	half4x4 tangentToWorld = CreateTangentToWorldPerVertexFantasy(i.normalOS, i.tangentOS, positionWS);
	o.tangentToWorld[0] = tangentToWorld[0];
	o.tangentToWorld[1] = tangentToWorld[1];
	o.tangentToWorld[2] = tangentToWorld[2];
#else
	o.normalWS = TransformObjectToWorldDir(i.normalOS);
	o.positionWS = positionWS;
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
		float4 nortex = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, i.texcoord.xy); \
		half3 normalTangent = UnpackNormalScale(nortex, _BumpScale).xyz; \
		half3 normalWS = normalize(mul(half3x3(i.tangentToWorld[0].xyz, i.tangentToWorld[1].xyz, i.tangentToWorld[2].xyz), normalTangent)); \
		half3 positionWS = half3(i.tangentToWorld[0].w, i.tangentToWorld[1].w, i.tangentToWorld[2].w); \
		half3 SH = SampleSH(normalWS.xyz); \
		float3 viewDirWS = normalize(UnityWorldSpaceViewDir(positionWS));
#else
	#define WORLD_NORMAL_POSITION_VIEWDIR(i) \
		half3 normalWS = i.normalWS; \
		half3 positionWS = i.positionWS; \
		half3 SH = i.vertexSH; \
		float3 viewDirWS = normalize(UnityWorldSpaceViewDir(positionWS));
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