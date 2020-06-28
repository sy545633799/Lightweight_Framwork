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
        public class ClickEventEx : UnityEvent<Vector2, GameObject> { }
        public class WheelEvent : UnityEvent<float> { }
        public class KeyCodeEvent : UnityEvent<KeyCode> { }

        public static WheelEvent onWheelEvent = new WheelEvent();
        public static KeyCodeEvent onkeyCodeDownEvent = new KeyCodeEvent();
        public static KeyCodeEvent onKeyCodeUpEvent = new KeyCodeEvent();

        public static ClickEvent onDown = new ClickEvent();
        public static ClickEvent onUp = new ClickEvent();
        public static ClickEvent onDrag = new ClickEvent();

        public static ClickEventEx onDownScene = new ClickEventEx();
        public static ClickEventEx onUpScene = new ClickEventEx();
        public static ClickEvent onDownUI = new ClickEvent();
        public static ClickEvent onUpUI = new ClickEvent();

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        private static KeyCode[] s_keycode = null;
        private static Hashtable mouseMap = new Hashtable();
        private static Hashtable mouseUIMap = new Hashtable();
#endif
        private static Hashtable touchMap = new Hashtable();
        private static Hashtable touchUIMap = new Hashtable();

        private static EventSystem eventSys;
        private static bool touchEnabled = true;
        private static int MAX_SUPPORT_FINGERID { get { return 10; } }

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
            for (int i = 0; i <= 1; i++)
            {
                if (Input.GetMouseButtonDown(i))
                {
                    if (!IsOverGUI(Input.mousePosition))
                    {
                        mouseMap[i] = true;
                        onDownScene.Invoke(Input.mousePosition, DetectHit(Input.mousePosition));
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
                    if (mouseMap[i] != null)
                    {
                        mouseMap[i] = null;
                        onUpScene.Invoke(Input.mousePosition, DetectHit(Input.mousePosition));
                    }
                    else if (mouseUIMap[i] != null)
                    {
                        mouseUIMap[i] = null;
                        onUpUI.Invoke(Input.mousePosition);
                    }
                    onUp.Invoke(Input.mousePosition);
                }

                if (mouseMap[i] != null)
                    onDrag.Invoke(Input.mousePosition);
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
						if (!IsOverGUI(position))
						{
							touchMap[fingerId] = true;
							onDownScene.Invoke(position, DetectHit(position));
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
						if (touchMap[fingerId] != null)
						{
							touchMap[fingerId] = null;
							onUpScene.Invoke(position, DetectHit(position));
						}
						else if (touchUIMap[fingerId] != null)
						{
							touchUIMap[fingerId] = null;
							onUpUI.Invoke(position);
						}
						onUp.Invoke(position);
						break;
					default:
						if (touchMap[fingerId] != null)
							onDrag.Invoke(position);
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

        public static bool IsOverGUI(Vector2 position)
        {
            //实例化点击事件
            PointerEventData eventDataCurrentPosition = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            //将点击位置的屏幕坐标赋值给点击事件
            eventDataCurrentPosition.position = new Vector2(position.x, position.y);

            List<RaycastResult> results = new List<RaycastResult>();

            if (EventSystem.current)
                EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            return results.Count > 0;
        }
    }
}