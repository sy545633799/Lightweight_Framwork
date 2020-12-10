// ========================================================
// des：按照不同的优先级创建不同的Queue, 插入时根据优先级存放到不同的Queue中, 取出时根据优先级取出
// author: 
// time：2020-07-06 11:57:56
// version：1.0
// ========================================================

using System;
using System.Collections.Generic;
using System.Linq;

public class QueueGroup<T> where T : IIDHandler
{
	private Dictionary<int, LinkedListDictionary<int,T>> m_Group = new Dictionary<int, LinkedListDictionary<int, T>>();
	public int Count { get; protected set; }

	public void Push(int order, T t)
	{
		if (!m_Group.ContainsKey(order))
		{
			Queue<T> item = new Queue<T>();
			m_Group[order] = new LinkedListDictionary<int, T>();
			m_Group = m_Group.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
		}
		int objId = t.UID;

		m_Group[order].AddLast(objId, t);
		Count++;
	}

	public T Pop()
	{
		T t = default(T);
		if (Count > 0)
		{
			foreach (var queue in m_Group.Values)
			{
				if (queue.Count > 0)
				{
					t = queue.FirstValue.Value;
					queue.Remove(t.UID);
					break;
				}
			}
			Count--;
		}
		return t;
	}

	public bool Contains(int order, int objId)
	{
		return m_Group.ContainsKey(order) && m_Group[order].Contains(objId);
	}

	public void Remove(int order, int objId)
	{
		if (m_Group.ContainsKey(order))
		{
			m_Group[order].Remove(objId);
			Count--;
		}
	}

	public void Clear()
	{
		foreach (var queue in m_Group.Values)
			queue.Clear();
		Count = 0;
	}
}
