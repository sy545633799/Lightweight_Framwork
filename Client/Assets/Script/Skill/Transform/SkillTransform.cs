// ========================================================
// des：
// author: 
// time：2020-12-30 19:41:52
// version：1.0
// ========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
	[Serializable]
	public abstract class SkillTransform
	{
		public float Speed = 10f;

		public abstract Vector3 OnUpdate(double deltaTime);
	}
}
