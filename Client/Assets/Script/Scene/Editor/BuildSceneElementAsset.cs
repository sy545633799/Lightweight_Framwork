// ========================================================
// des：
// author: 
// time：2020-12-10 14:52:21
// version：1.0
// ========================================================

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Game.Editor {
	public class BuildSceneElementAsset
	{
		public static string SceneElementAssetPath = "Assets/Art/Assets/Scene";
		public static string PrefabListPath = "Assets/Art/Assets/Config/PrefabPathConfig.asset";
		public static List<string> orderList = new List<string>() {"ground", "mountain", "stone","build",
		"tree", "decorate","effect","animation", "shadow" };

		private static Dictionary<int, string> id2path;

		[MenuItem("Tools/场景相关/导出场景元素信息", priority = 101)]
		public static void Build()
		{
			string sceneName = SceneManager.GetActiveScene().name;
			SceneElementAsset holder = ScriptableObject.CreateInstance<SceneElementAsset>();
			//总包围盒
			Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
			holder.Bounds = bounds;

			int index = 1;
			GameObject[] games = GameObject.FindObjectsOfType<GameObject>();
			Dictionary<string, List<GameObject>> grassDic = new Dictionary<string, List<GameObject>>();
			for (int i = 0; i < games.Length; i++)
			{
				GameObject go = games[i];
				if (!go.activeSelf) continue;

				Object prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(go);
				if (prefab)
				{
					if (go.transform.parent)
					{
						GameObject parent = go.transform.parent.gameObject;
						if (PrefabUtility.GetCorrespondingObjectFromOriginalSource(parent))
							continue;
					}
					string objPath = AssetDatabase.GetAssetPath(prefab);
					if (objPath.EndsWith("fbx") || objPath.EndsWith("FBX") || objPath.EndsWith("obj"))
					{
						Debug.LogError(go.name + " 没有保存成预制物");
						continue;
					}
					if (go.name.Contains("mojiazhuang_build_jishi_06"))
					{
						Debug.Log(go.transform.position);
					}

					int id = index++;
					if (go.transform.parent && go.transform.parent.name.StartsWith("grass"))
					{
						if (!grassDic.ContainsKey(objPath))
							grassDic[objPath] = new List<GameObject>();
						grassDic[objPath].Add(go);
					}
					else
						Record(go, holder, objPath, id); ;
				}
			}
			RecordGrass(grassDic, holder);

			holder.SceneName = sceneName;
			string sceneElementPath = Path.Combine(SceneElementAssetPath, $"{sceneName}");
			if (!Directory.Exists(sceneElementPath))
				Directory.CreateDirectory(sceneElementPath);
			AssetDatabase.CreateAsset(holder, Path.Combine(sceneElementPath, $"{sceneName}.asset"));

			AssetDatabase.Refresh();
		}

		private static void RecordGrass(Dictionary<string, List<GameObject>> grassDic, SceneElementAsset holder)
		{
			foreach (var pair in grassDic)
			{
				List<GameObject> objects = pair.Value;
				MeshRenderer mr = objects[0].GetComponentInChildren<MeshRenderer>();
				MeshFilter mf = objects[0].GetComponentInChildren<MeshFilter>();
				if (mr == null || mf == null) continue;

				GrassGroup group = new GrassGroup();
				group.matrixList = new Matrix4x4[objects.Count];

				for (int i = 0; i < objects.Count; i++)
				{
					Transform trans = objects[i].transform;
					group.matrixList[i] = Matrix4x4.TRS(trans.position, trans.rotation, trans.localScale);
				}
				group.mesh = mf.sharedMesh;
				group.mat = mr.sharedMaterial;
				holder.Add(group);
			}
		}

		private static List<PrefabPathCondig> m_PrefabList;
		private static int SearchPrefabIndex(string path)
		{
			if (m_PrefabList == null)
				m_PrefabList = new List<PrefabPathCondig>(AssetDatabase.LoadAssetAtPath<PrefabPathAsset>(PrefabListPath).Configs);
			PrefabPathCondig condig = m_PrefabList.Find(p => p.Path == path);
			if (condig != null)
				return condig.ID;
			else
			{
				Debug.LogError($"资源列表中没有{path}");
				return -1;
			}
		}

		private static void Record(GameObject o, SceneElementAsset holder, string prefabPath, int id)
		{
			if (!o.transform.parent) return;

			if (o.transform.parent && o.transform.parent.name == "preload")
			{
				PreloadElement element = new PreloadElement();
				element.ResId = SearchPrefabIndex(prefabPath);
				element.position = o.transform.position;
				element.rotation = o.transform.eulerAngles;
				element.scale = o.transform.localScale;
				holder.Add(element);
			}
			else
			{
				MeshRenderer[] mrs = o.GetComponentsInChildren<MeshRenderer>();
				SceneElement element = new SceneElement();
				element.ID = id;
				element.ResId = SearchPrefabIndex(prefabPath);
				if (element.ResId == -1) return;
				element.order = orderList.IndexOf(o.transform.parent.name);
				element.position = o.transform.position;
				element.rotation = o.transform.eulerAngles;
				element.scale = o.transform.localScale;
				element.lightmapIndex = new List<int>();
				element.lightmapOffset = new List<Vector4>();

				if (mrs != null && mrs.Length > 0)
				{
					Vector3 min = mrs[0].bounds.min;
					Vector3 max = mrs[0].bounds.max;
					foreach (MeshRenderer mr in mrs)
					{
						//添加光照贴图
						element.lightmapIndex.Add(mr.lightmapIndex);
						element.lightmapOffset.Add(mr.lightmapScaleOffset);
						//对比包围盒大小
						min = Vector3.Min(mr.bounds.min, min);
						max = Vector3.Max(mr.bounds.max, max);
					}
					Vector3 size = max - min;
					Bounds bounds = new Bounds(min + size / 2, size);
					if (size.x <= 0)
						size.x = 0.2f;
					if (size.y <= 0)
						size.y = 0.2f;
					if (size.z <= 0)
						size.z = 0.2f;
					bounds.size = size;
					element.Bounds = bounds;
					holder.Bounds.Encapsulate(bounds);
				}
				else
				{
					Bounds bounds = new Bounds(element.position, 0.1f * Vector3.one);
					element.Bounds = bounds;
				}
				holder.Add(element);
			}

		}
	}
}
