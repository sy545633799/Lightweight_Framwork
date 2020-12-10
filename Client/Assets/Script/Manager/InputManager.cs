using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game
{
	public class InputManager
	{

		public class ClickEvent : UnityEvent<Vector2> { }
		public class WheelEvent : UnityEvent<float> { }
		public class KeyCodeEvent : UnityEvent<KeyCode> { }

		public static WheelEvent onWheelEvent = new WheelEvent();
		public static KeyCodeEvent onkeyCodeDownEvent = new KeyCodeEvent();
		public static KeyCodeEvent onKeyCodeUpEvent = new KeyCodeEvent();

		public static ClickEvent onDown = new ClickEvent();
		public static ClickEvent onUp = new ClickEvent();
		public static ClickEvent onDrag = new ClickEvent();

		public static ClickEvent onDownScene = new ClickEvent();
		public static ClickEvent onUpScene = new ClickEvent();
		public static ClickEvent onDragScene = new ClickEvent();

		public static ClickEvent onDownUI = new ClickEvent();
		public static ClickEvent onUpUI = new ClickEvent();
		public static ClickEvent onDragUI = new ClickEvent();

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		private static KeyCode[] s_keycode = null;
		private static Dictionary<int, bool> mouseMap = new Dictionary<int, bool>();
		private static Dictionary<int, bool> mouseUIMap = new Dictionary<int, bool>();
#else
		private static Dictionary<int, bool> touchMap = new Dictionary<int, bool>();
		private static Dictionary<int, bool> touchUIMap = new Dictionary<int, bool>();
#endif

		private static EventSystem eventSys;
		private static bool touchEnabled = true;
		private static int MAX_SUPPORT_FINGERID { get { return 10; } }

		static InputManager()
		{
			for (int i = 0; i <= 1; i++)
			{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
				mouseMap[i] = false;
				mouseUIMap[i] = false;
#else
				touchMap[i] = false;
				touchUIMap[i] = false;
#endif
			}
		}


		public static void SetInputEnabled(bool bTouchEnabled)
		{
			touchEnabled = bTouchEnabled;
			if (!eventSys)
				eventSys = GameObject.FindObjectOfType<EventSystem>();
			if (eventSys)
				eventSys.enabled = bTouchEnabled;
		}

		public static bool GetInputEnabled()
		{
			return touchEnabled;
		}

		public static void Update()
		{
			if (!touchEnabled)
			{
				return;
			}

			// PC 特有, 键盘鼠标事件
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			var mousex = Input.GetAxis("Mouse X");
			var mousey = Input.GetAxis("Mouse Y");


			for (int i = 0; i <= 1; i++)
			{
				if (Input.GetMouseButtonDown(i))
				{
					if (!IsOverGUI())
					{
						mouseMap[i] = true;
						onDownScene.Invoke(Input.mousePosition);

					}
					else
					{
						mouseUIMap[i] = true;
						onDownUI.Invoke(Input.mousePosition);
					}
					onDown.Invoke(Input.mousePosition);
				}

				if (Input.GetMouseButtonUp(i))
				{
					if (mouseMap[i])
					{
						mouseMap[i] = false;
						onUpScene.Invoke(Input.mousePosition);
					}
					else if (mouseUIMap[i])
					{
						mouseUIMap[i] = false;
						onUpUI.Invoke(Input.mousePosition);
					}
					onUp.Invoke(Input.mousePosition);
				}

				if (mouseMap[i] && (mousex > 0.1 || mousex < -0.1 || mousey > 0.1 || mousey < -0.1))
				{
					if (mouseMap[i])
						onDragScene.Invoke(Input.mousePosition);
					else if (mouseUIMap[i])
						onDragUI.Invoke(Input.mousePosition);
					onDrag.Invoke(Input.mousePosition);
				}
			}

			var mouseWheel = Input.GetAxis("Mouse ScrollWheel");
			if (mouseWheel > 0.1 || mouseWheel < -0.1)
			{
				onWheelEvent.Invoke(mouseWheel);
			}

			if (s_keycode == null)
			{
				var codes = Enum.GetValues(typeof(KeyCode));
				s_keycode = new KeyCode[codes.Length];
				int index = 0;
				foreach (KeyCode k in codes)
				{
					s_keycode[index++] = k;
				}
			}
			foreach (KeyCode keycode in s_keycode)
			{
				if (Input.GetKeyDown(keycode))
				{
					onkeyCodeDownEvent.Invoke(keycode);
				}

				if (Input.GetKeyUp(keycode))
				{
					onKeyCodeUpEvent.Invoke(keycode);
				}
			}

#else
		// 触摸事件
		if (Input.touchSupported && Input.touchCount > 0)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				var touch = Input.GetTouch(i);
				var phase = touch.phase;
				var fingerId = touch.fingerId;
				var position = touch.position;
				if (fingerId >= MAX_SUPPORT_FINGERID)
				{
					return;
				}

				switch (phase)
				{
					case TouchPhase.Began:
						if (!IsOverGUI())
						{
							touchMap[fingerId] = true;
							onDownScene.Invoke(position);
						}
						else
						{
							touchUIMap[fingerId] = true;
							onDownUI.Invoke(position);
						}
						onDown.Invoke(position);
						break;
					case TouchPhase.Ended:
					case TouchPhase.Canceled:
						if (touchMap[fingerId])
						{
							touchMap[fingerId] = false;
							onUpScene.Invoke(position);
						}
						else if (touchUIMap[fingerId])
						{
							touchUIMap[fingerId] = false;
							onUpUI.Invoke(position);
						}
						onUp.Invoke(position);
						break;
					case TouchPhase.Moved:
						if (touchMap[fingerId])
							onDragScene.Invoke(position);
						else if (touchUIMap[fingerId])
							onDragUI.Invoke(position);
						onDrag.Invoke(position);
						break;
					default:
						break;
				};
			}
		}
#endif
		}

		public static GameObject DetectHit(Vector2 position)
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(position), Vector2.zero);
			if (hit.collider != null)
				return hit.collider.gameObject;
			else
				return null;
		}

		public static bool IsOverGUI()
		{
			////实例化点击事件
			//PointerEventData eventDataCurrentPosition = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
			////将点击位置的屏幕坐标赋值给点击事件
			//eventDataCurrentPosition.position = new Vector2(position.x, position.y);

			//List<RaycastResult> results = new List<RaycastResult>();

			//if (EventSystem.current)
			//    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

			//return results.Count > 0;
			return EventSystem.current.IsPointerOverGameObject();
		}
	}
}