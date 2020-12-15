// ========================================================
// des：玩家输入系统
// author: shenyi
// time：2020-12-11 13:48:07
// version：1.0
// ========================================================


using System;
using UnityEngine;

namespace Game
{
	public class InputComp : EntityComp
	{
		/// <summary>
		/// 默认重力
		/// </summary>
		private const float DefaultGravity = 20;

		/// <summary>
		/// 默认水平速度
		/// </summary>
		private const float DefaultHorizontalSpeed = 1;

		/// <summary>
		/// 默认垂直速度
		/// </summary>
		private const float DefaultVerticalSpeed = 0;

		/// <summary>
		/// 当前实际重力
		/// </summary>
		public float SelfGravity = 0;

		/// <summary>
		/// 当前水平重力 （仅在跳跃时生效 用于表现跳跃的力度）
		/// </summary>
		public float SelfHorizontalGravity = 0;

		/// <summary>
		/// 人物的水平移动速度
		/// </summary>
		public float SelfHorizontalSpeed = 0;

		/// <summary>
		/// 人物的竖直移动速度
		/// </summary>
		public float SelfVerticalSpeed = 0;

		/// <summary>
		/// 摇杆时人物的移动方向 从MoveDir中传入
		/// </summary>
		public Vector3 SelfMovingDirByJoyStick = Vector3.zero;

		/// <summary>
		/// 摇杆时人物的移动速度（大小+方向） 计算得到
		/// </summary>
		public Vector3 SelfMovingSpeedByStick = Vector3.zero;


		public bool IsJump = false;
		public Vector2 JoySticDir { get; private set; } = Vector2.zero;
		

		public override void OnAdd()
		{
			if (UIJoyStick.Instance == null)
			{
				Debug.LogError("请先加载摇杆，确保UIJoyStick组件存在");
				return;
			}

			UIJoyStick.OnJoyStickTouchMove += OnJoyStickTouchMove;
			UIJoyStick.OnJoyStickTouchEnd += OnJoyStickTouchEnd;
			
			SelfVerticalSpeed = DefaultVerticalSpeed;
			SelfHorizontalSpeed = DefaultHorizontalSpeed;
			SelfGravity = DefaultGravity;
		}

		private void OnJoyStickTouchMove(Vector2 vec)
		{
			
			
		}
		private void OnJoyStickTouchEnd()
		{
			
		}


		public override void OnUpdate(float deltaTime)
		{
			UpdateKeyEvent();
		}

		private void UpdateKeyEvent()
		{
			if (!Application.isMobilePlatform)
			{
				float x = 0, y = 0;
				bool isGetKey = false;
				if (Input.GetKey(KeyCode.W))
				{
					y = 50;
					isGetKey = true;
				}
				if (Input.GetKey(KeyCode.S))
				{
					y = -50;
					isGetKey = true;
				}
				if (Input.GetKey(KeyCode.A))
				{
					x = -50;
					isGetKey = true;
				}
				if (Input.GetKey(KeyCode.D))
				{
					x = 50;
					isGetKey = true;
				}
				if (Input.GetKeyDown(KeyCode.Space))
				{
					IsJump = true;
				}
				else
				{
					IsJump = false;
				}

				if (isGetKey)
				{
					Vector2 tragetDir = new Vector2(x, y);
					JoySticDir = tragetDir;
				}
				else
					JoySticDir = Vector3.zero;
			}
		}

		
		/// <summary>
		/// 移除组件时
		/// </summary>
		public override void OnRemove()
		{
			JoySticDir = Vector3.zero;
			JoySticDir = Vector3.zero;
			UIJoyStick.OnJoyStickTouchMove -= OnJoyStickTouchMove;
			UIJoyStick.OnJoyStickTouchEnd -= OnJoyStickTouchEnd;
		}

		

	}
}
