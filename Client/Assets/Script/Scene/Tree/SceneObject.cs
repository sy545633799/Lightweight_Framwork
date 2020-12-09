using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 场景物件（用来包装实际用于动态加载的物体）
/// </summary>
public class SceneObject : ISceneObject, IPoolAllocatedObject<SceneObject>
{
	/// <summary>
	/// 场景物件创建标记
	/// </summary>
	public enum CreateFlag
	{
		/// <summary>
		/// 未创建
		/// </summary>
		None,
		/// <summary>
		/// 当前检测到的物体
		/// </summary>
		Detected,
		/// <summary>
		/// 标记为旧物体(处理完成的物体)
		/// </summary>
		Old,

	}

	/// <summary>
	/// 场景物体包围盒
	/// </summary>
	public Bounds Bounds
	{
		get { return m_TargetObj.Bounds; }
	}

	public float Weight
	{
		get { return m_Weight; }
		set { m_Weight = value; }
	}

	private ISceneElement m_TargetObj;
	/// <summary>
	/// 被包装的实际用于动态加载和销毁的场景物体
	/// </summary>
	public ISceneElement TargetObj
	{
		get { return m_TargetObj; }
	}

	public CreateFlag Flag { get; set; }
	private float m_Weight;
	private Dictionary<uint, System.Object> m_Nodes;
	private ObjectPool<SceneObject> kPool = null;

	public void Init(ISceneElement obj)
	{
		m_Weight = 0;
		m_TargetObj = obj;
	}

	public Dictionary<uint, System.Object> GetNodes()
	{
		return m_Nodes;
	}

	public LinkedListNode<T> GetLinkedListNode<T>(uint morton) where T : ISceneObject
	{
		if (m_Nodes != null && m_Nodes.ContainsKey(morton))
			return (LinkedListNode<T>)m_Nodes[morton];
		return null;
	}

	public void SetLinkedListNode<T>(uint morton, LinkedListNode<T> node)
	{
		if (m_Nodes == null)
			m_Nodes = new Dictionary<uint, object>();
		m_Nodes[morton] = node;
	}

	public void OnHide()
	{
		Weight = 0;
		m_TargetObj.OnHide();
	}

	public void OnDestroy()
	{
		Weight = 0;
		m_TargetObj.OnDestroy();
		kPool.Recycle(this);
	}

	public void OnShow(Transform parent)
	{
		m_TargetObj.OnShow(parent);
	}

	public void InitPool(ObjectPool<SceneObject> pool)
	{
		kPool = pool;
	}

	public SceneObject Downcast()
	{
		m_TargetObj = null;
		return this;
	}

	public override string ToString()
	{
		return m_TargetObj.Bounds.ToString();
	}

#if UNITY_EDITOR
	public void DrawArea(Color color, Color hitColor)
	{
		if (Flag == CreateFlag.Detected || Flag == CreateFlag.Old)
		{
			m_TargetObj.Bounds.DrawBounds(hitColor);
		}
		else
			m_TargetObj.Bounds.DrawBounds(color);
	}

#endif
}
