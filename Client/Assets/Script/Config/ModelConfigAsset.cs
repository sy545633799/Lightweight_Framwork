namespace Game
{
	public class ModelConfigAsset : ConfigBaseAsset<ModelConfig> {}
	[System.Serializable]
	public class ModelConfig : ConfigBase
	{
		public string Resource;
	}
}
