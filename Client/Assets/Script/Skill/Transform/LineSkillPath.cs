// ========================================================
// des：直线
// author: shenyi
// time：2020-12-30 20:12:45
// version：1.0
// ========================================================

using System;
using UnityEngine;

namespace Game
{
	[Serializable]
	public class LineSkillPath : SkillPath
	{
		[Range(0, 10000)]
		public float Distance = 10;

		protected override Vector3 OnUpdate(Vector3 start, Vector3 end, Vector3 direction, float duration, float time, float deltaTime)
		{
			return start + direction * Distance * time / duration;
		}

		
	}
}
