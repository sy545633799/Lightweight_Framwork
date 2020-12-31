// ========================================================
// des：跟随技能
// author: shenyi
// time：2020-12-31 13:14:41
// version：1.0
// ========================================================

using System;
using UnityEngine;

namespace Game
{
	[Serializable]
	public class FollowSkillPath : SkillPath
	{
		public float MoveSpeed = 5;

		protected override Vector3 OnUpdate(Vector3 start, Vector3 end, Vector3 direction, float duration, float time, float deltaTime)
		{
			
			return start;
		}
	}
}
