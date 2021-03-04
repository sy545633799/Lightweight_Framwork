// ========================================================
// des：
// author: 
// time：2020-09-29 09:33:03
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using UnityEngine.UI;
//using SuperScrollView;
using System;

namespace Game.Editor
{
	[CustomEditor(typeof(UIView), true), CanEditMultipleObjects]
	public class UIViewEditor : UnityEditor.Editor
	{
		private static UIView mUIView;
		private string[] UITypeNames = { "不带页签的UI", "带页签的UI", "可以同时出现多个的Tips" };
		private string[] LayerNames = { "Default", "Scene", "Damage", "Base", "Notice", "Notice", "Guide", "Lock", "Network", "Loading" };
		private string UIConfigPath = "Lua/Framework/UI/UIConfig.lua";
		private string TipsConfigPath = "Lua/Framework/UI/TipsConfig.lua";
		private static string UILuaViewBasePath = "Lua/Logic/UI/BaseView/";
		private static string UILuaViewPath = "Lua/Logic/UI/View/";
		private static string UIPathAssetPath = "Assets/Art/Assets/Config/UIConfig.asset";


		private string AuthorName;
		private string UIName;
		private string description;
		private int SceneIndex;
		private static string[] SceneNames;
		private void OnEnable()
		{
			mUIView = target as UIView;
			string[] ids = AssetDatabase.FindAssets("t:Scene", new string[] { "Assets/Art/Scenes" });
			List<string> sceneNames = new List<string>();
			sceneNames.Add("无");
			for (int i = 0; i < ids.Length; i++)
			{
				string path = AssetDatabase.GUIDToAssetPath(ids[i]);
				string name = Path.GetFileNameWithoutExtension(path);
				if (name.StartsWith("3"))
				{
					sceneNames.Add(name);
				}
			}

			SceneNames = sceneNames.ToArray();
			UIName = mUIView.name;
		}

		public override void OnInspectorGUI()
		{

			if (!PrefabUtility.IsAnyPrefabInstanceRoot(mUIView.gameObject)) return;
			//base.OnInspectorGUI();
			EditorGUILayout.Space();

			//作者名
			GUILayout.Space(10);
			AuthorName = EditorGUILayout.TextField("作者", AuthorName);

			mUIView.ViewType = (UIType)EditorGUILayout.Popup("UI类型", (int)mUIView.ViewType, UITypeNames);
			if (mUIView.ViewType == UIType.CommonView || mUIView.ViewType == UIType.TabView)
			{
				UIName = EditorGUILayout.TextField("UIConfig键名", string.IsNullOrEmpty(UIName) ? mUIView.name : UIName);
				GUILayout.Space(10);
				mUIView.LayerIndex = EditorGUILayout.Popup("UI层级", mUIView.LayerIndex, LayerNames);
				if (mUIView.LayerIndex == LayerIndex.Base || mUIView.LayerIndex == LayerIndex.Base)
				{
					GUILayout.Space(10);
					mUIView.ShowMoneyBar = EditorGUILayout.Toggle("是否显示金钱条", mUIView.ShowMoneyBar);
					GUILayout.Space(10);
					mUIView.IsTransParent = EditorGUILayout.Toggle("是否显示Base层", mUIView.IsTransParent);
				}
				mUIView.DestroyOnUnload = EditorGUILayout.Toggle("关闭时是否销毁", mUIView.DestroyOnUnload);
				SceneIndex = EditorGUILayout.Popup("关联场景", SceneIndex, SceneNames);
			}
			else if (mUIView.ViewType == UIType.Tips)
			{
				UIName = EditorGUILayout.TextField("TipsConfig键名", UIName);
			}

			//绘制描述文本区域
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("Description", GUILayout.MaxWidth(80));
			description = EditorGUILayout.TextArea(description, GUILayout.MaxHeight(75));
			GUILayout.EndHorizontal();

			if (GUILayout.Button("导出UI配置"))
			{
				Canvas canvas = EnsureComponent<Canvas>(mUIView);
				canvas.overrideSorting = true;
				canvas.sortingLayerName = LayerNames[mUIView.LayerIndex];
				EnsureComponent<CanvasGroup>(mUIView);
				UIGraphicRaycaster raycaster = EnsureComponent<UIGraphicRaycaster>(mUIView);

				UIImage image = mUIView.gameObject.GetComponent<UIImage>();
				if (image)
				{
					GameObject.Destroy(image);
					Debug.LogError("不要在UI根节点放UIImage");
				}
				Dictionary<Transform, string> goPathDic = new Dictionary<Transform, string>();
				mUIView.transform.GetAllChildrenWithPath(ref goPathDic);
				EnsureUIView(goPathDic);
				GenUIFile(goPathDic);
				GenUIConfig(goPathDic);
			}

			EditorGUILayout.Space();
		}

