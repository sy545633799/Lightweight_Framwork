// ========================================================
// des：
// author: 
// time：2020-10-28 15:32:02
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Editor {
	[CustomEditor(typeof(UIMirrorImage), true), CanEditMultipleObjects]
	public class UIMirrorImageEditor : UIImageEditor
	{
		private string[] MirrorType = new string[] { "无", "水平", "垂直", "四分之一"};

		protected GUIContent m_MirrorTypeContent;
		protected SerializedProperty m_MirrorType;

		protected override void OnEnable()
		{
			base.OnEnable();
			m_MirrorTypeContent = new GUIContent("镜像类型");
			m_MirrorType = serializedObject.FindProperty("m_MirrorType");
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			EditorGUILayout.PropertyField(m_MirrorType, m_MirrorTypeContent);
			serializedObject.ApplyModifiedProperties();
		}
	}
}
