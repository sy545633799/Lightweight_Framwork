Shader "Unlit/WaveGrass"
{
	Properties
	{
		[HideInInspector] _Mode("__blend", float) = 0
		[HideInInspector][Enum(Off, 0, On, 1)] _ZWrite("__zw", Float) = 1.0
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("__src", Float) = 1.0
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("__dst", Float) = 0.0
		[HideInInspector][Enum(UnityEngine.Rendering.CullMode)] _Cull("__cull", Float) = 2.0

		[MainColor] _BaseColor("Color", Color) = (0.2552059, 0.3584906, 0.1843182, 1)
		[MainTexture] _BaseMap("BaseMap", 2D) = "white" {}
		
		[Header(Color)]
		[HDR] _TopColor("顶部颜色", Color) = (0.1215686, 0.4671336, 0.509804, 1)
		_TopColorRange("顶部颜色范围", Range(0, 1)) = 0.2

		[Header(Shadow)]
		_PushRadius("人物影响范围", Range(0, 1)) = 0.5
		_PlayerStrength("人物影响强度", Range(0, 1)) = 0.5
		[Header(Shadow)]
		_ShadowFade("阴影淡出", Range(0, 1)) = 0.5
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Transparent-10"}
		LOD 100

		Pass
		{
			Tags{"LightMode" = "UniversalForward" }
			Blend[_SrcBlend][_DstBlend]
			ZWrite[_ZWrite]
			Cull[_Cull]

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ LIGHTMAP_ON

			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#include "../Core/Common.hlsl"

			CBUFFER_START(UnityPerMaterial)
			half _Cutoff;
			float4 _BaseMap_ST;
			half4 _BaseColor;
			float4 _GradientNoiseMap_ST;
			half4 _TopColor;
			half _TopColorRange;
			//wind
			half _WindSpeed;
			half _WindDensity;
			half _WindStrenth;
			half4 _WindScale;
			//player
			half3 _PlayerPos;
			half _PlayerStrength;
			half _PushRadius;
			//shadow
			half _ShadowFade;
			CBUFFER_END

			TEXTURE2D(_GradientNoiseMap); SAMPLER(sampler_GradientNoiseMap);

			v2f vert(a2v i)
			{
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_TRANSFER_INSTANCE_ID(i, o);

				half3 positionWS = mul(UNITY_MATRIX_M, i.positionOS).xyz;
				//计算风吹草动
				half2 uv = half2(positionWS.x / _WindScale.z + _WindScale.x * _WindSpeed * _Time.x, positionWS.z / _WindScale.w + _WindScale.y * _WindSpeed * _Time.x);
				half noise = SAMPLE_TEXTURE2D_LOD(_GradientNoiseMap, sampler_GradientNoiseMap, uv * _WindDensity, 1).r;
				half offsetH = i.texcoord.y * _WindStrenth;
				half3 offset = noise * half3(-_WindScale.x * offsetH, -offsetH, -_WindScale.y * offsetH);
				//计算人物的影响
				float dis = distance(_PlayerPos, positionWS);
				float pushDown = saturate((1 - dis + _PushRadius) * i.texcoord.y * _PlayerStrength);
				float3 direction = normalize(positionWS.xyz - _PlayerPos.xyz);
				direction.y *= 0.5;
				positionWS.xyz += direction * pushDown;

				positionWS.xyz += offset;
				o.positionCS = mul(UNITY_MATRIX_VP, half4(positionWS, 1));
				o.texcoord.xy = TRANSFORM_TEX(i.texcoord, _BaseMap);

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
				half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.texcoord.xy);
				clip(albedo.a - 0.01);
//#if defined(_ALPHABLEND_ON)
				//half alpha = _BaseColor.a * albedo.a;
//#else
				half alpha = 1;
//#endif
				half3 color = lerp(_BaseColor.rgb, _TopColor.rgb, smoothstep(1 - _TopColorRange, 1, i.texcoord.y));
#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				Light mainLight = GetMainLight(i.shadowCoord);
#else
				Light mainLight = GetMainLight();
#endif
				half atten = max(_ShadowFade, mainLight.distanceAttenuation * mainLight.shadowAttenuation);
				color = color * atten * mainLight.color;

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
