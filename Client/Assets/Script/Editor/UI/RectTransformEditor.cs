//// ========================================================
//// des：
//// author: 
//// time：2020-07-14 15:45:55
//// version：1.0
//// ========================================================
//namespace Game.Editor
//{
//	using UnityEditor;
//	using UnityEngine;

//	[CustomEditor(typeof(RectTransform))]
//	public class RectTransformEditor : DecoratorEditor
//	{
//		public RectTransformEditor() : base("RectTransformEditor") { }
//		public override void OnInspectorGUI()
//		{
//			GameObject rect = Selection.activeGameObject;
//			if (rect == null)
//			{
//				base.OnInspectorGUI();
//				return;
//			}
//			string curSelectedPath = AssetDatabase.GetAssetPath(rect.gameObject);
//			string assetPath = "";
//			if (string.IsNullOrEmpty(curSelectedPath))
//			{
//				// 如果直接获得路径失败，尝试获得当前选中对象的关联父Prefab
//				var curSelectedParentPrefab = PrefabUtility.GetCorrespondingObjectFromSource(rect);
//				assetPath = AssetDatabase.GetAssetPath(curSelectedParentPrefab);
//			}
//			else
//			{
//				assetPath = curSelectedPath;
//			}

//			base.OnInspectorGUI();

//			GUILayout.Space(3);
//			GUILayout.Box(string.Empty, GUILayout.Height(1), GUILayout.MaxWidth(Screen.width - 30));
//			GUILayout.Space(3);
//			GUIContent autoBind = new GUIContent("autoBindView", "auto bind view to prefab");
//			//BindView.AutoBindView = EditorGUILayout.Toggle(autoBind , BindView.AutoBindView);
//			GUILayout.Space(3);
//			//using (new EditorGUILayout.HorizontalScope())
//			//{
//			//	if (GUILayout.Button("Generate Lua View"))
//			//		CreateViewWnd.ShowWindow();

//			//	//if (GUILayout.Button("Generate Lua View All"))
//			//	//	BindView.GenerateViewAll();
//			//}
//			GUILayout.Space(3);
//		}
//	}
//}