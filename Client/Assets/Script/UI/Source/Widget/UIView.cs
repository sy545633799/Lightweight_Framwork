// ========================================================
// des：shenyi
// author: 
// time：2020-09-25 19:45:08
// version：1.0
// ========================================================

using UnityEngine;

namespace Game {

	[System.Serializable]
	[DisallowMultipleComponent]
	public class UIView : UIContainer
	{
		public int LayerIndex = Game.LayerIndex.Base;
		public bool ShowMoneyBar = false;
		public bool IsTransParent = false;
		public bool DestroyOnUnload = true;
		public string SceneName = string.Empty;
	}
}
