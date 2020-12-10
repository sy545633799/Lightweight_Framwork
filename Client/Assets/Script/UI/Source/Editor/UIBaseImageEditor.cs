// ========================================================
// des：
// author: 
// time：2020-10-30 14:19:12
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace Game.Editor {
	[CustomEditor(typeof(UIBaseImage), true)]
	[CanEditMultipleObjects]
	public class UIBaseImageEditor : GraphicEditor
	{
		GUIContent m_SpriteContent;
		SerializedProperty m_Sprite;

		protected override void OnEnable()
		{
			base.OnEnable();
			m_Sprite = serializedObject.FindProperty("m_Sprite");
			m_SpriteContent = EditorGUIUtility.TrTextContent("Source Image");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(m_Sprite, m_SpriteContent);
			AppearanceControlsGUI();
			RaycastControlsGUI();

			serializedObject.ApplyModifiedProperties();
			
		}

	}
}
