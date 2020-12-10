// ========================================================
// des：
// author: 
// time：2020-12-10 19:06:42
// version：1.0
// ========================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

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

		public EntityComp GetEntityComp(Type type)
		{
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

		public void RemoveEntityComp(Type type)
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
			if (this.entityType < 0)
			{
				return;
			}
			// if (isBodyCreated) {
			// 	effectComp = addComp ("EffectComp") as EffectComp;
			// }

			//switch (this.entityType) {
			//	case 0: //player
			//		moveComp = addComp("Game.MoveComp") as MoveComp;
			//		rotateComp = addComp("Game.RotateComp") as RotateComp;
			//		animComp = addComp("Game.AnimComp") as AnimComp;
			//		break;
			//	case 2: //area
			//	case 3: //area1
			//		break;
			//	case 4: //monster
			//		moveComp = addComp("Game.MoveComp") as MoveComp;
			//		rotateComp = addComp("Game.RotateComp") as RotateComp;
			//		animComp = addComp("Game.AnimComp") as AnimComp;
			//		break;
			//	case 5: //plant
			//		break;
			//	case 101: //battle_unit
			//			  //if (isHero == true
			//			  //if (isBodyCreated) {
			//		rotateComp = addComp("Game.RotateComp") as RotateComp;
			//		// animComp = addComp("Game.AnimComp") as AnimComp;

			//		// 	// animEventComp = addComp ("AnimEventComp") as AnimEventComp;
			//		// 	// spurtComp = addComp ("SpurtComp") as SpurtComp;
			//		// } else {
			//		// 	//syncMoveComp = addComp ("SyncMoveComp") as SyncMoveComp;
			//		// }

			//		break;

			//}

		}

	}
}
