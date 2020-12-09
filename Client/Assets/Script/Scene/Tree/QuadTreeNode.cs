using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuadTreeNode<T> where T : ISceneObject
{
	public Bounds Bounds
	{
		get { return m_Bounds; }
	}

	/// <summary>
	/// 节点当前深度
	/// </summary>
	public int CurrentDepth
	{
		get { return m_CurrentDepth; }
	}

	/// <summary>
	/// 节点数据列表
	/// </summary>
	public LinkedList<T> ObjectList
	{
		get { return m_ObjectList; }
	}

	private int m_CurrentDepth;

	private Vector3 m_HalfSize;

	private LinkedList<T> m_ObjectList;

	private QuadTreeNode<T>[] m_ChildNodes;

	private int m_MaxDepth;
	private int m_ChildCount;

	private Bounds m_Bounds;

	public QuadTreeNode(Bounds bounds, int depth, int childCount)
	{
		m_Bounds = bounds;
		m_CurrentDepth = depth;
		m_ObjectList = new LinkedList<T>();
		m_ChildNodes = new QuadTreeNode<T>[childCount];

		if (childCount == 8)
			m_HalfSize = new Vector3(m_Bounds.size.x / 2, m_Bounds.size.y / 2, m_Bounds.size.z / 2);
		else
			m_HalfSize = new Vector3(m_Bounds.size.x / 2, m_Bounds.size.y, m_Bounds.size.z / 2);

		m_ChildCount = childCount;
	}

	public void Clear()
	{
		for (int i = 0; i < m_ChildNodes.Length; i++)
		{
			if (m_ChildNodes[i] != null)
				m_ChildNodes[i].Clear();
		}
		if (m_ObjectList != null)
		{
			var node = m_ObjectList.First;
			while (node != null)
			{
				//回收处理
				node = node.Next;
			}
			m_ObjectList.Clear();
		}
			
	}

	public bool Contains(T obj)
	{
		for (int i = 0; i < m_ChildNodes.Length; i++)
		{
			if (m_ChildNodes[i] != null && m_ChildNodes[i].Contains(obj))
				return true;
		}

		if (m_ObjectList != null && m_ObjectList.Contains(obj))
			return true;
		return false;
	}

	public void Insert(T obj, int depth, int maxDepth)
	{
		if (m_ObjectList.Contains(obj))
			return;
		if (depth < maxDepth)
		{
			//是否在下一个节点中, 节点包围盒包围物体包围盒
			QuadTreeNode<T> node = GetContainerNode(obj, depth);
			if (node != null)
			{
				node.Insert(obj, depth + 1, maxDepth);
				return;
			}

		}
		//如果不被子节点包含, 则自己包含
		var n = m_ObjectList.AddFirst(obj);
		obj.SetLinkedListNode(0, n);

		if (m_MaxDepth < maxDepth)
			m_MaxDepth = maxDepth;
	}

	public void Remove(T obj)
	{
		var node = obj.GetLinkedListNode<T>(0);
		if (node != null)
		{
			if (node.List == m_ObjectList)
			{
				m_ObjectList.Remove(node);
				var nodes = obj.GetNodes();
				if (nodes != null)
					nodes.Clear();
				return;
			}
		}
		if (m_ChildNodes != null && m_ChildNodes.Length > 0)
		{
			for (int i = 0; i < m_ChildNodes.Length; i++)
			{
				if (m_ChildNodes[i] != null)
					m_ChildNodes[i].Remove(obj);
			}
		}
	}

	public void Trigger(IDetector detector, TriggerHandle<T> handle)
	{
		if (handle == null)
			return;

		//bool isInView = false;
		if (!detector.IsDetected(m_Bounds))
			return;
		//触发当前节点下的所有物体
		var node = m_ObjectList.First;
		while (node != null)
		{
			//如果是最大节点， 就不检测了，直接触发
			if (m_CurrentDepth == m_MaxDepth || detector.IsDetected(node.Value.Bounds))
			{
				handle(node.Value);
			}
			node = node.Next;
		}

		for (int i = 0; i < m_ChildNodes.Length; i++)
		{
			if (m_ChildNodes[i] != null)
			{
				m_ChildNodes[i].Trigger(detector, handle);
			}
		}
	}

	protected QuadTreeNode<T> GetContainerNode(T obj, int depth)
	{
		QuadTreeNode<T> result = null;
		int ix = -1;
		int iz = -1;
		int iy = m_ChildNodes.Length == 4 ? 0 : -1;

		int nodeIndex = 0;

		//遍历4个子节点
		for (int i = ix; i <= 1; i += 2)
		{
			for (int j = iz; j <= 1; j += 2)
			{
				Vector3 centerPos = m_Bounds.center + new Vector3(i * m_HalfSize.x * 0.5f, 0, j * m_HalfSize.z * 0.5f);
				Vector3 size = m_HalfSize;
				QuadTreeNode<T> node = m_ChildNodes[nodeIndex];
				if (node == null)
				{
					Bounds bounds = new Bounds(centerPos, size);
					if (bounds.IsBoundsContainsAnotherBounds(obj.Bounds))
					{
						QuadTreeNode<T> newNode = new QuadTreeNode<T>(bounds, depth + 1, m_ChildNodes.Length);
						result = newNode;
						m_ChildNodes[nodeIndex] = newNode;
					}
				}
				else if (node.Bounds.IsBoundsContainsAnotherBounds(obj.Bounds))
				{
					result = node;
				}
				if (result != null)
				{
					return result;
				}

				nodeIndex += 1;
			}
		}
		return null;
	}

	#region DrawNode
#if UNITY_EDITOR
	public void DrawNode(Color treeMinDepthColor, Color treeMaxDepthColor, Color objColor, Color hitObjColor, int drawMinDepth, int drawMaxDepth, bool drawObj, int maxDepth)
	{
		if (m_ChildNodes != null)
		{
			for (int i = 0; i < m_ChildNodes.Length; i++)
			{
				var node = m_ChildNodes[i];
				if (node != null)
					node.DrawNode(treeMinDepthColor, treeMaxDepthColor, objColor, hitObjColor, drawMinDepth, drawMaxDepth, drawObj, maxDepth);
			}
		}

		if (m_CurrentDepth >= drawMinDepth && m_CurrentDepth <= drawMaxDepth)
		{
			float d = ((float)m_CurrentDepth) / maxDepth;
			Color color = Color.Lerp(treeMinDepthColor, treeMaxDepthColor, d);

			m_Bounds.DrawBounds(color);
		}
		if (drawObj)
		{
			var node = m_ObjectList.First;
			while (node != null)
			{
				var sceneobj = node.Value as SceneObject;
				if (sceneobj != null)
					sceneobj.DrawArea(objColor, hitObjColor);
				node = node.Next;
			}
		}

	}

#endif
	#endregion
}
