// ========================================================
// des：
// author: 
// time：2020-06-29 11:52:33
// version：1.0
// ========================================================

using System;
using System.Collections.Generic;

namespace Game
{
	public interface IObjectPool { }

	public class ObjectPool<T> : IObjectPool
	{
		protected Func<T> Func;
		protected Queue<T> m_UnusedObjects = new Queue<T>(16);

		public ObjectPool(Func<T> func, int initPoolSize = 0)
		{
			Func = func;
			Init(initPoolSize);
		}

		protected virtual void Init(int initPoolSize)
		{
			for (int i = 0; i < initPoolSize; ++i)
			{
				T t = Func.Invoke();
				m_UnusedObjects.Enqueue(t);
			}
		}

		public virtual T Alloc()
		{
			if (m_UnusedObjects.Count > 0)
			{
				return m_UnusedObjects.Dequeue();
			}
			else
			{
				T t = Func.Invoke();
				return t;
			}
		}

		public virtual void Recycle(T t)
		{
			if (null != t)
			{
				m_UnusedObjects.Enqueue(t);
			}
		}

		public int Count
		{
			get
			{
				return m_UnusedObjects.Count;
			}
		}
	}

	/// <summary>
	/// 初始化接口, 回收处理接口
	/// </summary>
	public interface IDowncast
	{
		void OnAlloc();
		void Downcast();
	}

	public class DowncastPool<T> : ObjectPool<T>
		where T : IDowncast
	{
		public DowncastPool(Func<T> func, int initPoolSize = 0)
			: base(func, initPoolSize) { }

		public override void Recycle(T t)
		{
			if (null != t)
			{
				(t as IDowncast).Downcast();
				m_UnusedObjects.Enqueue((T)t);
			}
		}
	}

	/// <summary>
	/// Object主动调用回收接口
	/// </summary>
	public interface IRecycle: IDowncast
	{
		void Recycle();
	}

	public abstract class RecycleObject<T> : IRecycle
		where T : RecycleObject<T>
	{
		private RecyclePool<T> objectPool;

		public void InitPool(RecyclePool<T> pool)
		{
			objectPool = pool;
		}

		public virtual void OnAlloc(){}
		public virtual void Downcast() { }

		public void Recycle()
		{
			objectPool.Recycle((T)this);
		}

		
	}


	public class RecyclePool<T> : DowncastPool<T>
		where T : RecycleObject<T>
	{
		public RecyclePool(Func<T> func, int initPoolSize = 0)
			: base(func, initPoolSize) { }

		protected override void Init(int initPoolSize)
		{
			for (int i = 0; i < initPoolSize; ++i)
			{
				T t = Func.Invoke();
				t.InitPool(this);
				m_UnusedObjects.Enqueue(t);
			}
		}

		public override T Alloc()
		{
			T t;
			if (m_UnusedObjects.Count > 0)
			{
				t = m_UnusedObjects.Dequeue();
			}
			else
			{
				t = Func.Invoke();
				t?.InitPool(this);
			}
			t?.OnAlloc();

			return t;
		}
	}

}
