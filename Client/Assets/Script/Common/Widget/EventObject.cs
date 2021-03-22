// ========================================================
// des：shenyi
// author: 
// time：2020-08-09 14:52:16
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game {

	public class ObjectTriggerEvent : UnityEvent<GameObject> { }
	public class EntityTriggerEvent : UnityEvent<string> { }
	public class EventObject : MonoBehaviour {

		private BoxCollider2D m_Collider;
		//public UnityEvent onMouseEneter = new UnityEvent();
		//public UnityEvent onMouseOver = new UnityEvent();
		//public UnityEvent onMouseExit = new UnityEvent();
		//public UnityEvent onMouseDown = new UnityEvent();
		//public UnityEvent onMouseUp = new UnityEvent();
		//public UnityEvent onMouseDrag = new UnityEvent();
		private UnityEvent onMouseUpAsButton = new UnityEvent();
		private ObjectTriggerEvent onTriggerEnter = new ObjectTriggerEvent();
		private ObjectTriggerEvent onTriggerExit = new ObjectTriggerEvent();

		protected void EnsureCollider()
		{
			if (!m_Collider)
			{
				CharacterController character = transform.GetComponent<CharacterController>();
				if (!character)
				{
					m_Collider = transform.GetComponent<BoxCollider2D>();
					if (m_Collider == null)
					{
						m_Collider = transform.gameObject.AddComponent<BoxCollider2D>();
						if (m_Collider)
							m_Collider.isTrigger = true;
						else
							Debug.LogError(transform.name);

					}
				}
			}
		}
		public void SetColliderSize(float x, float y, float z, float height)
		{
			EnsureCollider();
			m_Collider.size = new Vector3(x, y, z);
			m_Collider.offset = new Vector3(0, height, 0);
		}

		protected virtual void OnTriggerEnter(Collider other)
		{
			onTriggerEnter?.Invoke(other.gameObject);
		}

		protected virtual void OnTriggerExit(Collider other)
		{
			onTriggerExit?.Invoke(other.gameObject);
		}

		//private void OnMouseEnter()
		//{
		//	if (UIUtil.IsPointerOnUI() == PointerType.Other)
		//		onMouseEneter.Invoke();
		//}

		//private void OnMouseOver()
		//{
		//	if (UIUtil.IsPointerOnUI() == PointerType.Other)
		//		onMouseOver.Invoke();
		//}

		//private void OnMouseExit()
		//{
		//	if (UIUtil.IsPointerOnUI() == PointerType.Other)
		//		onMouseOver.Invoke();
		//}

		//private void OnMouseDown()
		//{
		//	if (UIUtil.IsPointerOnUI() == PointerType.Other)
		//		onMouseDown.Invoke();
		//}

		//private void OnMouseUp()
		//{
		//	if (UIUtil.IsPointerOnUI() == PointerType.Other)
		//		onMouseUp.Invoke();
		//}

		//private void OnMouseDrag()
		//{
		//	if (UIUtil.IsPointerOnUI() == PointerType.Other)
		//		onMouseDrag.Invoke();
		//}

		//在一个物体按下，在释放时鼠标还在物体上的时候调用
		protected void OnMouseUpAsButton()
		{
			//if (!InputManager.IsOverGUI(int touchId))
			//	onMouseUpAsButton.Invoke();
		}

		public void AddClickListener(UnityAction callback)
		{
			EnsureCollider();
			onMouseUpAsButton?.AddListener(callback);
		}
		public void RemoveClickListener(UnityAction callback)
		{
			onMouseUpAsButton?.RemoveListener(callback);
		}
		public void AddEnterListener(UnityAction<GameObject> callback)
		{
			EnsureCollider();
			onTriggerEnter?.AddListener(callback);
		}
		public void RemoveEnterListener(UnityAction<GameObject> callback)
		{
			onTriggerEnter?.RemoveListener(callback);
		}
		public void AddExitListener(UnityAction<GameObject> callback)
		{
			EnsureCollider();
			onTriggerExit?.AddListener(callback);
		}
		public void RemoveExitListener(UnityAction<GameObject> callback)
		{
			onTriggerExit?.RemoveListener(callback);
		}

		public virtual void RemoveAllListeners()
		{
			if (m_Collider)
			{
				GameObject.Destroy(m_Collider);
				m_Collider = null;
			}
			//onMouseEneter.RemoveAllListeners();
			//onMouseDrag.RemoveAllListeners();
			//onMouseDown.RemoveAllListeners();
			//onMouseUp.RemoveAllListeners();
			//onMouseOver.RemoveAllListeners();
			//onMouseExit.RemoveAllListeners();
			onMouseUpAsButton.RemoveAllListeners();
			onTriggerEnter.RemoveAllListeners();
			onTriggerExit.RemoveAllListeners();
		}
	}
	
}
