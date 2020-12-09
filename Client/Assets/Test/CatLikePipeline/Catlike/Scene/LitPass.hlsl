#ifndef CUSTOM_LIT_PASS_INCLUDED
#define CUSTOM_LIT_PASS_INCLUDED

#include "../Base/Catlike.hlsl"
 
float4 LitPassFragment(Varyings input) : SV_TARGET{
	UNITY_SETUP_INSTANCE_ID(input);
    
	half3 dLight = 0;
	half3 viewDir = normalize(_WorldSpaceCameraPos - input.positionWS);
	for (int n = 0; n < _DirectionalLightCount; n++)
	{ 
		SurfaceData surface = InitializeBlinnPhoneSurfaceData(input);
		GIData gi = GetGIDataByIndex(surface, n);
		half3 diff = LightingLambert(surface, gi);
		half3 specular = 0;
		//判断，仅第一盏光产生高光
		if (n == 0) specular = LightingBlinnPhone(surface, gi);		
		dLight += (diff + specular);
	}
	
	//点光源计算
	half3 pLight = 0;
	for (int n = 0; n < _PointLightCount; n++)
	{
		half3 specular = 0;
		half3 pLightVector = _PointLightPosition[n].xyz - input.positionWS;
		half3 pLightDir = normalize(pLightVector);
		//距离平方，用于计算点光衰减
		half distanceSqr = max(dot(pLightVector, pLightVector), 0.00001);
		//点光衰减公式pow(max(1 - pow((distance*distance/range*range),2),0),2)
		//点光源的强度衰减。一般来说，符合现实的距离衰减是距离的平方倒数
		half pLightAttenuation = pow(max(1 - pow((distanceSqr / (_PointLightColors[n].a * _PointLightColors[n].a)), 2), 0), 2);
		half3 halfDir = normalize(viewDir + pLightDir);
		specular = pow(saturate(dot(input.normalWS, halfDir)), _Shininess); 
		pLight += (1 + specular) * saturate(dot(input.normalWS, pLightDir)) * _PointLightColors[n].rgb * pLightAttenuation;
	}
	
	//计算需要的顶点位置与法线信息在计算平行光与点光时已经传入像素着色器。
	//下面直接开始像素着色器的相关计算
	half3 sLight = 0;
	for (int n = 0; n < _SpotLightCount; n++)
	{
		half3 specular = 0;
		//灯光到受光物体矢量，类似点光方向 
		half3 sLightVector = _SpotLightPosition[n].xyz - input.positionWS;
		//聚光灯朝向
		half3 sLightDir = normalize(_SpotLightDirections[n].xyz);
		//距离平方，与点光的距离衰减计算一样
		half distanceSqr = max(dot(sLightVector, sLightVector), 0.00001);
		//距离衰减公式同点光pow(max(1 - pow((distance*distance/range*range),2),0),2)
		half rangeAttenuation = pow(max(1 - pow((distanceSqr / (_SpotLightColors[n].a * _SpotLightColors[n].a)), 2), 0), 2);
		//灯光物体矢量与照射矢量点积
		float spotCos = saturate(dot(normalize(sLightVector), sLightDir));
		//角度衰减公式 
		float spotAttenuation = saturate((spotCos - _SpotLightDirections[n].w) / _SpotLightPosition[n].w);
		half3 halfDir = normalize(viewDir + sLightDir);
		specular = pow(saturate(dot(input.normalWS, halfDir)), _Shininess);
		sLight += (1 + specular) * saturate(dot(input.normalWS, sLightDir)) * _SpotLightColors[n].rgb * rangeAttenuation * spotAttenuation * spotAttenuation;
	} 
	
	half3 fragColor = GetBase(input.baseUV).xyz * (dLight + pLight + sLight); 
	//#if defined(_CLIPPING) 
		/*float cutoff = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Cutoff);
		clip(base.a - cutoff);*/
	//#endif 
		 
	return float4(fragColor, 1);
} 

#endif