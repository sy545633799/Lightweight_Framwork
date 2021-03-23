#define UNITY_PI            3.14159265359f

//C#端把两个float转换成一个Color:
//ushort rg = Mathf.FloatToHalf(val1);
//ushort ba = Mathf.FloatToHalf(val2);
//new Color((rg >> 8 & 0x00ff) / 255f, (rg & 0x00ff) / 255f, (ba >> 8 & 0x00ff) / 255f, (ba & 0x00ff) / 255f);
//shader端转回来：
float decode2half(float2 c) {
	float high = c.x * 255;
	float sign = floor(high / 128);
	high -= sign * 128;
	sign = 1 - 2 * sign;
	float exp = floor(high / 4) - 15;
	float mantissa = c.y * 255 + high % 2 * 256 + floor(high / 2 % 2) * 512;
	return sign * pow(2, exp) * (1 + mantissa / 1024);
}

float4 alphaBlend(float4 top, float4 bottom)
{
	float3 color = (top.rgb * top.a) + (bottom.rgb * (1 - top.a));
	float alpha = top.a + bottom.a * (1 - top.a);
	return float4(color, alpha);
}

half BRDF_DTerm(float NdotH, float i_roughness) {
	//DGGX =  a^2 / π((a^2 – 1) (n · h)^2 + 1)^2
	float a2 = i_roughness * i_roughness;
	float val = ((a2 - 1) * (NdotH * NdotH) + 1);
	return a2 / (UNITY_PI * (val * val));
}
half BRDF_GTerm(float NdotL, float NdotV, float i_roughness) {
	//G(l,v,h)=1/(((n·l)(1-k)+k)*((n·v)(1-k)+k))
	float k = i_roughness * i_roughness / 2;
	return 0.5 / ((NdotL * (1 - k) + k) + (NdotV * (1 - k) + k));
}

half3 BRDF_FresnelTerm(half3 F0, float LdotH) {
	//F(l,h) = F0+(1-F0)(1-l·h)^5
	return F0 + (1 - F0) * pow(1 - LdotH, 5);
}
half3 custom_FresnelLerp(half3 F0, half3 F90, half cosA, float fresnelPow)
{
	half t = pow(1 - cosA, fresnelPow);   // ala Schlick interpoliation
	return lerp(F0, F90, t);
}
