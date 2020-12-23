// ========================================================
// des：
// author: 
// time：2020-12-23 19:35:03
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {

	[System.Serializable]
	[CreateAssetMenu(menuName = "GameSetting/HUDConfigAsset")]
	public class HUDConfigAsset : SettingBaseAsset<HUDConfigAsset>
	{

		[Header("跳字的随机偏移半径")]
		public float m_offsetRandomRangeR = 1;
		[Header("相机坐标空间下的Render深度(值越大飘字越小)"), Range(1f, 10f)]
		public float m_viewportRenderDepth;
		//[Header("飘字动画相关设置")]
		//public Dictionary<string, HUDNumberAnimation> m_HUDNumberAnimationAttributes;
		//[Header("飘字网格相关设置")]
		//public Dictionary<HUDNumberType, HUDNumberAttributes> m_HUDNumberAttributes;
	}

}
