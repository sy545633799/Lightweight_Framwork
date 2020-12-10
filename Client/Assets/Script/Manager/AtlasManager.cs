// ========================================================
// des：
// author: 
// time：2020-07-10 15:18:17
// version：1.0
// ========================================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;

namespace Game
{
	public class AtlasManager
	{
		private static Dictionary<string, SpriteAtlas> SpriteAtlasMap = new Dictionary<string, SpriteAtlas>();
		private static Dictionary<string, List<Action<SpriteAtlas>>> SpriteAltasActions = new Dictionary<string, List<Action<SpriteAtlas>>>();
		private static string m_RootPath = "Atlas/";

		public static void Init()
		{
			SpriteAtlasManager.atlasRequested += OnAtlasRequestedAsync;
		}

		public static void OnAtlasRequestedAsync(string AtlasName, Action<SpriteAtlas> action)
		{
			TetstAsync(AtlasName, action);
		}

		public static async Task<SpriteAtlas> TetstAsync(string AtlasName, Action<SpriteAtlas> action)
		{
			SpriteAtlas sa;
			if (!SpriteAtlasMap.TryGetValue(AtlasName, out sa))
			{
				if (!SpriteAltasActions.ContainsKey(AtlasName))
				{
					SpriteAltasActions[AtlasName] = new List<Action<SpriteAtlas>>();
					SpriteAltasActions[AtlasName].Add(action);
				}
				else
				{
					SpriteAltasActions[AtlasName].Add(action);
					return sa;
				}
				sa = await ResourceManager.LoadAsset(m_RootPath + $"{AtlasName}/{AtlasName}.spriteatlas") as SpriteAtlas;
				if (sa == null)
				{
					Debug.LogError("加载  " + AtlasName + "失败");
					return sa;
				}
				SpriteAtlasMap.Add(AtlasName, sa);
				foreach (var a in SpriteAltasActions[AtlasName])
				{
					a?.Invoke(sa);
				}
				SpriteAltasActions.Remove(AtlasName);
				//action?.Invoke(sa);
			}
			else
				action?.Invoke(sa);
			return sa;
		}

		public static void Dispose()
		{
			SpriteAtlasManager.atlasRequested -= OnAtlasRequestedAsync;
			SpriteAtlasMap.Clear();
			SpriteAtlasMap = null;
		}
	}
}
