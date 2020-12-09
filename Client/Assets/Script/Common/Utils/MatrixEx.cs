using System;
using UnityEngine;

public static class MatrixEx
{
	/// <summary>
	/// 获取矩阵裁剪掩码
	/// </summary>
	/// <param name="pos"></param>
	/// <param name="projection"></param>
	/// <param name="leftex"></param>
	/// <param name="rightex"></param>
	/// <param name="downex"></param>
	/// <param name="upex"></param>
	/// <returns></returns>
	/// 则对应四个象限的碰撞掩码：
	/// |2|8|
	/// |1|4|
	public static int ComputeOutCode(this Matrix4x4 projection, Vector4 pos, float leftex = 0, float rightex = 0, float downex = 0, float upex = 0)
	{
		pos = projection * pos;
		int code = 0;
		if (pos.x < (-1 + leftex) * pos.w) code |= 0x01;
		if (pos.x > (1 + rightex) * pos.w) code |= 0x02;
		if (pos.y < (-1 + downex) * pos.w) code |= 0x04;
		if (pos.y > (1 + upex) * pos.w) code |= 0x08;
		if (pos.z < -pos.w) code |= 0x10;
		if (pos.z > pos.w) code |= 0x20;
		return code;
	}

	/// <summary>
	/// 联级阴影的矩阵转换
	/// </summary>
	/// <param name="m"></param>
	/// <param name="tileIndex">总共有灯光数量*split块</param>
	/// <param name="split">将图集进行split*split分块</param>
	/// <param name="tileSize">每一块小图的大小</param>
	/// <param name="viewport"></param>
	/// <returns></returns>
	public static Matrix4x4 ConvertToAtlasMatrix(this Matrix4x4 m, int tileIndex, int split, float tileSize, out Rect viewport)
	{
		Vector2 offset = new Vector2(tileIndex % split, tileIndex / split);
		viewport = new Rect(offset.x * tileSize, offset.y * tileSize, tileSize, tileSize);
		if (SystemInfo.usesReversedZBuffer)
		{
			m.m20 = -m.m20;
			m.m21 = -m.m21;
			m.m22 = -m.m22;
			m.m23 = -m.m23;
		}
		float scale = 1f / split;

		//先将结果 * 0.5 + 0.5 转换到屏幕空间(0, 1), 再对xy进行偏移
		//scale * (0.5 * mul（VP，worldPos）+ 0.5) + scale * offset
		//即先左乘A1
		//0.5	0,		0,		0.5
		//0		0.5,	0,		0.5
		//0		0,		0.5,	0.5
		//0		0,		0,		1
		//再左乘A2
		//scale	0,		0,		scale * offset.x
		//0		scale,	0,		scale * offset.y
		//0		0,		0,		0
		//0		0,		0,		0
		//A2*A1即矩阵
		//0.5 * scale	0,				0,		(0.5 + offset.x) * scale
		//0				0.5 * scale,	0,		(0.5 + offset.y) * scale
		//0				0,				0.5,	0.5
		//0				0,				0,		1
		//即
		//Matrix4x4 offsetMatrix = new Matrix4x4(
		//	new Vector4(0.5f * scale, 0, 0, 0),
		//	new Vector4(0, 0.5f * scale, 0, 0),
		//	new Vector4(0, 0, 0.5f, 0),
		//	new Vector4((0.5f + offset.x) * scale, (0.5f + offset.y) * scale, 0.5f, 1));
		//	左乘m展开得到
		m.m00 = (0.5f * (m.m00 + m.m30) + offset.x * m.m30) * scale;
		m.m01 = (0.5f * (m.m01 + m.m31) + offset.x * m.m31) * scale;
		m.m02 = (0.5f * (m.m02 + m.m32) + offset.x * m.m32) * scale;
		m.m03 = (0.5f * (m.m03 + m.m33) + offset.x * m.m33) * scale;
		m.m10 = (0.5f * (m.m10 + m.m30) + offset.y * m.m30) * scale;
		m.m11 = (0.5f * (m.m11 + m.m31) + offset.y * m.m31) * scale;
		m.m12 = (0.5f * (m.m12 + m.m32) + offset.y * m.m32) * scale;
		m.m13 = (0.5f * (m.m13 + m.m33) + offset.y * m.m33) * scale;
		m.m20 = 0.5f * (m.m20 + m.m30);
		m.m21 = 0.5f * (m.m21 + m.m31);
		m.m22 = 0.5f * (m.m22 + m.m32);
		m.m23 = 0.5f * (m.m23 + m.m33);
		return m;
	}

	
	
}
