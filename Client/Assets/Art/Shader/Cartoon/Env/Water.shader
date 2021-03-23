Shader "Cartoon/Env/Water"
{
	Properties
	{
		[MainColor]_BaseColor("水颜色", Color) = (0.325, 0.807, 0.971, 0.725)
		_ColorThreshold("颜色系数", Range(0, 1)) = 0.1
		_FoamScaleAndOffset("波纹流动", Vector) = (1, 1, 0.05, 0.05)
		_WaterSpeed("WaterSpeed", float) = 0.35  //水速度

		//foam
		_DepthScale("深度影响系数", Range(0, 100)) = 1
		_FoamThreshold("波纹范围", Range(0,1)) = 0.6
		//bump
		[Normal]_BumpMap("Normalmap", 2D) = "bump" {}
		_BumpScale("BumpScale", Range(0, 1)) = 1
		_Refract("Refract", float) = 0.27       //折射（法线偏移程度可控
		_Specular("Specular", float) =1.11     //反射系数
		_Gloss("Gloss", float) = 2.42          //折射光照
		_SpecColor("SpecColor", color) = (1, 1, 1, 1)   //折射颜色（一般为白色
		

	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Transparent"}
		LOD 100

		Pass
		{
			Tags{"LightMode" = "UniversalForward" }

			Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

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

			#define _NORMALMAP 1
            #define _USE_TEXCOORD2 1
			#define _USE_TEXCOORD3 1
            #define REQUIRES_WORLD_SPACE_POS_INTERPOLATOR 1
			#include "../Core/Mathf.hlsl"
			#include "../Core/Common.hlsl"
			CBUFFER_START(UnityPerMaterial)
			half _Cutoff;
			//float4 _BaseMap_ST;
			half4 _BaseColor;
			half _ColorThreshold;
			//bump
			float4 _BumpMap_ST;
			float _BumpScale;
			//float4 _Range;
			half _WaterSpeed;
			half _WaveSpeed;
			half _Refract;
			half _Specular;
			half4 _SpecColor;
			half _Gloss;
			//边缘
			float4 _FoamScaleAndOffset;
			float _DepthScale;
			float _FoamThreshold;
			CBUFFER_END

			TEXTURE2D(_PerlinNoiseMap); SAMPLER(sampler_PerlinNoiseMap);
			TEXTURE2D(_DistortionMap); SAMPLER(sampler_DistortionMap);

			v2f vert(a2v i)
			{
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_TRANSFER_INSTANCE_ID(i, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				CommonInitV2F(i, o);
				
				o.texcoord.xy = i.texcoord;
#ifdef LIGHTMAP_ON
				o.texcoord.zw = i.lightmapUV.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif
				o.texcoord2 = ComputeScreenPos(o.positionCS);
				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				Light mainLight = GetMainLight(i.shadowCoord);
#else
				Light mainLight = GetMainLight();
#endif		
				half atten = mainLight.distanceAttenuation * mainLight.shadowAttenuation;
				//采样两次法线，交叉移动形成波光粼粼的样子
				half2 uv = i.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
				half2 uv1 = uv + float2(_WaterSpeed * _Time.x, 0);
				half2 uv2 = float2(1 - uv.y, uv.x) + float2(_WaterSpeed * _Time.x, 0);
				float4 bumpColor1 = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, uv1);
				float4 bumpColor2 = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, uv2);
				half2 offset = UnpackNormal((bumpColor1 + bumpColor2) / 2).xy * _Refract;
				bumpColor1 = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, uv1 + offset);
				bumpColor2 = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, uv2 + offset);
				half3 normalTS = UnpackNormal((bumpColor1 + bumpColor2) / 2).xyz;
				float sgn = i.tangentWS.w; 
				float3 bitangent = sgn * cross(i.normalWS.xyz, i.tangentWS.xyz);
				float3 normalWS = SafeNormalize(TransformTangentToWorld(normalTS, half3x3(i.tangentWS.xyz, bitangent.xyz, i.normalWS.xyz)));
				
				half3 positionWS = i.positionWS;
				half3 viewDirWS = SafeNormalize(UnityWorldSpaceViewDir(positionWS));
				
				//color
				float2 screenUV = i.texcoord2.xy / i.texcoord2.w;
				float depthTex = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, screenUV);
				float sceneZ = LinearEyeDepth(depthTex, _ZBufferParams);
				//float depthLerp = pow(clamp(saturate(abs(sceneZ - i.texcoord2.w) / _DepthScale), 0, 1), _FoamRange);
				float depth = clamp(saturate(abs(sceneZ - i.texcoord2.w) / _DepthScale), 0, 1);
				float2 noiseUV = i.texcoord *_FoamScaleAndOffset.xy + _Time.x * _FoamScaleAndOffset.zw;
				float noise = SAMPLE_TEXTURE2D(_PerlinNoiseMap, sampler_PerlinNoiseMap, noiseUV).r;
				float foamColor = step(noise, _FoamThreshold * (1 - depth));
				half4 waterColor = _BaseColor + foamColor;   //根据深度显示水的颜色
				
				float3 lightDir = normalize(_MainLightPosition.xyz);
				float NdotL = saturate(dot(lightDir, normalWS));
				//float3 viewDirWS = normalize(UnityWorldSpaceViewDir(i.worldPos));
				half3 halfDir = normalize(lightDir + viewDirWS);
				float HdotA = max(0, dot(halfDir, normalWS));
				float spec = saturate(pow(HdotA, _Specular * 128) * _Gloss);

				half3 color = (waterColor * mainLight.color * step(_ColorThreshold, NdotL) + _SpecColor.rgb * spec * mainLight.color) * atten;
				half alpha = depth;

#if defined(_ADDITIONAL_LIGHTS_VERTEX)
				color += i.fogFactorAndVertexLight.yzw;
#endif
				color = MixFog(color, i.fogFactorAndVertexLight.x);

				return half4(color, alpha);
			}

			ENDHLSL
		}
	}
}
