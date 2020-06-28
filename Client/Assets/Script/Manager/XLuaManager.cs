using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using XLua;

namespace Game
{
    [Hotfix]
    [LuaCallCSharp]
    public class XLuaManager
    {
        public const string luaAssetbundleAssetName = "Lua";
        const string commonMainScriptName = "Common.Main";
        const string gameMainScriptName = "GameMain";
        const string hotfixMainScriptName = "HotfixMain";
        private static LuaEnv luaEnv = null;

		private static Action<float, float> luaUpdate = null;
		private static Action luaLateUpdate = null;
		private static Action<float> luaFixedUpdate = null;

#if UNITY_EDITOR
#pragma warning disable 0414
		// added by wsh @ 2017-12-29
		[SerializeField]
		private static long updateElapsedMilliseconds = 0;
		[SerializeField]
		private static long lateUpdateElapsedMilliseconds = 0;
		[SerializeField]
		private static long fixedUpdateElapsedMilliseconds = 0;
#pragma warning restore 0414
		private static Stopwatch sw = new Stopwatch();
#endif

		public static void Init()
        {
            //string path = AssetBundleUtility.PackagePathToAssetsPath(luaAssetbundleAssetName);
            //AssetbundleName = AssetBundleUtility.AssetBundlePathToAssetBundleName(path);
            InitLuaEnv();
			OnInit();
		}

        public static bool HasGameStart
        {
            get;
            protected set;
        }

		public static string AssetbundleName
		{
			get;
			protected set;
		}

		public static LuaEnv GetLuaEnv()
        {
            return luaEnv;
        }

		private static void InitLuaEnv()
        {
            luaEnv = new LuaEnv();
            HasGameStart = false;
            if (luaEnv != null)
            {
                luaEnv.AddLoader(CustomLoader);
				//         luaEnv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
				luaEnv.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
				luaEnv.AddBuildin("sproto.core", XLua.LuaDLL.Lua.LoadSproto);
				luaEnv.AddBuildin("crypt", XLua.LuaDLL.Lua.LoadCrypt);
			}
            else
            {
                Logger.LogError("InitLuaEnv null!!!");
            }
        }

        // 这里必须要等待资源管理模块加载Lua AB包以后才能初始化
        public static void OnInit()
        {
            if (luaEnv != null)
            {
                LoadScript(commonMainScriptName);
#if UNITY_EDITOR
				sw.Start();
#endif
				luaUpdate = luaEnv.Global.Get<Action<float, float>>("Update");
				luaLateUpdate = luaEnv.Global.Get<Action>("LateUpdate");
				luaFixedUpdate = luaEnv.Global.Get<Action<float>>("FixedUpdate");
			}
        }

		// 重启虚拟机：热更资源以后被加载的lua脚本可能已经过时，需要重新加载
		// 最简单和安全的方式是另外创建一个虚拟器，所有东西一概重启
		public static void Restart()
        {
            StopHotfix();
            Dispose();
            InitLuaEnv();
            OnInit();
        }

