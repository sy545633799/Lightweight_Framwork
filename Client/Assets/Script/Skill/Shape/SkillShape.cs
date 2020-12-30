// ========================================================
// des：
// author: shenyi
// time：2020-12-30 13:29:56
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
	public abstract class SkillShape
	{
		[HideInInspector]
		public SkillShapeType shapeType;

		public float Scale = 1;

		public Vector3 Position = Vector3.zero;

		

		public void OnUpdate(Vector3 position)
		{
			Position = position;
		}


#if UNITY_EDITOR
		public abstract void DrawGizmos();
#endif
	}
}
