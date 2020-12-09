#ifndef CUSTOM_FANTASY_LITING_INCLUDED
#define CUSTOM_FANTASY_LITING_INCLUDED

inline half3 LightingLambert(SurfaceData o, GIData gi)
{
	half nl = saturate(dot(o.normal, gi.light.direction));
	return nl * gi.light.shadowAttenuation * gi.light.color;
}

inline half3 LightingBlinnPhone(SurfaceData o, GIData gi)
{
	half3 h = normalize(o.viewDirection + gi.light.direction);
	half nh = max(0, dot(o.normal, h));
	
	half3 spec = pow(nh, o.specular);
	return spec * gi.light.color;
}

#endif