// ========================================================
// des：
// author: 
// time：2020-12-22 14:44:30
// version：1.0
// ========================================================

namespace Game
{
	public class UIEquipAsset : ConfigBaseAsset<UIEquip> {}
	[System.Serializable]
	public class UIEquip : ConfigBase
	{
		public string SimplifiedChinese;
		public string TraditionalChinese;
		public string English;
		public string Japanese;
		public string Korean;
		public string Vietnamese;
	}
}
