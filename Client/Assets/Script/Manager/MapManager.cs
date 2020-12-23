// ========================================================
// des：shenyi
// author: 
// time：2020-08-09 15:04:41
// version：1.0
// ========================================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game {
	public class MapManager
	{
		public static string Name;
		/// <summary>
		/// 挂载体
		/// </summary>
		private static GameObject m_Env;
		/// <summary>
		/// 主灯光
		/// </summary>
		private static Light m_Light;

		private static int LoadCount = 20;

		/// <summary>
		/// 当前正在加载的index
		/// </summary>
		private static int m_CurrentLoadIndex;
		/// <summary>
		/// 还未加载的元素
		/// </summary>
		private static List<SceneElement> m_CurrenLoadElements;
		/// <summary>
		/// 正在加载的物体
		/// </summary>
		private static List<SceneElement> m_LoadingObjectList = new List<SceneElement>();
		/// <summary>
		/// 已经加载的物体
		/// </summary>
		private static List<GameObject> m_LoadedObjectList = new List<GameObject>();

		private static SceneEnvAsset m_CurrentEnv;
		private static SceneElementAsset m_CurrentElements;
		private static System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

		private static Camera m_Cam;
		private static CommandBuffer cmdBuffer;
		private static Dictionary<int, string> Id2Path = new Dictionary<int, string>();

		public static void Init()
		{
			cmdBuffer = new CommandBuffer();
			m_Cam = Camera.main;
			m_Cam.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, cmdBuffer);
			Dictionary<int, PrefabPathConfig> map = PrefabPathAsset.GetAll();
			foreach (var item in map)
			{
				Id2Path.Add(item.Key, item.Value.Path);
			}
		}


		public static async void LoadMapInternal(XLua.LuaTable precess,string mapName, Action callback = null)
		{
			m_Env = GameObject.Find("Env");
			m_Light = GameObject.Find("Sun").GetComponent<Light>();
			//1.加载场景信息
			string InfoPath = $"Assets/Art/Scenes/{mapName}/{mapName}/sceneenv.asset";
			SceneEnvAsset env = await ResourceManager.LoadAsset(InfoPath) as SceneEnvAsset;
			string ElementPath = $"Assets/Art/Assets/Scene/{mapName}/{mapName}.asset";
			//2.加载地形
			SceneElementAsset sceneElements = await ResourceManager.LoadAsset(ElementPath) as SceneElementAsset;
			List<GameObject> list = await MapManager.LoadPreElement(sceneElements);

			m_CurrentEnv = env;
			m_CurrentElements = sceneElements;
			m_LoadedObjectList.AddRange(list);
			callback?.Invoke();
			//4.优先展示出来基本的灯光、草的环境
			Show(true);
			Name = mapName;
			//5.最后去异步加载出来场景中的元素
			AddSceneElements();


			////3.如果是野外场景需要动态生成导航网格，战斗场景和UI场景不需要
			//if (env.sceneType == SceneType.Wild)
			//{
			//	LocalNavMeshLoading.Instance.UpdateNavMesh(true);
			//	Debug.Log($"[C#] SceneTree.cs ::load({mapName}) 展示基本灯光、草后开启异步加载场景元素并开始异步烘焙导航网格");
			//}

			callback?.Invoke();
			//加载完成, 通知lua
			precess.Set("IsDone", true);
			precess.Set("proess", 1);
		}

		public static XLua.LuaTable LoadMap(string mapName, Action callback = null)
		{
			XLua.LuaTable table = (XLua.LuaTable)XLuaManager.OperationTablePool.Alloc();
			table.Set("IsDone", false);
			table.Set("proess", 0);
			LoadMapInternal(table, mapName, callback);
			return table;
		}

		public static void Show(bool active)
		{
			if (active)
			{
				m_CurrentEnv.lightData.SetLight(m_Light);
				m_CurrentEnv.SetEnv();
				ShowGrass(true);
			}
			else
			{
				ShowGrass(false);
			}
			
		}

		public static void ShowGrass(bool show)
		{
			if (show)
			{
				if (m_CurrentElements != null && m_CurrentElements.grass != null)
				{
					foreach (var grass in m_CurrentElements.grass)
						cmdBuffer.DrawMeshInstanced(grass.mesh, 0, grass.mat, 0, grass.matrixList, grass.matrixList.Length);
				}
			}
			else
				cmdBuffer.Clear();
		}


		public static async Task<GameObject> LoadSceneElement(int id)
		{
			string path;
			if (Id2Path.TryGetValue(id, out path))
			{
				GameObject go = await ResourceManager.LoadPrefab(path);
				if (!go) Debug.LogError($"{path} 资源不存在");
				return go;
			}
			else
			{
				Debug.LogError($"id:{id} 资源不存在");
				return null;
			}
		}

		public static async Task<List<GameObject>> LoadPreElement(SceneElementAsset sceneElements)
		{
			List<GameObject> list = new List<GameObject>();
			//三.加载地形
			if (sceneElements.preload != null)
			{
				for (int i = 0; i < sceneElements.preload.Count; i++)
				{
					PreloadElement element = sceneElements.preload[i];
					GameObject @object = await LoadSceneElement(element.ResId);
					@object.transform.parent = m_Env.transform;
					@object.transform.position = element.position;
					@object.transform.eulerAngles = element.rotation;
					@object.transform.localScale = element.scale;
					list.Add(@object);
				}
			}

			return list;
		}

		public static async Task LoadSceneElement(SceneElement e)
		{
			if (e.doRelease)
			{
				e.doRelease = false;
				m_LoadingObjectList.Add(e);
				return;
			}
			e.doRelease = false;
			m_LoadingObjectList.Add(e);
			GameObject @object = await LoadSceneElement(e.ResId);
			if (e.doRelease)
			{
				if (!ResourceManager.UnloadPrefab(@object))
					GameObject.Destroy(@object);
				e.doRelease = false;
			}
			else
			{
				m_LoadingObjectList.Remove(e);
				@object.transform.parent = m_Env.transform;
				@object.transform.position = e.position;
				@object.transform.eulerAngles = e.rotation;
				@object.transform.localScale = e.scale;
				m_LoadedObjectList.Add(@object);
				//LoadOperation.process = (float)m_LoadedObjectList.Count / m_CurrenLoadElements.Count;
				if (m_LoadedObjectList.Count == m_CurrenLoadElements.Count)
				{
					stopwatch.Stop();
					Debug.Log($"加载场景花费了: {stopwatch.ElapsedMilliseconds} (ms)");
					// StaticBatchingUtility.Combine(m_Env);//这里别删除UnityAPI合批 后续看时机再启用
					//LoadOperation.IsDone = true;
				}
				if (m_CurrenLoadElements.Count > m_CurrentLoadIndex)
					LoadSceneElement(m_CurrenLoadElements[m_CurrentLoadIndex++]);

			}
		}

		public static void AddSceneElements()
		{
			int loadCount = LoadCount > m_CurrentElements.content.Count ? m_CurrentElements.content.Count : LoadCount;
			m_CurrentLoadIndex = 0;
			m_CurrenLoadElements = m_CurrentElements.content;
			for (int i = 0; i < loadCount; i++)
			{
				if (m_CurrenLoadElements.Count > m_CurrentLoadIndex)
					LoadSceneElement(m_CurrenLoadElements[m_CurrentLoadIndex++]);
			}
		}


		public static void Cleanup()
		{
			m_Env = null;
			m_Light = null;
			cmdBuffer.Clear();
			//for (int i = 0; i < m_LoadingObjectList.Count; i++)
			//	m_LoadingObjectList[i].doRelease = true;
			m_LoadingObjectList.Clear();
			for (int i = 0; i < m_LoadedObjectList.Count; i++)
			{
				if (!ResourceManager.UnloadPrefab(m_LoadedObjectList[i]))
					GameObject.Destroy(m_LoadedObjectList[i]);
			}

			m_LoadedObjectList.Clear();
			if (m_CurrentElements)
			{
				ResourceManager.UnloadAsset(m_CurrentElements);
				m_CurrentElements = null;
			}
			if (m_CurrentEnv)
			{
				ResourceManager.UnloadAsset(m_CurrentEnv);
				m_CurrentEnv = null;
			}
			Name = null;
		}

		public static void Release ()
		{
			Cleanup();
			m_Cam.RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha, cmdBuffer);
		}
	}
}
