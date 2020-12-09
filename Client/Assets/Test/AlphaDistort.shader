Shader "FGQJ/Effect/AlphaDistort"
{
    Properties
    {
        _MainTex ("textures", 2D) = "white" {}
        _Intensity ("Intensity", Range(-1, 1)) = 0.3247863
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 2
    }
    SubShader
    {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 100
        Pass {
            // Tags{
            //     "LightMode"="UniversalForward"
            // }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull[_Cull]
            ZWrite Off

            HLSLPROGRAM
            // #include "UnityCG.cginc"
            // #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal n3ds wiiu
            #pragma multi_compile_instancing
            #pragma target 3.0
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"
            #pragma vertex vert
            #pragma fragment frag

            CBUFFER_START(UnityPerMaterial)
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float _Intensity;
            CBUFFER_END

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : SV_Target {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float4 _MainTex_var = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (_MainTex_var.rgb*i.vertexColor.a*_Intensity).rg;
                float4 sceneColor = SAMPLE_TEXTURE2D(_CameraOpaqueTexture,sampler_CameraOpaqueTexture,sceneUVs);
                ////// Lighting:
                float3 finalColor = 0;
                return half4(lerp(sceneColor.rgb, finalColor,0.0),1);
            }
            ENDHLSL
        }
    }
    // CustomEditor "ShaderForgeMaterialInspector"
}
