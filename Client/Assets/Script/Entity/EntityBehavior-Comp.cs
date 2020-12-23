// ========================================================
// des：
// author: shenyi
// time：2020-12-10 19:06:42
// version：1.0
// ========================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game {
	public partial class EntityBehavior : EventObject, IMonoPoolObject<EntityBehavior>
	{
		public EntityComp AddEntityComp<T>(long aoiId)
			where T : EntityComp
		{
			Type type = typeof(T);

			EntityComp entitycomp = null;
			if (!entitiesCompList.TryGetValue(type, out entitycomp))
			{
				entitycomp = EntityCompFactory.Instance.Get<T>(aoiId) as EntityComp;
				entitiesCompList.Add(type, entitycomp);
			}
			return entitycomp;
		}

		public EntityComp GetEntityComp<T>()
			where T : EntityComp
		{
			Type type = typeof(T);
			EntityComp entitycmp = null;
			if (entitiesCompList.TryGetValue(type, out entitycmp))
			{
				return entitycmp;
			}
			return entitycmp;
		}

		public void RemoveEntityComp<T>()
			where T : EntityComp
		{
			Type type = typeof(T);
			RemoveEntityComp(type);
		}

		private void RemoveEntityComp(Type type)
		{
			if (!entitiesCompList.ContainsKey(type))
			{
				Debug.Log("entity not contain comp named " + name);
				return;
			}
			EntityComp comp = entitiesCompList[type];
			comp.Remove();
			EntityCompFactory.Instance.Recycle(AoiId, comp);
			entitiesCompList.Remove(type);
		}

		public void RemoveAllEntityComp()
		{
			Type[] keyStr = entitiesCompList.Keys.ToArray();
			if (keyStr.Length < 1) return;
			for (int i = keyStr.Length - 1; i >= 0; i--)
				RemoveEntityComp(keyStr[i]);
			
			entitiesCompList.Clear();
		}

		public void InitComp(bool isBodyCreated)
		{

			AddEntityComp<NameComp>(AoiId);
			switch (EntityType)
			{
				case 1://hero
					AddEntityComp<HUDComp>(AoiId);
					AddEntityComp<AnimComp>(AoiId);
					AddEntityComp<InputComp>(AoiId);
					AddEntityComp<RotateComp>(AoiId);
					AddEntityComp<NavComp>(AoiId);
					AddEntityComp<MoveComp>(AoiId);
					AddEntityComp<SyncStatusComp>(AoiId);
					AddEntityComp<SyncTransComp>(AoiId);
					break;
				case 2: //player
					AddEntityComp<HUDComp>(AoiId);
					AddEntityComp<AnimComp>(AoiId);
					AddEntityComp<RotateComp>(AoiId);
					AddEntityComp<SyncTransComp>(AoiId);
					AddEntityComp<SyncStatusComp>(AoiId);
					break;
				case 3: //monster
					AddEntityComp<AnimComp>(AoiId);
					AddEntityComp<RotateComp>(AoiId);
					AddEntityComp<SyncTransComp>(AoiId);
					AddEntityComp<SyncStatusComp>(AoiId);
					break;
				case 4: //plant
					break;
			}
			var Enumerator = entitiesCompList.GetEnumerator();
			while (Enumerator.MoveNext())
				Enumerator.Current.Value.Add(this);
		}


	}
}
