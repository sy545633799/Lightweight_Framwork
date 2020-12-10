// ========================================================
// des：
// author: 
// time：2020-11-17 16:03:46
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class AtlasConfigAsset : ConfigBaseAsset<AtlasConfig> { }
	[System.Serializable]
	public class AtlasConfig : ConfigBase
	{
		public string Name;
	}
}
