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
		public override void DrawGizmos(Vector3 position, Vector3 direction)
		{
			Gizmos.color = Color.red;
			float r = Radius * Scale;

			// 设置颜色
			Color defaultColor = Gizmos.color;
			Gizmos.color = Color.cyan;

			float theta = 0.01f;

			Vector3 beginPoint = position;
			for (float delta = - Angle / 2; delta < Angle / 2; delta= delta + theta)
			{
				Vector3 pos = Vector3.Normalize(Quaternion.AngleAxis(delta, Vector3.up) * direction) * r + position;
				Gizmos.DrawLine(beginPoint, pos);
				beginPoint = pos;
			}

			// 绘制最后一条线段
			Gizmos.DrawLine(beginPoint, position);

			// 恢复默认颜色
			Gizmos.color = defaultColor;

			Gizmos.color = Color.white;
		}
#endif
	}
}
