Shader "Cartoon/Shadow/Terrain"
{
	Properties
	{
		[MainTexture] _BaseMap("MainTex", 2D) = "white" {}
		_MetallicGlossMap("Metallic(R), AlphaTest(G), (B未定), Smooth(A)", 2D) = "gray" {}
	}

	SubShader
	{
		Tags{ "Queue" = "Geometry" }
		LOD 100
		Pass
		{
			Name "ShadowCaster"
			Tags{"LightMode" = "ShadowCaster"}
			ColorMask 0
			HLSLPROGRAM
			#pragma target 3.5
			#pragma shader_feature _CLIPPING
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "./TerrainInput.hlsl"
			#include "../Core/ShadowCaster.hlsl"
			ENDHLSL
		}
	}
}
