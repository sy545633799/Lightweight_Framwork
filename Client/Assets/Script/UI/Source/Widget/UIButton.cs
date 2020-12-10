using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game
{
	public class UIButton : Selectable, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
	{
		private bool isPress = false;
		private bool isLongPress = false;
		private Vector2? pos;
		private float clickTime;
		private float pressTime;
		//Lua的UI中，卸载UI的时候都已经RemoveAllListener, 所以不用继承IDowncast
		public class ClickEvent : UnityEvent { }
		public class UIButtonEvent : UnityEvent<Vector2> { }
		public class DoubleClickEvent : UnityEvent<float> { }
		private static ObjectPool<ClickEvent> ClickEventPool = new ObjectPool<ClickEvent>(() => new ClickEvent(), 20);
		private static ObjectPool<UIButtonEvent> UIButtonEventPool = new ObjectPool<UIButtonEvent>(() => new UIButtonEvent(), 10);
		private static ObjectPool<DoubleClickEvent> DoubleClickEventPool = new ObjectPool<DoubleClickEvent>(() => new DoubleClickEvent(), 10);

		private ClickEvent m_onClick;
		public ClickEvent onClick
		{
			get
			{
				if (m_onClick == null)
					m_onClick = ClickEventPool.Alloc();
				return m_onClick;
			}
		}

		private UIButtonEvent m_onDown;
		public UIButtonEvent onDown
		{
			get
			{
				if (m_onDown == null)
					m_onDown = UIButtonEventPool.Alloc();
				return m_onDown;
			}
		}

		private UIButtonEvent m_onPress;
		public UIButtonEvent onPress
		{
			get
			{
				if (m_onPress == null)
					m_onPress = UIButtonEventPool.Alloc();
				return m_onPress;
			}
		}

		private UIButtonEvent m_onUp;
		public UIButtonEvent onUp
		{
			get
			{
				if (m_onUp == null)
					m_onUp = UIButtonEventPool.Alloc();
				return m_onUp;
			}
		}

		private DoubleClickEvent m_ondoubleClick;
		public DoubleClickEvent ondoubleClick
		{
			get
			{
				if (m_ondoubleClick == null)
					m_ondoubleClick = DoubleClickEventPool.Alloc();
				return m_ondoubleClick;
			}
		}

		[HideInInspector]
		public AudioClip EnterClip;
        [HideInInspector]
        public AudioClip ClickClip;
        [HideInInspector]
        public AudioClip ExitClip;
        [HideInInspector]
        public AudioSource AudioSource;

		protected override void Awake()
		{
			base.Awake();
		}

		private void PlayAudio(AudioClip ac)
		{
			if (AudioSource && ac)
                this.AudioSource.PlayOneShot(ac);
        }
		public void OnPointerClick(PointerEventData eventData)
		{
			if (!isLongPress)
			{
				onClick.Invoke();
                PlayAudio(ClickClip);
            }
			isLongPress = false;
		}

		public override void OnPointerDown(PointerEventData eventData)
        {
            isPress = true;
            pressTime = Time.time;
            pos = eventData.position;
            onDown?.Invoke((Vector2)pos);
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            isPress = false;
            onUp?.Invoke((Vector2)pos);
            pos = null;
            float deltaTime = Time.time - clickTime;
            if (deltaTime < 0.3f)
            {
                ondoubleClick?.Invoke(deltaTime);
                clickTime = 0;
            }
            clickTime = Time.time;
            base.OnPointerUp(eventData);
        }

		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
            PlayAudio(EnterClip);
        }

		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
            PlayAudio(ExitClip);
        }

		//不能继承IDragHandler,会引起拖拽问题
		//public void OnDrag(PointerEventData eventData)
		//{
		//    pos = eventData.position;
		//    onDrag.Invoke(eventData.delta);
		//}

		void Update()
        {
            if (isPress)
            {
                float deltaTime = Time.time - pressTime;
                //长按判定 每帧执行
                if (isLongPress || (deltaTime >= 0.35f))
                {
                    if (base.IsPressed())
                    {
                        isLongPress = true;
                        onPress?.Invoke((Vector2)pos);
                    }
                    else
                    {
                        isPress = false;
                        isLongPress = false;
                    }
                }
            }
        }

		protected override void OnDestroy()
		{
			ClickEventPool.Recycle(m_onClick);
			m_onClick = null;
			UIButtonEventPool.Recycle(m_onDown);
			m_onDown = null;
			UIButtonEventPool.Recycle(m_onPress);
			m_onPress = null;
			UIButtonEventPool.Recycle(m_onUp);
			m_onUp = null;
			DoubleClickEventPool.Recycle(m_ondoubleClick);
			m_ondoubleClick = null;
		}
	}
}