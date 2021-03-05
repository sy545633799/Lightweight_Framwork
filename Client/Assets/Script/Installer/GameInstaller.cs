﻿using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.IO;
using System;

namespace Game
{
	public class GameInstaller : MonoSingleton<GameInstaller>
	{
		public GameSettings GameSettings;
		private long LuaLoadedTime = 0;

		private void Awake()
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			Application.targetFrameRate = GameSettings.PCFrameRate;
#else
			Application.targetFrameRate = GameSettings.MobileFrameRate;
			int width = Screen.width;
			int high = Screen.height;
			if (high > 1334)
			{
				float aspect = (float)(high) / (float)width;
				high = 1334;
				width = (int)(1334.0f / aspect);
			}

			Screen.SetResolution(width, high, true);
#endif
		}

		public async void Start()
		{
			//初始化zstring
			using (zstring.Block()) { }

			LaunchUI.Init();
			List<Func<Task>> initFunctions = new List<Func<Task>>();
			//config
			initFunctions.Add(UILocationAsset.Refresh);
			initFunctions.Add(UICodeTextAsset.Refresh);
			initFunctions.Add(UINamesAsset.Refresh);
			initFunctions.Add(AtlasConfigAsset.Refresh);
			initFunctions.Add(UIConfigAsset.Refresh);
			initFunctions.Add(AudioConfigAsset.Refresh);
			initFunctions.Add(PrefabPathAsset.Refresh);
			initFunctions.Add(ModelConfigAsset.Refresh);
			//setting
			initFunctions.Add(HUDConfigAsset.Load);
			for (int i = 0; i < initFunctions.Count; i++)
			{
				await initFunctions[i].Invoke();
				LaunchUI.ShowProcess((float)(i + 1) / (float)initFunctions.Count * 0.5f);
			}

			SoundManager.Init();
			TcpManager.Init();
			MapManager.Init();
			AOIManager.Init();
            HUDManager.Init();
            XLuaManager.Init();
			XLuaManager.Inject<GameSettings>("GameSettings", GameSettings);
			XLuaManager.StartGame();
			LuaLoadedTime = DateTime.UtcNow.Ticks;
			
		}

		private void OnEnable()
		{
			AtlasManager.Init();
		}

		private void Update()
		{
			InputManager.Update();
			TcpManager.Update();
			//TimeManager放在XLuaManager之后，保证OperationLuaTable即使0帧回收，也不会在XLuaUpdate里被利用
			XLuaManager.Update();
			TimeManager.Update();
			EntityBehaviorManager.Update();
			HUDManager.Update();
		}

		private void LateUpdate()
		{
			XLuaManager.LateUpdate();
#if UNITY_EDITOR
			if (Input.GetKeyDown(KeyCode.F1))
			{
				string path = Application.dataPath.Replace("Assets", "Lua/");
				DirectoryInfo fdir = new DirectoryInfo(path);
				FileInfo[] file = fdir.GetFiles("*.lua", SearchOption.AllDirectories);


				for (int i = 0; i < file.Length; i++)
				{
					if (file[i].LastWriteTimeUtc.Ticks - LuaLoadedTime > 0)
					{
						//string luaFile = file[i].FullName.Replace("\\", "/").Replace(path, "").Replace(".lua", "");
						//XLuaManager.HotfixLua(luaFile);
					}
				}

				LuaLoadedTime = DateTime.UtcNow.Ticks;
			}

#endif
			
		}

		private void FixedUpdate()
		{
			TcpManager.FixedUpdate();
			TimeManager.FixedUpdate();
			XLuaManager.FixedUpdate();
			EntityBehaviorManager.FixedUpdate();
		}

		private void OnLevelWasLoaded()
		{
			XLuaManager.OnLevelWasLoaded();
		}

		private async void OnApplicationQuit()
		{
			AtlasManager.Dispose();
			XLuaManager.Dispose();
			AOIManager.Dispose();
			TcpManager.Dispose();
			TimeManager.Dispose();
			MapManager.Dispose();
			HUDManager.Dispose();
			//why
			await Task.Delay(1);
		}
	}
}