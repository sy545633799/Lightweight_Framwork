// ========================================================
// des：
// author: shenyi
// time：2020-06-07 10:51:30
// version：1.0
// ========================================================

#if UNITY_EDITOR
using UnityEngine;
using System.IO;
using System.Text;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.SceneManagement;
using System.Reflection;
using System.Linq;
using Game;
using XLua.LuaDLL;
using Game.Editor;

public enum ScriptType
{
	eLua = 0,
	eCSharp = 1
}

public class BindView
{
	public static string UILuaViewPath = "Lua/Logic/UI/View/";

	static string[] StrAnalysisType =
	{
		"(Text)", "(Button)", "(Toggle)","(Image)", "(Widget)", "(Slider)",
	};

	public static void GenerateView(string assetPath, ScriptType eType)
	{
		GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
		if (prefab == null)
		{
			Debug.LogError("Load prefab failed! path = " + (assetPath));
			return;
		}
		List<GameObject> tagChildren = new List<GameObject>();
		GetTagChildren(prefab, ref tagChildren);

		if (eType == ScriptType.eCSharp)
		{
			//GenerateCSViewFile(prefab.name, tagChildren);
		}
		else if (eType == ScriptType.eLua)
		{
			GenerateLuaViewFile(prefab.name, tagChildren);
		}
		AssetDatabase.Refresh();
	}

	public static void GenerateViewAll(ScriptType eType)
	{
		string[] ids = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets/AssetRes/UI" });

		foreach (var id in ids)
		{
			string file = AssetDatabase.GUIDToAssetPath(id);
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(file);
			if (prefab == null)
			{
				Debug.LogError("Load prefab failed! path = " + (file));
				return;
			}
			List<GameObject> tagChildren = new List<GameObject>();
			GetTagChildren(prefab, ref tagChildren);

			if (eType == ScriptType.eCSharp)
			{
				//GenerateCSViewFile(prefab.name, tagChildren);
			}
			else if (eType == ScriptType.eLua)
			{
				GenerateLuaViewFile(prefab.name, tagChildren);
			}
			AssetDatabase.Refresh();
		}
	}

	static void GetTagChildren(GameObject parent, ref List<GameObject> children)
	{
		if (parent != null)
		{
			for (int i = 0; i < parent.transform.childCount; i++)
			{
				GameObject child = parent.transform.GetChild(i).gameObject;
				string childName = child.name;
				children.Add(child);
				GetTagChildren(child, ref children);
			}
		}
	}

	static string GenerateLuaViewFile(string className, List<GameObject> children)
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine("----------------------- auto generate code --------------------------");
		sb.AppendLine("local base = UIBaseView");
		sb.AppendFormat("---@class {0}:UIBaseView\r\n", className);
		sb.AppendFormat("local view = BaseClass(\"{0}\", base)\r\n", className);
		sb.AppendLine();

		sb.AppendLine("function view:OnCreate()");

		for (int i = 0; i < children.Count; i++)
		{
			string childName = children[i].name;
			string fieldName = GetChildName(children[i]);
			string childPath = GetChildPath(children[i]);
			if (childName.Contains(StrAnalysisType[0]))
			{
				sb.Append("\t---@type UIText\r\n");
				string CompName = "text_" + fieldName;
				sb.AppendFormat("\tself.{0} = self:AddComponent(UIText, UIUtil.FindText(self.gameObject, \"{1}\"))\r\n", CompName, childPath);
			}
			if (childName.Contains(StrAnalysisType[1]))
			{
				sb.Append("\t---@type UIButton\r\n");
				string CompName = "btn_" + fieldName;
				sb.AppendFormat("\tself.{0} = self:AddComponent(UIButton, UIUtil.FindButton(self.gameObject,  \"{1}\"))\r\n", CompName, childPath);
			}
			if (childName.Contains(StrAnalysisType[2]))
			{
				sb.Append("\t---@type UIToggle\r\n");
				string CompName = "Tog_" + fieldName;
				sb.AppendFormat("\tself.{0} = self:AddComponent(UIToggle, UIUtil.FindToggle(self.gameObject,  \"{1}\"))\r\n", CompName, childPath);
			}
			if (childName.Contains(StrAnalysisType[3]))
			{
				sb.Append("\t---@type UIImage\r\n");
				string CompName = "img_" + fieldName;
				sb.AppendFormat("\tself.{0} = self:AddComponent(UIImage, UIUtil.FindImage(self.gameObject,  \"{1}\"))\r\n", CompName, childPath);
			}
			if (childName.Contains(StrAnalysisType[4]))
			{
				sb.Append("\t---@type UIWidget\r\n");
				string CompName = "widget_" + fieldName;
				sb.AppendFormat("\tself.{0} = self:AddComponent(UIWidget, UIUtil.FindRectTransform(self.gameObject,  \"{1}\"))\r\n", CompName, childPath);
			}
			if (childName.Contains(StrAnalysisType[5]))
			{
				sb.Append("\t---@type UISlider\r\n");
				string CompName = "slider_" + fieldName;
				sb.AppendFormat("\tself.{0} = self:AddComponent(UISlider, UIUtil.FindSlider(self.gameObject,  \"{1}\"))\r\n", CompName, childPath);
			}
		}
		sb.AppendLine("end");
		sb.AppendLine();
		sb.AppendLine("return view");

