// ========================================================
// des：
// author: 
// time：2020-12-30 11:37:33
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Editor {
	[CustomEditor(typeof(SkillShot), true)]
	public class SkillShotEditor : UnityEditor.Editor
	{
		SkillShot m_target;
		private void OnEnable()
		{
			m_target = (SkillShot)target;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			//m_target.template.Condition = (SkillCondition)EditorGUILayout.EnumFlagsField("技能释放条件", m_target.template.Condition);

			//if (GUI.changed)
			//	EditorUtility.SetDirty(target);
			//serializedObject.ApplyModifiedProperties();
		}

	}
}
