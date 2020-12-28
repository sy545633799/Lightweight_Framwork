// ========================================================
// des：
// author: 
// time：2020-12-23 20:32:01
// version：1.0
// ========================================================

using System.Threading.Tasks;
using UnityEngine;

namespace Game {
	public class SettingBaseAsset<T> : ScriptableObject
		where T : SettingBaseAsset<T>
	{
		private static T Instance;
		public static async Task Load()
		{
#if UNITY_EDITOR
			string path = $"Assets/Art/Assets/Setting/{typeof(T).Name}.asset";
			Instance = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
#else
			//编辑器情况下无法使用await
			//using (zstring.Block())
			//{
				 Instance = await ResourceManager.LoadAsset($"Assets/Art/Assets/Setting/{typeof(T).Name}.asset") as SettingBaseAsset<T>;
			//}
#endif
		}

		public static T Get()
		{
			return Instance;
		}
	}
}
