// ========================================================
// des：曲线路径(TODO)
// author: shenyi
// time：2020-12-31 13:35:26
// version：1.0
// ========================================================

using System;
using UnityEngine;

namespace Game
{
	[Serializable]
	public class CurveSkillPath : SkillPath
	{
		public Bezier bezier;

		protected override Vector3 OnUpdate(Transform target, float duration, float time, float deltaTime)
		{
			return bezier.GetPointAtTime(time/duration);
		}
	}
}
