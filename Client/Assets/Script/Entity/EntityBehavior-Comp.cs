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
		public EntityComp AddEntityComp<T>()
			where T : EntityComp
		{
			Type type = typeof(T);

			EntityComp entitycomp = EntityCompFactory.Instance.Get<T>() as EntityComp;
			entitycomp.SetBehavior(this);
			entitiesCompList.Add(type, entitycomp);
			entitiesCompList[type].OnAdd();
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
			EntityCompFactory.Instance.Recycle(entitiesCompList[type]);
			entitiesCompList.Remove(type);
		}

		public void RemoveAllEntityComp()
		{
			Type[] keyStr = entitiesCompList.Keys.ToArray();
			if (keyStr.Length < 1) return;
			for (int i = keyStr.Length - 1; i >= 0; i--)
				RemoveEntityComp(keyStr[i]);
			var Enumerator = entitiesCompList.GetEnumerator();
			while (Enumerator.MoveNext())
				EntityCompFactory.Instance.Recycle(Enumerator.Current.Value);
			entitiesCompList.Clear();
		}

		public void InitComp(bool isBodyCreated)
		{
			if (this.entityType < 0) return;
			//if (isBodyCreated) effectComp = addComp("EffectComp") as EffectComp;

			switch (this.entityType)
			{
				case 1: //self
					AddEntityComp<MoveComp>();
					AddEntityComp<RotateComp>();
					AddEntityComp<AnimComp>();
					break;
				case 2: //player
					AddEntityComp<RotateComp>();
					AddEntityComp<AnimComp>();
					break;
				case 3: //monster
					AddEntityComp<RotateComp>();
					AddEntityComp<AnimComp>();
					break;
				case 4: //plant
					break;
			}
		}


	}
}
