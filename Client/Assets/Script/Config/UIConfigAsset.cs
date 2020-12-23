// ========================================================
// des：
// author: 
// time：2020-09-28 16:45:41
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {


	public class UIConfigAsset : ConfigBaseAsset<UIConfig> { }

	[System.Serializable]
	public class UIConfig: LocationConfig
	{
		[Tooltip("UI名字")]
		public string Name;
		[Tooltip("UI路径")]
		public string Path;
		[Tooltip("附加场景名字")]
		public string SceneName;
		[Tooltip("依赖图集名字")]
		public List<string> AtlasNames;
	}

}
