// ========================================================
// des：
// author: 
// time：2020-07-13 13:53:27
// version：1.0
// ========================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {


    public interface IMonoPoolObject<T> where T :  MonoBehaviour, IMonoPoolObject<T>
    {
        T Downcast();
    }


    /// <summary>
    /// 用于复用继承字Monoehaviour上的脚本
    /// </summary>
    public interface IMonoObjectPool
    {
        
    }

	public class MonoObjectPool<T> : IMonoObjectPool where T : MonoBehaviour, IMonoPoolObject<T>
	{
		private Func<T> AllocFunc;
		private Queue<T> m_UnusedObjects = new Queue<T>();


		public MonoObjectPool(Func<T> func)
		{
			AllocFunc = func;
		}

		public T Alloc()
        {
            if (m_UnusedObjects.Count > 0)
            {
                return m_UnusedObjects.Dequeue();
            }
            else
            {
                T t = AllocFunc.Invoke();
                return t;
            }
        }

        public void Recycle(IMonoPoolObject<T> t)
        {
            if (null != t)
            {
                
                m_UnusedObjects.Enqueue(t.Downcast());
            }
        }

        public int Count
        {
            get
            {
                return m_UnusedObjects.Count;
            }
        }

        public void Cleanup()
        {
            m_UnusedObjects.Clear();
        }


    }
}
