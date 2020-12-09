// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Unlit/GPUAnimVertex"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _AnimMap ("AnimMap", 2D) = "white" { }
        // [Toggle(_GPUAnimation)] _GPUAnimation ("Enable GPUAnimation", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Cull Off
        LOD 100

        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"


            struct appdata
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
                float4 color: COLOR;
                float4 tangent: TANGENT;
            };

            struct v2f
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex: SV_POSITION;
                float2 uv: TEXCOORD0;
                UNITY_FOG_COORDS(5)
            };

            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(float, _AnimID)
#define _AnimID_arr Props
            UNITY_DEFINE_INSTANCED_PROP(float, _Basis)
#define _Basis_arr Props
            UNITY_DEFINE_INSTANCED_PROP(float, _Speed)
#define _Speed_arr Props
            UNITY_INSTANCING_BUFFER_END(Props)

            float4 _AnimInfo[12];//十二段动画
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _AnimMap;
            float4 _AnimMap_TexelSize;

            inline float3 anim(float4 animInfo, uint vid)// animInfo:(_AnimLength, _StartFrame ,_frameCount,0)
            {
                float f = (_Time.y - UNITY_ACCESS_INSTANCED_PROP(_Basis_arr, _Basis)) * UNITY_ACCESS_INSTANCED_PROP(_Speed_arr, _Speed) / animInfo.x;
                f = fmod(f, 1.0f);
                float animMap_x = (animInfo.y + animInfo.z * f) * _AnimMap_TexelSize.x;
                float animMap_y = (vid + 0.5) * _AnimMap_TexelSize.y;

                float4 pos = tex2Dlod(_AnimMap, float4(animMap_x, animMap_y, 0, 0));
                return pos.xyz;
            }
            v2f vert(appdata v, uint vid: SV_VertexID)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                v2f o;
                v.vertex.xyz = anim(_AnimInfo[(uint)UNITY_ACCESS_INSTANCED_PROP(_AnimID_arr, _AnimID)], vid);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i): SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
            
        }
    }
}
