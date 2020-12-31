// ========================================================
// des：
// author: shenyi
// time：2020-12-30 13:29:56
// version：1.0
// ========================================================

using System;
using UnityEngine;

namespace Game
{
	[Serializable]
	public abstract class SkillShape
	{
		[HideInInspector]
		public SkillShapeType shapeType;
		public float Scale = 1;

#if UNITY_EDITOR
		public abstract void DrawGizmos(Vector3 position);
#endif
	}
}
