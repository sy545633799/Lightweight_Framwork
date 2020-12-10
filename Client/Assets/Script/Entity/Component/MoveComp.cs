// ========================================================
// des：
// author: shenyi
// time：2020-07-09 14:13:27
// version：1.0
// ========================================================
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class MoveComp : EntityComp {
		public MoveComp()
		{
			this.m_NavimeshPathCorners= new List<Vector3>();
		}

		public MoveComp(EntityBehavior behavior) : base(behavior) { }
		
		//位移类型 [注意：SkillEditor模块脚本中也存在一个public的MoveType]
		private enum MovingType:byte {
			None = 0,
			Navigation,
			JoyStick,
		}
		//位移组件所挂载的UnityGameObject宿主Transform
		private Transform m_SelfTransform;
		private MovingType m_MovingType = MovingType.None;
		
		//位移组件中关联动画组件 [编码时以animComp属性为准]
		private AnimComp m_AnimComp = null;
		private AnimComp animComp {
			get {
				if (m_AnimComp == null)
					m_AnimComp = (AnimComp)behavior.GetEntityComp<AnimComp>();
				return m_AnimComp;
			}
			set {
				m_AnimComp = value;
			}
		}
		
		/// <summary>
		/// 是否产生了水平方向的位移
		/// </summary>
		public bool IsMoving {
			get {
				//摇杆或寻路
				if (m_MovingType == MovingType.Navigation || m_MovingType == MovingType.JoyStick)
					return true;
				//跳跃 或 御剑且水平速度不为0
				if (animComp != null && m_SelfVerticalSpeed != 0.0f && (animComp.IsJumping || animComp.IsRidingSword))
					return true;
				return false;
			}
		}
		
		// 【有位移则需要同步到服务端】
		#region ==================== 导航位移变量 ====================
		//导航网格上的顶点集合
		private List<Vector3> m_NavimeshPathCorners;
		//导航时距离目的地多远停止
		private float m_NaviStopDistance;
		//导航时（moveTo）每帧移动的距离
		public float m_NaviMovingSpeed = 6;
		//给外界提供当前设置的导航目的地坐标[只读]
		public Vector3 Destionation { get; private set; }
		//导航位移时到达目的地的事件
		private Action<bool> m_ActionNavigationArrived;
		//导航位移时正在位移中事件
		private Action m_ActionNavigationMoving;
		#endregion

		#region ==================== 摇杆位移变量 ====================
		/// <summary>
		/// 默认重力
		/// </summary>
		private const float m_SelfDefaultGravity = 20;

		/// <summary>
		/// 默认水平速度
		/// </summary>
		private const float m_SelfDefaultHorizontalSpeed = 1;

		/// <summary>
		/// 默认垂直速度
		/// </summary>
		private const float m_SelfDefaultVerticalSpeed = 0;

		/// <summary>
		/// 当前实际重力
		/// </summary>
		private float m_SelfGravity = 0;

		/// <summary>
		/// 当前水平重力 （仅在跳跃时生效 用于表现跳跃的力度）
		/// </summary>
		private float m_SelfHorizontalGravity = 0;

		/// <summary>
		/// 人物的水平移动速度
		/// </summary>
		private float m_SelfHorizontalSpeed = 0;

		/// <summary>
		/// 人物的竖直移动速度
		/// </summary>
		private float m_SelfVerticalSpeed = 0;

		/// <summary>
		/// 摇杆时人物的移动方向 从MoveDir中传入
		/// </summary>
		private Vector3 m_SelfMovingDirByJoyStick = Vector3.zero;

		/// <summary>
		/// 摇杆时人物的移动速度（大小+方向） 计算得到
		/// </summary>
		private Vector3 m_SelfMovingSpeedByStick = Vector3.zero;

		#endregion
		

        #region 【==================== Update帧驱动 这里的后续会被专门的输入系统取代,暂时先这样 ====================】

        public override void OnUpdate(float deltaTime)
        {
            UpdateKeyEvent();
        }

        private bool isDraging = false; // 这个是为了区分移遥感和键盘的 这里其实是没必要加的
        private bool isKeyDraging = false;
        private int frames = 0;

        private void UpdateKeyEvent()
        {
	        if (!this.behavior.isControlledByJoystick)
		        return;
            Vector3 dragDir = Vector3.zero;
            if (isDraging)
                isKeyDraging = false;
            else
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

                    if (isGetKey)
                    {
	                    Vector2 tragetDir = new Vector2(x, y);
                        dragDir = behavior.MainCamera.ConvertDirByCam(tragetDir);
                        //Debug.LogError("dragDir + " + dragDir);
                        if (!isKeyDraging)
                        {
                            isKeyDraging = true;
                            this.MoveByJoystick(dragDir);
                        }
                    }
                    else
                    {
                        if(isKeyDraging)
                        {
	                        Debug.LogWarning("[C#] MoveComp.cs ::UpdateKeyEvent() 当前被isKeyDraging参数被迫停止Moving");
	                        this.StopMoveByNavimesh();
                        }
                        isKeyDraging = false;
                    }
                }
            }

            // 防止指令过于频繁
            if(isDraging || isKeyDraging)
            {
                frames++;
                if(frames == 1)
	                this.MoveByJoystick(dragDir);
                if(frames > 5)
	                frames = 0;
            }
            else
	            frames = 0;
        }

        // 计算位移
        public override void OnFixedUpdate (float deltaTime) {
	        // 正在跳跃的时候 不执行
			if (animComp != null && animComp.jumpFlag)
				return;
			UpdateSpeed (deltaTime);
			OnMoving (deltaTime);
		}

		/// <summary>
		/// [每帧]
		/// (大小+方向）
		/// </summary>
		/// <param name="deltaTime"></param>
		private void UpdateSpeed (float deltaTime) {
			// 不在地面
			if (behavior.characterController != null && !behavior.characterController.isGrounded) {
				// 跳跃处理水平加速度
				if (animComp != null && animComp.IsJumping) {
					m_SelfHorizontalSpeed -= m_SelfHorizontalGravity * deltaTime;
					if (m_SelfHorizontalSpeed < 0)
						m_SelfHorizontalSpeed = 0;
				}
				m_SelfVerticalSpeed -= m_SelfGravity * deltaTime;
			}

			if (m_MovingType == MovingType.Navigation) {
				m_SelfMovingSpeedByStick = Vector3.zero;
			} else if (m_MovingType == MovingType.JoyStick) {
				// 御剑或者跳跃时 水平速度才会被覆盖
				if (animComp != null && (animComp.IsJumping || animComp.IsRidingSword) && m_SelfHorizontalSpeed > m_SelfDefaultHorizontalSpeed) {
					m_SelfMovingSpeedByStick = m_SelfHorizontalSpeed * m_NaviMovingSpeed * behavior.transform.forward.normalized + m_SelfVerticalSpeed * Vector3.up;
				} else {
					m_SelfMovingSpeedByStick = m_SelfDefaultHorizontalSpeed * m_NaviMovingSpeed * behavior.transform.forward.normalized + m_SelfVerticalSpeed * Vector3.up; //movrDir
				}
            } else if (m_MovingType == MovingType.None) // 可能存在上升 、下降
			{
				m_SelfMovingSpeedByStick = m_SelfHorizontalSpeed * m_NaviMovingSpeed * behavior.transform.forward.normalized + m_SelfVerticalSpeed * Vector3.up;
			}
		}

		/// <summary>
		/// 移动ing
		/// </summary>
		/// <param name="deltaTime"></param>
		void OnMoving (float deltaTime) {
			//@TobyStark 这里如果是None类型就不在执行，不然无法停止[非导航也非摇杆模式]
			if (m_MovingType == MovingType.None)
				return;
			if (m_MovingType == MovingType.Navigation) // 导航
			{
				if (m_NavimeshPathCorners.Count <= 0)
					return;
				if (m_ActionNavigationArrived != null && m_NavimeshPathCorners.Count > 0) {
					// 如果 当前离终点（路径的最后一个点）的距离 小于一个规定值 则 视为 到达寻路终点
					Vector3 destination = m_NavimeshPathCorners[m_NavimeshPathCorners.Count - 1];
					if (Vector3.Distance (destination, m_SelfTransform.position) <= m_NaviStopDistance) {
						m_ActionNavigationArrived (true);
						this.StopMoveByNavimesh ();
						return;
					}
				}
				// 寻路路径上的各点 按照直线行走
				Vector3 nextCorner = m_NavimeshPathCorners[0];
				Vector3 dir = nextCorner - m_SelfTransform.position;
				dir.y = 0;
				Vector3 deltaDir = (m_NaviMovingSpeed * deltaTime * dir.normalized); // 移动的距离
				Vector3 nextPos = m_SelfTransform.position + deltaDir; // 可能不在网格上

				// 判断是否穿过了网格（穿墙 判断）
				UnityEngine.AI.NavMeshHit hit;
				//向目标点发射一条射线，返回true说明碰到了边缘，Hit就是边缘那个点
				if (UnityEngine.AI.NavMesh.Raycast (m_SelfTransform.position, nextPos, out hit, UnityEngine.AI.NavMesh.AllAreas)) {
					nextPos = m_SelfTransform.position + m_NaviMovingSpeed * 0.02f * dir.normalized; //这里给一个不会穿墙的距离
				}
				// 将nextPos 矫正到网格上
				nextPos = Util_SamplePosition (nextPos, true);
				// // 是否碰到障碍物
				// if (EntityBehaviorManager.Instance ().HitBarrier (m_SelfTransform.position, nextPos)) {
				// 	if (moveType == MoveType.Navigation) {
				// 		StopMove ();
				// 	}
				// 	return;
				// }

				// 修改朝向  
				RotateComp rotateComp = (RotateComp)behavior.GetEntityComp<RotateComp>();
				if (rotateComp != null)
					rotateComp.SetLookAt (nextCorner, true);

				// dirAfter：这一帧之后的朝向     dir：移动前的朝向
				Vector3 dirAfter = nextCorner - nextPos;
				dirAfter.y = 0;

				float dot = dir.x * dirAfter.x + dir.z * dirAfter.z;
				// 前后dir方向相反 多走一步就走过头了 或者 dir太小
				// 视为 到达 当前路径上的寻路点
				if (dot < 0 || dir.magnitude < 0.01f) {
					m_SelfTransform.position = nextCorner;
					if (m_NavimeshPathCorners.Count > 1) {
						m_NavimeshPathCorners.RemoveAt(0);
					} else // 最后一个点 到达终点
					{
						m_ActionNavigationArrived?.Invoke (true);
						this.StopMoveByNavimesh ();
					}
				} else // 还没有到终点
				{
					//  ** 更新位置 **
					m_SelfTransform.position = nextPos;
					m_ActionNavigationMoving?.Invoke ();
				}
				// Debug.Log($"[C#] MoveComp.cs ::MoveTo() => 导航模式{moveType}");

			} 
			else if (m_MovingType == MovingType.JoyStick) // 摇杆
			{
				//需要修改朝向
				Vector3 dirAfter = behavior.transform.position + m_SelfMovingDirByJoyStick * deltaTime;
				dirAfter.y = 0;
				RotateComp rotateComp = (RotateComp)behavior.GetEntityComp<RotateComp>();
				rotateComp.SetLookAt (dirAfter, true);
				if (behavior.characterController != null) 
					behavior.characterController.Move (m_SelfMovingSpeedByStick * deltaTime);
			} 
			else {
				if (behavior.characterController != null) {
					//如果是Navi导航起步，中途操作了摇杆，那么操作期间为moveType == MoveType.JoyStick，以后就是非导航也非摇杆模式
					//或者御剑等飞行期间
					// Debug.Log($"[C#] MoveComp.cs ::MoveTo() => 非导航也非摇杆模式{moveType}");
					if (behavior.characterController.isGrounded) {
						m_SelfMovingSpeedByStick.x = 0;
						m_SelfMovingSpeedByStick.z = 0;
					}
					//else  // 这里可能是跳跃或御剑 水平速度自己管理
					//{

					//}
					behavior.characterController.Move (m_SelfMovingSpeedByStick * deltaTime);
				}
			}

			// // 同步到服务器
			// if (behavior.isSyncable && syncComp != null && behavior.rotateComp != null && IsNeedSyncMove) {
			// 	syncComp.SyncMove (m_SelfTransform.position, moveType, behavior.rotateComp.rotationY, speed);
			// }
		}

		#endregion

		#region 【==================== 状态变更的方法 ====================】
		/// <summary>
		/// 将普通V3坐标映射到Navimesh上  [validateXZ 是否验证xz方向]
		/// （可以用来获得地形高度，但是最大距离不要太大，注意效率）
		/// </summary>
		private Vector3 Util_SamplePosition (Vector3 pos, bool validateXZ = false) {
			UnityEngine.AI.NavMeshHit hit;
			if (UnityEngine.AI.NavMesh.SamplePosition (pos, out hit, 20, UnityEngine.AI.NavMesh.AllAreas)) {
				if (validateXZ) {
					return hit.position;
				} else {
					pos.y = hit.position.y;
					return pos;
				}
			}
			return pos;
		}
		/// <summary>
		/// 设置位置
		/// </summary>
		public void SetPosition (Vector3 pos, bool isJoyStick = false)
		{
			if (m_SelfTransform == null)
				return;
			m_SelfTransform.position = isJoyStick ? pos : Util_SamplePosition (pos, true);
		}
		/// <summary>
		/// 导航位移 【对外】
		/// </summary>
		/// <param name="dst"></param>
		/// <param name="stopDis"></param>
		/// <param name="onArr"></param>
		public void MoveByNavimesh (Vector3 dst, float stopDis = 0.01f, Action<bool> onArr = null, Action onMoving = null) {
			if (this.behavior.isControlledByJoystick)
			{
				Debug.LogWarning ("MoveByNavimesh failed! entity isControlledByJoystick");
				return;
			}
			if (behavior.IsPlayingDie) {
				Debug.LogWarning ("MoveByNavimesh failed! entity IsPlayingDie");
				return;
			}
			m_ActionNavigationArrived = onArr;
			this.m_ActionNavigationMoving = onMoving;
			m_NaviStopDistance = stopDis;
			Destionation = dst;
			UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath ();
			//计算路径，返回false就走不过去，返回true说明可以走过去，Corners就是路径点
			if (UnityEngine.AI.NavMesh.CalculatePath (m_SelfTransform.position, dst, UnityEngine.AI.NavMesh.AllAreas, path)) {
				m_NavimeshPathCorners.Clear ();
				m_NavimeshPathCorners.AddRange (path.corners);
				m_NavimeshPathCorners.RemoveAt (0); //移除第一个点, 即当前位置点
				if (m_NavimeshPathCorners.Count == 0) //当前的点可能是到达自身的点
				{
					m_ActionNavigationArrived?.Invoke (true);
					this.StopMoveByNavimesh ();
				}
				m_MovingType = MovingType.Navigation;
			} else {
				Debug.LogWarning("Navi导航寻路异常=>没有可寻路的网格数据，请Editor下查看NaviMeshSurface");
				//每一个异常导致的StopMove()都需要告知外界停止运动事件的发生
				m_ActionNavigationArrived?.Invoke (false);
				this.StopMoveByNavimesh ();
			}
		}

		/// <summary>
		/// 摇杆位移 [私有]
		/// </summary>
		/// <param name="dir"></param>
		private void MoveByJoystick (Vector3 dir) {
			//从导航到摇杆状态改变需要告知外界
			if (m_MovingType == MovingType.Navigation)
				m_ActionNavigationArrived?.Invoke (false);
			m_ActionNavigationArrived = null;
			m_ActionNavigationMoving = null;
			m_NaviStopDistance = 0;
			m_SelfMovingDirByJoyStick = dir;
			m_MovingType = MovingType.JoyStick;
		}

		/// <summary>
		/// 停止导航移动
		/// </summary>
		public void StopMoveByNavimesh () {
			m_NavimeshPathCorners.Clear ();
			m_MovingType = MovingType.None;
			m_ActionNavigationArrived = null;
			m_ActionNavigationMoving = null;
			m_NaviStopDistance = 0;
		}

		#endregion

		#region 【==================== 事件回调 ====================】
		/// <summary>
		/// 添加组件时
		/// </summary>
		public override void OnAdd () {
			if (UIJoyStick.Instance == null)
			{
				Debug.LogError("请先加载摇杆，确保UIJoyStick组件存在");
				return;
			}
			UIJoyStick.OnJoyStickTouchMove += OnJoyStickTouchMove;
            UIJoyStick.OnJoyStickTouchEnd += OnJoyStickTouchEnd;
            m_SelfTransform = behavior.transform;
			behavior.onJump += OnJump;
			behavior.onLand += OnLand;
			m_MovingType = MovingType.None;
			m_SelfVerticalSpeed = m_SelfDefaultVerticalSpeed;
			m_SelfHorizontalSpeed = m_SelfDefaultHorizontalSpeed;
			m_SelfGravity = m_SelfDefaultGravity;
		}

		/// <summary>
		/// 移除组件时
		/// </summary>
		public override void OnRemove () {
			UIJoyStick.OnJoyStickTouchMove -= OnJoyStickTouchMove;
			UIJoyStick.OnJoyStickTouchEnd -= OnJoyStickTouchEnd;
			this.m_NavimeshPathCorners?.Clear ();
			behavior.onJump -= OnJump;
			behavior.onLand -= OnLand;
			m_ActionNavigationArrived = null;
			m_ActionNavigationMoving = null;
			//syncComp = null;
			animComp = null;
		}

		private void OnJoyStickTouchMove(Vector2 vec)
		{
			if (this.behavior.isControlledByJoystick)
				this.MoveByJoystick(behavior.MainCamera.ConvertDirByCam(vec));
		}
		private void OnJoyStickTouchEnd()
		{
			if (this.behavior.isControlledByJoystick)
				this.StopMoveByNavimesh();
		}
		/// <summary>
		/// 起跳的瞬间
		/// </summary>
		private void OnJump () {

		}

		/// <summary>
		/// 落地的瞬间 这里的逻辑有点问题
		/// </summary>
		private void OnLand () //async
		{
			if (animComp != null) {
				//非御剑设置重力
				if (animComp.IsRidingSword) {
					m_SelfGravity = m_SelfDefaultGravity;
					this.StopMoveByNavimesh ();
				}
				if (animComp.IsJumping) {
					m_SelfHorizontalSpeed = 0;
					this.StopMoveByNavimesh ();
				}
			}
		}
		#endregion

		// 暂时用不到
		#region 【==================== 移动参数改变（跳跃、御剑相关） ====================】

		/// <summary>
		/// 设置跳跃时的 移动参数的变化
		/// </summary>
		/// <param name="jumpIndex"></param>
		/// <param name="vs"></param>
		/// <param name="g"></param>
		/// <param name="hs"></param>
		/// <param name="hg"></param>
		public void Jump (int jumpIndex, float vs, float g, float hs, float hg) {
			// 这里的jumpIndex也是需要同步给服务器的 用于校验参数值
			m_SelfVerticalSpeed = vs;
			m_SelfGravity = g;
			m_SelfHorizontalSpeed = hs;
			m_SelfHorizontalGravity = hg;
			SyncMoveParam ();
		}

		/// <summary>
		/// 开始上御剑 
		/// 给一个上剑的速度
		/// </summary>
		/// <param name="vs"></param>
		public void BeginRideOnSword (float vs) {
			m_SelfVerticalSpeed = vs;
			m_SelfGravity = 0;
			m_SelfHorizontalSpeed = 0; // 这一句可能不需要
			SyncMoveParam ();
		}

		/// <summary>
		/// 结束上御剑
		/// </summary>
		public void FinishRideOnSword () {
			m_SelfVerticalSpeed = 0;
			m_SelfHorizontalSpeed = 0;
			SyncMoveParam ();
			this.StopMoveByNavimesh ();
		}

		/// <summary>
		/// 开始下御剑
		/// 给个下剑的速度
		/// </summary>
		/// <param name="vs"></param>
		public void BeginRideOffSword (float vs) {
			m_SelfVerticalSpeed = vs;
			m_SelfGravity = m_SelfDefaultGravity;
			SyncMoveParam ();
		}

		/// <summary>
		/// 结束下御剑
		/// </summary>
		public void FinishRideOffSword () {
			m_SelfVerticalSpeed = 0;
			SyncMoveParam ();
			this.StopMoveByNavimesh ();
		}

		/// <summary>
		/// 御剑上升
		/// </summary>
		/// <param name="vs"></param>
		public void RideSwordUp (float vs) {
			m_SelfVerticalSpeed = vs;
			SyncMoveParam ();
		}

		/// <summary>
		/// 御剑下降
		/// </summary>
		/// <param name="vs"></param>
		public void RideSwordDown (float vs) {
			m_SelfVerticalSpeed = vs;
			SyncMoveParam ();
		}
		/// <summary>
		/// 恢复到御剑站立
		/// </summary>
		public void RideSwordIdle () {
			m_SelfVerticalSpeed = 0;
			SyncMoveParam ();
		}
		/// <summary>
		/// 同步当前移动的参数
		/// 服务器并没有加上相关的接口 或者 没生效 先注释
		/// </summary>
		public void SyncMoveParam () {
			//if (behavior.isSyncable && syncComp != null)
			//    syncComp.SyncMoveParam(gravity, m_SelfHorizontalGravity, verticalSpeed, horizontalSpeed);
		}

		#endregion
		
	}

}