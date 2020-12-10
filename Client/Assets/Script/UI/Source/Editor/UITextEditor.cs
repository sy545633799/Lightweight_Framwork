// ========================================================
// des：
// author: 
// time：2020-12-10 09:58:19
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Game.Editor
{
	[CustomEditor(typeof(UIText), true), CanEditMultipleObjects]
	public class UITextEditor : TMP_UiEditorPanel
	{
		private static UIText mFishText;

		//private string Searchname = string.Empty;
		protected override void OnEnable()
		{
			base.OnEnable();
			mFishText = m_TextComponent as UIText;
		}

		public static void SetTextId(int textId)
		{
			mFishText.ID = textId;
		}

		protected new void DrawTextInput()
		{
			EditorGUILayout.Space();

			if (GUILayout.Button(" 刷新表格 "))
				UILocationAsset.Refresh();

			serializedObject.Update();
			EditorGUILayout.LabelField($"当前文本:{mFishText.text}");

			if (GUILayout.Button(" 赋值 "))
				SearchLocationTextWnd.ShowWindow();

			bool show = EditorGUILayout.Toggle("中文竖显", mFishText.showVertical);
			if (mFishText.showVertical != show)
				mFishText.showVertical = show;

			bool addColon = mFishText.addColon = EditorGUILayout.Toggle("添加冒号", mFishText.addColon);
			if (mFishText.addColon != addColon)
				mFishText.addColon = addColon;
		}


		public override void OnInspectorGUI()
		{
			if (IsMixSelectionTypes()) return;
			serializedObject.Update();
			DrawTextInput();
			DrawMainSettings();
			DrawExtraSettings();
			EditorGUILayout.Space();
			if (m_HavePropertiesChanged)
			{
				m_HavePropertiesChanged = false;
				m_TextComponent.havePropertiesChanged = true;
				m_TextComponent.ComputeMarginSize();
				EditorUtility.SetDirty(target);
			}
			serializedObject.ApplyModifiedProperties();
		}
	}
}
