// ========================================================
// des：
// author: 
// time：2020-12-10 09:58:19
// version：1.0
// ========================================================

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
namespace Game {

	[Serializable]
	public class GameSettings
	{
		public bool AssetBundleMode = false;
		public bool UpdateMode = false;
		public bool MiniMode = false;
		public bool LogDebug = false;
		public bool UwaMode = false;

		public int PCFrameRate = 60;
		public int MobileFrameRate = 30;
	}
}
