using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using XLua;

namespace Game
{
	public static class SceneNames
	{
		public const string LaunchScene = "LaunchScene";
		public const string LoadingScene = "LoadingScene";
		public const string LoginScene = "LoginScene";
		public const string HomeScene = "HomeScene";
		public const string BattleScene = "BattleScene";
	}

	public class LoadManager
	{
		private static string TargetScene = "";
		private static Action LoadCompleteCallaback;
		private static AsyncOperation m_AsyncOperation;
		private static LuaTable m_Operation;

		public static string CurrenScene { get; private set; } = SceneNames.LaunchScene;
		public static bool IsLoading { get; private set; } = false;

		public static LuaTable Load(string sceneName, Action loadCompleteCallback)
		{
			ResourceManager.Cleanup();

			TargetScene = sceneName;
			LoadCompleteCallaback = loadCompleteCallback;
			IsLoading = true;
			SceneManager.LoadSceneAsync(SceneNames.LoadingScene).completed += LoadEmptyComplete;

			if (m_Operation == null)
			{
				m_Operation = XLuaManager.GetLuaEnv().NewTable();
			}
			m_Operation.Set("IsDone", false);
			m_Operation.Set("process", 0);
			return m_Operation;
		}

		public static void LoadEmptyComplete(AsyncOperation operation)
		{
			operation.completed -= LoadEmptyComplete;
			m_AsyncOperation = SceneManager.LoadSceneAsync(TargetScene);
			m_AsyncOperation.completed += LoadTargetComplete;
		}

		public static void LoadTargetComplete(AsyncOperation operation)
		{
			operation.completed -= LoadTargetComplete;
			m_AsyncOperation = null;

			CurrenScene = TargetScene;
			IsLoading = false;
			m_Operation.Set("IsDone", true);
			m_Operation.Set("process", 1);
			LoadCompleteCallaback?.Invoke();
			LoadCompleteCallaback = null;
		}


		public static void Update()
		{
			if (IsLoading && m_AsyncOperation != null)
				m_Operation.Set("process", m_AsyncOperation.progress);
		}

	}
}