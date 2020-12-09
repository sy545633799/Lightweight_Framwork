// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Unlit/GPUAnimSkinning"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _AnimMap ("AnimMap", 2D) = "white" { }
        // [Toggle(_GPUAnimation)] _GPUAnimation ("Enable GPUAnimation", Float) = 1
        // _AnimID ("AnimID", float) = 0
        // _Basis ("Basis", float) = 0
        // _Speed ("Speed", float) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Cull Off
        LOD 100

        Pass
        {
            CGPROGRAM
            
            #pragma target 3.5            
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
                float2 uv3: TEXCOORD2;
                float2 uv4: TEXCOORD3;
                float4 color: COLOR;
                // float4 tangent: TANGENT;//暂存两根骨骼及权重信息
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex: SV_POSITION;
                float2 uv: TEXCOORD0;
                UNITY_FOG_COORDS(5)
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(float, _AnimID)
#define _AnimID_arr Props
            UNITY_DEFINE_INSTANCED_PROP(float, _Basis)
#define _Basis_arr Props
            UNITY_DEFINE_INSTANCED_PROP(float, _Speed)
#define _Speed_arr Props
            UNITY_INSTANCING_BUFFER_END(Props)

            float4 _AnimInfo[12];//十二段动画 _AnimInfo[i]=float4(_AnimLength, _StartFrame ,_frameCount,0)
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _AnimMap;
            float4 _AnimMap_TexelSize;

            float4 anim(float uv_x, float boneid, float4 vertex)
            {
                float uv_y = (boneid * 3 + 0.5) * _AnimMap_TexelSize.y;
                float4 mx0 = tex2Dlod(_AnimMap, float4(uv_x, uv_y, 0, 0));
                uv_y += _AnimMap_TexelSize.y;
                float4 mx1 = tex2Dlod(_AnimMap, float4(uv_x, uv_y, 0, 0));
                uv_y += _AnimMap_TexelSize.y;
                float4 mx2 = tex2Dlod(_AnimMap, float4(uv_x, uv_y, 0, 0));

                float4x4 matrice = float4x4(
                    mx0.x, mx0.y, mx0.z, mx0.w,
                    mx1.x, mx1.y, mx1.z, mx1.w,
                    mx2.x, mx2.y, mx2.z, mx2.w,
                    0, 0, 0, 1
                );
                return mul(matrice, vertex);
            }

            v2f vert(appdata v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                v2f o;

                int id = (int)UNITY_ACCESS_INSTANCED_PROP(_AnimID_arr, _AnimID);
                float f = (_Time.y - UNITY_ACCESS_INSTANCED_PROP(_Basis_arr, _Basis)) * UNITY_ACCESS_INSTANCED_PROP(_Speed_arr, _Speed) / _AnimInfo[id].x;
                f = fmod(f, 1.0f);
                float frame = (_AnimInfo[id].y + _AnimInfo[id].z * f) * _AnimMap_TexelSize, x;
                float boneid = v.uv3.x;
                float4 pos = anim(frame, boneid, v.vertex) * v.color.r;
                boneid = v.uv3.y;
                pos += anim(frame, boneid, v.vertex) * v.color.g;
                boneid = v.uv4.x;//可只使用两根骨骼进一步提升性能
                pos += anim(frame, boneid, v.vertex) * v.color.b;
                // boneid = v.uv4.y;
                // pos += anim(frame, boneid, v.vertex) * v.color.a;

                o.vertex = UnityObjectToClipPos(pos);
                // o.vertex = UnityObjectToClipPos(v.vertex);
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
