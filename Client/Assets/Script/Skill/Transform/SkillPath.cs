// ========================================================
// des：技能路径, 
// author: shenyi
// time：2020-12-30 14:48:40
// version：1.0
// ========================================================

using System;
using UnityEngine;

namespace Game
{
	[Serializable]
	public abstract class SkillPath
	{
		[NonSerialized]
		public Vector3 Position;
		/// <summary>
		/// 速度，旋转，大小曲线，TODO
		/// </summary>
		public AnimationCurve MoveAcceclerate;
		public AnimationCurve RotateAcceclerate;
		public AnimationCurve ScaleSAcceclerate;

		public void Update(Vector3 start, Vector3 end, Vector3 direction, float duration, float time, float deltaTime)
		{
			Position = OnUpdate(start, end, direction, duration, time, deltaTime);
		}

		protected abstract Vector3 OnUpdate(Vector3 start, Vector3 end, Vector3 direction, float duration, float time, float deltaTime);

	}
}
