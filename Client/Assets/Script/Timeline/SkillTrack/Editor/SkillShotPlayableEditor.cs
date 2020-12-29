// ========================================================
// des：
// author: 
// time：2020-12-29 16:12:39
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Game;

namespace Game.Editor {
	public class SkillShotPlayableEditor : UnityEditor.Editor
	{
		SkillShot m_target;
		private void OnEnable()
		{
			m_target = (SkillShot)target;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			Debug.DrawLine(Vector3.one, Vector3.zero, Color.red);
		}

		
	}
}