		private T EnsureComponent<T>(Component comp)
			where T : Component
		{
			T t = mUIView.GetComponent<T>();
			if (!t) t= comp.gameObject.AddComponent<T>();
			return t;
		}

		private void EnsureUIContainer(UIContainer uiConTainer)
		{
			if (uiConTainer)
			{
				CanvasGroup canvasGroup = uiConTainer.GetComponent<CanvasGroup>();
				if (!canvasGroup) canvasGroup = uiConTainer.gameObject.AddComponent<CanvasGroup>();
				uiConTainer.canvasGroup = canvasGroup;
			}
		}

		private void EnsureUIView(Dictionary<Transform, string> goPathDic)
		{
			EnsureUIContainer(mUIView);
			foreach (var item in goPathDic)
			{
				Transform child = item.Key;
				UIView uIView = child.GetComponent<UIView>();
				if (uIView) DestroyImmediate(uIView);
				UIContainer uiConTainer = child.GetComponent<UIContainer>();

				if (child.name.Contains("(Item)"))
				{
					if (!uiConTainer)
						uiConTainer = child.AddComponent<UIContainer>();
				}
				else if (uiConTainer)
					DestroyImmediate(uiConTainer);

				EnsureUIContainer(uiConTainer);
			}
		}

		private void GenUIFile(Dictionary<Transform, string> goPathDic)
		{
			//判断是View还是tips
			GenerateLuaViewBase(mUIView, goPathDic);
			GenerateLuaView(mUIView, goPathDic);
			foreach (var item in goPathDic)
			{
				Transform child = item.Key;
				if (child.name.Contains("(Item)"))
				{
					UIContainer container = child.GetComponent<UIContainer>();
					container.ViewType = UIType.Item;
					goPathDic = new Dictionary<Transform, string>();
					child.GetAllChildrenWithPath(ref goPathDic);
					GenerateLuaViewBase(container, goPathDic);
					GenerateLuaView(container, goPathDic);
				}
			}
		}

		private static void GetComponent<T>(UIContainer container, string name, Transform trans, StringBuilder sb)
			where T : Component
		{

			container.names.Add(name);
			T t = trans.GetComponent<T>();
			if (t == null)
				Debug.LogError($"{trans}不存在组件{typeof(T)}");
			container.behaviours.Add(t);
		}

		private void GenerateLuaViewBase(UIContainer container, Dictionary<Transform, string> goPathDic)
		{
			string containerName = container.name.GetNameNoBrackets().GetEnglishName();
			string className = container is UIView ? UIName : UIName + containerName;
			className = (container.name.StartsWith("(Item)Com") ? containerName : className) + "Base";
			//上级文件夹名
			string parentDirName = container.name.StartsWith("(Item)Com") ? "Common" : mUIView.name;
			string fileDir = UILuaViewBasePath + parentDirName;
			string filePath = $"{fileDir}/{className}.lua";

			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"function {className}:ctor(container)");

