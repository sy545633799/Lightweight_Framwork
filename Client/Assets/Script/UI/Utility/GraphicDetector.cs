// ========================================================
// des：
// author: 
// time：2020-10-26 10:50:12
// version：1.0
// ========================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
	public class GraphicDetector : MonoBehaviour
	{
		IList<ICanvasElement> m_LayoutRebuildQueue;
		IList<ICanvasElement> m_GraphicRebuildQueue;

		private void Awake()
		{
			Type type = typeof(CanvasUpdateRegistry);
			FieldInfo field = type.GetField("m_LayoutRebuildQueue", BindingFlags.NonPublic | BindingFlags.Instance);
			m_LayoutRebuildQueue = (IList<ICanvasElement>)field.GetValue(CanvasUpdateRegistry.instance);
			field = type.GetField("m_GraphicRebuildQueue", BindingFlags.NonPublic | BindingFlags.Instance);
			m_GraphicRebuildQueue = (IList<ICanvasElement>)field.GetValue(CanvasUpdateRegistry.instance);
		}

		private void Update()
		{
			for (int i = 0; i < m_LayoutRebuildQueue.Count; i++)
			{
				var rebuild = m_LayoutRebuildQueue[i];
				if (ObjectValidForUpdate(rebuild))
				{
					Debug.LogError($"{rebuild.transform.name}引起了网格重建");
				}
			}

			for (int i = 0; i < m_GraphicRebuildQueue.Count; i++)
			{
				var rebuild = m_GraphicRebuildQueue[i];
				if (ObjectValidForUpdate(rebuild))
				{
					Debug.LogError($"{rebuild.transform.name}引起了{rebuild.transform.GetComponent<Graphic>().canvas.name}重建");
				}
			}
		}

		private bool ObjectValidForUpdate(ICanvasElement element)
		{
			var valid = element != null;
			var isUnityObject = element is UnityEngine.Object;
			if (isUnityObject)
			{
				valid = (element as UnityEngine.Object) != null;
			}

			return valid;
		}
	}
}
