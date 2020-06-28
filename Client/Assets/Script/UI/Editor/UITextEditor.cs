using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    [CustomEditor(typeof(UIText), true), CanEditMultipleObjects]
    public class UITextEditor : TMP_UiEditorPanel
    {
        UIText mFishText;

        protected override void OnEnable()
        {
            base.OnEnable();
            mFishText = m_TextComponent as UIText;
        }

        protected new void DrawTextInput()
        {
            EditorGUILayout.Space();

            if (GUILayout.Button(" 刷新表格 "))
                LocationAsset.Refresh();

            serializedObject.Update();
            EditorGUILayout.LabelField($"当前文本:{mFishText.text}");

            List<string> modules = new List<string>(LocationAsset.Instance.NameToLabID.Keys);
            LocationTable table = LocationAsset.GetTable(mFishText.ID);
            string text;
            int typeIndex = 0;
            int newTypeIndex = 0;
            if (table == null)
            {
                text = "";
                newTypeIndex = EditorGUILayout.Popup("模块", 0, modules.ToArray());
            }
            else
            {
                text = table.Text;
                string module = table.Module;
                typeIndex = modules.IndexOf(module);
                newTypeIndex = EditorGUILayout.Popup("模块", typeIndex, modules.ToArray());
            }

            Dictionary<string, int> dic = LocationAsset.Instance.NameToLabID[modules[newTypeIndex]];
            List<string> texts = new List<string>(dic.Keys);

            if (newTypeIndex != typeIndex) text = texts[0];
            int textIndex = texts.IndexOf(text);
            int newTextIndex = EditorGUILayout.Popup("文本", textIndex, texts.ToArray());
            text = texts[newTextIndex];
            int newID = LocationAsset.Instance.NameToLabID[modules[newTypeIndex]][text];

            if (newID != mFishText.ID)
            {
                mFishText.ID = newID;
                EditorUtility.SetDirty(target);
            }
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