// ========================================================
// des：
// author: 
// time：2020-05-03 14:50:21
// version：1.0
// ========================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game {
	//[CreateAssetMenu(fileName = "GameSettingInstaller", menuName = "Installer/GameSettingInstaller")]
	public class GameSettingInstaller : ScriptableObjectInstaller<GameSettingInstaller>
	{
		public GameSettings Settings;

		public override void InstallBindings()
		{
			Container.BindInstance(Settings);
		}
	}

	[Serializable]
	public class GameSettings
	{
		public bool AssetBundleMode = false;
		public bool UpdateMode = false;
		public bool MiniMode = false;
		public bool LogDebug = false;
		public bool UwaMode = false;
	}
}
