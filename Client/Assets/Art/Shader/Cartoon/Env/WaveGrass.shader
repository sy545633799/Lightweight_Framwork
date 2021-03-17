Shader "Unlit/WaveGrass"
{
    Properties
    {
	/*	[HideInInspector] _Mode("__blend", float) = 0
		[HideInInspector][Enum(Off, 0, On, 1)] _ZWrite("__zw", Float) = 1.0
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("__src", Float) = 1.0
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("__dst", Float) = 0.0
		[HideInInspector][Enum(UnityEngine.Rendering.CullMode)] _Cull("__cull", Float) = 2.0*/

		[MainColor] _BaseColor("Color", Color) = (0.2552059, 0.3584906, 0.1843182, 1)
		[MainTexture] _BaseMap("BaseMap", 2D) = "white" {}
		[HDR] _TopColor("顶部颜色", Color) = (0.1215686, 0.4671336, 0.509804, 1)
		_TopColorRange("顶部颜色范围", Range(0, 1)) = 0.2

		_BttomHeight("底部高度", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Transparent"}
        LOD 100

        Pass
        {
			Tags{"LightMode" = "UniversalForward" }
			//Blend[_SrcBlend][_DstBlend]
			//ZWrite[_ZWrite]
			//Cull[_Cull]

			HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
			
			half _WindSpeed;
			half _WindDensity;
			half _WindStrenth;
			half4 _WindScale;

			half _BttomHeight;
			CBUFFER_END

			TEXTURE2D(_GradientNoiseMap); SAMPLER(sampler_GradientNoiseMap);

            v2f vert (a2v i)
            {
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_TRANSFER_INSTANCE_ID(i, o);

				half3 positionWS = TransformObjectToWorld(i.positionOS.xyz);
				half2 uv = half2( positionWS.x / _WindScale.z + _WindScale.x * _WindSpeed * _Time.x, positionWS.z / _WindScale.w + _WindScale.y * _WindSpeed * _Time.x);
				half noise = SAMPLE_TEXTURE2D_LOD(_GradientNoiseMap, sampler_GradientNoiseMap, uv * _WindDensity, 1).r;
				half3 offset = noise * half3(-_WindScale.x, _WindStrenth * -0.1, -_WindScale.y) * max(i.positionOS.y - _BttomHeight, 0);
				o.positionCS = TransformObjectToHClip(i.positionOS.xyz + offset);

				o.texcoord.xy = TRANSFORM_TEX(i.texcoord, _BaseMap);
				
				return o;
            }

			half4 frag (v2f i) : SV_Target
            {
				half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.texcoord.xy);
				clip(albedo.a - 0.01);
//#if defined(_ALPHABLEND_ON)
				//half alpha = _BaseColor.a * albedo.a;
//#else
				half alpha = 1;
//#endif
				half3 color = lerp(_BaseColor.rgb, _TopColor.rgb, smoothstep(1 - _TopColorRange, 1, i.texcoord.y));
                return half4(color, alpha);
            }
			ENDHLSL
        }
    }
}
