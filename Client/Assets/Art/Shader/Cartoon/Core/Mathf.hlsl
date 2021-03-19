

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