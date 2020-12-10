// ========================================================
// author: 
// time：2020-08-09 13:59:34
// version：1.0
// ========================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
	public class CameraDrag : MonoSingleton<CameraDrag>
	{

		public float dragSpeed = 0.5f;
		private Vector3 dragOrigin;
		public float SizeXmap, SizeYmap;
		public float h, w;
		public float distanceX, distanceY;

		private GameObject m_Map;
		private Camera m_Cam;
		private Vector3 oldPos, panOrigin;

		private Action onMoveEnd;
		private Vector3 targetPos;
		private bool bMoveToWorldPos = false;
		private void Start()
		{
			DontDestroyOnLoad(gameObject);
		}


		public void SetMap(GameObject map)
		{
			m_Map = map;
			if (m_Map == null) return;
			SizeXmap = m_Map.GetComponent<SpriteRenderer>().sprite.bounds.size.x * 0.5f;
			SizeYmap = m_Map.GetComponent<SpriteRenderer>().sprite.bounds.size.y * 0.5f;
			m_Cam = Camera.main;
			h = m_Cam.orthographicSize;
			w = h * m_Cam.aspect;
			distanceX = Mathf.Abs(SizeXmap - w);
			distanceY = Mathf.Abs(SizeYmap - h);
			transform.position = new Vector3(Mathf.Clamp(transform.position.x, m_Map.transform.position.x - distanceX, m_Map.transform.position.x + distanceX), Mathf.Clamp(transform.position.y, m_Map.transform.position.y - distanceY, m_Map.transform.position.y + distanceY), -10f);
		}


		public void MoveToWorldPosition(Transform trans, Action callback = null)
		{
			onMoveEnd = callback;
			targetPos = new Vector3(Mathf.Clamp(trans.position.x, m_Map.transform.position.x - distanceX, m_Map.transform.position.x + distanceX), Mathf.Clamp(trans.position.y, m_Map.transform.position.y - distanceY, m_Map.transform.position.y + distanceY), -10f);
			bMoveToWorldPos = true;
		}

		void Update()
		{
			if (bMoveToWorldPos)
			{
				//Vector2 pos = Camera.main.WorldToScreenPoint(targetTrans.position);
				Vector3 target = Vector3.Lerp(this.transform.position, targetPos, 0.3f);
				transform.position = target;
				if (Vector3.Distance(transform.position, targetPos) < 0.1f)
				{
					onMoveEnd?.Invoke();
					bMoveToWorldPos = false;
				}
			}
			else
			{
				if (!m_Map || !m_Cam || IsPointerOverGameObject())
					return;

				if (Input.GetMouseButtonDown(0))
				{
					//bDragging = true;
					oldPos = transform.position;
					panOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
				}

				if (Input.GetMouseButton(0))
				{
					Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - panOrigin;
					transform.position = oldPos + -pos * dragSpeed;
					transform.position = new Vector3(Mathf.Clamp(transform.position.x, m_Map.transform.position.x - distanceX, m_Map.transform.position.x + distanceX), Mathf.Clamp(transform.position.y, m_Map.transform.position.y - distanceY, m_Map.transform.position.y + distanceY), -10f);
				}
			}

		}

		private bool IsPointerOverGameObject()
		{
			return EventSystem.current.IsPointerOverGameObject();
		}
	}
}