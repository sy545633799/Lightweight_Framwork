// ========================================================
// des：
// author: 
// time：2020-12-10 19:08:27
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public partial class EntityBehavior : EventObject, IMonoPoolObject<EntityBehavior>
	{

		private GameObject body;
		public GameObject Body
		{
			get
			{
				return body;
			}
			set
			{
				body = value;
				if (body != null)
				{
					Transform hn = body.transform.Find("hp");
					//if (hn != null)
					//{
					//	//hp_name = hn.gameObject;
					//	for (int i = 0; i < hn.childCount; i++)
					//	{
					//		ResourceManager.RecyclePrefab(hn.GetChild(0).gameObject);
					//	}
					//}


				}
			}
		}

	}
}
