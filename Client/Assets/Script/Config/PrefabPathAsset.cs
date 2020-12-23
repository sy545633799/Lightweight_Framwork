// ========================================================
// des：
// author: 
// time：2020-12-10 14:24:47
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {

	public class PrefabPathAsset : ConfigBaseAsset<PrefabPathConfig> { }
	[System.Serializable]
	public class PrefabPathConfig : ConfigBase
	{
		public string Path;
	}
}
