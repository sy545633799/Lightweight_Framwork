using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UObject = UnityEngine.Object;

namespace Game
{
    public class ResourceManager
    {
        public static async void LoadPrefab(string path, Action<UObject> action)
        {
            string totalPath = "Assets/Art/" + path;
			//UObject @object = await Addressables.LoadAssetAsync<UObject>("Assets/Art/" + path).Task;
			GameObject go = await Addressables.InstantiateAsync(totalPath).Task;
			action?.Invoke(go);
		}

		public static void UnloadAssets(GameObject @object)
        {
            Addressables.ReleaseInstance(@object);
        }
    }
}