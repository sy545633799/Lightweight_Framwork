// ========================================================
// des：shenyi
// author: 
// time：2020-08-09 15:04:41
// version：1.0
// ========================================================

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class MapManager
	{
		private static GameObject m_Map;
		private static List<GameObject> m_Levels = new List<GameObject>();

		public static async void LoadMap(string path, int tagetIndex,  Action<int> callback)
		{
			m_Map = await ResourceManager.LoadPrefab(path);
			CameraDrag.Instance.SetMap(m_Map);
			int index = 1;
			while (true)
			{
				int i = index++;
				Transform level = m_Map.transform.Find($"Group/Level{i}");
				if (!level) break;
				if (i == tagetIndex)
					CameraDrag.Instance.MoveToWorldPosition(level);
				level.gameObject.AddClickListener(() => callback?.Invoke(i));
				m_Levels.Add(level.gameObject);
			}
		}

		public static void ReleaseMap()
		{
			ResourceManager.UnloadPrefab(m_Map);
			m_Map = null;
			for (int i = 0; i < m_Levels.Count; i++)
				m_Levels[i].RemoveAllListener();
			m_Levels.Clear();
			CameraDrag.Instance.SetMap(null);
			CameraDrag.Instance.transform.position = Vector3.zero;
		}
	}
}
