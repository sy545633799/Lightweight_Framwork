using System.Collections.Generic;
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
			if (width > 1920)
			{
				float aspect = (float)(width) / (float)high;
				width = 1920;
				high = (int)(1920.0f / aspect);
			}

			Debug.Log(Screen.width + "-" + Screen.height);
			Screen.SetResolution(Screen.width, Screen.height, true);
			Debug.Log(Screen.width + ":" + Screen.height);
#endif
			//TaskInfo info = TimeManager.AddTaskYearly(11, 4, 5, 0, 0, 0, null);
		}

		public async void Start()
		{
			//初始化zstring
			using (zstring.Block()) { }

			List<Func<Task>> initFunctions = new List<Func<Task>>();
			//launch
			initFunctions.Add(LaunchUI.Init);
			//config
			initFunctions.Add(UILocationAsset.Refresh);
			initFunctions.Add(UICodeTextAsset.Refresh);
			initFunctions.Add(AtlasConfigAsset.Refresh);
			initFunctions.Add(UIConfigAsset.Refresh);
			//manager
			initFunctions.Add(SoundManager.Init);
			initFunctions.Add(TcpManager.Init);
			initFunctions.Add(MapManager.Init);
			initFunctions.Add(EntityBehaviorManager.Init);
			initFunctions.Add(AOIManager.Init);
			for (int i = 0; i < initFunctions.Count; i++)
			{
				await initFunctions[i].Invoke();
				LaunchUI.ShowProcess((float)(i + 1) / (float)initFunctions.Count * 0.5f);
			}

			await XLuaManager.Init();
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
			MapManager.Release();
			//why
			await Task.Delay(1);
		}
	}
}