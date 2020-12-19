// ========================================================
// des：
// author: shenyi
// time：2020-07-02 17:28:59
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
		
		private const string RecycleName = "objRecyle";
		private static Vector3 OnRecylePos = new Vector3(-888f, -888f, -888f);
		private Dictionary<Type, EntityComp> entitiesCompList = new Dictionary<Type, EntityComp> ();

		[Header("加速")]
		public float Acceleration = 5.0f;
		public float Decceleration = 5.0f;
		public float MaxMoveSpeed = 5.0f;
		public float RotateSpeed = 300f;

		[Header("跳跃")]
		public float Gravity = 0.1f;
		public float JumpSpeed = 0.1f;
		public float MaxFallSpeed = 0.1f;

		public Action<EntityComp[]> onBodyCreate { get; set; }

		public void Update () {
			float fUpdateTime = Time.deltaTime;
			var Enumerator = entitiesCompList.GetEnumerator ();
			while (Enumerator.MoveNext ()) {
				Enumerator.Current.Value.OnUpdate (fUpdateTime * logicSpeed);
			}
            foreach (EntityComp entitycmp in entitiesCompList.Values)
            {
                entitycmp.OnUpdate(fUpdateTime * logicSpeed);
            }

			if (Controller != null)
			{
				if (Controller.isGrounded)
				{
					if (lastGroundState == false) onLand?.Invoke();
					lastGroundState = true;
				}
				else
				{
					if (lastGroundState == true) onJump?.Invoke();
					lastGroundState = false;
				}
			}

		}

		public void FixedUpdate () {
			var Enumerator = entitiesCompList.GetEnumerator ();
			while (Enumerator.MoveNext ()) {
				Enumerator.Current.Value.OnFixedUpdate (Time.fixedDeltaTime);
			}
		}

		public void LateUpdate()
		{
			var Enumerator = entitiesCompList.GetEnumerator();
			while (Enumerator.MoveNext())
			{
				Enumerator.Current.Value.OnLateUpdate(Time.fixedDeltaTime);
			}
		}

		/// <summary>
		/// EntityBehavior回收进对象池时被调用
		/// </summary>
		/// <returns></returns>
		public EntityBehavior Downcast()
        {
			RemoveAllEntityComp();
			if (body != null)
			{
				ResourceManager.RecyclePrefab(body);
				body = null;
			}
			gameObject.name = RecycleName;
			onBodyCreate = null;
			isSyncable = false;
			sceneid = 0;
			entityType = 0;
			aoiId = 0;
			sceneid = 0;
			destroyed = false;
			bodyLoading = false;
			logicSpeed = 1f;
			body = null;
			head = null;
			middle = null;
			root = null;
			transform.position = OnRecylePos;
			transform.localScale = Vector3.one;
			if (transform.childCount > 0)
			{
				int max = transform.childCount;
				for (int index = 0; index < max; index++)
				{
					Transform trans = transform.GetChild(index);
					if (trans != null)
						GameObject.Destroy(trans.gameObject);
				}
			}
			return this;
        }

	}
}
