// ========================================================
// des：三角形攻击区域
// author: shenyi
// time：2020-12-31 10:57:23
// version：1.0
// ========================================================

using System;
using UnityEngine;

namespace Game
{
	[Serializable]
	public class TriangleSkillShape : SkillShape
	{
		public float Bottom = 3;
		public float Height = 4;

#if UNITY_EDITOR
		public override void DrawGizmos(Vector3 position)
		{
			float w = Bottom * Scale;
			float h = Height * Scale;
			// 设置颜色
			Color defaultColor = Gizmos.color;
			Gizmos.color = Color.magenta;
			Vector3 pointA = new Vector3(-w / 2f, 0, 0) + position;
			Vector3 pointB = new Vector3(w / 2f, 0, 0) + position;
			Vector3 pointC = new Vector3(0, 0, h) + position;
			// 绘制方形

			// 绘制最后一条线段
			Gizmos.DrawLine(pointA, pointB);
			Gizmos.DrawLine(pointB, pointC);
			Gizmos.DrawLine(pointC, pointA);

			// 恢复默认颜色
			Gizmos.color = defaultColor;
			
			
		}
#endif
	}
}
