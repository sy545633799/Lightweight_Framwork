using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System.Runtime.CompilerServices;
using static Unity.Mathematics.math;

public class MathLib
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 GetWorldToLocal(ref float4x4 localToWorld)
	{
		float4x4 rotation = float4x4(float4(localToWorld.c0.xyz, 0), float4(localToWorld.c1.xyz, 0), float4(localToWorld.c2.xyz, 0), float4(0, 0, 0, 1));
		rotation = transpose(rotation);
		float3 localPos = mul(rotation, localToWorld.c3).xyz;
		localPos = -localPos;
		rotation.c3 = float4(localPos.xyz, 1);
		return rotation;
	}


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 GetPlane(float3 a, float3 b, float3 c)
	{
		float3 normal = normalize(cross(b - a, c - a));
		return float4(normal, -dot(normal, a));
	}

	public unsafe static bool BoxIntersect(double3 position, double3 extent, float4* planes, int len)
	{
		for (uint i = 0; i < len; ++i)
		{ 
			float4 plane = planes[i];
			float3 absNormal = abs(plane.xyz);
			//if ((dot(position, plane.xyz) - dot(absNormal, extent)) > -plane.w)
			//{
			//	return false;
			//}

			//球体平面求交?????
			//dot(absNormal, extent)
			double distance = dot(position, plane.xyz) + plane.w;
			if (distance > dot(absNormal, extent))
			{
				return false;
			}
		}
		return true;
	}
}