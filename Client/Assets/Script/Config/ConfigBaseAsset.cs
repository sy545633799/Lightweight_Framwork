// ========================================================
// des：
// author: 
// time：2020-09-22 09:26:07
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game {

	public class ConfigBase
	{
		public int ID;
	}

	public class LocationConfig: ConfigBase
	{
		public string SimplifiedChinese;
		public string TraditionalChinese;
		public string English;
		public string Japanese;
		public string Korean;
		public string Vietnamese;

		public string Text
		{
			get
			{
#if UNITY_EDITOR
				SystemLanguage language;
				if (UnityEditor.EditorPrefs.GetInt("Language") == 0)
					language = SystemLanguage.Chinese;
				else
					language = (SystemLanguage)UnityEditor.EditorPrefs.GetInt("Language");
#else
			SystemLanguage language = Application.systemLanguage;
#endif
				switch (language)
				{
					case SystemLanguage.Chinese:
						return SimplifiedChinese;
					case SystemLanguage.English:
						return English;
					case SystemLanguage.Japanese:
						return Japanese;
					case SystemLanguage.Vietnamese:
						return Vietnamese;
					case SystemLanguage.ChineseSimplified:
						return SimplifiedChinese;
					case SystemLanguage.ChineseTraditional:
						return TraditionalChinese;
					default:
						return English;
				}
			}
		}
	}

	public class ConfigBaseAsset<T> : ScriptableObject
		where T: ConfigBase
		
	{
		public T[] Configs;

		private static Dictionary<int, T> Map = new Dictionary<int, T>();
		private static ConfigBaseAsset<T> Instance;
		public static async Task Refresh()
		{
#if UNITY_EDITOR
			string path = $"Assets/Art/Assets/Config/{typeof(T).Name}.asset";
			Instance = UnityEditor.AssetDatabase.LoadAssetAtPath<ConfigBaseAsset<T>>(path);
#else
			//编辑器情况下无法使用await
			//using (zstring.Block())
			//{
				Instance = await ResourceManager.LoadAsset($"Assets/Config/{typeof(T).Name}.asset") as ConfigBaseAsset<T>;
			//}
				
#endif
			Map.Clear();
			if (Instance.Configs != null)
			{
				for (int i = 0; i < Instance.Configs.Length; i++)
				{
					T t = Instance.Configs[i];
					Map.Add(t.ID, t);
				}
			}

		}

		public static T Get(int id)
		{	
			T table = null;
			if (Map.TryGetValue(id, out table))
				return table;
			else
				return null;
		}


		public static Dictionary<int, T> GetAll()
		{
			return Map;
		}
	}
}
