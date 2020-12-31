// ========================================================
// des：圆形攻击区域
// author: shenyi
// time：2020-12-30 17:58:18
// version：1.0
// ========================================================

using System;
using UnityEngine;

namespace Game
{
	[Serializable]
	public class CircleSkillShape : SkillShape
	{
		public float Radius = 1;
		//值越低圆环越平滑
		private float m_Theta = 0.1f; 



#if UNITY_EDITOR
		public override void DrawGizmos(Vector3 position)
		{
			Gizmos.color = Color.red;
			float r = Radius * Scale;
			//Gizmos.DrawWireSphere (transform.position,r);
			//Gizmos.DrawSphere(transform.position,0.1f);

			if (m_Theta < 0.0001f) m_Theta = 0.0001f;
			//// 设置矩阵
			//Matrix4x4 defaultMatrix = Gizmos.matrix;
			//Gizmos.matrix = Target.localToWorldMatrix;

			// 设置颜色
			Color defaultColor = Gizmos.color;
			Gizmos.color = Color.red;

			// 绘制圆环
			Vector3 beginPoint = position;
			Vector3 firstPoint = position;
			for (float theta = 0; theta < 2 * Mathf.PI; theta += m_Theta)
			{
				float x = r * Mathf.Cos(theta);
				float z = r * Mathf.Sin(theta);
				Vector3 endPoint = new Vector3(x, 0, z) + position;
				if (theta == 0)
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
			Gizmos.DrawLine(firstPoint, beginPoint);

			// 恢复默认颜色
			Gizmos.color = defaultColor;

			//// 恢复默认矩阵
			//Gizmos.matrix = defaultMatrix;
			//Gizmos.color = Color.white;

		}
#endif

	}
}
