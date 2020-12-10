// ========================================================
// des：
// author: 
// time：2020-10-30 14:36:09
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace Game.Editor {
	[CustomEditor(typeof(UICircleImage), true)]
	[CanEditMultipleObjects]
	public class UICircleImageEditor : UIBaseImageEditor
	{
		protected UICircleImage uiImage;

		protected GUIContent m_FillPercentContent;
		protected SerializedProperty m_FillPercent;
		protected GUIContent m_FillContent;
		protected SerializedProperty m_Fill;
		protected GUIContent m_ThicknessContent;
		protected SerializedProperty m_Thickness;
		protected GUIContent m_SegementsContent;
		protected SerializedProperty m_Segements;

		protected override void OnEnable()
		{
			base.OnEnable();
			uiImage = target as UICircleImage;
			m_FillPercentContent = new GUIContent("FillPercent");
			m_FillPercent = serializedObject.FindProperty("FillPercent");
			m_FillContent = new GUIContent("Fill");
			m_Fill = serializedObject.FindProperty("Fill");
			m_ThicknessContent = new GUIContent("Thickness");
			m_Thickness = serializedObject.FindProperty("Thickness");
			m_SegementsContent = new GUIContent("Segements");
			m_Segements = serializedObject.FindProperty("Segements");
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			EditorGUILayout.PropertyField(m_FillPercent, m_FillPercentContent);
			EditorGUILayout.PropertyField(m_Fill, m_FillContent);
			EditorGUILayout.PropertyField(m_Thickness, m_ThicknessContent);
			EditorGUILayout.PropertyField(m_Segements, m_SegementsContent);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
