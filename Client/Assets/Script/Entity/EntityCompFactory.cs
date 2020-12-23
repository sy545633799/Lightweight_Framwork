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
	public sealed class EntityCompFactory : Singleton<EntityCompFactory> {
		private static Dictionary<Type, RecyclePool<EntityComp>> poolMap = new Dictionary<Type, RecyclePool<EntityComp>> ();
		private static Dictionary<Type, Dictionary<long, EntityComp>> compListMap = new Dictionary<Type, Dictionary<long, EntityComp>>();
		public override void Init () {
			poolMap.Add (typeof (AnimComp), new RecyclePool<EntityComp> (() => new AnimComp()));
			poolMap.Add(typeof(InputComp), new RecyclePool<EntityComp>(() => new InputComp()));
			poolMap.Add(typeof(RotateComp), new RecyclePool<EntityComp>(() => new RotateComp()));
			poolMap.Add(typeof(NavComp), new RecyclePool<EntityComp>(() => new NavComp()));
			poolMap.Add(typeof(MoveComp), new RecyclePool<EntityComp>(() => new MoveComp()));
			poolMap.Add(typeof(SyncTransComp), new RecyclePool<EntityComp>(() => new SyncTransComp()));
			poolMap.Add(typeof(SyncStatusComp), new RecyclePool<EntityComp>(() => new SyncStatusComp()));


			compListMap.Add(typeof(AnimComp), new Dictionary<long, EntityComp>(64));
			compListMap.Add(typeof(InputComp), new Dictionary<long, EntityComp>(64));
			compListMap.Add(typeof(RotateComp), new Dictionary<long, EntityComp>(64));
			compListMap.Add(typeof(NavComp), new Dictionary<long, EntityComp>(64));
			compListMap.Add(typeof(MoveComp), new Dictionary<long, EntityComp>(64));
			compListMap.Add(typeof(SyncTransComp), new Dictionary<long, EntityComp>(64));
			compListMap.Add(typeof(SyncStatusComp), new Dictionary<long, EntityComp>(64));

		}

		public T Get<T>(long aoiId)
			where T: EntityComp
		{
			EntityComp protoData = null;
			Type protoType = typeof(T);
			RecyclePool<EntityComp> pool = null;

			if (poolMap.TryGetValue(protoType, out pool))
			{
				protoData = pool.Alloc();
				Dictionary<long, EntityComp> comps = null;
				if (compListMap.TryGetValue(typeof(T), out comps))
					comps.Add(aoiId, protoData);
			}
			return (T)protoData;
		}

		public Dictionary<long, EntityComp> GetComponentsInUse<T>()
		{
			Dictionary<long, EntityComp> comps = null;
			compListMap.TryGetValue(typeof(T), out comps);
			return comps;
		}

		/// <summary>
		/// 回收EntityComp
		/// </summary>
		/// <param name="protoData"></param>
		public void Recycle(long aoiId, EntityComp comp) {
			if (comp == null) return;
			Type protoType = comp.GetType();

			comp.Recycle();
			Dictionary<long, EntityComp> comps = null;
			if (compListMap.TryGetValue(comp.GetType(), out comps))
				comps.Remove(aoiId);
		}

	}
}
