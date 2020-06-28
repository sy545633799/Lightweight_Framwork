// ========================================================
// des：
// author: 
// time：2020-06-13 13:37:59
// version：1.0
// ========================================================

using Boo.Lang;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

namespace Game.Editor {
	public class CreateCtrlWnd : EditorWindow
    {
        private string UICtrlName = "";
        private string FolderName = "Lua/Logic/UI/Controller/";
        private string AuthorName = "Auto";
        private string description = "请输入内容描述";
        private string LuaScriptName = "";
        private string UIConfigPath = "Lua/Logic/UI/UIConfig.lua";
        private string PrefabFloder = "";
        private string UICtrlFloder = "UI/Controller/";
        private string UIManagerText = "";
        private string SplitIndex = "--#预制体名字# == {'#预制体路径#','#UICTRL路径#'},";
        private string NextIndex = "--#在末尾自动添注释，便于下次生成#";
        private string CustomFloder = "";
        private int LayerIndex = 3;
        private string[] LayerNames = { "Scene", "Damage", "Base", "Pop", "Guide", "Lock", "Network", "Loading"};
        private bool DestroyOnUnload = true;
        
        private GameObject SelectPrefab;

        CreateCtrlWnd()
        {
            this.titleContent = new GUIContent("Auto UICtrl");
        }

        public static void ShowWindow()
		{
            EditorWindow.GetWindow(typeof(CreateCtrlWnd));
        }

        [SerializeField]
        void OnGUI()
        {
            GUILayout.BeginVertical();

            //绘制标题
            GUILayout.Space(10);
            GUI.skin.label.fontSize = 24;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Auto UICtrl");

            //绘制当前时间
            GUILayout.Space(10);
            GUI.skin.label.fontSize = 15;
            GUILayout.Label("Time: " + System.DateTime.Now);

            //要保存的文件夹
            GUILayout.Space(10);
            GUI.skin.label.fontSize = 12;
            GUILayout.Space(10);
            if(string.IsNullOrEmpty(UICtrlName) || SelectPrefab != Selection.activeObject) 
                UICtrlName = Selection.activeObject.name;
            UICtrlName = EditorGUILayout.TextField("UIConfig键名", UICtrlName);
            GUILayout.Space(10);
            CustomFloder = EditorGUILayout.TextField("请填写文件夹", CustomFloder);
            GUILayout.Space(10);
            FolderName = EditorGUILayout.TextField("目标文件夹", "Lua/Logic/UI/Controller/" + CustomFloder);
            GUILayout.Space(10);
            LayerIndex = EditorGUILayout.Popup("UI层级", LayerIndex, LayerNames);
            GUILayout.Space(10);
            DestroyOnUnload = EditorGUILayout.Toggle("关闭时是否销毁", DestroyOnUnload);
            //选择的预设物
            GUILayout.Space(10);
            SelectPrefab = (GameObject)EditorGUILayout.ObjectField("选中的prefab" + "Prefab", Selection.activeObject, typeof(GameObject), true);

            string curSelectedPath = AssetDatabase.GetAssetPath(SelectPrefab.gameObject);
            string assetPath = "";
            if (string.IsNullOrEmpty(curSelectedPath))
            {
                // 如果直接获得路径失败，尝试获得当前选中对象的关联父Prefab
                var curSelectedParentPrefab = PrefabUtility.GetCorrespondingObjectFromSource(SelectPrefab);
                assetPath = AssetDatabase.GetAssetPath(curSelectedParentPrefab);
            }
            else
            {
                assetPath = curSelectedPath;
            }

            string[] PrefabPath = assetPath.Split('/');
            PrefabFloder = PrefabPath[PrefabPath.Length - 2] + "/" + Selection.activeObject.name;

            //作者名
            GUILayout.Space(10);
            AuthorName = EditorGUILayout.TextField("作者", AuthorName);

            //绘制当前正在编辑的场景
            GUILayout.Space(10);
            GUI.skin.label.fontSize = 12;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            //GUILayout.Label("Currently Scene:" + EditorSceneManager.GetActiveScene().name);

            //绘制描述文本区域
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Description", GUILayout.MaxWidth(80));
            description = EditorGUILayout.TextArea(description, GUILayout.MaxHeight(75));
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            //添加名为"Create"按钮，用于调用创建Lua脚本函数
            if (GUILayout.Button("Create"))
            {
                GenUICtrl();
                GenUIConfig();
            }
            GUILayout.EndVertical();
        }

        private void GenUICtrl()
        {
            string viewName = Selection.activeObject.name;
            string ctrlName = UICtrlName + "Ctrl";
            string ctrlInfo = $@"
-------------------------------------------------------------
local base = UIBaseCtrl
---@type UIBaseCtrl
local {ctrlName} = BaseClass('{ctrlName}', base)
-------------------------------------------------------------

function {ctrlName}: OnCreate(view)
    ---@type {viewName}
    self.view = view
end

function {ctrlName}: OnLoad(...)
end

function {ctrlName}: OnUnLoad()

end

function {ctrlName}: OnDestroy()

end

return {ctrlName}";
			if (!Directory.Exists(FolderName))
			{
                Directory.CreateDirectory(FolderName);
			}
            string ctrlPath = Path.Combine(FolderName, UICtrlName) + "Ctrl.lua";
            File.WriteAllText(ctrlPath, ctrlInfo);
        }

        private void GenUIConfig()
        {
            GUILayout.Space(10);
            string _name = Selection.activeObject.name;
            string _prefabpath = PrefabFloder;
            string _uictrlname = UICtrlFloder + CustomFloder + "/" + _name + "Ctrl";
            _uictrlname = _uictrlname.Replace("//", "/");
			if (File.Exists(UIConfigPath))
			{
                string[] lines = File.ReadAllLines(UIConfigPath);
                int index = -1;
				for (int i = 0; i < lines.Length; i++)
				{
					if (lines[i].Contains(UICtrlName))
					{
                        index = i;
                        break;
					}
				}

                string viewName = Selection.activeObject.name;
                var curSelectedParentPrefab = PrefabUtility.GetCorrespondingObjectFromSource(SelectPrefab);
                var uiPath = AssetDatabase.GetAssetPath(curSelectedParentPrefab).Replace("Assets/Art/", "");
                string ctrlInfo = $"UIConfig.{UICtrlName} = {"{"} Name = \"{UICtrlName}\", Path = \"{uiPath}\", View = \"Logic/UI/View/{viewName}\", Ctrl = \"{Path.Combine(FolderName.Replace("Lua/", ""), UICtrlName).Replace("\\", "/")}Ctrl\", Layer = LayerGroup.{LayerNames[LayerIndex]},  DestroyWhenUnload = {DestroyOnUnload.ToString().ToLower()}{"}"}";
				if (index >= 0)
				{
					lines[index] = ctrlInfo;
                    File.WriteAllLines(UIConfigPath, lines);
				}
				else
				{
                    File.AppendAllText(UIConfigPath, "\r\n" + ctrlInfo);
				}
			}
        }

    }
}
