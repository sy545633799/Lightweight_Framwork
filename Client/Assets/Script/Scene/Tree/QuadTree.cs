using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 场景树（非线性结构）
/// </summary>
/// <typeparam name="T"></typeparam>
public class QuadTree<T> : ISeparateTree<T> where T : ISceneObject
{
	public Bounds Bounds
	{
		get
		{
			if (m_Root != null)
				return m_Root.Bounds;
			return default(Bounds);
		}
	}

	public int MaxDepth
	{
		get { return m_MaxDepth; }
	}

	/// <summary>
	/// 最大深度
	/// </summary>
	protected int m_MaxDepth;

	protected QuadTreeNode<T> m_Root;

	public QuadTree(Vector3 center, Vector3 size, int maxDepth)
	{
		this.m_MaxDepth = maxDepth;
		this.m_Root = new QuadTreeNode<T>(new Bounds(center, size), 0, 4);
	}

	public void Add(T item)
	{
		m_Root.Insert(item, 0, m_MaxDepth);
	}

	public void Clear()
	{
		m_Root.Clear();
	}

	public bool Contains(T item)
	{
		return m_Root.Contains(item);
	}

	public void Remove(T item)
	{
		m_Root.Remove(item);
	}
	public void Trigger(IDetector detector, TriggerHandle<T> handle)
	{
		m_Root.Trigger(detector, handle);
	}

	public static implicit operator bool(QuadTree<T> tree)
	{
		return tree != null;
	}

#if UNITY_EDITOR
	public void DrawTree(Color treeMinDepthColor, Color treeMaxDepthColor, Color objColor, Color hitObjColor, int drawMinDepth, int drawMaxDepth, bool drawObj)
	{
		if (m_Root != null)
			m_Root.DrawNode(treeMinDepthColor, treeMaxDepthColor, objColor, hitObjColor, drawMinDepth, drawMaxDepth, drawObj, m_MaxDepth);
	}
#endif
}
