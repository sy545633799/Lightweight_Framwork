// ========================================================
// des：shenyi
// author: 
// time：2020-08-09 14:53:18
// version：1.0
// ========================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using XLua;

namespace Game
{
	public static class EventExtension
	{
		public static void AddClickListener(this GameObject go, UnityAction callback)
		{
			//EventTrigger trigger = go.transform.GetComponent<EventTrigger>();
			//if (trigger == null)
			//{
			//	trigger = go.transform.gameObject.AddComponent<EventTrigger>();
			//	trigger.triggers = new List<EventTrigger.Entry>();
			//}

			//EventTrigger.Entry entry = new EventTrigger.Entry();
			//entry.eventID = EventTriggerType.PointerDown;
			//UnityAction<BaseEventData> callback1 = new UnityAction<BaseEventData>(eventData => callback());
			//entry.callback.AddListener(callback1);
			//trigger.triggers.Add(entry);
			EventObject eventObject = go.transform.GetComponent<EventObject>();
			if (eventObject == null)
				eventObject = go.transform.gameObject.AddComponent<EventObject>();
			eventObject.AddClickListener(callback);
		}

		public static void AddClickListener(this GameObject go, float x, float y, float z, float height, UnityAction callback)
		{
			EventObject eventObject = go.transform.GetComponent<EventObject>();
			if (eventObject == null)
				eventObject = go.transform.gameObject.AddComponent<EventObject>();
			eventObject.SetColliderSize(x, y, z, height);
			eventObject.AddClickListener(callback);
		}

		public static void RemoveClickListener(this GameObject go, UnityAction callback)
		{
			EventObject eventObject = go.transform.GetComponent<EventObject>();
			eventObject.RemoveClickListener(callback);
		}

		public static void AddEnterListener(this GameObject go, float x, float y, float z, float height, UnityAction<GameObject> callback)
		{
			EventObject eventObject = go.transform.GetComponent<EventObject>();
			if (eventObject == null)
				eventObject = go.transform.gameObject.AddComponent<EventObject>();
			eventObject.AddEnterListener(callback);
		}

		public static void RemoveEnterListener(this GameObject go, UnityAction<GameObject> callback)
		{
			EventObject eventObject = go.transform.GetComponent<EventObject>();
			eventObject?.RemoveEnterListener(callback);
		}

		public static void AddExitListener(this GameObject go, float x, float y, float z, float height, UnityAction<GameObject> callback)
		{
			EventObject eventObject = go.transform.GetComponent<EventObject>();
			if (eventObject == null)
				eventObject = go.transform.gameObject.AddComponent<EventObject>();
			eventObject.AddExitListener(callback);
		}

		public static void RemoveExitListener(this GameObject go, UnityAction<GameObject> callback)
		{
			EventObject eventObject = go.transform.GetComponent<EventObject>();
			eventObject?.AddExitListener(callback);
		}

		public static void RemoveAllListener(this GameObject go)
		{
			EventObject eventObject = go.transform.GetComponent<EventObject>();
			if (eventObject == null) return;
			eventObject.RemoveAllListeners();
		}
	}
}