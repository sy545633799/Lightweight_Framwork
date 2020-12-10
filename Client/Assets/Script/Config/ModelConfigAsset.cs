// ========================================================
// des：
// author: 
// time：2020-11-22 15:43:29
// version：1.0
// ========================================================

namespace Game
{
	public class ModelConfigAsset : ConfigBaseAsset<ModelConfig> {}
	[System.Serializable]
	public class ModelConfig : ConfigBase
	{
		public string Resource;
	}
}
