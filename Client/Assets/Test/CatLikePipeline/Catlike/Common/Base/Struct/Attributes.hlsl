struct Attributes {
	float3 positionOS : POSITION;
	float3 normalOS : NORMAL;
	float2 baseUV : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
	//float4  shadowCoord;
	//half    fogCoord;
	//half3   vertexLighting;
	//half3   bakedGI;
};

