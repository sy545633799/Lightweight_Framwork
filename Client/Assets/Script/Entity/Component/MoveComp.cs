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

		public bool IsMoving { get; private set; }
	
		public override void OnAdd()
		{
			m_SelfTransform = behavior.transform;
			m_AnimComp = behavior.GetEntityComp<AnimComp>() as AnimComp;
			m_InputComp = behavior.GetEntityComp<InputComp>() as InputComp;
			m_RotateComp = behavior.GetEntityComp<RotateComp>() as RotateComp;
		}


		public override void OnLateUpdate(float deltaTime)
        {
			if (m_InputComp.KeyDir == Vector2.zero && m_InputComp.JoySticDir == Vector2.zero)
			{
				if (IsMoving)
				{
					IsMoving = false;
					m_AnimComp.StopMoving();
				}

				return;
			}
			IsMoving = true;

			
			Vector3 dirAfter = behavior.transform.position + MainCamera.Instance.transform.forward;
			m_RotateComp.SetLookAt(dirAfter, true);
			m_AnimComp.Moving(m_InputComp.KeyDir);
			Vector3 pos = behavior.MainCamera.ConvertDirByCam(m_InputComp.KeyDir) + Vector3.down * 20;
			behavior.Controller.Move(pos * deltaTime * 4);
			
		}


		public override void OnRemove()
		{
			IsMoving = false;
		}

	}

}