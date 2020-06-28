using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class UIButton : Button, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private bool isPress = false;
        private Vector2? pos;
        private float clickTime;
        public class UIButtonEvent : UnityEvent<Vector2> { }
        public UIButtonEvent onDown { get; } = new UIButtonEvent();
        public UIButtonEvent onPress { get; } = new UIButtonEvent();
        public UIButtonEvent onUp { get; } = new UIButtonEvent();
        public UIButtonEvent onDrag { get; } = new UIButtonEvent();
        public class DoubleClickEvent : UnityEvent<float> { }
        public DoubleClickEvent ondoubleClick { get; } = new DoubleClickEvent();

        public override void OnPointerDown(PointerEventData eventData)
        {
            isPress = true;
            pos = eventData.position;
            onDown.Invoke((Vector2)pos);
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            isPress = false;
            onUp.Invoke((Vector2)pos);
            pos = null;
            float deltaTime = Time.time - clickTime;
            if (deltaTime < 0.3f)
            {
                ondoubleClick.Invoke(deltaTime);
                clickTime = 0;
            }
            clickTime = Time.time;
            base.OnPointerUp(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            pos = eventData.position;
            onDrag.Invoke(eventData.delta);
        }

        void Update()
        {
            //无法与drag区分
            if (isPress)
            {
                onPress?.Invoke((Vector2)pos);
            }
        }
    }
}