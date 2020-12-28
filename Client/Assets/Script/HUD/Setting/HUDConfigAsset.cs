// ========================================================
// des：头顶名字, 血条
// author: shenyi
// time：2020-12-23 19:35:03
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Game {
	public enum HUDNumberType
	{
		NORMAL_CRIT,
		NORMAL_HIT,
		NORMAL_HEAL,
		NORMAL_PASS,
		NORMAL_SHIELD,
	}

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
		public HUDNumberType Type;
		[Header("备注")]
		public string Note;
		[Header("数字的前缀(图集中的前缀名)")]
		public string NumberKey;
		[Header("单个数字的宽高")]
		public Vector2 Rect;
		[Header("数字的间隔")]
		public float Interval_X;
		[Header("网格整体缩放"), Range(0.1f, 1.5f)]
		public float Scale;
		[Header("功能列表")]
		public FacilityType Facility;
		[Header("底板信息")]
		public HUDMeshAttributes BackGroudAttributes;

		public bool IsSelectEnumType(FacilityType _enumType)
		{
			int _type = (int)_enumType;
			return ((int)Facility & _type) == _type;
		}
	}

	[System.Serializable]
	public struct HUDNumberAnimation
	{
		[Header("名称")]
		public string Name;
		[Header("位移动画曲线X轴")]
		public AnimationCurve MoveCurveX;
		[Header("位移动画曲线Y轴")]
		public AnimationCurve MoveCurveY;
		[Header("缩放动画曲线(time, value 取值 0 - 1)")]
		public AnimationCurve Scale;
		[Header("透明动画曲线(time, value 取值 0 - 1)")]
		public AnimationCurve Alpha;
		[Header("位移动画的值的倍率"), Range(0.01f, 1f)]
		public float MoveCurveScale;
		[Header("动画的总时间"), Range(0.1f, 5f)]
		public float Time;
	}

	[System.Serializable]
	[CreateAssetMenu(menuName = "GameSetting/HUDConfigAsset")]
	public class HUDConfigAsset : SettingBaseAsset<HUDConfigAsset>
	{
		[Header("图集")]
		public SpriteAtlas Atlas;
		[Header("跳字的随机偏移半径")]
		public float OffsetRandomRangeR = 1;
		[Header("相机坐标空间下的Render深度(值越大飘字越小)"), Range(1f, 10f)]
		public float ViewportRenderDepth;
		[Header("飘字动画相关设置")]
		public List<HUDNumberAnimation> HUDNumberAnimationAttributesList;
		[Header("飘字网格相关设置")]
		public List<HUDNumberAttributes> HUDNumberAttributesList;
		 
		
	}

}
