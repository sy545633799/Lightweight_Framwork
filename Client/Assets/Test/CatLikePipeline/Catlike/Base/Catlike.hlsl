#ifndef CUSTOM_FANTASY_INCLUDED
#define CUSTOM_FANTASY_INCLUDED

#include "../Common/Base/Common.hlsl"
#include "./Core/Input.hlsl"
#include "./Core/GI.hlsl"
#include "./Core/Lighting.hlsl"

Varyings CommonVertex(Attributes input) {
	Varyings output;
	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_TRANSFER_INSTANCE_ID(input, output);
	output.positionWS = TransformObjectToWorld(input.positionOS);
	output.positionCS = TransformWorldToHClip(output.positionWS);
	output.normalWS = TransformObjectToWorldNormal(input.normalOS);
	output.baseUV = TransformBaseUV(input.baseUV);
	return output;
}
 
float4 CommonFragment(Varyings input) : SV_TARGET{
	return (1,1,1,1);
}

SurfaceData InitializeLambertSurfaceData(Varyings input)
{
	SurfaceData outSurfaceData;
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(outSurfaceData);
	outSurfaceData.position = input.positionWS; 
	outSurfaceData.normal = input.normalWS;
	
	return outSurfaceData;
}

SurfaceData InitializeBlinnPhoneSurfaceData(Varyings input)
{
	SurfaceData outSurfaceData;
	outSurfaceData.position = input.positionWS;
	outSurfaceData.normal = input.normalWS;
	outSurfaceData.viewDirection = normalize(_WorldSpaceCameraPos - input.positionWS);
	outSurfaceData.specular = _Shininess;

	return outSurfaceData;
}

#endif