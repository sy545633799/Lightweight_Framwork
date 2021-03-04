// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.35 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge

Shader "Cartoon/Effect/Freezing" {
    Properties {
        _YUAN ("YUAN", 2D) = "white" {}
        [HDR]_TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _TUQI ("TUQI", 2D) = "white" {}
        _U_Speed_YUANTU ("U_Speed_YUANTU", Float ) = 0
        _V_Speed_YUANTU ("V_Speed_YUANTU", Float ) = 0.3
        _RAOLUAN ("RAOLUAN", 2D) = "white" {}
        _U_Speed_RAOLUAN ("U_Speed_RAOLUAN", Float ) = 0
        _V_Speed_RAOLUAN ("V_Speed_RAOLUAN", Float ) = 0.9
        _MOHU ("MOHU", Float ) = 0.1
        _TU_QAINGDU ("TU_QAINGDU", Float ) = 0.1
        _SECAI ("SECAI", 2D) = "white" {}
        _BAILANG ("BAILANG", 2D) = "white" {}
        _BAI_v ("BAI_v", Float ) = 0
        _BAI_u ("BAI_u", Float ) = 0
        _SECAI_copy ("SECAI_copy", 2D) = "white" {}
		_BumpMap("NormalTex", 2D) = "bump" {}
        _BumpScale ("NormalStrength", Float ) = 1
        _DistortStrength ("DistortStrength", Float ) = 5
        _Fresnelwidth ("Fresnelwidth", Float ) = 1
        [HDR]_FresnelColor ("FresnelColor", Color) = (0.5,0.5,0.5,1)
        _Direction ("Direction", Vector) = (0,0,0,0)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
			Tags{"LightMode" = "UniversalForward"}
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            Stencil {
                Ref 128
            }
			HLSLPROGRAM

			#define REQUIRES_WORLD_SPACE_POS_INTERPOLATOR 1
			#define _USE_VERTEX_COLOR 1
			#define _USE_OUTPUT_COLOR 1
			#define _USE_TEXCOORD2 1
			#define _NORMALMAP 1
			#include "../Core/Common.hlsl"
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform float4 _TimeEditor;
            uniform sampler2D _YUAN; uniform float4 _YUAN_ST;
            uniform float4 _TintColor;
            uniform sampler2D _TUQI; uniform float4 _TUQI_ST;
            uniform float _U_Speed_YUANTU;
            uniform float _V_Speed_YUANTU;
            uniform sampler2D _RAOLUAN; uniform float4 _RAOLUAN_ST;
            uniform float _U_Speed_RAOLUAN;
            uniform float _V_Speed_RAOLUAN;
            uniform float _MOHU;
            uniform float _TU_QAINGDU;
            uniform sampler2D _SECAI; uniform float4 _SECAI_ST;
            uniform sampler2D _BAILANG; uniform float4 _BAILANG_ST;
            uniform float _BAI_u;
            uniform float _BAI_v;
            uniform sampler2D _SECAI_copy; uniform float4 _SECAI_copy_ST;
            //uniform sampler2D _NormalTex; uniform float4 _NormalTex_ST;
            uniform float _BumpScale;
            uniform float _DistortStrength;
            uniform float _Fresnelwidth;
            uniform float4 _FresnelColor;
            uniform float4 _Direction;
            
			v2f vert(a2v i)
			{
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_TRANSFER_INSTANCE_ID(i, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float4 _TUQI_var = tex2Dlod(_TUQI, float4(TRANSFORM_TEX(o.texcoord, _TUQI), 0.0, _MOHU));
				i.positionOS.xyz += (((_TUQI_var.b*_TUQI_var.a)*_TU_QAINGDU)*_Direction.rgb);

				CommonInitV2F(i, o);
				o.color = i.color;
				o.texcoord.xy = i.texcoord;
				o.texcoord2 = o.positionCS;

				return o;
			}

            float4 frag(v2f i, float facing : VFACE) : COLOR {

				

                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );

                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif

				i.texcoord2 = float4(i.texcoord2.xy / i.texcoord2.w, 0, 0);
				i.texcoord2.y *= _ProjectionParams.x;
#if defined(_NORMALMAP)
				i.normalWS *= faceSign;
#endif
				WORLD_NORMAL_POSITION_VIEWDIR(i);
				half3 positionWS = i.positionWS;
				float3 viewDirWS = SafeNormalize(UnityWorldSpaceViewDir(positionWS));

                float3 viewReflectDirection = reflect( -viewDirWS, normalWS);
                float4 node_7633 = _Time + _TimeEditor;
                float2 node_8301 = (((i.texcoord.xy +(node_7633.g*_U_Speed_RAOLUAN)*float2(1,0))+(i.texcoord.xy +(node_7633.g*_V_Speed_RAOLUAN)*float2(0,1)))*0.5);
                float4 _RAOLUAN_var = tex2D(_RAOLUAN,TRANSFORM_TEX(node_8301, _RAOLUAN));
                float4 node_2740 = _Time + _TimeEditor;
                float2 node_3691 = ((((i.texcoord.xy +(node_2740.g*_U_Speed_YUANTU)*float2(1,0))+(i.texcoord.xy +(node_2740.g*_V_Speed_YUANTU)*float2(0,1)))*0.5)+_RAOLUAN_var.b*float2(1,1));
                float4 _YUAN_var = tex2D(_YUAN,TRANSFORM_TEX(node_3691, _YUAN));
                float node_798 = (_YUAN_var.a*i.color.a*_TintColor.a);
                float2 sceneUVs = float2(1,grabSign)*i.texcoord2.xy*0.5+0.5 + (float2(_YUAN_var.r,_YUAN_var.g)*i.color.a*_TintColor.a*node_798*_DistortStrength);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
////// Emissive:
                float4 node_9475 = _Time + _TimeEditor;
                float2 node_817 = (((i.texcoord.xy +(node_9475.g*_BAI_u)*float2(1,0))+(i.texcoord.xy +(node_9475.g*_BAI_v)*float2(0,1)))*0.5);
                float4 _BAILANG_var = tex2D(_BAILANG,TRANSFORM_TEX(node_817, _BAILANG));
                float4 _SECAI_copy_var = tex2D(_SECAI_copy,TRANSFORM_TEX(i.texcoord.xy, _SECAI_copy));
                float4 _SECAI_var = tex2D(_SECAI,TRANSFORM_TEX(node_817, _SECAI));
                float3 emissive = (((1.0-max(0,dot(normalWS, viewDirWS)))*_Fresnelwidth*_FresnelColor.rgb)+saturate((_BAILANG_var.b+((_SECAI_copy_var.rgb*(_SECAI_var.rgb*_YUAN_var.rgb)*3.0)*i.color.rgb*_TintColor.rgb*2.0))));
                float3 finalColor = emissive;
				half4 finalRGBA = half4(lerp(sceneColor.rgb, finalColor,node_798),0.2);

				finalRGBA.rgb = MixFog(finalRGBA.rgb, i.fogFactorAndVertexLight.x);
                return finalRGBA;
            }
			ENDHLSL
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
