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
		static Dictionary<Type, RecyclePool<EntityComp>> poolMap = new Dictionary<Type, RecyclePool<EntityComp>> ();
		override public void Init () {
			poolMap.Add (typeof (AnimComp), new RecyclePool<EntityComp> (() => new AnimComp()));
			poolMap.Add(typeof(InputComp), new RecyclePool<EntityComp>(() => new InputComp()));
			poolMap.Add(typeof(RotateComp), new RecyclePool<EntityComp>(() => new RotateComp()));
			poolMap.Add(typeof(NavComp), new RecyclePool<EntityComp>(() => new NavComp()));
			poolMap.Add(typeof(MoveComp), new RecyclePool<EntityComp>(() => new MoveComp()));
		}

		public T Get<T>()
			where T: EntityComp
		{

			EntityComp protoData = null;
			Type protoType = typeof(T);
			RecyclePool<EntityComp> pool = null;

			if (poolMap.TryGetValue(protoType, out pool))
			{
				//RecyclePool<EntityComp> tPool = pool as RecyclePool<EntityComp>;
				protoData = pool.Alloc();
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

			RecyclePool<EntityComp> pool;
			if (poolMap.TryGetValue(protoType, out pool)) {
				RecyclePool<EntityComp> tPool = pool as RecyclePool<EntityComp>;
				((EntityComp)protoData).Recycle();
			} else
				Debug.LogError ("cant find +" + protoType.Name);
		}

	}
}
