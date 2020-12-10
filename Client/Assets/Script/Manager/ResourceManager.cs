// ========================================================
// des：
// author: 
// time：2020-07-11 16:55:36
// version：1.0
// ========================================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UObject = UnityEngine.Object;

namespace Game
{
    public class Process
    {
        public bool IsDone = false;
    }

    public class ResourceManager
    {
        public static Dictionary<string, Queue<UObject>> m_Pools = new Dictionary<string, Queue<UObject>>();
        public static Dictionary<int, string> m_Id2PoolDic = new Dictionary<int, string>();
        private static GameObject m_PoolParent;
        //为了保证连续异步加载不出现顺序问题
        private static Dictionary<Image, string> m_img2SpriteName = new Dictionary<Image, string>();

        public static async Task<GameObject> LoadPrefab(string path)
            => await Addressables.InstantiateAsync(path).Task;

        public static AsyncOperationHandle<GameObject> LuaLoadPrefab(string path)
            => Addressables.InstantiateAsync(path);


        public static bool UnloadPrefab(GameObject @object)
            => Addressables.ReleaseInstance(@object);

		public static void UnloadPrefab(Transform transform)
		   => Addressables.ReleaseInstance(transform.gameObject);

		public static async Task<GameObject> LoadPrefabFromePool(string path)
        {
            if (!m_Pools.TryGetValue(path, out var pool))
            {
                pool = new Queue<UObject>();
                m_Pools.Add(path, pool);
            }
            GameObject @object = pool.Count == 0 ? await LoadPrefab(path) : pool.Dequeue() as GameObject;
            m_Id2PoolDic[@object.GetInstanceID()] = path;
            return @object;
        }

        public static async void LoadPrefabFromePool(string path, Action<GameObject> action)
        => action?.Invoke(await LoadPrefabFromePool(path));

        public static void RecyclePrefab(GameObject @object)
        {
            if (m_Id2PoolDic.TryGetValue(@object.GetInstanceID(), out var path)
                && m_Pools.TryGetValue(path, out var pool))
            {
                pool.Enqueue(@object);
                if (!m_PoolParent)
                {
                    m_PoolParent = new GameObject("PoolParent");
                    m_PoolParent.SetActive(false);
                }
                @object.transform.SetParent(m_PoolParent.transform);
            }
            else
            {
                UnloadPrefab(@object);
                Debug.LogError("该物体不是对象池中取出");
            }
        }
        public static async Task<UObject> LoadAsset(string path)
        {
            UObject @object = await Addressables.LoadAssetAsync<UObject>(path).Task;
			
			if (@object ==null)
			{
                Debug.LogError($"can't find {path}");
                return null;
			}
            if (@object is AudioClip)
                (@object as AudioClip).LoadAudioData();
            return @object;
        }


		private static async Task LuaLoadUIInternerl(XLua.LuaTable table, UIConfig info)
		{
			GameObject result = await Addressables.InstantiateAsync(info.Path).Task;
			Canvas canvas = result.GetComponent<Canvas>();
			canvas.worldCamera = Camera.main;
			table.Set("Result", result.GetComponent<UIView>().GetUIView());
			table.Set("IsDone", true);
			table.Set("proess", 1);
			TimeManager.DelayRecycle(table, 1);
		}

		public static XLua.LuaTable LuaLoadUI(int id)
		{
            UIConfig uIConfig = UIConfigAsset.Get(id);
			if (uIConfig != null)
			{
				XLua.LuaTable table = (XLua.LuaTable)XLuaManager.OperationTablePool.Alloc();
				table.Set("IsDone", false);
				table.Set("proess", 0);
				LuaLoadUIInternerl(table, uIConfig);
				return table;
			}
			else
			{
				throw new Exception($"招不到UI路径{id}");
			}
		}

		public static async void LoadAsset(string path, Action<UObject> action)
        => action?.Invoke(await LoadAsset(path));

        public static void UnloadAsset(UObject @object)
            => Addressables.Release(@object);

        //public static byte[] LoadLuaFileSync(string path)
        //    => SyncAddressables.LoadAsset<TextAsset>(m_RootPath + path)?.bytes;

        //public static async Task<TextAsset> LoadLuaFileAsync(string path, Action<TextAsset> action = null)
        //{
        //    /// Log
        //    Debug.Log("LoadLuaFileAsync : " + m_RootPath + path);

        //    TextAsset @object = await Addressables.LoadAssetAsync<TextAsset>(m_RootPath + path).Task;
        //    action?.Invoke(@object);
        //    return @object;
        //}

        //public static void LoadLuaFilesAsync(string label, Action<IList<TextAsset>> action = null)
        //{
        //    /// Log
        //    Debug.Log("LoadLuaFiles label : " + label);

        //    Addressables.LoadAssetsAsync<TextAsset>(label, null).Completed += (aoh) =>
        //    {
        //        action?.Invoke(aoh.Task.Result);
        //    };
        //}

		/// <summary>
		/// 加载图片
		/// </summary>
		/// <param name="atlasName"></param>
		/// <param name="spriteName">不带后缀</param>
		/// <param name="action"></param>
		public static void LoadSprite(string atlasName, string spriteName, Action<Sprite> action = null)
		{
			if (string.IsNullOrEmpty(atlasName) || string.IsNullOrEmpty(spriteName))
			{
				Debug.LogError("图集或者图片名称为空");
				action(null);
			}
			else
			{
				AtlasManager.OnAtlasRequestedAsync(atlasName, (atlas) =>
				{
					Sprite gObj = atlas.GetSprite(spriteName);
					if (gObj == null)
					{
						Debug.LogError("图集中无此图" + spriteName);
						return;
					}
					action?.Invoke(gObj);
				});
			}
		}


		public static void Cleanup()
        {
			if (m_PoolParent)
				GameObject.Destroy(m_PoolParent);
			m_Id2PoolDic.Clear();
			m_Pools.Clear();
			m_img2SpriteName.Clear();
        }
	}
}
