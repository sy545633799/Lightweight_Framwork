// ========================================================
// des：
// author: 
// time：2020-07-09 13:14:43
// version：1.0
// ========================================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{

    public class AnimComp : EntityComp
    {
		private MoveComp m_MoveComp = null;
		private Animator animator;

		public override void OnAdd()
		{
			animator = behavior.Body.GetComponent<Animator>();
			animator.SetBool("IsGrounded", true);
			UIAction.OnActionTrigger += OnActionTrigger;
		}

		private void OnActionTrigger(string triggerName)
		{
			animator.CrossFade(triggerName, 0.2f);
		}

		public void Moving(float speedX, float speedY)
		{
			animator.SetFloat("Velocity X", speedX);
			animator.SetFloat("Velocity Y", speedY);
		}

		public override void OnUpdate(float deltaTime)
		{
			animator.SetBool("IsGrounded", behavior.Controller.isGrounded);
		}

		public override void OnRemove()
		{
			animator = null;
		}

		

		
	}
}