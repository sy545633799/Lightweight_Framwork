// ========================================================
// des：导航组件
// author: shenyi
// time：2020-12-11 13:48:07
// version：1.0
// ========================================================

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class NavComp : EntityComp
	{
		private InputComp m_InputComp;

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

		public override void OnAdd()
		{
			m_InputComp = behavior.GetEntityComp<InputComp>() as InputComp;
		}

		public override void OnUpdate(float deltaTime)
		{
			////从导航到摇杆状态改变需要告知外界
			//if (m_MovingType == MovingType.Navigation)
			//	m_ActionNavigationArrived?.Invoke(false);
			//m_ActionNavigationArrived = null;
			//m_ActionNavigationMoving = null;
			//m_NaviStopDistance = 0;
		}

		/// <summary>
		/// 移动ing
		/// </summary>
		/// <param name="deltaTime"></param>
		void OnMoving(float deltaTime)
		{
			////@TobyStark 这里如果是None类型就不在执行，不然无法停止[非导航也非摇杆模式]
			//if (m_MovingType == MovingType.None)
			//	return;
			//if (m_MovingType == MovingType.Navigation) // 导航
			//{
			//	if (m_NavimeshPathCorners.Count <= 0)
			//		return;
			//	if (m_ActionNavigationArrived != null && m_NavimeshPathCorners.Count > 0)
			//	{
			//		// 如果 当前离终点（路径的最后一个点）的距离 小于一个规定值 则 视为 到达寻路终点
			//		Vector3 destination = m_NavimeshPathCorners[m_NavimeshPathCorners.Count - 1];
			//		if (Vector3.Distance(destination, m_SelfTransform.position) <= m_NaviStopDistance)
			//		{
			//			m_ActionNavigationArrived(true);
			//			this.StopMoveByNavimesh();
			//			return;
			//		}
			//	}
			//	// 寻路路径上的各点 按照直线行走
			//	Vector3 nextCorner = m_NavimeshPathCorners[0];
			//	Vector3 dir = nextCorner - m_SelfTransform.position;
			//	dir.y = 0;
			//	Vector3 deltaDir = (m_NaviMovingSpeed * deltaTime * dir.normalized); // 移动的距离
			//	Vector3 nextPos = m_SelfTransform.position + deltaDir; // 可能不在网格上

			//	// 判断是否穿过了网格（穿墙 判断）
			//	UnityEngine.AI.NavMeshHit hit;
			//	//向目标点发射一条射线，返回true说明碰到了边缘，Hit就是边缘那个点
			//	if (UnityEngine.AI.NavMesh.Raycast(m_SelfTransform.position, nextPos, out hit, UnityEngine.AI.NavMesh.AllAreas))
			//	{
			//		nextPos = m_SelfTransform.position + m_NaviMovingSpeed * 0.02f * dir.normalized; //这里给一个不会穿墙的距离
			//	}
			//	// 将nextPos 矫正到网格上
			//	nextPos = MoveHelper.Util_SamplePosition(nextPos, true);
			//	// // 是否碰到障碍物
			//	// if (EntityBehaviorManager.Instance ().HitBarrier (m_SelfTransform.position, nextPos)) {
			//	// 	if (moveType == MoveType.Navigation) {
			//	// 		StopMove ();
			//	// 	}
			//	// 	return;
			//	// }

			//	// 修改朝向  
			//	RotateComp rotateComp = (RotateComp)behavior.GetEntityComp<RotateComp>();
			//	if (rotateComp != null)
			//		rotateComp.SetLookAt(nextCorner, true);

			//	// dirAfter：这一帧之后的朝向     dir：移动前的朝向
			//	Vector3 dirAfter = nextCorner - nextPos;
			//	dirAfter.y = 0;

			//	float dot = dir.x * dirAfter.x + dir.z * dirAfter.z;
			//	// 前后dir方向相反 多走一步就走过头了 或者 dir太小
			//	// 视为 到达 当前路径上的寻路点
			//	if (dot < 0 || dir.magnitude < 0.01f)
			//	{
			//		m_SelfTransform.position = nextCorner;
			//		if (m_NavimeshPathCorners.Count > 1)
			//		{
			//			m_NavimeshPathCorners.RemoveAt(0);
			//		}
			//		else // 最后一个点 到达终点
			//		{
			//			m_ActionNavigationArrived?.Invoke(true);
			//			this.StopMoveByNavimesh();
			//		}
			//	}
			//	else // 还没有到终点
			//	{
			//		//  ** 更新位置 **
			//		m_SelfTransform.position = nextPos;
			//		m_ActionNavigationMoving?.Invoke();
			//	}
			//	// Debug.Log($"[C#] MoveComp.cs ::MoveTo() => 导航模式{moveType}");

			//}
			//else
			//{
			//	if (behavior.Controller != null)
			//	{
			//		//如果是Navi导航起步，中途操作了摇杆，那么操作期间为moveType == MoveType.JoyStick，以后就是非导航也非摇杆模式
			//		//或者御剑等飞行期间
			//		// Debug.Log($"[C#] MoveComp.cs ::MoveTo() => 非导航也非摇杆模式{moveType}");
			//		if (behavior.Controller.isGrounded)
			//		{
			//			m_SelfMovingSpeedByStick.x = 0;
			//			m_SelfMovingSpeedByStick.z = 0;
			//		}
			//		//else  // 这里可能是跳跃或御剑 水平速度自己管理
			//		//{

			//		//}
			//		behavior.Controller.Move(m_SelfMovingSpeedByStick * deltaTime);
			//	}
			//}

			//// // 同步到服务器
			//// if (behavior.isSyncable && syncComp != null && behavior.rotateComp != null && IsNeedSyncMove) {
			//// 	syncComp.SyncMove (m_SelfTransform.position, moveType, behavior.rotateComp.rotationY, speed);
			//// }
		}


		//#region 【==================== 状态变更的方法 ====================】

		///// <summary>
		///// 设置位置
		///// </summary>
		//public void SetPosition(Vector3 pos, bool isJoyStick = false)
		//{
		//	if (m_SelfTransform == null)
		//		return;
		//	m_SelfTransform.position = isJoyStick ? pos : MoveHelper.Util_SamplePosition(pos, true);
		//}
		///// <summary>
		///// 导航位移 【对外】
		///// </summary>
		///// <param name="dst"></param>
		///// <param name="stopDis"></param>
		///// <param name="onArr"></param>
		//public void MoveByNavimesh(Vector3 dst, float stopDis = 0.01f, Action<bool> onArr = null, Action onMoving = null)
		//{
		//	if (behavior.IsPlayingDie)
		//	{
		//		Debug.LogWarning("MoveByNavimesh failed! entity IsPlayingDie");
		//		return;
		//	}
		//	m_ActionNavigationArrived = onArr;
		//	this.m_ActionNavigationMoving = onMoving;
		//	m_NaviStopDistance = stopDis;
		//	Destionation = dst;
		//	UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
		//	//计算路径，返回false就走不过去，返回true说明可以走过去，Corners就是路径点
		//	if (UnityEngine.AI.NavMesh.CalculatePath(m_SelfTransform.position, dst, UnityEngine.AI.NavMesh.AllAreas, path))
		//	{
		//		m_NavimeshPathCorners.Clear();
		//		m_NavimeshPathCorners.AddRange(path.corners);
		//		m_NavimeshPathCorners.RemoveAt(0); //移除第一个点, 即当前位置点
		//		if (m_NavimeshPathCorners.Count == 0) //当前的点可能是到达自身的点
		//		{
		//			m_ActionNavigationArrived?.Invoke(true);
		//			this.StopMoveByNavimesh();
		//		}
		//		m_MovingType = MovingType.Navigation;
		//	}
		//	else
		//	{
		//		Debug.LogWarning("Navi导航寻路异常=>没有可寻路的网格数据，请Editor下查看NaviMeshSurface");
		//		//每一个异常导致的StopMove()都需要告知外界停止运动事件的发生
		//		m_ActionNavigationArrived?.Invoke(false);
		//		this.StopMoveByNavimesh();
		//	}
		//}

		///// <summary>
		///// 停止导航移动
		///// </summary>
		//public void StopMoveByNavimesh()
		//{
		//	m_NavimeshPathCorners.Clear();
		//	m_MovingType = MovingType.None;
		//	m_ActionNavigationArrived = null;
		//	m_ActionNavigationMoving = null;
		//	m_NaviStopDistance = 0;
		//}

		//#endregion
	}

}
