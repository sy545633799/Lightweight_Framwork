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
		public class ClickStatus
		{
			public bool IsDown;
			public bool IsMove;
			public bool IsOverUI;
			public Vector3 Position;
			public Vector2 Delta;
		}

		public class ClickEvent : UnityEvent<int, ClickStatus> { }
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

		public static Dictionary<int, ClickStatus> ClickMap = new Dictionary<int, ClickStatus>();
#if UNITY_EDITOR || UNITY_STANDALONE
		private static KeyCode[] m_keycode = null;
#endif


		private static EventSystem eventSys;
		private static bool touchEnabled = true;
		private static float doubleTouchLastDis;
		private static int MAX_SUPPORT_FINGERID { get { return 10; } }

		static InputManager()
		{
			for (int i = 0; i <= MAX_SUPPORT_FINGERID; i++)
				ClickMap[i] = new ClickStatus();
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
#if UNITY_EDITOR || UNITY_STANDALONE
			var mousex = Input.GetAxis("Mouse X");
			var mousey = Input.GetAxis("Mouse Y");
			bool mouseMove = mousex > 0.1 || mousex < -0.1 || mousey > 0.1 || mousey < -0.1;

			for (int i = 0; i <= 1; i++)
			{
				if (Input.GetMouseButtonDown(i))
				{
					ClickMap[i].IsDown = true;
					ClickMap[i].Position = Input.mousePosition;
					ClickMap[i].Delta = Vector2.zero;
					if (IsOverGUI(i))
						ClickMap[i].IsOverUI = true;
					else
						ClickMap[i].IsOverUI = false;
				}
				else if (Input.GetMouseButtonUp(i))
				{
					if (ClickMap[i].IsOverUI)
						ClickMap[i].IsOverUI = false;
					ClickMap[i].IsDown = false;
					ClickMap[i].Position = Vector3.zero;
					ClickMap[i].Delta = Vector2.zero;
				}
				else if (Input.GetMouseButton(i))
				{
					if (mouseMove)
					{
						ClickMap[i].Delta = Input.mousePosition - ClickMap[i].Position;
						ClickMap[i].Position = Input.mousePosition;
						ClickMap[i].IsMove = true;
					}
					else
						ClickMap[i].IsMove = false;
				}
			}

			for (int i = 0; i <= 1; i++)
			{
				if (Input.GetMouseButtonDown(i))
				{
					if (IsOverGUI(i))
						onDownUI.Invoke(i, ClickMap[i]);
					else
						onDownScene.Invoke(i, ClickMap[i]);
					onDown.Invoke(i, ClickMap[i]);
				}
				else if (Input.GetMouseButtonUp(i))
				{
					if (ClickMap[i].IsOverUI)
						onUpUI.Invoke(i, ClickMap[i]);
					else
						onUp.Invoke(i, ClickMap[i]);
				}
				else if (Input.GetMouseButton(i))
				{
					if (mouseMove)
					{
						onDrag.Invoke(i, ClickMap[i]);
						if (ClickMap[i].IsDown)
						{
							if (ClickMap[i].IsOverUI)
								onDragUI.Invoke(i, ClickMap[i]);
							else
								onDragScene.Invoke(i, ClickMap[i]);
						}
					}
					else
						ClickMap[i].IsMove = false;
				}
			}

			var mouseWheel = Input.GetAxis("Mouse ScrollWheel");
			if (mouseWheel > 0.1 || mouseWheel < -0.1)
			{
				onWheelEvent.Invoke(mouseWheel);
			}

			if (m_keycode == null)
			{
				var codes = Enum.GetValues(typeof(KeyCode));
				m_keycode = new KeyCode[codes.Length];
				int index = 0;
				foreach (KeyCode k in codes)
				{
					m_keycode[index++] = k;
				}
			}
			foreach (KeyCode keycode in m_keycode)
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
					var deltaPosition = touch.deltaPosition;
					if (fingerId >= MAX_SUPPORT_FINGERID)
					{
						return;
					}

					ClickMap[i].IsMove = false;
					switch (phase)
					{
						case TouchPhase.Began:
							ClickMap[i].IsDown = true;
							ClickMap[i].Position = position;
							ClickMap[i].Delta = Vector2.zero;
							onDown.Invoke(i, ClickMap[i]);
							if (IsOverGUI(fingerId))
								ClickMap[i].IsOverUI = true;
							else
							{
								if (i == 1)
									doubleTouchLastDis = Vector3.Distance(ClickMap[0].Position, ClickMap[1].Position);
								ClickMap[i].IsOverUI = false;
							}
							break;
						case TouchPhase.Ended:
						case TouchPhase.Canceled:
							ClickMap[i].IsDown = false;
							ClickMap[i].IsMove = false;
							ClickMap[i].IsOverUI = false;
							ClickMap[i].Position = Vector3.zero;
							ClickMap[i].Delta = Vector2.zero;
							break;
						case TouchPhase.Stationary:
							ClickMap[i].IsMove = false;
							ClickMap[i].Position = position;
							ClickMap[i].Delta = Vector2.zero;
							break;
						case TouchPhase.Moved:
							ClickMap[i].Delta = deltaPosition;
							ClickMap[i].Position = position;
							ClickMap[i].IsMove = true;
							break;
						default:
							break;
					};

					
				}

				for (int i = 0; i < Input.touchCount; i++)
				{
					var touch = Input.GetTouch(i);
					var phase = touch.phase;
					var fingerId = touch.fingerId;
					switch (phase)
					{
						case TouchPhase.Began:
							onDown.Invoke(i, ClickMap[i]);
							if (IsOverGUI(fingerId))
								onDownUI.Invoke(i, ClickMap[i]);
							else
								onDownScene.Invoke(i, ClickMap[i]);
							break;
						case TouchPhase.Ended:
							onUp.Invoke(i, ClickMap[i]);
							if (ClickMap[i].IsOverUI)
								onUpUI.Invoke(i, ClickMap[i]);
							else
								onUpScene.Invoke(i, ClickMap[i]);
							break;
						case TouchPhase.Moved:
							onDrag.Invoke(i, ClickMap[i]);
							if (ClickMap[i].IsDown)
							{
								if (ClickMap[i].IsOverUI)
									onDragUI.Invoke(i, ClickMap[i]);
								else
									onDragScene.Invoke(i, ClickMap[i]);
							}
							break;
						default:
							break;
					};
				}

				if (Input.touchCount == 2 && !ClickMap[0].IsOverUI && ClickMap[0].IsMove && !ClickMap[1].IsOverUI && ClickMap[1].IsMove)
				{
					float doubleTouchCurrDis = Vector3.Distance(ClickMap[0].Position, ClickMap[1].Position);
					float distance = doubleTouchCurrDis - doubleTouchLastDis;
					onWheelEvent.Invoke((distance < 0 ? 1 : -1) * 0.1f);
					doubleTouchLastDis = doubleTouchCurrDis;
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

		public static bool IsOverGUI(int touchId)
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			if (EventSystem.current.IsPointerOverGameObject())

#else
			if (EventSystem.current.IsPointerOverGameObject(touchId))
#endif
				return true;

			else
					return false;
		}
	}
}