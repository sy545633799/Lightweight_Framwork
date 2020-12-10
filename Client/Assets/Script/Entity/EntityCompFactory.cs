// ========================================================
// des：
// author: 
// time：2020-12-10 17:32:12
// version：1.0
// ========================================================

// ========================================================
// des：
// author: 
// time：2020-07-10 11:15:55
// version：1.0
// ========================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {

	/*
	这里 反正改写了好多
	*/

	public sealed class EntityCompFactory : Singleton<EntityCompFactory> {
		static Dictionary<Type, IObjectPool> poolMap = new Dictionary<Type, IObjectPool> ();
		override public void Init () {
			poolMap.Add (typeof (AnimComp), new RecyclePool<EntityComp> (() => new AnimComp()));
			poolMap.Add (typeof (MoveComp), new RecyclePool<EntityComp> (() => new MoveComp()));
			poolMap.Add (typeof (RotateComp), new RecyclePool<EntityComp> (() => new RotateComp()));
			// poolMap.Add (typeof (AnimEventComp), new EntityCompPool<AnimEventComp> ());
			// poolMap.Add (typeof (BulletComp), new EntityCompPool<BulletComp> ());
			// poolMap.Add (typeof (CameraEffectComp), new EntityCompPool<CameraEffectComp> ());
			// poolMap.Add (typeof (EffectComp), new EntityCompPool<EffectComp> ());
			// poolMap.Add (typeof (SpurtComp), new EntityCompPool<SpurtComp> ());
			// poolMap.Add (typeof (SyncComp), new EntityCompPool<SyncComp> ());
			// poolMap.Add (typeof (SyncMoveComp), new EntityCompPool<SyncMoveComp>());
		}

		public T Get<T>()
			where T: EntityComp
		{

			EntityComp protoData = null;
			Type protoType = typeof(T);
			IObjectPool pool = null;

			if (poolMap.TryGetValue(protoType, out pool))
			{
				if (protoType == typeof(AnimComp) || protoType == typeof(MoveComp) || protoType == typeof(RotateComp))
				{
					RecyclePool<EntityComp> tPool = pool as RecyclePool<EntityComp>;
					protoData = tPool.Alloc();
				}
				else
				{
					Debug.LogWarning("请检查 EntityComp 类型！");
				}
			}
			return (T)protoData;
		}

		/// <summary>
		/// 回收EntityComp
		/// </summary>
		/// <param name="protoData"></param>
		public void Recycle(object protoData) {
			if (protoData == null) return;
			Type protoType = protoData.GetType();

			IObjectPool pool;
			if (poolMap.TryGetValue(protoType, out pool)) {
				if (protoType == typeof(AnimComp) || protoType == typeof(MoveComp) || protoType == typeof(RotateComp)) {
					RecyclePool<EntityComp> tPool = pool as RecyclePool<EntityComp>;
					((EntityComp)protoData).Recycle();
				} else {
					Debug.LogWarning ("请检查 EntityComp 类型！");
				}				
			} else
				Debug.LogError ("cant find +" + protoType.Name);
		}

	}
}
