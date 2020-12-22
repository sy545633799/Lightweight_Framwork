// ========================================================
// des：
// author: 
// time：2020-07-18 11:20:25
// version：1.0
// ========================================================


using UnityEditor;
using UnityEngine;
using NPOI.SS.Formula.Functions;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;

namespace Game.Editor
{

	public class SearchLocationTextWnd : EditorWindow
	{
		private string SearchText = string.Empty;
		private int SearchTextID = 0;
		private static Dictionary<int, string> textInfoList = new Dictionary<int, string>();
		private Vector2 scrollView = Vector2.zero;
		SearchLocationTextWnd()
		{
			this.titleContent = new GUIContent("本地化文本");
		}

		public static async void ShowWindow()
		{
			await UILocationAsset.Refresh();
			EditorWindow.GetWindow(typeof(SearchLocationTextWnd));
		}

		[SerializeField]
		void OnGUI()
		{
			GUILayout.Space(10);
			GUILayout.BeginVertical();
			SearchText = EditorGUILayout.TextField("搜索", SearchText, GUILayout.MinWidth(300));
			GUILayout.EndVertical();
			GUILayout.Space(10);
			GetLocationTextList(SearchText);
			scrollView = GUILayout.BeginScrollView(scrollView);
			foreach (var textInfo in textInfoList)
			{
				if (GUILayout.Button(textInfo.Value, GUILayout.Height(20)))
				{
					UITextEditor.SetTextId(textInfo.Key);
				}
			}
			GUILayout.EndScrollView();
		}
		private static void GetLocationTextList(string testStr)
		{
			textInfoList.Clear();
			if (string.IsNullOrEmpty(testStr))
			{
				textInfoList.Add(0, "不赋值");

				foreach (var location in UILocationAsset.GetAll())
				{
					if (textInfoList.ContainsKey(location.Key))
					{
						//Debug.LogError(location.Key)
					}

					else
						textInfoList.Add(location.Key, location.Value.Text);
				}
			}
			else
			{
				foreach (var location in UILocationAsset.GetAll())
				{
					if (location.Value.Text.Contains(testStr))
						textInfoList.Add(location.Key, location.Value.Text);
				}
			}

		}

	}
}
