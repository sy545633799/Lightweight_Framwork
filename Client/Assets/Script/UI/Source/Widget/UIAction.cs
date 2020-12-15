// ========================================================
// des：
// author: 
// time：2020-12-15 13:25:01
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class UIAction : UIButton
	{
		public string TriggerName;
		/// <summary>
		/// 动作事件
		/// </summary>
		/// <param name="triggerName"></param>
		public delegate void ActionTrigger(string triggerName);
		/// <summary>
		/// 定义动作事件委托
		/// </summary>
		public static event ActionTrigger OnActionTrigger;

		protected override void Start()
	    {
			onClick.AddListener(ClickButton);
	    }

		void ClickButton()
		{
			if (!string.IsNullOrEmpty(TriggerName))
			{
				OnActionTrigger?.Invoke(TriggerName);
			}
		}
		
	}
}
