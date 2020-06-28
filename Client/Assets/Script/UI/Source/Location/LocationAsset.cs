using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class LocationTable
    {
        public int ID;
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
        public string Module;
        public string SimplifiedChinese;
        public string TraditionalChinese;
        public string English;
        public string Japanese;
        public string Korean;
        public string Vietnamese;
    }


    public class LocationAsset : ScriptableObject
    {
        private static LocationAsset instance;
        public static LocationAsset Instance
        {
            get
            {
                if (instance == null)
                    Refresh();
                return instance;
            }
        }

        public List<LocationTable> List;
        public Dictionary<int, LocationTable> Dictionary { get; private set; }
        //编辑器中取值
#if UNITY_EDITOR
        public Dictionary<string, Dictionary<string, int>> NameToLabID { get; private set; }
#endif
        public static void Refresh()
        {
#if UNITY_EDITOR
            instance = UnityEditor.AssetDatabase.LoadAssetAtPath<LocationAsset>("Assets/Art/Assets/Location/Location.asset");
#else
		instance = ResourceManager.LoadAssetSync("Location/Location.asset", EResType.eResAsset) as LocationAsset;
		//ObjectPoolManager.GetSharedResource("Location/Location.asset", EResType.eResAsset) as LocationAsset;
#endif
            instance.Dictionary = new Dictionary<int, LocationTable>();
#if UNITY_EDITOR
            instance.NameToLabID = new Dictionary<string, Dictionary<string, int>>();
            instance.NameToLabID["空"] = new Dictionary<string, int>();
            instance.NameToLabID["空"].Add("", 0);
#endif
            if (instance.List != null)
            {
                for (int i = 0; i < instance.List.Count; i++)
                {
                    LocationTable location = instance.List[i];
                    instance.Dictionary.Add(location.ID, location);
#if UNITY_EDITOR
                    if (!instance.NameToLabID.ContainsKey(location.Module))
                        instance.NameToLabID[location.Module] = new Dictionary<string, int>();
                    instance.NameToLabID[location.Module][location.Text] = location.ID;
#endif
                }
            }

        }

        public static LocationTable GetTable(int id)
        {
            LocationTable table = null;
            if (Instance.Dictionary.TryGetValue(id, out table))
                return table;
            else
                return null;
        }
    }
}