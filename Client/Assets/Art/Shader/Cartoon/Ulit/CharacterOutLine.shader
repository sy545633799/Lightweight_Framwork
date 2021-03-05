Shader "Cartoon/Outline/Character"
{
	Properties
	{
		_OutlineColor("_OutlineColor: 描边颜色",COLOR) = (0,0.0,0.0,1)
		_OutlineWidth("_OutlineWidth: 描边宽度",Range(0,1)) = 0.2
		_Outlinethreshold("_Outlinethreshold: 描边距离系数",Range(0,1)) = 0.5
		//_OffsetFactor("_OffsetFactor",float) = -0.1
		//_OffsetUnits("_OffsetUnits",float) = 0.1
	}

	SubShader
	{
		LOD 100
		Pass 
		{
			Name "Outline"
			Tags { "LightMode" = "LightweightForward" "RenderType" = "Opaque" "Queue" = "Geometry+10" }
			Cull Front
			ZWrite On
			ZTest LEqual
			// Offset [_OffsetFactor],[_OffsetUnits]
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing


			//#pragma multi_compile _ _AnimSkin _AnimVertex

			#include "./CharacterInput.hlsl"
			#include "../Core/OutLine.hlsl" 
			ENDHLSL
		}
	}
}
