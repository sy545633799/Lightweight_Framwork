// ========================================================
// des：原地技能
// author: shenyi
// time：2020-12-31 13:14:41
// version：1.0
// ========================================================

using System;
using UnityEngine;

namespace Game
{
	[Serializable]
	public class NoSkillPath : SkillPath
	{
		protected override Vector3 OnUpdate(Transform target, float duration, float time, float deltaTime)
		{
			return StartPostion;
		}
	}
}
