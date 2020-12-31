// ========================================================
// des：扇形攻击区域
// author: shenyi
// time：2020-12-31 10:49:28
// version：1.0
// ========================================================

using System;
using UnityEngine;

namespace Game
{
	[Serializable]
	public class SectorSkillShape : SkillShape
	{
		public float Radius = 3;

		public float Angle = 90;

#if UNITY_EDITOR
		public override void DrawGizmos(Vector3 position)
		{
			Gizmos.color = Color.red;
			float r = Radius * Scale;

			float m_Theta = 0.0001f;

			// 设置颜色
			Color defaultColor = Gizmos.color;
			Gizmos.color = Color.cyan;

			// 绘制圆环
			Vector3 beginPoint = Vector3.zero + position;
			Vector3 firstPoint = Vector3.zero + position;
			float rad = Mathf.Deg2Rad * Angle;
			for (float theta = Mathf.PI / 2f - rad / 2f; theta <= Mathf.PI / 2f + rad / 2; theta += m_Theta)
			{
				float x = r * Mathf.Cos(theta);
				float z = r * Mathf.Sin(theta);
				Vector3 endPoint = new Vector3(x, 0, z) + position;
				if (theta == -rad / 2f)
				{
					firstPoint = endPoint;
				}
				else
				{
					Gizmos.DrawLine(beginPoint, endPoint);
				}
				beginPoint = endPoint;
			}

			// 绘制最后一条线段
			Gizmos.DrawLine(firstPoint, position);
			Gizmos.DrawLine(beginPoint, position);

			// 恢复默认颜色
			Gizmos.color = defaultColor;

			Gizmos.color = Color.white;
		}
#endif
	}
}
