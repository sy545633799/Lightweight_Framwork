Shader "Cartoon/Env/Water"
{
	Properties
	{
		_DepthGradientShallow("浅滩水颜色", Color) = (0.325, 0.807, 0.971, 0.725)
		_DepthGradientDeep("深水区颜色", Color) = (0.086, 0.407, 1, 0.749)
		_DepthMaxDistance("最大水深", Float) = 1
		_FoamColor("波纹颜色", Color) = (1,1,1,1)
		_SurfaceNoise("水面波纹", 2D) = "white" {}
		_SurfaceNoiseScroll("波纹流动", Vector) = (0.03, 0.03, 0, 0)
		_SurfaceNoiseCutoff("波纹数量阈值", Range(0, 1)) = 0.777
		_SurfaceDistortion("波纹干扰图", 2D) = "white" {}	
		_SurfaceDistortionAmount("波纹干扰数量", Range(0, 1)) = 0.27
		_FoamMaxDistance("最大波纹偏移", Float) = 0.4
		_FoamMinDistance("最小波纹偏移", Float) = 0.04		
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Transparent"}
		LOD 100

		Pass
		{
			Tags{"LightMode" = "UniversalForward" }

			//Blend SrcAlpha OneMinusSrcAlpha
            //ZWrite Off

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON

			#pragma multi_compile_fog
			#pragma multi_compile_instancing

            #define _USE_TEXCOORD2 1
			#define _USE_TEXCOORD3 1
            #define REQUIRES_WORLD_SPACE_POS_INTERPOLATOR 1
			#include "../Core/Mathf.hlsl"
			#include "../Core/Common.hlsl"

			CBUFFER_START(UnityPerMaterial)
			half _Cutoff;
			float4 _BaseMap_ST;
			half4 _BaseColor;

			float4 _GradientNoiseMap_ST;
			float4 _PerlinNoiseMap_ST;
			float4 _DistortionMap_ST;

			float4 _DepthGradientShallow;
			float4 _DepthGradientDeep;
			float4 _FoamColor;

			float _DepthMaxDistance;
			float _FoamMaxDistance;
			float _FoamMinDistance;
			float _SurfaceNoiseCutoff;
			float _SurfaceDistortionAmount;
			float2 _SurfaceNoiseScroll;
			CBUFFER_END

			TEXTURE2D(_PerlinNoiseMap); SAMPLER(sampler_PerlinNoiseMap);
			TEXTURE2D(_DistortionMap); SAMPLER(sampler_DistortionMap);

			v2f vert(a2v i)
			{
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_TRANSFER_INSTANCE_ID(i, o);

				half3 positionWS = TransformObjectToWorld(i.positionOS.xyz);
				o.positionWS = positionWS;
				o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
				o.normalWS = TransformObjectToWorldDir(i.normalOS);

				o.texcoord.xy = TRANSFORM_TEX(i.texcoord, _BaseMap);
#ifdef LIGHTMAP_ON
				o.texcoord.zw = i.lightmapUV.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif
				o.texcoord2.xy = TRANSFORM_TEX(i.texcoord, _PerlinNoiseMap);
				o.texcoord2.zw = TRANSFORM_TEX(i.texcoord, _DistortionMap);
				o.texcoord3 = ComputeScreenPos(o.positionCS);
				o.texcoord3.z = -mul(UNITY_MATRIX_MV, i.positionOS).z;
				o.fogFactorAndVertexLight.r = ComputeFogFactor(o.positionCS.z);
#if defined(_ADDITIONAL_LIGHTS_VERTEX) 
				o.fogFactorAndVertexLight.gba = VertexLighting(positionWS, TransformObjectToWorldDir(i.normalOS));
#endif

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				o.shadowCoord = TransformWorldToShadowCoord(positionWS);
#endif
				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				float4 depth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(i.texcoord3.xy / i.texcoord3.w)).r;
				float sceneZ = LinearEyeDepth(depth, _ZBufferParams);
				float depthDifference = abs(sceneZ - i.texcoord3.w);
				float4 color = _DepthGradientShallow;
				
				
				float4 distortionMap = SAMPLE_TEXTURE2D(_DistortionMap, sampler_DistortionMap, i.texcoord2.zw * 10);

				float surfaceNoiseCutoff = depthDifference * _SurfaceNoiseCutoff;
				float2 distortSample = (distortionMap.xy * 2 - 1) * _SurfaceDistortionAmount;
				float2 noiseUV = float2((i.texcoord2.x + _Time.y * _SurfaceNoiseScroll.x) + distortSample.x,
					(i.texcoord2.y + _Time.y * _SurfaceNoiseScroll.y) + distortSample.y);
				float4 noiseMap = SAMPLE_TEXTURE2D(_PerlinNoiseMap, sampler_PerlinNoiseMap, noiseUV);

				float surfaceNoise = smoothstep(surfaceNoiseCutoff - 0.01, surfaceNoiseCutoff + 0.01, noiseMap.r);
				float4 surfaceNoiseColor = _FoamColor;
				surfaceNoiseColor.a *= surfaceNoise;


			

				
#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				Light mainLight = GetMainLight(i.shadowCoord);
#else
				Light mainLight = GetMainLight();
#endif
				half atten = mainLight.distanceAttenuation * mainLight.shadowAttenuation;
				color.xyz = color.xyz * atten * mainLight.color;

#if defined(_ADDITIONAL_LIGHTS_VERTEX)
				color.xyz += i.fogFactorAndVertexLight.yzw;
#endif
				color.xyz = MixFog(color.xyz, i.fogFactorAndVertexLight.x);

				float4 finalColor = alphaBlend(surfaceNoiseColor, color);
				finalColor.a = 1;
				return finalColor;
			}

			ENDHLSL
		}
	}
}
