// ========================================================
// des：
// author: 
// time：2020-09-21 16:29:49
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UI;

namespace Game.Editor
{

	[CustomEditor(typeof(UIButton), true), CanEditMultipleObjects]
	public class UIButtonEditor : SelectableEditor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (EditorTools.DrawHeader("音效"))
			{
				EditorTools.BeginContents();
				GUILayout.BeginVertical();
				GUILayout.Space(4f);
				UIButton m_Button = target as UIButton;
				m_Button.ClickClip = (EditorGUILayout.ObjectField("ClickClip", m_Button.ClickClip, typeof(AudioClip), true)) as AudioClip;
				m_Button.EnterClip = (EditorGUILayout.ObjectField("EnterClip", m_Button.EnterClip, typeof(AudioClip), true)) as AudioClip;
				m_Button.ExitClip = (EditorGUILayout.ObjectField("ExitClip", m_Button.ExitClip, typeof(AudioClip), true)) as AudioClip;
				if (m_Button.ClickClip || m_Button.EnterClip || m_Button.ExitClip)
				{
					if (!m_Button.AudioSource)
						m_Button.AudioSource = m_Button.gameObject.AddComponent<AudioSource>();
				}
				else if (m_Button.AudioSource)
					GameObject.DestroyImmediate(m_Button.AudioSource);

				GUILayout.Space(4f);
				GUILayout.EndVertical();
				EditorTools.EndContents();
			}
		}
	}
}
