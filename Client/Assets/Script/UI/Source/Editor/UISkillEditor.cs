// ========================================================
// des：
// author: 
// time：2020-12-15 13:13:13
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Editor {


	[CustomEditor(typeof(UISkill), true), CanEditMultipleObjects]
	public class UISkillEditor : UIButtonEditor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			UISkill m_Button = target as UISkill;
			if (EditorTools.DrawHeader("技能相关"))
			{
				EditorTools.BeginContents();
				GUILayout.BeginVertical();
				GUILayout.Space(4f);
				m_Button.Mask = (EditorGUILayout.ObjectField("冷却遮罩", m_Button.ClickClip, typeof(UIImage), true)) as UIImage;

				GUILayout.Space(4f);
				GUILayout.EndVertical();
				EditorTools.EndContents();
			}
		}
	}


	
			
}
