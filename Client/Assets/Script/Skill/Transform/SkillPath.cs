// ========================================================
// des：
// author: 
// time：2020-12-30 14:48:40
// version：1.0
// ========================================================

using System;
using UnityEngine;

namespace Game
{
	[Serializable]
	public abstract class SkillPath: SkillTransform
	{
		protected Vector3 StartPostion;
		protected Vector3 Direction;
		public void OnStart(Transform Caller)
		{
			StartPostion = Caller.position;
			Direction = Caller.forward;
		}


		public void OnStop()
		{
			StartPostion = Vector3.zero;
			Direction = Vector3.zero;
		}

	}
}