		//if (!Directory.Exists(UILuaViewPath))
		//	Directory.CreateDirectory(UILuaViewPath);
		string file = UILuaViewPath + className + ".lua";
		System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding(false);
		using (StreamWriter textWriter = new StreamWriter(file, false, utf8))
		{
			textWriter.Write(sb.ToString());
			textWriter.Flush();
			textWriter.Close();
		}
		return file;
	}

	static string GetChildName(GameObject child)
	{
		string name = child.name;
		name = Regex.Replace(name, @"\([^\(]*\)", "");
		return name;
	}

	static string GetChildPath(GameObject child)
	{
		string path = child.name;
		GameObject go = child;
		while (go.transform.parent != null && go.transform.parent.parent != null)
		{
			path = go.transform.parent.name + "/" + path;
			go = go.transform.parent.gameObject;
		}
		return path;
	}
}


[CustomEditor(typeof(RectTransform))]
public class prefabInspector : DecoratorEditor
{
	public prefabInspector() : base("RectTransformEditor") { }
	public override void OnInspectorGUI()
	{
		GameObject rect = Selection.activeGameObject;
		if (rect == null)
		{
			base.OnInspectorGUI();
			return;
		}
		string curSelectedPath = AssetDatabase.GetAssetPath(rect.gameObject);
		string assetPath = "";
		if (string.IsNullOrEmpty(curSelectedPath))
		{
			// 如果直接获得路径失败，尝试获得当前选中对象的关联父Prefab
			var curSelectedParentPrefab = PrefabUtility.GetCorrespondingObjectFromSource(rect);
			assetPath = AssetDatabase.GetAssetPath(curSelectedParentPrefab);
		}
		else
		{
			assetPath = curSelectedPath;
		}

		base.OnInspectorGUI();

		GUILayout.Space(3);
		GUILayout.Box(string.Empty, GUILayout.Height(1), GUILayout.MaxWidth(Screen.width - 30));
		GUILayout.Space(3);
		GUIContent autoBind = new GUIContent("autoBindView", "auto bind view to prefab");
		//BindView.AutoBindView = EditorGUILayout.Toggle(autoBind , BindView.AutoBindView);
		GUILayout.Space(3);
		using (new EditorGUILayout.HorizontalScope())
		{
			if (GUILayout.Button("Generate Lua View"))
				BindView.GenerateView(assetPath, ScriptType.eLua);

			if (GUILayout.Button("Generate Lua View All"))
				BindView.GenerateViewAll(ScriptType.eLua);

			//if (GUILayout.Button("Delete bind Script"))
			//{
			//	string luaAssetPath = BindView.UILuaViewPath + Path.GetFileNameWithoutExtension(assetPath) + ".lua";
			//	File.Delete(luaAssetPath);
			//}
			if (GUILayout.Button("Generate UICtrl"))
				CreateCtrlWnd.ShowWindow();
		}
		GUILayout.Space(3);
	}

	string GetLuaScriptFullPath(string assetPath)
	{
		string scriptName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
		return BindView.UILuaViewPath + scriptName + ".lua";
	}
}

public abstract class DecoratorEditor : Editor
{
	// empty array for invoking methods using reflection
	private static readonly object[] EMPTY_ARRAY = new object[0];

	#region Editor Fields

	/// <summary>
	/// Type object for the internally used (decorated) editor.
	/// </summary>
	private System.Type decoratedEditorType;