			container.names = new List<string>();
			container.behaviours = new List<Component>();
			bool isTabClass = false;
			foreach (var item in goPathDic)
			{
				Transform child = item.Key;
				string childPath = item.Value;
				//屏蔽子Item中的控件
				if (Path.GetDirectoryName(childPath).Contains("(Item)")) continue;
				string childName = child.name;
				string fieldName = childName.GetNameNoBrackets();
				if (childName.Contains("(Item)"))
				{
					if (childName.Contains("(Item)Tab"))
						isTabClass = true;
					string CompName = "item_" + fieldName;
					GetComponent<UIContainer>(container, CompName, child, sb);
					sb.AppendFormat($"\tself.{CompName} = container.{CompName}\r\n");
				}
				if (childName.Contains("(Text)"))
				{
					//sb.Append("\t---@type UIText\r\n");
					string CompName = "text_" + fieldName;
					GetComponent<UIText>(container, CompName, child, sb);
					sb.AppendFormat($"\tself.{CompName} = self:AddText(container.{CompName})\r\n");
				}
				if (childName.Contains("(Button)"))
				{
					//sb.Append("\t---@type UIButton\r\n");
					string CompName = "btn_" + fieldName;
					GetComponent<UIButton>(container, CompName, child, sb);
					sb.AppendFormat($"\tself.{CompName} = self:AddButton(container.{CompName})\r\n");
				}
				if (childName.Contains("(Toggle)"))
				{
					//sb.Append("\t---@type UIToggle\r\n");
					string CompName = "Tog_" + fieldName;
					GetComponent<UIToggle>(container, CompName, child, sb);
					sb.AppendFormat($"\tself.{CompName} = self:AddToggle(container.{CompName})\r\n");
				}
				if (childName.Contains("(ToggleGroup)"))
				{
					//sb.Append("\t---@type UIToggle\r\n");
					string CompName = "Toggroup_" + fieldName;
					GetComponent<ToggleGroup>(container, CompName, child, sb);
					sb.AppendFormat($"\tself.{CompName} = self:AddToggleGroup(container.{CompName})\r\n");
				}
				if (childName.Contains("(DropDown)"))
				{
					//sb.Append("\t---@type AddDropDown\r\n");
					string CompName = "DropDown_" + fieldName;
					GetComponent<UIDropDown>(container, CompName, child, sb);
					sb.AppendFormat($"\tself.{CompName} = self:AddDropDown(container.{CompName})\r\n");
				}
				if (childName.Contains("(Image)"))
				{
					//sb.Append("\t---@type UIImage\r\n");
					string CompName = "img_" + fieldName;
					GetComponent<UIImage>(container, CompName, child, sb);
					sb.AppendFormat($"\tself.{CompName} = self:AddImage(container.{CompName})\r\n");
				}
				if (childName.Contains("(Widget)"))
				{
					//sb.Append("\t---@type UIWidget\r\n");
					string CompName = "widget_" + fieldName;
					GetComponent<RectTransform>(container, CompName, child, sb);
					sb.AppendFormat($"\tself.{CompName} = self:AddWidget(container.{CompName})\r\n");
				}
				if (childName.Contains("(Slider)"))
				{
					//sb.Append("\t---@type UISlider\r\n");
					string CompName = "slider_" + fieldName;
					GetComponent<UISlider>(container, CompName, child, sb);
					sb.AppendFormat($"\tself.{CompName} = self:AddSlider(container.{CompName})\r\n");
				}
				//if (childName.Contains("(ListView)"))
				//{
				//	//sb.Append("\t---@type UIListView\r\n");
				//	string CompName = "list_" + fieldName;
				//	GetComponent<UIListView>(container, CompName, child, sb);
				//	sb.AppendFormat($"\tself.{CompName} = self:AddListView(container.{CompName})\r\n");
				//}
				if (childName.Contains("(Input)"))
				{
					//sb.Append("\t---@type UIListView\r\n");
					string CompName = "input_" + fieldName;
					GetComponent<UIInput>(container, CompName, child, sb);
					sb.AppendFormat($"\tself.{CompName} = self:AddInput(container.{CompName})\r\n");
				}
				if (childName.Contains("(CanvasGroup)"))
				{
					//sb.Append("\t---@type UIListView\r\n");
					string CompName = "canvasGroup_" + fieldName;
					GetComponent<CanvasGroup>(container, CompName, child, sb);
					sb.AppendFormat($"\tself.{CompName} = self:AddCanvasGroup(container.{CompName})\r\n");
				}
				if (childName.Contains("(Model)"))
				{
					//sb.Append("\t---@type UIListView\r\n");
					string CompName = "model_" + fieldName;
					GetComponent<Transform>(container, CompName, child, sb);
					sb.AppendFormat($"\tself.{CompName} = self:AddModel(container.{CompName})\r\n");
				}
			}

			sb.AppendLine("end");
			sb.AppendLine();
			sb.AppendLine($"return {className}");


			string baseClassName = "";
			switch (container.ViewType)
			{
				case UIType.CommonView:
					baseClassName = "UIBaseView";
					break;
				case UIType.TabView:
					baseClassName = "UITabView";
					break;
				case UIType.Tips:
					baseClassName = "UIBaseTips";
					break;
				case UIType.Item:
					baseClassName = "UIBaseItem";
					break;
				default:
					break;
			}
			StringBuilder sb2 = new StringBuilder();
			sb2.AppendLine("----------------------- auto generate code --------------------------");
			sb2.AppendLine($"local base = {baseClassName}");
			sb2.AppendLine($"---@class {className}:{baseClassName}");
			sb2.AppendLine($"local {className} = BaseClass(\"{className}\", base)");
			sb2.AppendLine();
			sb2.Append(sb);

