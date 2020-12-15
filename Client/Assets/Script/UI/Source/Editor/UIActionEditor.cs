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


	[CustomEditor(typeof(UIAction), true), CanEditMultipleObjects]
	public class UIActionEditor : UIButtonEditor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			UIAction m_Button = target as UIAction;
			if (EditorTools.DrawHeader("动作相关"))
			{
				EditorTools.BeginContents();
				GUILayout.BeginVertical();
				GUILayout.Space(4f);
				m_Button.TriggerName = EditorGUILayout.TextField("动画状态机参数名", m_Button.TriggerName);

				GUILayout.Space(4f);
				GUILayout.EndVertical();
				EditorTools.EndContents();
			}
		}
	}


	
			
}
