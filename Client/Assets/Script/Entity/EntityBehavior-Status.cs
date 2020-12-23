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
using XLua;

namespace Game
{
	public partial class EntityBehavior : EventObject, IMonoPoolObject<EntityBehavior>
	{
		public int AoiId = 0;
		public int SceneId = 0;
		public int EntityType = 0;
		public bool bodyLoading = false;
		public int ResId = 0;
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

		private CharacterController m_Controller;
		public CharacterController Controller
		{
			get
			{
				if (!m_Controller)
				{
					m_Controller = gameObject.AddComponent<CharacterController>();
					m_Controller.center = new Vector3(0, m_Controller.height * 0.5f, 0);
					m_Controller.radius = 0.5f;
				}
				return m_Controller;
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


		public virtual void SyncStatus(s2c_aoi_trans status)
		{
			
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
