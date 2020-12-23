// ========================================================
// des：
// author: shenyi
// time：2020-12-21 17:54:44
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;

namespace Game {

	[System.Serializable]
	public enum FacilityType
	{
		Background = 1 << 1,
		Symbol_Add = 1 << 2,
		Symbol_Sub = 1 << 3,
		All = Background | Symbol_Add | Symbol_Sub
	}

	[System.Serializable]
	public struct HUDMeshAttributes
	{
		[Header("底板宽高")]
		public Vector2 m_rect;
		[Header("底板距离第一个数字的偏移")]
		public Vector2 m_nextOffset;
	}

	[System.Serializable]
	public struct HUDNumberAttributes
	{
		[Header("备注")]
		public string m_note;
		[Header("数字的前缀(图集中的前缀名)")]
		public string m_numberKey;
		[Header("单个数字的宽高")]
		public Vector2 m_rect;
		[Header("数字的间隔")]
		public float m_interval_X;
		[Header("网格整体缩放"), Range(0.1f, 1.5f)]
		public float m_scale;
		[Header("功能列表")]
		public FacilityType m_facility;
		[Header("底板信息")]
		public HUDMeshAttributes m_backGroudAttributes;

		public bool IsSelectEnumType(FacilityType _enumType)
		{
			int _type = (int)_enumType;
			return ((int)m_facility & _type) == _type;
		}
	}

	public class HUDManager : MonoBehaviour
	{
		public HUDManager Instance { get; private set; }
		public SpriteAtlas Atlas { get; private set; }
		public CommandBuffer Command { get; private set; }
		private HUDConfigAsset Config;

		[Header("配置加载路径")]
		public string SettingPath ;

		private void Awake()
		{
			Instance = this;
			Command = new CommandBuffer();
			if (string.IsNullOrEmpty(SettingPath))
				Debug.LogError("请设置HUDConfig加载路径");
			
		}

		

	}
}
