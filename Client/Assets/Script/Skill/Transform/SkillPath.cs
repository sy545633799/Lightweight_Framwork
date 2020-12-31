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
		public Vector3 StartPostion;
		[NonSerialized]
		public float Height;

		[NonSerialized]
		public Vector3 Position;
		[NonSerialized]
		public Vector3 Direction;
		/// <summary>
		/// 速度，旋转，大小曲线，TODO
		/// </summary>
		public AnimationCurve MoveAcceclerate;
		public AnimationCurve RotateAcceclerate;
		public AnimationCurve ScaleSAcceclerate;

		public void Start(Transform self, Vector3 offset)
		{
			StartPostion = self.position + offset;
			Position = StartPostion;
			Direction = self.forward;
			Height = offset.y;
		}

		public Vector3 Update(Transform target, float duration, float time, float deltaTime)
		{
			Position = OnUpdate(target, duration, time, deltaTime);
			return Position;
		}

		protected abstract Vector3 OnUpdate(Transform target, float duration, float time, float deltaTime);
		

	}
}
