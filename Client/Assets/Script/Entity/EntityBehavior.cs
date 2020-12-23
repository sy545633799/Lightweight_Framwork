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
using XLua;

namespace Game
{
	public partial class EntityBehavior : EventObject, IMonoPoolObject<EntityBehavior> 
    {
		
		private static string RecycleName = "objRecyle";
		private static Vector3 OnRecylePos = new Vector3(-888f, -888f, -888f);
		private static RecyclePool<LuaBase> m_CompTablePool = new RecyclePool<LuaBase>(() => XLuaManager.GetLuaEnv().NewTable());
		private static RecyclePool<LuaBase> m_StatusTablePool = new RecyclePool<LuaBase>(() => XLuaManager.GetLuaEnv().NewTable());

		[Header("加速")]
		public float Acceleration = 5.0f;
		public float Decceleration = 5.0f;
		public float MaxMoveSpeed = 5.0f;
		public float RotateSpeed = 300f;

		[Header("跳跃")]
		public float Gravity = 0.1f;
		public float JumpSpeed = 0.1f;
		public float MaxFallSpeed = 0.1f;

		
		public LuaTable StatusTable { get; private set; }
		public LuaTable CompTable { get; private set; }
		public Action<LuaTable> OnBodyCreate { get; set; }
		private Dictionary<Type, EntityComp> entitiesCompList = new Dictionary<Type, EntityComp>();

		public void Init(int sceneId, int aoiId, int resId, int entityType, Action<LuaTable> onBodycreated)
		{
			AoiId = aoiId;
			SceneId = sceneId;
			ResId = resId;
			EntityType = entityType;
			OnBodyCreate = onBodycreated;
			StatusTable = m_StatusTablePool.Alloc() as LuaTable;
			CompTable = m_CompTablePool.Alloc() as LuaTable;
		}

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
			StatusTable.Recycle();
			StatusTable = null;
			for (int index = 1; index <= CompIndex.Count; index++)
				CompTable.Set(index, 0);
			CompTable.Recycle();
			CompTable = null;
			RemoveAllEntityComp();
			if (body != null)
			{
				ResourceManager.RecyclePrefab(body);
				body = null;
			}
			gameObject.name = RecycleName;
			OnBodyCreate = null;
			SceneId = 0;
			EntityType = 0;
			AoiId = 0;
			destroyed = false;
			bodyLoading = false;
			logicSpeed = 1f;
			body = null;

			transform.position = OnRecylePos;
			transform.localScale = Vector3.one;
			return this;
        }

	}
}
