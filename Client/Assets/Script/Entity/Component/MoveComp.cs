// ========================================================
// des：主角控制玩家移动的控件
// author: shenyi
// time：2020-07-09 14:13:27
// version：1.0
// ========================================================
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class MoveComp : EntityComp {

		private Transform m_SelfTransform;
		private AnimComp m_AnimComp = null;
		private InputComp m_InputComp = null;
		private RotateComp m_RotateComp = null;

		private float CurrentHorizontalSpeed = 0f;
		private float TargetHorizontalSpeed = 0f;
		private float CurrentVirticalSpeed = 0f;
		private float TargetVirticalSpeed = 0f;
		private Vector3 Current_Dir = Vector3.zero;

		public override void OnAdd()
		{
			m_SelfTransform = behavior.transform;
			TargetVirticalSpeed = -behavior.MaxFallSpeed;
			m_AnimComp = behavior.GetEntityComp<AnimComp>() as AnimComp;
			m_InputComp = behavior.GetEntityComp<InputComp>() as InputComp;
			m_RotateComp = behavior.GetEntityComp<RotateComp>() as RotateComp;
		}

		public override void OnFixedUpdate(float deltaTime)
		{
			if (m_InputComp.JoySticDir == Vector2.zero && !m_InputComp.IsJump
				&& CurrentHorizontalSpeed == 0
				&& CurrentVirticalSpeed == TargetVirticalSpeed)
				return;

			//这里先不做跳跃了，跳跃必须用blendtree做，因为跳跃时间不确定，只能用跳跃时的纵向速度去混合动画
			//if (m_InputComp.IsJump && behavior.Controller.isGrounded)
			//	CurrentVirticalSpeed = behavior.JumpSpeed;
			//纵向速度
			CurrentVirticalSpeed = Mathf.MoveTowards(CurrentVirticalSpeed, TargetVirticalSpeed, behavior.Gravity * Time.deltaTime);
			if (Mathf.Abs(CurrentVirticalSpeed - TargetVirticalSpeed) < 0.1f) CurrentVirticalSpeed = TargetVirticalSpeed;
			//横向速度
			Vector3 horizontal = behavior.MainCamera.ConvertDirByCam(m_InputComp.JoySticDir);
			//停止输入时进入惯性预测
			if (horizontal.sqrMagnitude == 0)
			{
				TargetHorizontalSpeed = 0;
				CurrentHorizontalSpeed = Mathf.Lerp(CurrentHorizontalSpeed, TargetHorizontalSpeed, behavior.Decceleration * Time.deltaTime);
			}
			else
			{
				Current_Dir = horizontal;
				TargetHorizontalSpeed = behavior.MaxMoveSpeed;
				CurrentHorizontalSpeed = Mathf.MoveTowards(CurrentHorizontalSpeed, TargetHorizontalSpeed, behavior.Acceleration * Time.deltaTime);
				m_RotateComp.SetLookAt(Current_Dir, behavior.RotateSpeed * Time.deltaTime, true);
			}
			if (Mathf.Abs(CurrentHorizontalSpeed - TargetHorizontalSpeed) < 0.1f) CurrentHorizontalSpeed = TargetHorizontalSpeed;

			//移动
			float speedX = CurrentHorizontalSpeed / behavior.MaxMoveSpeed;
			float speedY = CurrentVirticalSpeed / behavior.MaxFallSpeed;
			m_AnimComp.Moving(speedX, speedY);
			behavior.Controller.Move(Current_Dir * deltaTime * CurrentHorizontalSpeed + Vector3.up * CurrentVirticalSpeed);
			//Debug.LogError($"{CurrentHorizontalSpeed}:{CurrentVirticalSpeed}");
		}


		public override void OnRemove()
		{
			
		}

	}

}