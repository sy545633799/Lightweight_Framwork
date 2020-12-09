Shader "Catlike/Scene/Lit"
{
    Properties
    {
		[MainColor] _BaseColor("Color", Color) = (0.5,0.5,0.5,1)
		[MainTexture] _BaseMap("Albedo", 2D) = "white" {}
		 _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

		_Shininess("SpecularPow", Range(1,50)) = 1


		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend", Float) = 0
		[Enum(Off, 0, On, 1)] _ZWrite("Z Write", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Pass
        {
			Tags{ "LightMode" = "BaseLit" }
			
			Blend[_SrcBlend][_DstBlend]
			ZWrite[_ZWrite]
			HLSLPROGRAM 
			#pragma target 3.5
			//#pragma shader_feature _CLIPPING
			#pragma vertex CommonVertex  
			#pragma fragment LitPassFragment  
			#pragma multi_compile_instancing
			#pragma multi_compile _ _DIRECTIONAL_PCF3 _DIRECTIONAL_PCF5 _DIRECTIONAL_PCF7
			#pragma multi_compile _ _CASCADE_BLEND_SOFT _CASCADE_BLEND_DITHER

			#include "./LitPass.hlsl"    
			ENDHLSL
        }

		UsePass "Catlike/Common/Shadow/Default/ShadowCaster"
    }
}
