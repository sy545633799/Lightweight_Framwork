// ========================================================
// des：
// author: 
// time：2020-10-28 15:08:31
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public static class ListPool<T>
	{

		private static readonly ObjectPool<List<T>> _listPool = new ObjectPool<List<T>>(() => new List<T>(), 16);

		public static List<T> Get()
		{
			return _listPool.Alloc();
			
		}

		public static void Recycle(List<T> element)
		{
			_listPool.Recycle(element);
		}
	}
}
