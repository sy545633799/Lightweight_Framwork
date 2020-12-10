// ========================================================
// des：频繁用到增加与删除少，但根据id查询对象与遍历对象都很多的情形。这个类提供对这种应用场景具有较好性能的容器实现。
// author: 
// time：2020-07-06 11:57:56
// version：1.0
// ========================================================

using System;
using System.Collections.Generic;

public sealed class LinkedListDictionary<TKey, TValue>
{
	private Dictionary<TKey, LinkedListNode<TValue>> kLinkNodeDictionary = new Dictionary<TKey, LinkedListNode<TValue>>();
	private LinkedList<TValue> kObjects = new LinkedList<TValue>();
	public bool Contains(TKey id)
	{
		return kLinkNodeDictionary.ContainsKey(id);
	}
    public bool AddFirst(TKey id, TValue obj)
	{
		if (kLinkNodeDictionary.ContainsKey(id))
			return false;
		LinkedListNode<TValue> linkNode = kObjects.AddFirst(obj);
		if (null != linkNode)
		{
			kLinkNodeDictionary.Add(id, linkNode);
			return true;
		}
		return false;
	}
	public bool AddLast(TKey id, TValue obj)
	{
		if (kLinkNodeDictionary.ContainsKey(id))
			return false;
		LinkedListNode<TValue> linkNode = kObjects.AddLast(obj);
		if (null != linkNode)
		{
			kLinkNodeDictionary.Add(id, linkNode);
			return true;
		}
		return false;
	}
	public void Remove(TKey id)
	{
		if (kLinkNodeDictionary.ContainsKey(id))
		{
			LinkedListNode<TValue> linkNode = kLinkNodeDictionary[id];
			kLinkNodeDictionary.Remove(id);
			try { kObjects.Remove(linkNode); } catch (Exception) { }
		}
	}
	public void Clear()
	{
		kLinkNodeDictionary.Clear();
		kObjects.Clear();
	}
	public bool TryGetValue(TKey id, out TValue value)
	{
		LinkedListNode<TValue> linkNode;
		bool ret = kLinkNodeDictionary.TryGetValue(id, out linkNode);
		if (ret)
		{
			value = linkNode.Value;
		}
		else
		{
			value = default(TValue);
		}
		return ret;
	}
	public int Count
	{
		get
		{
			return kLinkNodeDictionary.Count;
		}
	}
	public TValue this[TKey id]
	{
		get
		{
			TValue ret;
			if (Contains(id))
			{
				LinkedListNode<TValue> linkNode = kLinkNodeDictionary[id];
				ret = linkNode.Value;
			}
			else
			{
				ret = default(TValue);
			}
			return ret;
		}
		set
		{
			if (Contains(id))
			{
				LinkedListNode<TValue> linkNode = kLinkNodeDictionary[id];
				linkNode.Value = value;
			}
			else
			{
				AddLast(id, value);
			}
		}
	}
	public LinkedListNode<TValue> FirstValue
	{
		get
		{
			return kObjects.First;
		}
	}
	public LinkedListNode<TValue> LastValue
	{
		get
		{
			return kObjects.Last;
		}
	}
	public void CopyValuesTo(TValue[] array, int index)
	{
		kObjects.CopyTo(array, index);
	}
}
