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
		public float MoveSpeed = 10;

		protected override Vector3 OnUpdate(Transform target, float duration, float time, float deltaTime)
		{
			//暂时都先以平地计算
			Vector3 targetPos = new Vector3(target.position.x, Height, target.position.z);
			Direction = Vector3.Normalize(targetPos - Position);

			return Position + Direction * MoveSpeed * deltaTime;
		}
	}
}
