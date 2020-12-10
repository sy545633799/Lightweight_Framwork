// ========================================================
// des：
// author: 
// time：2020-10-23 20:25:22
// version：1.0
// ========================================================

// ========================================================
// des：
// author: 
// time：2020-08-04 11:16:06
// version：1.0
// ========================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Game
{
    public class SoundManager
    {

		public enum MusicType
		{
			//音乐
			Music = 1,
			//音效
			Sound = 2,
		}

		//音频类型枚举
		public enum PlayMode
        {
			//播放一次，播放下一个时， 中断前一个
			OnceBreak = 1,
			//循环播放
			Loop,
			//播放一次，多次点击播放同一个，后面点击无效
			OnceNotBreak,
			//播放一次，播放下一个时， 不中断前一个
			OnceContinue,
		}
        
        private static Transform m_mainCamera;
        private static Hashtable clipTable = new Hashtable();
        private static List<AudioSource> soundAudioList = new List<AudioSource>();
        public static Dictionary<int, AudioConfig> SoundConfigMap = new Dictionary<int, AudioConfig>();

        private static readonly string BackKey = "CanPlayBackSound";
        private static readonly string EffectKey = "CanPlaySoundEffect";

        private static readonly string MusicValumeKey = "MusicValume";
        private static readonly string SoundValumeKey = "SoundValume";

		private static AudioSource m_BackMusicSource;
		private static AudioSource m_SoundSource;
		public static GameObject m_MainCameraTrans;
		public async static Task Init()
		{
			m_MainCameraTrans = GameObject.Find("MainCamera");
			m_BackMusicSource = m_MainCameraTrans.AddComponent<AudioSource>();
			m_SoundSource = m_MainCameraTrans.AddComponent<AudioSource>();

			AudioConfigAsset audioConfig = await ResourceManager.LoadAsset("Assets/Art/Assets/Config/AudioConfig.asset") as AudioConfigAsset;
			AudioConfig[] configs = audioConfig.Configs;
			for (int i = 0; i < configs.Length; i++)
			{
				int id = configs[i].ID;
				SoundConfigMap.Add(id, configs[i]);
			}
		}

		public static async void PlaySound(int SoundID, GameObject go = null)
		{
			AudioConfig model;
			if (SoundConfigMap.TryGetValue(SoundID, out model))
			{
				switch ((PlayMode)model.PlayMode)
				{
					case PlayMode.OnceBreak:
						PlayByEntity(SoundID, go);
						break;
					case PlayMode.Loop:
						PlaySoundByAudio(SoundID, m_SoundSource);
						break;
					case PlayMode.OnceNotBreak:
						PlayOnce(SoundID, go);
						break;
					case PlayMode.OnceContinue:
						PlayOnce(SoundID, go);
						break;
					default:
						break;
				}
			}
		}

		public static async void StopByAudio(AudioSource source)
		{
			source.Stop();
		}

		public static void PlayBackMusic(int SoundID)
		{
			PlaySoundByAudio(SoundID, m_BackMusicSource);
		}

		public static void StopBackMusic()
		{
			m_BackMusicSource.Stop();
		}

		public static void PauseBackMusic()
		{
			m_BackMusicSource.Pause();
		}
		
		public static void ResumeBackMusic()
		{
			m_BackMusicSource.UnPause();
		}

		public static void PlayMusic(int SoundID)
		{
			PlaySoundByAudio(SoundID, m_SoundSource);
		}

		public static void StopMusic()
		{
			m_SoundSource.Stop();
		}

		public static void PauseMusic()
		{
			m_SoundSource.Pause();
		}

		public static void ResumeMusic()
		{
			m_SoundSource.UnPause();
		}

		#region private
		/// <summary>
		/// 载入一个音频
		/// </summary>
		private static async Task<AudioClip> LoadAudioClip(string path)
		{
			AudioClip ac = Get(path);
			if (ac == null)
			{
				ac = await ResourceManager.LoadAsset($"Assets/Art/Audio/{path}") as AudioClip;
				Add(path, ac);
			}
			return ac;
		}

		/// <summary>
		/// 获取一个AudioSource
		/// </summary>
		/// <param name="go"></param>
		/// <returns></returns>
		private static AudioSource GetSoundAudioSource(GameObject go)
		{
			foreach (var item in soundAudioList)
			{
				if (!item.isPlaying)
				{
					return item;
				}
			}
			AudioSource source = go.GetComponent<AudioSource>();
			if (!source) source = go.AddComponent<AudioSource>();
			soundAudioList.Add(source);
			return source;
		}

		/// <summary>
		/// 添加一个声音
		/// </summary>
		private static void Add(string key, AudioClip value)
		{
			if (clipTable[key] != null || value == null) return;
			clipTable.Add(key, value);
		}

		/// <summary>
		/// 获取一个声音
		/// </summary>
		private static AudioClip Get(string key)
		{
			if (clipTable[key] == null) return null;
			return clipTable[key] as AudioClip;
		}
		/// <summary>
		/// 播放一次， 不会被打断
		/// </summary>
		/// <param name="SoundID"></param>
		/// <param name="pos"></param>
		private async static Task<bool> PlayOnce(int SoundID, GameObject go = null)
		{
			AudioConfig model;
			if (SoundConfigMap.TryGetValue(SoundID, out model))
			{
				AudioClip clip = await LoadAudioClip(model.Resource);
				Vector3 pos = go == null ? m_MainCameraTrans.transform.position : go.transform.position;
				switch ((MusicType)model.MusicOrSound)
				{
					case MusicType.Music:
						AudioSource.PlayClipAtPoint(clip, pos, GetMusicVolume());
						break;
					case MusicType.Sound:
						AudioSource.PlayClipAtPoint(clip, pos, GetSoundVolume());
						break;
					default:
						break;
				}
				return true;
			}
			return false;
		}


		private static async void PlayByEntity(int soundId, GameObject go)
		{
			AudioSource source = GetSoundAudioSource(go);
			PlaySoundByAudio(soundId, source);
		}

		/// <summary>
		/// 根据播放器播放
		/// </summary>
		/// <param name="SoundID"></param>
		/// <param name="go"></param>
		/// <returns></returns>
		private static async Task<bool> PlaySoundByAudio(int SoundID, AudioSource source)
		{
			AudioConfig model;
			if (SoundConfigMap.TryGetValue(SoundID, out model))
			{
				AudioClip clip = await LoadAudioClip(model.Resource);
				source.loop = true;
				source.spatialBlend = 1;
				source.rolloffMode = AudioRolloffMode.Linear;
				switch ((MusicType)model.MusicOrSound)
				{
					case MusicType.Music:
						source.volume = GetMusicVolume();
						break;
					case MusicType.Sound:
						source.volume = GetSoundVolume();
						break;
					default:
						break;
				}
				source.spread = 360;
				source.playOnAwake = false;
				source.clip = clip;
				source.Play();
				return true;
			}
			return false;
		}

		#endregion

		#region 设置部分接口
		/// <summary>
		/// 开启音乐
		/// </summary>
		public static void OpenMusic()
		{
			PlayerPrefs.SetInt(BackKey, 1);
			if (m_BackMusicSource.isPlaying)
				return;
			else
				m_BackMusicSource.UnPause();
		}
		/// <summary>
		/// 关闭音乐
		/// </summary>
		public static void CloseMusic()
		{
			PlayerPrefs.SetInt(BackKey, 0);
			if (!m_BackMusicSource.isPlaying)
				return;
			else
				m_BackMusicSource.Pause();
		}
		/// <summary>
		/// 设置音乐音量
		/// </summary>
		/// <param name="num"></param>
		public static void SetMusicVolume(float num)
		{
			PlayerPrefs.SetFloat(MusicValumeKey, num);
			if (m_BackMusicSource.isPlaying)
				m_BackMusicSource.volume = num;
			
		}
		/// <summary>
		/// 获取音乐音量
		/// </summary>
		/// <returns></returns>
		public static float GetMusicVolume(float defaultVal = 1)
		{
			return PlayerPrefs.GetFloat(MusicValumeKey, defaultVal);
		}
		/// <summary>
		/// 开启音效
		/// </summary>
		public static void OpenSound()
		{
			PlayerPrefs.SetInt(EffectKey, 1);
		}
		/// <summary>
		/// 关闭音效
		/// </summary>
		public static void CloseSound()
		{
			PlayerPrefs.SetInt(EffectKey, 0);
		}
		/// <summary>
		/// 设置音效音量
		/// </summary>
		/// <param name="num"></param>
		public static void SetSoundVolume(float num)
		{
			PlayerPrefs.SetFloat(SoundValumeKey, num);
		}

		/// <summary>
		/// 获取音乐音量
		/// </summary>
		/// <returns></returns>
		public static float GetSoundVolume(float defaultVal = 1)
		{
			return PlayerPrefs.GetFloat(SoundValumeKey, defaultVal);
		}
		/// <summary>
		/// 是否播放背景音乐，默认是1：播放
		/// </summary>
		/// <returns></returns>
		public static bool CanPlayBackSound()
		{
			return PlayerPrefs.GetInt(BackKey, 1) == 1;
		}
		/// <summary>
		/// 是否播放音效,默认是1：播放
		/// </summary>
		/// <returns></returns>
		public static bool CanPlaySound()
		{
			return PlayerPrefs.GetInt(EffectKey, 1) == 1;
		}

		#endregion

		public static void Dispose()
		{
			
		}
	}
}
