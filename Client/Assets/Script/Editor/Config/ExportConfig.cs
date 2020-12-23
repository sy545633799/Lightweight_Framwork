// ========================================================
// des：
// author: 
// time：2020-09-21 20:11:32
// version：1.0
// ========================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Game.Editor {
	public class ExportConfigWindow : EditorWindow
	{
		
		private Vector2 scrollView = Vector2.zero;
		ExportConfigWindow()
		{
			this.titleContent = new GUIContent("导表工具");
		}

		public static void ShowWindow()
		{
			EditorWindow.GetWindow(typeof(SearchLocationTextWnd));
		}

		public static bool GenerateScript(string excelPath)
		{
			
			if (EditorApplication.isCompiling)
				return false;
			string fileName = Path.GetFileName(excelPath);
			Dictionary<string, List<Dictionary<string, ExcelObjectElement>>> excelData = ExcelReader.ReadExcel(excelPath);

			foreach (var item in excelData)
			{
				List<Dictionary<string, ExcelObjectElement>> list = item.Value;
				if (list.Count == 0) continue;
				string sheetName = item.Key;
				string _filePath = $"Assets/Script/Config/{sheetName}Asset.cs";
				if (File.Exists(_filePath))
					File.Delete(_filePath);
				FileStream _spriteFile = new FileStream(_filePath, FileMode.Create, FileAccess.Write);
				StreamWriter m_streamWriter = new StreamWriter(_spriteFile);
				m_streamWriter.WriteLine("namespace Game");
				m_streamWriter.WriteLine("{");
				m_streamWriter.WriteLine($"\tpublic class {sheetName}Asset : ConfigBaseAsset<{sheetName}> {{}}");

				m_streamWriter.WriteLine("\t[System.Serializable]");
				if (fileName.Contains("本地化"))
					m_streamWriter.WriteLine($"\tpublic class {sheetName} : LocationConfig");
				else
					m_streamWriter.WriteLine($"\tpublic class {sheetName} : ConfigBase");
				m_streamWriter.WriteLine("\t{");
				Dictionary<string, ExcelObjectElement> row = list[0];
				if (!fileName.Contains("本地化"))
				{
					foreach (var cell in row)
					{
						if (cell.Key == "ID") continue;
						m_streamWriter.WriteLine($"\t\tpublic {cell.Value.FieldType} {cell.Key};");
					}
				}
				m_streamWriter.WriteLine("\t}");
				m_streamWriter.WriteLine("}");
				m_streamWriter.Close();
			}

			AssetDatabase.Refresh();
			return true;
		}

		public static bool GenerateScriptable(string excelPath)
		{
			if (EditorApplication.isCompiling)
				return false;

			Dictionary<string, List<Dictionary<string, ExcelObjectElement>>> excelData = ExcelReader.ReadExcel(excelPath);

			foreach (var sheet in excelData)
			{
				string clsName = $"Game.{sheet.Key}";
				Type type = typeof(Game.ConfigBase).Assembly.GetType(clsName);

				FieldInfo[] _modelFields = type.GetFields();
				ArrayList objs = new ArrayList();
				for (int i = 0; i < sheet.Value.Count; i++)
				{
					Dictionary<string, ExcelObjectElement> element = sheet.Value[i];
					object _object = Activator.CreateInstance(type);
					foreach (var item in element)
					{
						foreach (var field in _modelFields)
						{
							if (item.Key == field.Name)
								field.SetValue(_object, item.Value.GetValue());
						}
					}
					objs.Add(_object);
				}
				
				Array _array = Array.CreateInstance(type, objs.Count);
				for (int i = 0; i < objs.Count; i++)
					_array.SetValue(objs[i], i);
				string assetsClsName = $"Game.{sheet.Key}Asset";//命名空间.类型名,程序集
				Type assetType = typeof(Game.UIText).Assembly.GetType(assetsClsName);
				object _config = Activator.CreateInstance(assetType);
				FieldInfo assetsField = assetType.GetField($"Configs");
				assetsField.SetValue(_config, _array);

				string path = $"Assets/Art/Assets/Config/{sheet.Key}.asset";
				AssetDatabase.CreateAsset((UnityEngine.Object)_config, path);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
				Debug.Log("生成成功 : " + path);
			}

			return true;
		}
		void OnGUI()
		{
			GUILayout.Space(10);
			GUILayout.BeginVertical();
			
			GUILayout.EndVertical();
			GUILayout.Space(10);
			scrollView = GUILayout.BeginScrollView(scrollView);
			DirectoryInfo directory = new DirectoryInfo("Assets/Publish/Config");
			FileInfo[] fileInfos = directory.GetFiles("*.xlsx");
			foreach (var fileInfo in fileInfos)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(fileInfo.Name);
				if (GUILayout.Button("生成代码", GUILayout.Height(20)))
				{
					GenerateScript(fileInfo.FullName);
				}

				if (GUILayout.Button("生成Assets", GUILayout.Height(20)))
				{
					GenerateScriptable(fileInfo.FullName);
				}

				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
		}

		[MenuItem("Tools/导表工具/生成ScriptableObject")]
		public static void GenModelData()
		{
			EditorWindow.GetWindow<ExportConfigWindow>(false, "生成ScriptableObject", true);
		}
	}
}
