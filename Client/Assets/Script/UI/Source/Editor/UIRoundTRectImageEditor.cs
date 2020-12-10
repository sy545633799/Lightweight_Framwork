// ========================================================
// des：
// author: 
// time：2020-10-29 11:19:04
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Game.Editor {
	[CustomEditor(typeof(UIRoundTRectImage), true)]
	[CanEditMultipleObjects]
	public class UIRoundTRectImageEditor : UIBaseImageEditor
	{
		protected GUIContent m_FillCenterContent;
		protected SerializedProperty m_FillCenter;
		protected GUIContent m_RadiusContent;
		protected SerializedProperty m_Radius;

		protected override void OnEnable()
		{
			base.OnEnable();
			m_FillCenterContent = new GUIContent("FillCenter");
			m_FillCenter = serializedObject.FindProperty("m_FillCenter");
			m_RadiusContent = new GUIContent("Radius");
			m_Radius = serializedObject.FindProperty("m_Radius");
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			EditorGUILayout.PropertyField(m_FillCenter, m_FillCenterContent);
			EditorGUILayout.PropertyField(m_Radius, m_RadiusContent);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
