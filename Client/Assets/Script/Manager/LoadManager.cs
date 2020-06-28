using System;
using UnityEngine.SceneManagement;
using UniRx;
using UnityEngine;

namespace Game
{
    public class LoadManager
    {
		public static Action<string> StartLoadEvent;
		public static Action<string> EndLoadEvent;

		public static async void Load(string sceneName, Action loadCompleteCallback, Action<float> processCallback = null)
        {
			StartLoadEvent?.Invoke(sceneName);
			var progressNotifier = new ScheduledNotifier<float>();
			progressNotifier.Subscribe(process => processCallback?.Invoke(process));
			await SceneManager.LoadSceneAsync(sceneName).AsObservable(progressNotifier);

			loadCompleteCallback?.Invoke();
			EndLoadEvent?.Invoke(sceneName);

		}
	}
}