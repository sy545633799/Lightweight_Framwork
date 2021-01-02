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
		public override void DrawGizmos(Vector3 position, Vector3 direction)
		{
			float w = Bottom * Scale;
			float h = Height * Scale;
			// 设置颜色
			Color defaultColor = Gizmos.color;
			Gizmos.color = Color.magenta;

			Vector3 right = Vector3.Normalize(Vector3.Cross(direction, Vector3.up));
			
			Vector3 pointA = right * (w / 2f) + position;
			Vector3 pointB = -right * (w / 2f) + position;
			Vector3 pointC = direction * h + position;
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
