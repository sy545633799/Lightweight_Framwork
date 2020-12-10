using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{

	public class UIJoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler//需要注意继承的接口，接口内的方法需要实现
	{
		public static UIJoyStick Instance { private set; get; }
		/// <summary>
		/// 摇杆最大半径
		/// 以像素为单位
		/// </summary>
		public float JoyStickRadius = 50;

		/// <summary>
		/// 摇杆重置所诉
		/// </summary>
		public float JoyStickResetSpeed = 5.0f;

		/// <summary>
		/// 当前物体的Transform组件
		/// </summary>
		private RectTransform selfTransform;

		/// <summary>
		/// 是否触摸了虚拟摇杆
		/// </summary>
		private bool isTouched = false;

		/// <summary>
		/// 虚拟摇杆的默认位置
		/// </summary>
		private Vector2 originPosition;

		/// <summary>
		/// 虚拟摇杆的移动方向
		/// </summary>
		private Vector2 touchedAxis;
		public Vector2 TouchedAxis
		{
			get
			{
				if (touchedAxis.magnitude < JoyStickRadius)
					return touchedAxis.normalized / JoyStickRadius;
				return touchedAxis.normalized;
			}
		}

		/// <summary>
		/// 定义触摸开始事件委托 
		/// </summary>
		public delegate void JoyStickTouchBegin(Vector2 vec);

		/// <summary>
		/// 定义触摸过程事件委托 
		/// </summary>
		/// <param name="vec">虚拟摇杆的移动方向</param>
		public delegate void JoyStickTouchMove(Vector2 vec);

		/// <summary>
		/// 定义触摸结束事件委托
		/// </summary>
		public delegate void JoyStickTouchEnd();

		/// <summary>
		/// 注册触摸开始事件
		/// </summary>
		public event JoyStickTouchBegin OnJoyStickTouchBegin;

		/// <summary>
		/// 注册触摸过程事件
		/// </summary>
		public event JoyStickTouchMove OnJoyStickTouchMove;

		/// <summary>
		/// 注册触摸结束事件
		/// </summary>
		public event JoyStickTouchEnd OnJoyStickTouchEnd;

		void Start()
		{
			Instance = this;
			//初始化虚拟摇杆的默认方向
			selfTransform = this.GetComponent<RectTransform>();
			originPosition = selfTransform.anchoredPosition;
		}


		public void OnPointerDown(PointerEventData eventData)
		{
			isTouched = true;
			touchedAxis = GetJoyStickAxis(eventData);
			if (this.OnJoyStickTouchBegin != null)
				this.OnJoyStickTouchBegin(TouchedAxis);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			isTouched = false;
			selfTransform.anchoredPosition = originPosition;
			touchedAxis = Vector2.zero;
			if (this.OnJoyStickTouchEnd != null)
				this.OnJoyStickTouchEnd();
		}

		public void OnDrag(PointerEventData eventData)
		{
			touchedAxis = GetJoyStickAxis(eventData);
			if (this.OnJoyStickTouchMove != null)
				this.OnJoyStickTouchMove(TouchedAxis);
		}


		void Update()
		{
			//当虚拟摇杆移动到最大半径时摇杆无法拖动
			//为了确保被控制物体可以继续移动
			//在这里手动触发OnJoyStickTouchMove事件
			if (isTouched && touchedAxis.magnitude >= JoyStickRadius)
			{
				if (this.OnJoyStickTouchMove != null)
					this.OnJoyStickTouchMove(TouchedAxis);
			}

			//松开虚拟摇杆后让虚拟摇杆回到默认位置
			if (selfTransform.anchoredPosition.magnitude > originPosition.magnitude)
				selfTransform.anchoredPosition -= TouchedAxis * Time.deltaTime * JoyStickResetSpeed;
		}

		/// <summary>
		/// 返回虚拟摇杆的偏移量
		/// </summary>
		/// <returns>The joy stick axis.</returns>
		/// <param name="eventData">Event data.</param>
		private Vector2 GetJoyStickAxis(PointerEventData eventData)
		{
			//获取手指位置的世界坐标
			Vector3 worldPosition;
			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(selfTransform,
					 eventData.position, eventData.pressEventCamera, out worldPosition))
				selfTransform.position = worldPosition;
			//获取摇杆的偏移量
			Vector2 touchAxis = selfTransform.anchoredPosition - originPosition;
			//摇杆偏移量限制
			if (touchAxis.magnitude >= JoyStickRadius)
			{
				touchAxis = touchAxis.normalized * JoyStickRadius;
				selfTransform.anchoredPosition = touchAxis;
			}
			return touchAxis;
		}

		private void OnDestroy()
		{
			Instance = null;
		}
	}
}