        public static void SafeDoString(string scriptContent)
        {
            if (luaEnv != null)
            {
                try
                {
                    luaEnv.DoString(scriptContent);
                }
                catch (System.Exception ex)
                {
                    string msg = string.Format("xLua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                    Logger.LogError(msg, null);
                }
            }
        }

        public static void StartHotfix(bool restart = false)
        {
            if (luaEnv == null)
            {
                return;
            }

            if (restart)
            {
                StopHotfix();
                ReloadScript(hotfixMainScriptName);
            }
            else
            {
                LoadScript(hotfixMainScriptName);
            }
            SafeDoString("HotfixMain.Start()");
        }

        public static void StopHotfix()
        {
            SafeDoString("HotfixMain.Stop()");
        }

        public static void StartGame()
        {
            if (luaEnv != null)
            {
                LoadScript(gameMainScriptName);
                SafeDoString("GameMain.Start()");
                HasGameStart = true;
            }
        }

        public static void ReloadScript(string scriptName)
        {
            SafeDoString(string.Format("package.loaded['{0}'] = nil", scriptName));
            LoadScript(scriptName);
        }

        private static void LoadScript(string scriptName)
        {
            SafeDoString(string.Format("require('{0}')", scriptName));
        }

        public static byte[] CustomLoader(ref string filepath)
        {
            string scriptPath = string.Empty;
            filepath = filepath.Replace(".", "/") + ".lua";

            scriptPath = Path.Combine(Application.persistentDataPath, "Lua");
            scriptPath = Path.Combine(scriptPath, filepath);
            if (File.Exists(scriptPath))
                return GameUtility.SafeReadAllBytes(scriptPath);

#if UNITY_EDITOR
            string path = Application.dataPath + "/../Lua/" + filepath;
            return GameUtility.SafeReadAllBytes(path);
#endif


            //scriptPath = string.Format("{0}/{1}.bytes", luaAssetbundleAssetName, filepath);

            //      string assetbundleName = null;
            //      string assetName = null;
            //      bool status = AssetBundleManager.Instance.MapAssetPath(scriptPath, out assetbundleName, out assetName);
            //      if (!status)
            //      {
            //          Logger.LogError("MapAssetPath failed : " + scriptPath);
            //          return null;
            //      }
            //      var asset = AssetBundleManager.Instance.GetAssetCache(assetName) as TextAsset;
            //      if (asset != null)
            //      {
            //          //Logger.Log("Load lua script : " + scriptPath);
            //          return asset.bytes;
            //      }
            //      Logger.LogError("Load lua script failed : " + scriptPath + ", You should preload lua assetbundle first!!!");
            return null;
        }

        public static void Update()
        {
            if (luaEnv != null)
            {
                luaEnv.Tick();

                if (Time.frameCount % 100 == 0)
                {
                    luaEnv.FullGc();
                }

				if (luaUpdate != null)
				{
#if UNITY_EDITOR
					var start = sw.ElapsedMilliseconds;
#endif
					try
					{
						luaUpdate(Time.deltaTime, Time.unscaledDeltaTime);
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogError("luaUpdate err : " + ex.Message + "\n" + ex.StackTrace);
					}
#if UNITY_EDITOR
					updateElapsedMilliseconds = sw.ElapsedMilliseconds - start;
#endif
				}
			}
        }

		public static void LateUpdate()
		{
			if (luaLateUpdate != null)
			{
#if UNITY_EDITOR
				var start = sw.ElapsedMilliseconds;
#endif
				try
				{
					luaLateUpdate();
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError("luaLateUpdate err : " + ex.Message + "\n" + ex.StackTrace);
				}
#if UNITY_EDITOR
				lateUpdateElapsedMilliseconds = sw.ElapsedMilliseconds - start;
#endif
			}
		}

		public static void FixedUpdate()
		{
			if (luaFixedUpdate != null)
			{
#if UNITY_EDITOR
				var start = sw.ElapsedMilliseconds;
#endif
				try
				{
					luaFixedUpdate(Time.fixedDeltaTime);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError("luaFixedUpdate err : " + ex.Message + "\n" + ex.StackTrace);
				}
#if UNITY_EDITOR
				fixedUpdateElapsedMilliseconds = sw.ElapsedMilliseconds - start;
#endif
			}
		}

		public static void OnLevelWasLoaded()
        {
            if (luaEnv != null && HasGameStart)
            {
                SafeDoString("GameMain.OnLevelWasLoaded()");
            }
        }

        public static void OnApplicationQuit()
        {
            if (luaEnv != null && HasGameStart)
            {
                SafeDoString("GameMain.OnApplicationQuit()");
            }
        }

        public static void Dispose()
        {
			luaUpdate = null;
			luaLateUpdate = null;
			luaFixedUpdate = null;
			if (luaEnv != null)
            {
                try
                {
                    luaEnv.Dispose();
                    luaEnv = null;
                }
                catch (System.Exception ex)
                {
                    string msg = string.Format("xLua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                    Logger.LogError(msg, null);
                }
            }
        }


	}
}