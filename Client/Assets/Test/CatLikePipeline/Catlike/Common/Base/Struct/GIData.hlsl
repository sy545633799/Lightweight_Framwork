
struct Light 
{
	float3 color;
	float3 direction;
	float shadowAttenuation;
};

struct GIData
{
	Light light;
	Light lightBaked;
	//UnityIndirect indirect;
};