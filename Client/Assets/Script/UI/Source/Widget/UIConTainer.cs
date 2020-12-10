// ========================================================
// des：
// author: 
// time：2020-09-29 10:39:47
// version：1.0
// ========================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;
//using DG.Tweening;

namespace Game {
	public enum UIType
	{
		CommonView = 0,
		TabView,
		Tips,
		Item,
	}

	[System.Serializable]
	[DisallowMultipleComponent]
	public class UIContainer : UIBehaviour
	{
		[HideInInspector]
		public UIType ViewType = UIType.CommonView;
		public CanvasGroup canvasGroup;
		//[HideInInspector]
		public List<Component> behaviours;
		//[HideInInspector]
		public List<string> names;

		private LuaTable luaTable;

		private LuaTable GenUIView(UIContainer container)
		{
			if (container == null) return null;
			LuaTable table = XLuaManager.GetLuaEnv().NewTable();
			table.Set<string, UIContainer>("container", container);
			table.Set<string, Transform>("transform", container.transform);
			for (int i = 0; i < container.names.Count; i++)
			{
				string name = container.names[i];
				Component component = container.behaviours[i];
				LuaTable compTable;
				if (component is UIContainer)
				{
					compTable = GenUIView(component as UIContainer);
					table.Set(name, compTable);
				}
				else
				{
					table.Set(name, component);
				}
				
			}

			return table;
		}

		public LuaTable GetUIView()
		{
			if (luaTable == null)
				luaTable = GenUIView(this);
			return luaTable;
		}

		public void SetActive(bool active)
		{
			if (active)
			{
				canvasGroup.alpha = 1;
				canvasGroup.blocksRaycasts = true;
			}
			else
			{
				canvasGroup.alpha = 0;
				canvasGroup.blocksRaycasts = false;
			}
		}

		public void SetAlpha(float val)
		{
			canvasGroup.alpha = val;
		}

		public void DOFade(float endValue, float duration)
		{
			//canvasGroup.DOFade(endValue, duration);
		}
	}
}
