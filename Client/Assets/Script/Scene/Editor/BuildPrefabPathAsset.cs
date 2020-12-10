// ========================================================
// des：
// author: 
// time：2020-12-10 14:44:10
// version：1.0
// ========================================================

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Game.Editor {
	public class BuildPrefabPathAsset 
	{
		public static string PrefabListPath = "Assets/Art/Assets/Config/PrefabPathConfig.asset";
		[MenuItem("Tools/场景相关/导出场景资源列表", priority = 100)]
		public static void Build()
		{
			PrefabPathAsset configAsset;
			List<PrefabPathCondig> configs;
			if (File.Exists(PrefabListPath))
			{
				configAsset = AssetDatabase.LoadAssetAtPath<PrefabPathAsset>(PrefabListPath);
				configs = new List<PrefabPathCondig>(configAsset.Configs);
			}
			else
			{
				configAsset = new PrefabPathAsset();
				configs = new List<PrefabPathCondig>();
			}

			DirectoryInfo root = new DirectoryInfo(Application.dataPath + "/Art/SceneRes");
			FileInfo[] fileInfos = root.GetFiles("*.prefab", SearchOption.AllDirectories);

			int index = configs.Count == 0 ? 1 : configs[configs.Count - 1].ID + 1;
			for (int i = configs.Count - 1; i >= 0; i--)
			{
				string path = configs[i].Path;
				if (!File.Exists(path))
					configs.Remove(configs[i]);
			}


			foreach (var item in fileInfos)
			{
				string path = item.FullName.Replace("\\", "/");
				path = path.Replace(Application.dataPath, "Assets");
				if (configs.Find(p => p.Path == path) == null)
				{
					PrefabPathCondig config = new PrefabPathCondig();
					config.ID = index++;
					config.Path = path;
					configs.Add(config);
				}
			}
			configAsset.Configs = configs.ToArray();
			if (File.Exists(PrefabListPath))
				EditorUtility.SetDirty(configAsset);
			else
				AssetDatabase.CreateAsset(configAsset, PrefabListPath);
		}
	}
}
