Shader "Cartoon/FXMain" {
	Properties {
		[Enum(Alpha Blend,10,Addtive,1)] _DestBlend("混合模式", Float) = 1
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("剔除", Float) = 0
		[Enum(Off,0,On,1)] _ZWrite("深度写入", Float) = 0
		_Zoffset("深度偏移",Range(-0.5,0.5)) = 0
		[Space]
		[Space]
		[MaterialToggle] _Main ("基础效果开关", Float ) = 1
		[MaterialToggle] _ContrastToggle("幂开关",Float) = 0
		_MainContrast("幂", float) = 1
		[Space]
		[Space]
		[Toggle(_FRESNEL_ON)] _Fresnel ("Fresnel开关", Float ) = 0
		_Exp ("Exp", Float ) = 1
		[MaterialToggle] _CVS_W("自定义W=Fresnel范围", Float) = 0
		[HDR]_FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
		_FresInvert ("Fresnel 反相", Range(0, 1)) = 0
		[Toggle(_FRESNELCHANEL_ON)] _FresnelChanel ("Fresnel作为alpha", Float ) = 0
		[Space]
		[Space]
		[MaterialToggle] _DissolveToggle ("溶解开关",Float) = 0
		[MaterialToggle] _DissolveTex ("更换Mask作为溶解纹理（默认为Main）", Float ) = 0
		[MaterialToggle] _CVS_Z("自定义Z=溶解强度", Float) = 0
		_DissolveInt ("溶解强度", Range(0, 1)) = 0
		_DissolveStep ("边缘羽化", Range(-1, 1)) = 0
		_DissolveEdge ("边缘范围", Range(-1, 1)) = 0
		[HDR]_EdgeColor ("边缘 Color", Color) = (1,1,1,1)
		[Space]
		[Space]
		_Intensity ("Main/Mask/边缘/Fresnel 强度", Vector) = (1,1,1,1)
		[HDR]_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("MainTex", 2D) = "white" {}
		[MaterialToggle] _MainAlpha("Main alpha开关", Float) = 1
		[MaterialToggle] _CVS_UV ("自定义XY=MainUV", Float ) = 0
		[MaterialToggle] _MainPolarCoord ("Main极坐标", Float ) = 0
		_MainAlphaCut ("Main Alpha cut", Range(-1,1)) = 0
		[MaterialToggle] _ColorOffset ("色调偏移开关",Float) = 0
		_Mainoffset ("Main色调偏离", Range(-0.1,0.1)) = 0
		_MainSpeed ("Main UV流动/旋转", Vector) = (0,0,0,0)
		[Space]
		[Space]
		[MaterialToggle] _Mask ("开启Mask图", Float) = 0
		_MaskTex ("Mask Tex", 2D) = "white" {}
		[MaterialToggle] _CVS_UV2 ("自定义XY=MaskUV", Float ) = 0
		[MaterialToggle] _MaskPolarCoord ("Mask极坐标", Float ) = 0
		[MaterialToggle] _AlphaMask ("Mask作为Main遮罩", Float ) = 0
		//[MaterialToggle] _NoiseMask ("Mask作为Noise遮罩", Float ) = 0
		[Toggle(_TEXTURE02_ON)] _Texture02 ("Mask作为叠加纹理",Float) = 0
		[MaterialToggle] _MainMask ("Main * Mask", Float ) = 0
		_MaskAlphaCut ("Mask Alpha cut", Range(-1,1)) = 0
		[HDR]_Tex02Color ("叠加纹理 Color", Color) = (0.5,0.5,0.5,0.5)
		_MaskSpeed ("Mask UV流动/旋转", Vector) = (0,0,0,0)
		[MaterialToggle] _MaskNoise("遮罩图紊乱", Float ) = 0
		[Space]
		[Space]
		[MaterialToggle] _Noise ("开启Noise图", Float) = 0
		_NoiseTex ("Noise Tex", 2D) = "white" {}
		[MaterialToggle] _CVS_UV3 ("自定义XY=NoiseUV", Float ) = 0
		[MaterialToggle] _NoisePolarCoord ("Noise极坐标", Float ) = 0
		[MaterialToggle] _Mask02 ("开启Noise作为全局遮罩", Float ) = 0
		_NoiseInt ("紊乱强度", Range(-1, 1)) = 0
		_NoiseSpeed ("Noise UV流动/旋转", Vector) = (0,0,0,0)
		[Space]
		[Space]
		/*[MaterialToggle] _VertexOffset ("顶点偏移开关", Float) = 0
		_VertexOffsetInt ("顶点偏移", Range(-1,1)) = 0
		[MaterialToggle] _CVS_V("自定义W=顶点偏移", Float) = 0
		[HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		*/
		[Space]
		[Space]
		_StencilComp("Stencil Comparison", Float) = 8.000000
		_Stencil("Stencil ID", Float) = 0.000000
		_StencilOp("Stencil Operation", Float) = 0.000000
		_StencilWriteMask("Stencil Write Mask", Float) = 255.000000
		_StencilReadMask("Stencil Read Mask", Float) = 255.000000
	}
	SubShader {
		Tags {
			"IgnoreProjector"="True"
			"Queue"="Transparent"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
		}
		LOD 100
		Pass {
			Name "FORWARD"
			Tags {
				"LightMode"="UniversalForward"
			}
			Blend SrcAlpha[_DestBlend]
			Cull[_Cull]
			ZWrite[_ZWrite]
			Offset[_Zoffset],0

			Stencil
			{
				Ref[_Stencil]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
				Comp[_StencilComp]
				Pass[_StencilOp]
			}
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma multi_compile_instancing
			#pragma target 3.0
			#pragma multi_compile _ _FRESNEL_ON
			#pragma multi_compile _ _COLOROFFSET_ON
			#pragma multi_compile _ _CONTRASTTOGGLE_ON
			#pragma multi_compile _ _MAIN_ON
			#pragma multi_compile _ _NOISE_ON
			#pragma multi_compile _ _MASK_ON
			#pragma multi_compile _ _DISSOLVETOGGLE_ON
			//#pragma multi_compile _ _VERTEXOFFSET_ON
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			CBUFFER_START(UnityPerMaterial)

			TEXTURE2D(_NoiseTex); SAMPLER(sampler_NoiseTex); half4 _NoiseTex_ST;
			half _NoisePolarCoord;
			half _DissolveInt;
			half _CVS_Z;
			half _DissolveStep;
			half _DissolveEdge;
			half _Mask02;
			half4 _TintColor;
			half4 _Intensity;
			half4 _EdgeColor;
			TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); half4 _MainTex_ST;
			half _MainPolarCoord;
			half _AlphaMask;
			half _MainAlphaCut;
			half4 _MainSpeed;
			half _Mainoffset;
			half _CVS_UV;
			half _DissolveTex;
			TEXTURE2D(_MaskTex); SAMPLER(sampler_MaskTex); half4 _MaskTex_ST;
			half _MaskPolarCoord;
			half4 _MaskSpeed;
			half _MaskAlphaCut;
			half _Texture02;
			half _MainMask;
			half _CVS_UV2;
			half4 _Tex02Color;
			//half _NoiseMask;
			half _NoiseInt;
			half4 _NoiseSpeed;
			half _Exp;
			half _CVS_W;
			half _CVS_V;
			half4 _FresnelColor;
			half _FresInvert;
			half _Main;
			half _MainAlpha;
			half _MainContrast;
			half _FresnelChanel;
			//half _VertexOffsetInt;
			half _MaskNoise;
			half _CVS_UV3;
			CBUFFER_END

			struct VertexInput {
				half4 vertex : POSITION;
				half3 normal : NORMAL;
				half2 texcoord0 : TEXCOORD0;
				half4 texcoord1 : TEXCOORD1;
				half4 vertexColor : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID

			};
			struct VertexOutput {
				half4 pos : SV_POSITION;
				half2 uv0 : TEXCOORD0;
				half4 uv1 : TEXCOORD1;
				half4 posWorld : TEXCOORD2;
				half4 vertexColor : COLOR;
				half3 normalDir : TEXCOORD3;
				float fogCoord : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			VertexOutput vert (VertexInput v) {
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v,o);
				o.uv0 = v.texcoord0;
				o.uv1 = v.texcoord1;
				o.vertexColor = v.vertexColor;
				o.normalDir = TransformObjectToWorldNormal(v.normal);
				o.fogCoord = ComputeFogFactor(o.pos.z);
				//顶点偏移
				/*#if defined(_VERTEXOFFSET_ON) && defined(_NOISE_ON)
				half2 VOPolarUVRamp = o.uv0 * 2.0 +- 1.0 ;
				half2 VOPolarCoord = half2(((atan2(VOPolarUVRamp.r,VOPolarUVRamp.g)/UNITY_TWO_PI)+0.5),length(VOPolarUVRamp));
				half2 VOSpeed = _Time.g * _NoiseSpeed.xy;
				half4 _VO_var = tex2Dlod(_NoiseTex,half4(TRANSFORM_TEX((VOPolarCoord + VOSpeed), _NoiseTex),0.0,0));
				v.vertex.xyz += (_VO_var.rgb * v.normal * (lerp(_VertexOffsetInt,o.uv1.w,_CVS_V)));
			#endif
			*/

			o.posWorld = mul(unity_ObjectToWorld, v.vertex);
			o.pos = TransformObjectToHClip(v.vertex.xyz);
			o.fogCoord = ComputeFogFactor(o.pos.z);

			return o;
		}
		half4 frag(VertexOutput i, half facing : VFACE) : COLOR {
			half isFrontFace = ( facing >= 0 ? 1 : 0 );
			half faceSign = ( facing >= 0 ? 1 : -1 );
			i.normalDir = normalize(i.normalDir);
			i.normalDir *= faceSign;
			half3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
			half3 normalDirection = i.normalDir;
			half3 viewReflectDirection = reflect( -viewDirection, normalDirection );
			////// Lighting:
			////// Emissive:


			//极坐标UV
			#if defined(_MAIN_ON) || defined(_MASK_ON) || defined(_NOISE_ON)
				float2 PolarUVRamp = i.uv0 * 2.0 +- 1.0 ;
				float2 PolarCoord = float2(((atan2(PolarUVRamp.r,PolarUVRamp.g)/TWO_PI)+0.5),length(PolarUVRamp));
			#endif


			//溶解边缘平滑
			#if defined(_DISSOLVETOGGLE_ON)
				half SmoothStepMax = (_DissolveStep*0.5+0.5);
				half SmoothStepMin = (1.0 - SmoothStepMax);
			#endif


			//噪波图
			#if defined(_NOISE_ON)
				float2 NoiseUVPolar = _NoisePolarCoord == 1 ? PolarCoord :i.uv0 ;
				float2 NoiseUV = lerp(half2(_Time.g * _NoiseSpeed.xy), i.uv1.xy, _CVS_UV3 ) + NoiseUVPolar;
				half4 _NoiseTex_var = SAMPLE_TEXTURE2D(_NoiseTex,sampler_NoiseTex,TRANSFORM_TEX(NoiseUV, _NoiseTex));
				half2 NoiseInt = (_NoiseTex_var.rg * _NoiseInt);
			#endif


			//遮罩图
			#if defined(_MASK_ON)
				float2 MaskUVPolar = _MaskPolarCoord == 1 ? PolarCoord :i.uv0 ;
				float2 MaskUV = lerp( half2(_Time.g * _MaskSpeed.xy), i.uv1.xy, _CVS_UV2 ) + MaskUVPolar;

				//开启遮罩图紊乱
				#if defined(_NOISE_ON)
					MaskUV = _MaskNoise ==1 ? (MaskUV + NoiseInt) : MaskUV ;
				#endif

				half4 _MaskTex_var = SAMPLE_TEXTURE2D(_MaskTex,sampler_MaskTex,TRANSFORM_TEX(MaskUV , _MaskTex));
				half4 MaskColor = half4(_MaskTex_var.rgb,saturate(_MaskTex_var.a - _MaskAlphaCut)) * _Intensity.y * _Tex02Color * i.vertexColor.a;
			#endif



			//主贴图
			#if defined(_MAIN_ON)
				float2 MainUVPolar = _MainPolarCoord == 1 ? PolarCoord :i.uv0 ;
				float2 MainUV = lerp(half2(_Time.g * _MainSpeed.xy), i.uv1.xy, _CVS_UV ) + MainUVPolar;
				half4 _MainTex_var = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,TRANSFORM_TEX(MainUV , _MainTex));
				half4 MainColor = _Intensity.x * _TintColor * i.vertexColor;

				//开启遮罩图
				#if defined(_MASK_ON)
					half MainMask = _AlphaMask == 1 ? MaskColor.a :1 ;
					MainColor *= MainMask;
				#endif

				//开启噪波图
				#if defined(_NOISE_ON)
					MainUV += NoiseInt;
					_MainTex_var = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,TRANSFORM_TEX(MainUV , _MainTex));
				#endif

				//主贴图alpha通道开关
				_MainTex_var.a = _MainAlpha == 1 ? _MainTex_var.a : 1 ;

				//主贴图色调偏离
				#if defined(_COLOROFFSET_ON)
					half4 Mainoffset_R = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,TRANSFORM_TEX((MainUV + _Mainoffset.xx), _MainTex));
					half4 Mainoffset_B = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,TRANSFORM_TEX((MainUV - _Mainoffset.xx), _MainTex));
					_MainTex_var = half4(Mainoffset_R.r, _MainTex_var.g, Mainoffset_B.b, _MainTex_var.a);
				#endif

				half4 finalMain = half4(_MainTex_var.rgb * MainColor.rgb,saturate((_MainTex_var.a - _MainAlphaCut) * MainColor.a));

				//遮罩==>纹理2
				#if defined(_MASK_ON)
					half4 Tex02 = _Texture02 == 1 ? MaskColor : 0 ;
					half4 AddTex = finalMain + Tex02 ;
					half Tex02Alpha = _MainMask == 1 ? finalMain.a : AddTex.a ;
					finalMain = half4(AddTex.rgb,saturate(Tex02Alpha));
				#endif

				//噪波==>全局遮罩
				#if defined(_NOISE_ON)
					half Mask02 = _Mask02 == 1 ? _NoiseTex_var.a : 1;
					finalMain *= Mask02;
				#endif

				//幂开关
				#if defined(_CONTRASTTOGGLE_ON)
					finalMain = pow(finalMain,_MainContrast);
				#endif

			#endif


			//菲涅尔
			#if defined(_FRESNEL_ON)
				half FresnelAlpha = max(0, dot(normalDirection, viewDirection));
				FresnelAlpha = lerp((1 - FresnelAlpha), FresnelAlpha, floor(_FresInvert));
				FresnelAlpha = pow(FresnelAlpha,lerp(_Exp,i.uv1.a,_CVS_W));
				half4 finalFres = i.vertexColor * _FresnelColor * FresnelAlpha * _Intensity.w;

				//遮罩==>菲涅尔
				#if defined(_MASK_ON)
					half FresMask = _AlphaMask == 1 ? MaskColor.a :1 ;
					finalFres *= FresMask;
				#endif

			#endif


			#if defined(_DISSOLVETOGGLE_ON)
				half DissolveTex = 0;
				#if defined(_MAIN_ON) && !defined(_MASK_ON)
					DissolveTex = _MainTex_var.a ;
				#endif

				#if defined(_MAIN_ON) && defined(_MASK_ON)
					//half MainDissolveTex = _DissolveTex == 1 ? _NoiseTex_var.a : _MainTex_var.a ;
					DissolveTex = _DissolveTex == 1 ? _MaskTex_var.a : _MainTex_var.a ;
				#endif

				#if !defined(_MAIN_ON) && defined(_MASK_ON)
					DissolveTex = _MaskTex_var.a ;
				#endif


				half DisInt = DissolveTex + ((lerp(_DissolveInt,i.uv1.b,_CVS_Z))*-1.1+0.55);
				half Dissolve = smoothstep(SmoothStepMin, SmoothStepMax, saturate(DisInt) );
				half DissolveRange = smoothstep(SmoothStepMin, SmoothStepMax, saturate((DisInt +(_DissolveEdge*0.2+0.0))));
				half DissolveEdge = (DissolveRange-Dissolve);
				half3 EdgeColor = _EdgeColor * DissolveEdge * _Intensity.z;
				half EdgeAlpha = DissolveEdge * i.vertexColor.a * _Intensity.z ;
				#if defined(_MAIN_ON)
					finalMain = half4(finalMain.rgb * Dissolve + EdgeColor,saturate(finalMain.a * (Dissolve + EdgeAlpha))) ;
				#endif

				#if defined(_FRESNEL_ON)
					finalFres = half4(finalFres.rgb * Dissolve + EdgeColor,finalFres.a * (Dissolve + EdgeAlpha)) ;
				#endif
			#endif


			half4 finalEffect = 0;

			#if defined(_MAIN_ON) && !defined(_FRESNEL_ON)
				finalEffect = finalMain;
			#endif

			#if defined(_FRESNEL_ON) && !defined(_MAIN_ON)
				finalEffect = finalFres;
			#endif

			#if defined(_MAIN_ON) && (_FRESNEL_ON)
				finalMain.a = _FresnelChanel == 1 ? (finalMain.a * finalFres.a) : (finalMain.a + finalFres.a);
				finalEffect = half4(finalMain.rgb + finalFres.rgb,saturate(finalMain.a));

			#endif

			finalEffect.rgb = MixFog(finalEffect.rgb,i.fogCoord);

			return finalEffect;
		}
		ENDHLSL
	}
}
}