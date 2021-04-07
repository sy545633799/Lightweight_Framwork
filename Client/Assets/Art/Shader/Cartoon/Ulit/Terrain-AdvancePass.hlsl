#ifndef TETTRAIN_H
#define TETTRAIN_H

#define _USE_TEXCOORD2 1
#define REQUIRES_WORLD_SPACE_POS_INTERPOLATOR 1

#include "../Core/Common.hlsl"

v2f TerrainPassVertex(a2v i)
{
	v2f o = (v2f)0;
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_TRANSFER_INSTANCE_ID(i, o);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

	CommonInitV2F(i, o);
	o.texcoord.xy = TRANSFORM_TEX(i.texcoord, _Control);
#ifdef LIGHTMAP_ON
	o.texcoord.zw = i.lightmapUV.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif

	o.texcoord2.xy = TRANSFORM_TEX(i.texcoord, _Index);
	return o;
}

half4 TerrainPassFragment(v2f i) : SV_Target
{
	UNITY_SETUP_INSTANCE_ID(i);
// UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

	_MaxSubTexCount = _MaxSubTexCount - 1;
	float4 indexTex = SAMPLE_TEXTURE2D(_Index, sampler_Index, i.texcoord2.xy);
	float indexLayer1 = round(indexTex.r * _MaxSubTexCount);
	float indexLayer2 = round(indexTex.g * _MaxSubTexCount);
	float indexLayer3 = round(indexTex.b * _MaxSubTexCount);
	float indexLayer4 = round(indexTex.a * _MaxSubTexCount);

	half4 albedo = 0.0h;
	half3 normalTS = 0.0f;
	float4 blendTex = SAMPLE_TEXTURE2D(_Control, sampler_Control, i.texcoord.xy);
	float2 scaledUV = i.texcoord.xy;
	if (blendTex.r > 0)
	{
		float2 uv = scaledUV * _UV_STArray[indexLayer1].xy + _UV_STArray[indexLayer1].zw;
		albedo += blendTex.r * SAMPLE_TEXTURE2D_ARRAY(_Diffuse, sampler_Diffuse, uv, indexLayer1);
#if defined(_NORMALMAP)
		normalTS += UnpackNormalScale(SAMPLE_TEXTURE2D_ARRAY(_Normal, sampler_Normal, uv, indexLayer1), blendTex.r);
#endif
	}
	if (blendTex.g > 0)
	{
		float2 uv = scaledUV * _UV_STArray[indexLayer2].xy + _UV_STArray[indexLayer2].zw;
		albedo += blendTex.g * SAMPLE_TEXTURE2D_ARRAY(_Diffuse, sampler_Diffuse, uv, indexLayer2);
#if defined(_NORMALMAP)
		normalTS += UnpackNormalScale(SAMPLE_TEXTURE2D_ARRAY(_Normal, sampler_Normal, uv, indexLayer2), blendTex.g);
#endif
	}
	if (blendTex.b > 0)
	{
		float2 uv = scaledUV * _UV_STArray[indexLayer3].xy + _UV_STArray[indexLayer3].zw;
		albedo += blendTex.b * SAMPLE_TEXTURE2D_ARRAY(_Diffuse, sampler_Diffuse, uv, indexLayer3);
#if defined(_NORMALMAP)
		normalTS += UnpackNormalScale(SAMPLE_TEXTURE2D_ARRAY(_Normal, sampler_Normal, uv, indexLayer3), blendTex.b);
#endif
	}
	if (blendTex.a > 0)
	{
		float2 uv = scaledUV * _UV_STArray[indexLayer4].xy + _UV_STArray[indexLayer4].zw;
		albedo += blendTex.a * SAMPLE_TEXTURE2D_ARRAY(_Diffuse, sampler_Diffuse, uv, indexLayer4);
#if defined(_NORMALMAP)
		normalTS += UnpackNormalScale(SAMPLE_TEXTURE2D_ARRAY(_Normal, sampler_Normal, uv, indexLayer4), blendTex.a);
#endif
	}

#if defined(_NORMALMAP)
	normalTS = normalize(normalTS.xyz);
	float sgn = i.tangentWS.w;
	float3 bitangent = sgn * cross(i.normalWS.xyz, i.tangentWS.xyz);
	half3 normalWS = NormalizeNormalPerPixel(TransformTangentToWorld(normalTS, half3x3(i.tangentWS.xyz, bitangent.xyz, i.normalWS.xyz)));
	half3 SH = SampleSH(normalWS.xyz);
#else 
	half3 normalWS = i.normalWS;
	half3 SH = i.vertexSH;
#endif

	half3 positionWS = i.positionWS;
	float3 viewDirWS = normalize(UnityWorldSpaceViewDir(positionWS));
	InputData inputData = GetInputData(i, positionWS, normalWS, viewDirWS, SH);

#if defined(_USE_PBR)
	half smoothness =  0.5;
	half metallic = 0;
	half4 color = UniversalFragmentPBR(inputData, albedo, metallic, half3(0.0h, 0.0h, 0.0h), smoothness, 1.0, 0, 1);
#else
	half4 color = UniversalFragmentBlinnPhong(inputData, albedo.rgb, albedo * _SpecColor, _Shininess, 0, 1);
#endif


	color.rgb = MixFog(color.rgb, i.fogFactorAndVertexLight.x);

	return half4(color.rgb, 1.0h);
}
#endif