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
		private GameObject root;
		private GameObject head;
		private GameObject middle;
		private GameObject hp_name;

		public GameObject Body
		{
			get
			{
				return body;
			}
			set
			{
				body = value;
				//if (body != null)
				//{
				//	if (animComp != null)
				//	{
				//		animComp.Reset(true);
				//	}
				//	Transform r = body.transform.Find("root");
				//	if (r != null)
				//	{
				//		root = r.gameObject;
				//		for (int i = 0; i < r.childCount; i++)
				//		{
				//			ResourceManager.RecyclePrefab(r.GetChild(0).gameObject);
				//		}
				//	}

				//	Transform hn = body.transform.Find("HP_name");
				//	if (hn != null)
				//	{
				//		hp_name = hn.gameObject;
				//		for (int i = 0; i < hn.childCount; i++)
				//		{
				//			ResourceManager.RecyclePrefab(hn.GetChild(0).gameObject);
				//		}
				//	}

				//	if (body.transform.childCount > 0)
				//	{
				//		Transform child0 = body.transform.GetChild(0);
				//		Transform h = child0.Find("head");
				//		if (h != null)
				//		{
				//			head = h.gameObject;
				//			for (int i = 0; i < h.childCount; i++)
				//			{
				//				ResourceManager.RecyclePrefab(h.GetChild(0).gameObject);
				//			}
				//		}
				//		Transform m = child0.Find("middle");
				//		if (m != null)
				//		{
				//			middle = m.gameObject;
				//			for (int i = 0; i < m.childCount; i++)
				//			{
				//				ResourceManager.RecyclePrefab(m.GetChild(0).gameObject);
				//			}
				//		}
				//	}
				//}
			}
		}
		public GameObject Root
		{
			get
			{
				return root;
			}
		}
		public GameObject Middle
		{
			get
			{
				return middle;
			}
		}

		public GameObject HP_name
		{
			get
			{
				return hp_name;
			}
		}

	}
}
