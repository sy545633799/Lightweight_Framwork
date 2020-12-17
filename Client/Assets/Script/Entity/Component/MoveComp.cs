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

		private float CurHSpeed = 0f;
		private float TarHSpeed = 0f;
		private float CurVSpeed = 0f;
		private float TarVSpeed = 0f;
		private Vector3 Target_Dir = Vector3.zero;

		public override void OnAdd()
		{
			m_SelfTransform = behavior.transform;
			TarVSpeed = -behavior.MaxFallSpeed;
			m_AnimComp = behavior.GetEntityComp<AnimComp>() as AnimComp;
			m_InputComp = behavior.GetEntityComp<InputComp>() as InputComp;
			m_RotateComp = behavior.GetEntityComp<RotateComp>() as RotateComp;
		}

		public override void OnFixedUpdate(float deltaTime)
		{
			if (m_InputComp.JoySticDir == Vector2.zero && !m_InputComp.IsJump
				&& CurHSpeed == 0
				&& CurVSpeed == TarVSpeed
				&& behavior.Controller.isGrounded)
				return;

			//这里先不做跳跃了，跳跃必须用blendtree做，因为跳跃时间不确定，只能用跳跃时的纵向速度去混合动画
			//if (m_InputComp.IsJump && behavior.Controller.isGrounded)
			//	CurrentVirticalSpeed = behavior.JumpSpeed;
			//纵向速度
			CurVSpeed = MoveHelper.MoveTowards(CurVSpeed, TarVSpeed, behavior.Gravity * Time.deltaTime, 0.1f);
			//停止输入时进入惯性预测
			if (m_InputComp.JoySticDir.sqrMagnitude == 0)
			{
				if (behavior.Controller.isGrounded)
				{
					TarHSpeed = 0;
					CurHSpeed = MoveHelper.Lerp(CurHSpeed, TarHSpeed, behavior.Decceleration * Time.deltaTime, 0.1f);
				}
			}
			else
			{
				//横向速度
				Vector3 horizontal;
				if (behavior.Controller.isGrounded) // TODO:判断着陆动画播放完？
					horizontal = behavior.MainCamera.ConvertDirByCam(m_InputComp.JoySticDir);
				else
					horizontal = Target_Dir;
				//TODO:这里要不要用当前方向叉乘上一帧方向, 作为TargetHorizontalSpeed的系数


				TarHSpeed = behavior.MaxMoveSpeed;
				CurHSpeed = MoveHelper.MoveTowards(CurHSpeed, TarHSpeed, behavior.Acceleration * Time.deltaTime, 0.1f);
				//方向
				Target_Dir = horizontal;
				m_RotateComp.SetLookAt(Target_Dir, behavior.RotateSpeed * Time.deltaTime, true);
			}

			//移动
			float speedX = CurHSpeed / behavior.MaxMoveSpeed;
			float speedY = CurVSpeed / behavior.MaxFallSpeed;
			m_AnimComp.Moving(speedX, speedY);
			behavior.Controller.Move(Target_Dir * deltaTime * CurHSpeed + Vector3.up * CurVSpeed);


			//发送同步信息
		}


		public override void OnRemove()
		{
			m_AnimComp = null;
			m_InputComp = null;
			m_RotateComp = null;
		}

	}

}