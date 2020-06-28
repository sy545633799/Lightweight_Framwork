// ========================================================
// des：
// author: 
// time：2020-05-01 14:04:46
// version：1.0
// ========================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game {

	public class LaunchUI : IInitializable, IDisposable, ITickable
	{
		private readonly SignalBus _signalBus;
		private GameObject mLauchUI;

		public LaunchUI(SignalBus signalBus)
		{
			_signalBus = signalBus;
		}

		public void Initialize()
		{
			mLauchUI = GameObject.Find("Canvas/Loading/LaunchUI");
			mLauchUI.SetActive(true);
			_signalBus.Subscribe<LoadSceneStartSignal>(Dispose);
		}

		public void Dispose()
		{
			GameObject.Destroy(mLauchUI); 
		}

		public void Tick()
		{

		}
	}
}
