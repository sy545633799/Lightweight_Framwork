// ========================================================
// des：
// author: 
// time：2020-12-10 19:05:03
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game {
	public partial class EntityBehavior : EventObject, IMonoPoolObject<EntityBehavior>
	{

		private EntityTriggerEvent onEntityEnter = new EntityTriggerEvent();
		private EntityTriggerEvent onEntityExit = new EntityTriggerEvent();

		public void AddEntityEnterListener(UnityAction<string> callback)
		{
			if (bodyLoading) return;
			EnsureCollider();
			onEntityEnter?.AddListener(callback);
		}
		public void RemoveEntityEnterListener(UnityAction<string> callback)
		{
			onEntityEnter?.RemoveListener(callback);
		}
		public void AddEntityExitListener(UnityAction<string> callback)
		{
			if (bodyLoading) return;
			EnsureCollider();
			onEntityExit?.AddListener(callback);
		}
		public void RemovEntityeExitListener(UnityAction<string> callback)
		{
			onEntityExit?.RemoveListener(callback);
		}
		public override void RemoveAllListeners()
		{
			base.RemoveAllListeners();
			onEntityEnter.RemoveAllListeners();
			onEntityExit.RemoveAllListeners();
		}
	}
}
