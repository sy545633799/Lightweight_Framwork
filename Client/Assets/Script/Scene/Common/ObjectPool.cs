/********************************************************************************
** auth： yanwei
** date： 2016-08-08
** desc： 对象池类 包括实例化 回收等。
*********************************************************************************/

using System;
using System.Collections.Generic;


  public interface IPoolAllocatedObject<T> where T : IPoolAllocatedObject<T>, new()
  {
    void InitPool(ObjectPool<T> pool);
     T Downcast();
  }


  public class ObjectPool<T> where T : IPoolAllocatedObject<T>, new()
  {
    public void Init(int initPoolSize)
    {
      for (int i = 0; i < initPoolSize; ++i)
      {
         T t = new T();
         t.InitPool(this);
         m_UnusedObjects.Enqueue(t);
      }
    }

    public T Alloc()
    {
      if (m_UnusedObjects.Count > 0)
        {
            return m_UnusedObjects.Dequeue();
        }
      else
        {
           T t = new T();
           if (null != t)
           {
              t.InitPool(this);
           }
          return t;
       }
    }
    public void Recycle(IPoolAllocatedObject<T> t)
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

    private Queue<T> m_UnusedObjects = new Queue<T>();
  }