	/// <summary>
	/// Type object for the object that is edited by this editor.
	/// </summary>
	private System.Type editedObjectType;

	private Editor editorInstance;

	#endregion

	private static Dictionary<string, MethodInfo> decoratedMethods = new Dictionary<string, MethodInfo>();

	private static Assembly editorAssembly = Assembly.GetAssembly(typeof(Editor));

	protected Editor EditorInstance
	{
		get
		{
			if (editorInstance == null && targets != null && targets.Length > 0)
			{
				editorInstance = Editor.CreateEditor(targets, decoratedEditorType);
			}

			if (editorInstance == null)
			{
				Debug.LogError("Could not create editor !");
			}

			return editorInstance;
		}
	}

	public DecoratorEditor(string editorTypeName)
	{
		this.decoratedEditorType = editorAssembly.GetTypes().Where(t => t.Name == editorTypeName).FirstOrDefault();

		Init();

		// Check CustomEditor types.
		var originalEditedType = GetCustomEditorType(decoratedEditorType);

		if (originalEditedType != editedObjectType)
		{
			throw new System.ArgumentException(
				string.Format("Type {0} does not match the editor {1} type {2}",
						  editedObjectType, editorTypeName, originalEditedType));
		}
	}

	private System.Type GetCustomEditorType(System.Type type)
	{
		var flags = BindingFlags.NonPublic | BindingFlags.Instance;

		var attributes = type.GetCustomAttributes(typeof(CustomEditor), true) as CustomEditor[];
		var field = attributes.Select(editor => editor.GetType().GetField("m_InspectedType", flags)).First();

		return field.GetValue(attributes[0]) as System.Type;
	}

	private void Init()
	{
		var flags = BindingFlags.NonPublic | BindingFlags.Instance;

		var attributes = this.GetType().GetCustomAttributes(typeof(CustomEditor), true) as CustomEditor[];
		var field = attributes.Select(editor => editor.GetType().GetField("m_InspectedType", flags)).First();

		editedObjectType = field.GetValue(attributes[0]) as System.Type;
	}

	void OnDisable()
	{
		if (editorInstance != null)
		{
			DestroyImmediate(editorInstance);
		}
	}

	/// <summary>
	/// Delegates a method call with the given name to the decorated editor instance.
	/// </summary>
	protected void CallInspectorMethod(string methodName)
	{
		MethodInfo method = null;

		// Add MethodInfo to cache
		if (!decoratedMethods.ContainsKey(methodName))
		{
			var flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

			method = decoratedEditorType.GetMethod(methodName, flags);

			if (method != null)
			{
				decoratedMethods[methodName] = method;
			}
			else
			{
				Debug.LogError(string.Format("Could not find method {0}", method));
			}
		}
		else
		{
			method = decoratedMethods[methodName];
		}

		if (method != null)
		{
			method.Invoke(EditorInstance, EMPTY_ARRAY);
		}
	}

	public void OnSceneGUI()
	{
		CallInspectorMethod("OnSceneGUI");
	}

	protected override void OnHeaderGUI()
	{
		CallInspectorMethod("OnHeaderGUI");
	}

	public override void OnInspectorGUI()
	{
		EditorInstance.OnInspectorGUI();
	}

	public override void DrawPreview(Rect previewArea)
	{
		EditorInstance.DrawPreview(previewArea);
	}

	public override string GetInfoString()
	{
		return EditorInstance.GetInfoString();
	}

	public override GUIContent GetPreviewTitle()
	{
		return EditorInstance.GetPreviewTitle();
	}

	public override bool HasPreviewGUI()
	{
		return EditorInstance.HasPreviewGUI();
	}

	public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
	{
		EditorInstance.OnInteractivePreviewGUI(r, background);
	}

	public override void OnPreviewGUI(Rect r, GUIStyle background)
	{
		EditorInstance.OnPreviewGUI(r, background);
	}

	public override void OnPreviewSettings()
	{
		EditorInstance.OnPreviewSettings();
	}

	public override void ReloadPreviewInstances()
	{
		EditorInstance.ReloadPreviewInstances();
	}

	public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
	{
		return EditorInstance.RenderStaticPreview(assetPath, subAssets, width, height);
	}

	public override bool RequiresConstantRepaint()
	{
		return EditorInstance.RequiresConstantRepaint();
	}

	public override bool UseDefaultMargins()
	{
		return EditorInstance.UseDefaultMargins();
	}
}

#endif