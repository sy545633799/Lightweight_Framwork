using UnityEngine;
using UnityEditor;

namespace Game.Editor
{
    public class EditorTools
    {
        static public bool DrawHeader(string text)
        {
            bool state = EditorPrefs.GetBool(text, true);

            GUILayout.Space(3f);
            if (!state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
            GUILayout.BeginHorizontal();
            GUI.changed = false;

            text = "<b><size=11>" + text + "</size></b>";
            if (state) text = "\u25BC " + text;
            else text = "\u25BA " + text;
            if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;

            if (GUI.changed) EditorPrefs.SetBool(text, state);

            GUILayout.Space(2f);
            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
            if (!state) GUILayout.Space(3f);
            return state;
        }

        static bool _mEndHorizontal = false;

        static public void BeginContents()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(10f));

            GUILayout.BeginVertical();
            GUILayout.Space(2f);
        }

        static public void EndContents()
        {
            GUILayout.Space(3f);
            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(3f);
            GUILayout.EndHorizontal();

            GUILayout.Space(3f);
        }
    }
}