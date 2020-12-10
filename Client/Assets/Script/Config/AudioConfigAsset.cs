namespace Game
{
	public class AudioConfigAsset : ConfigBaseAsset<AudioConfig> {}
	[System.Serializable]
	public class AudioConfig : ConfigBase
	{
		public int MusicOrSound;
		public int SoundType;
		public int PlayMode;
		public string Resource;
	}
}
