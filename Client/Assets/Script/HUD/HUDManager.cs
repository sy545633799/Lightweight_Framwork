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
	public class HUDManager
	{
		public static CommandBuffer Command { get; private set; }

		private static Camera m_MainCamera;
		private static Dictionary<string, HUDNumberAnimation> m_HUDNumberAnimationAttributes = new Dictionary<string, HUDNumberAnimation>();
		private static Dictionary<HUDNumberType, HUDNumberAttributes> m_HUDNumberAttributes = new Dictionary<HUDNumberType, HUDNumberAttributes>();

		public static void Init()
		{
			m_MainCamera = Camera.main;
			Command = new CommandBuffer();
			List<HUDNumberAnimation> list = HUDConfigAsset.Get().HUDNumberAnimationAttributesList;
			for (int i = 0; i < list.Count; i++)
				m_HUDNumberAnimationAttributes.Add(list[i].Name, list[i]);
			List<HUDNumberAttributes> list2 = HUDConfigAsset.Get().HUDNumberAttributesList;
			for (int i = 0; i < list2.Count; i++)
				m_HUDNumberAttributes.Add(list2[i].Type, list2[i]);
		}
		
		

	}
}
