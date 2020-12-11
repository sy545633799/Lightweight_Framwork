// ========================================================
// des：
// author: 
// time：2020-07-09 13:14:43
// version：1.0
// ========================================================

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
		}


		public void Moving(Vector3 dir)
		{
			animator.SetBool("Moving", true);
			animator.SetFloat("Velocity X", dir.x);
			animator.SetFloat("Velocity Y", dir.z);
		}

		public void StopMoving()
		{
			animator.SetBool("Moving", false);
		}

		public override void OnRemove()
		{
			animator = null;
		}



		
	}
}