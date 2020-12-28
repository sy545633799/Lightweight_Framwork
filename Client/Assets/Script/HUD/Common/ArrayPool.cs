// ========================================================
// des：可能引起内存爆炸, 慎用
// author: shenyi
// time：2020-12-25 14:05:25
// version：1.0
// ========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
	public class ArrayPool<T>
	{
		private static Dictionary<int, ObjectPool<T[]>> m_Pools = new Dictionary<int, ObjectPool<T[]>>();

		public static T[] Get(int len)
		{
			ObjectPool<T[]> pool;
			if (!m_Pools.TryGetValue(len, out pool))
			{
				pool = new ObjectPool<T[]>(() => new T[len]);
				m_Pools.Add(len, pool);
			}
			return pool.Alloc();
		}

		public static void Recycle(T[] t)
		{
			ObjectPool<T[]> pool;
			if (m_Pools.TryGetValue(t.Length, out pool))
			{
				pool.Recycle(t);
			}
				
		}
	}
}