			if (!Directory.Exists(fileDir))
				Directory.CreateDirectory(fileDir);
			File.WriteAllText(filePath, sb2.ToString());
		}

		private void GenerateLuaView(UIContainer container, Dictionary<Transform, string> childen)
		{
			string containerName = container.name.GetNameNoBrackets().GetEnglishName();
			string className = container is UIView ? UIName : UIName + containerName;
			className = container.name.StartsWith("(Item)Com") ? containerName : className;
			string baseClassName = className + "Base";
			//上级文件夹名
			string parentDirName = container.name.StartsWith("(Item)Com") ? "Common" : mUIView.name;
			string fileDir = UILuaViewPath + parentDirName;
			string filePath = $"{fileDir}/{className}.lua";

			if (!Directory.Exists(fileDir))
				Directory.CreateDirectory(fileDir);
			if (File.Exists(filePath)) return;

			StringBuilder sb = new StringBuilder();
			sb.AppendLine($@"
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/{parentDirName}/{baseClassName}')
--- @class {className}: {className + "Base"}
local {className} = BaseClass('{className}', base)");
			List<string> subItemNames = new List<string>();
			foreach (var item in childen)
			{
				Transform child = item.Key;
				string childPath = item.Value;
				if (Path.GetDirectoryName(childPath).Contains("(Item)")) continue;
				if (child.name.Contains("(Item)"))
				{
					string subItemName = child.name.GetNameNoBrackets().GetEnglishName();
					subItemName = child.name.StartsWith("(Item)Com") ? subItemName : UIName + subItemName;
					if (!subItemNames.Contains(subItemName))
					{
						string itemPath = child.name.StartsWith("(Item)Com") ? $"Common/{subItemName}" : $"{UIName}/{subItemName}";
						sb.AppendLine($"local {subItemName} = require('Logic/UI/View/{itemPath}')");
						subItemNames.Add(subItemName);
					}
				}
			}
			sb.AppendLine("-------------------------------------------------------------");

			if (container.ViewType == UIType.CommonView || container.ViewType == UIType.TabView)
			{
				sb.Append($@"
function {className}:ctor()

end

function {className}:OnLoad(...)

end

function {className}:OnUnLoad()

end

return {className}");
			}
			else if (container.ViewType == UIType.Item)
			{
				sb.Append($@"
function {className}:ctor()

end

function {className}:OnRefresh(...)
    
end

return {className}");
			}
			else if (container.ViewType == UIType.Tips)
			{
				sb.Append($@"
function {className}:ctor()

end

function {className}:OnShowTips(...)
    
end

function {className}:OnReset()
    
end

return {className}");
			}

			if (!Directory.Exists(UILuaViewPath))
				Directory.CreateDirectory(UILuaViewPath);

			File.WriteAllText(filePath, sb.ToString());
		}

		private void GenUIConfig(Dictionary<Transform, string> goPathDic)
		{
			GUILayout.Space(10);
			var curSelectedParentPrefab = PrefabUtility.GetCorrespondingObjectFromSource(mUIView.gameObject);
			var uiPath = AssetDatabase.GetAssetPath(curSelectedParentPrefab).Replace("Assets/AssetRes/", "");


			UIConfigAsset uiConfigAsset = AssetDatabase.LoadAssetAtPath<UIConfigAsset>(UIPathAssetPath);
			if (!uiConfigAsset)
			{
				uiConfigAsset = new UIConfigAsset();
				uiConfigAsset.Configs = new UIConfig[0];
			}


			UIConfig uiConfig = null;
			for (int i = 0; i < uiConfigAsset.Configs.Length; i++)
			{
				UIConfig tmpConfig = uiConfigAsset.Configs[i];
				if (tmpConfig.Name == UIName)
				{
					uiConfig = tmpConfig;
					if (uiConfig.ID == 0)
						uiConfig.ID = TimeUtility.GetTimeStampEx();
					uiConfig.Path = uiPath;
					break;
				}
			}

			if (uiConfig == null)
			{
				uiConfig = new UIConfig();
				uiConfig.ID = TimeUtility.GetTimeStampEx();
				uiConfig.Path = uiPath;
				List<UIConfig> configs = new List<UIConfig>(uiConfigAsset.Configs);
				configs.Add(uiConfig);
				uiConfigAsset.Configs = configs.ToArray();
			}
			uiConfig.SceneName = SceneIndex == 0 ? string.Empty : SceneNames[SceneIndex];
			uiConfig.AtlasNames = GetAtlasNames(goPathDic);
			if (File.Exists(UIPathAssetPath))
				EditorUtility.SetDirty(uiConfigAsset);
			else
				AssetDatabase.CreateAsset(uiConfigAsset, UIPathAssetPath);

			string filePath = "";
			switch (mUIView.ViewType)
			{
				case UIType.CommonView:
				case UIType.TabView:
					filePath = UIConfigPath;
					break;
				case UIType.Tips:
					filePath = TipsConfigPath;
					break;
				default:
					break;
			}
			if (File.Exists(filePath))
			{
				string[] lines = File.ReadAllLines(filePath);
				int uiIndex = -1;
				for (int i = 0; i < lines.Length; i++)
				{
					if (lines[i].Contains(UIName))
					{
						uiIndex = i;
						break;
					}
				}
				string newLine = "";
				switch (mUIView.ViewType)
				{
					case UIType.CommonView:
					case UIType.TabView:
						newLine = $"\t{UIName} = {"{"} Name = \"{UIName}\", " +
					$"ID = {uiConfig.ID}, " +
					$"View = \"Logic/UI/View/{UIName}/{UIName}\", " +
					$"Layer = LayerGroup.{LayerNames[mUIView.LayerIndex]},  " +
					$"IsTransParent = {mUIView.IsTransParent.ToString().ToLower()},  " +
					$"ShowMoneyBar = {mUIView.ShowMoneyBar.ToString().ToLower()},  " +
					$"DestroyWhenUnload = {mUIView.DestroyOnUnload.ToString().ToLower()} {"}"},";
						break;
					case UIType.Tips:
						newLine = $"\t{UIName} = {"{"} Name = \"{UIName}\", " +
					$"ID = {uiConfig.ID} , View = \"Logic/UI/View/{UIName}/{UIName}\"{"}"},";
						break;
					default:
						break;
				}

				EditorUtility.SetDirty(mUIView);
				PrefabUtility.ApplyPrefabInstance(mUIView.gameObject, InteractionMode.AutomatedAction);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				if (uiIndex >= 0)
				{
					lines[uiIndex] = newLine;
					File.WriteAllLines(filePath, lines);
				}
				else
				{
					string[] newLines = new string[lines.Length + 1];
					Array.Copy(lines, newLines, lines.Length - 1);
					newLines[lines.Length - 1] = newLine;
					newLines[lines.Length] = "}";
					File.WriteAllLines(filePath, newLines);
				}
			}
		}

		private List<string> GetAtlasNames(Dictionary<Transform, string> goPathDic)
		{
			StringBuilder sb = new StringBuilder();
			List<string> atlasNames = new List<string>();
			
			UIImage[] images = mUIView.GetComponentsInChildren<UIImage>();
			for (int i = 0; i < images.Length; i++)
			{
				Sprite sprite = images[i].sprite;
				string spritePath = AssetDatabase.GetAssetPath(sprite);
				if (string.IsNullOrEmpty(spritePath) || !spritePath.Contains("Assets/Art/Picture/"))
				{
					Debug.LogError($"{goPathDic[images[i].transform]}带有默认图片, 请去除");
				}
					
				else
				{
					string atlasName = Path.GetDirectoryName(spritePath).GetLastPartNameOfPath();
					if (atlasName == "Resources")
					{
						Debug.LogError($"{goPathDic[images[i].transform]}带有默认图片, 请去除");
						continue;
					}
					if (!atlasNames.Contains(atlasName))
						atlasNames.Add(atlasName);
					if (goPathDic.ContainsKey(images[i].transform))
					{
						sb.AppendLine($"{atlasName}\t{goPathDic[images[i].transform]}");
					}
					else //自身
						sb.AppendLine($"{atlasName}\t{images[i].name}");

				}
			}

			Debug.Log("图集依赖关系:\n" + sb.ToString());
			return atlasNames;
		}
	}
}
