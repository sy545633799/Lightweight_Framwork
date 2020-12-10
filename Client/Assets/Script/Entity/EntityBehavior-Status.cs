// ========================================================
// des：
// author: 
// time：2020-12-10 18:06:02
// version：1.0
// ========================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

namespace Game
{
	public partial class EntityBehavior : EventObject, IMonoPoolObject<EntityBehavior>
	{
		public string uid = null;
		public int sceneid = 0;
		public int entityType = 0;
		// public bool isControlable = false;
		public bool isControlledByJoystick = false;
		public bool isSyncable = false;
		public bool isHero = false;
		public bool bodyLoading = false;
		public int res_id = 0;

		public int unitCamp = 0;

		/// <summary>
		/// 上一帧的跳跃状态
		/// </summary>
		private bool lastGroundState = true;

		/// <summary>
		/// 着地
		/// </summary>
		public Action onLand;

		/// <summary>
		/// 起跳
		/// </summary>
		public Action onJump;

		private MainCamera m_MainCamera;
		public MainCamera MainCamera
		{
			get
			{
				if (m_MainCamera == null)
					m_MainCamera = MainCamera.Instance;

				return m_MainCamera;
			}
		}

		private CharacterController m_CharacterController;
		/// <summary>
		/// 角色控制器 控制移动
		/// </summary>
		public CharacterController characterController
		{
			get
			{
				switch (this.entityType)
				{
					case 0:
						if (m_CharacterController == null)
						{
							m_CharacterController = gameObject.GetComponent<CharacterController>();
							if (m_CharacterController == null)
								m_CharacterController = gameObject.AddComponent<CharacterController>();
							m_CharacterController.center = new Vector3(0, m_CharacterController.height * 0.5f, 0);
							m_CharacterController.radius = 0.5f;
						}
						break;
					default:
						if (m_CharacterController != null)
						{
							Destroy(m_CharacterController);
							m_CharacterController = null;
						}
						break;

				}
				return m_CharacterController;
			}
		}

		private bool destroyed = false;
		public bool Destroyed
		{
			set
			{
				destroyed = value;
			}
			get
			{
				return destroyed;
			}
		}

		private float logicSpeed = 1f;

		public float GetLogicSpeed()
		{
			return logicSpeed;
		}

		// 感觉这里并不是严格意义上的逻辑速度
		public virtual void SetLogicSpeed(float speed, bool effect = false)
		{
			logicSpeed = speed;
		}

		//public bool IsRidingSword {
		//	get {
		//		if (animComp != null)
		//			return animComp.IsRidingSword;
		//		return false;
		//	}
		//}

		//public bool IsJumping {
		//	get {
		//		if (animComp != null)
		//			return animComp.IsJumping;
		//		return false;
		//	}
		//}

		public bool IsPlayingDie
		{
			get
			{
				//if (animComp != null)
				//{
				//	return animComp.currentAnimStateName == animComp.dieAnimation;
				//}
				return false;
			}
		}
		//public bool IsPlayingAttack {
		//	get {
		//		if (animComp != null) {
		//			return animComp.IsAttackAnim (animComp.currentAnimStateName);
		//		}
		//		return false;
		//	}
		//}
	}
}
