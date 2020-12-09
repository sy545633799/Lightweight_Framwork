using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景物体
/// </summary>
public interface ISceneObject {
	/// <summary>
	/// 该物体的包围盒
	/// </summary>
	Bounds Bounds { get; }

	/// <summary>
	/// 该物体进入显示区域时调用（在这里处理物体的加载或显示）
	/// </summary>
	/// <param name="parent"></param>
	/// <returns></returns>
	void OnShow(Transform parent);
	
	/// <summary>
	/// 该物体离开显示区域时调用（在这里处理物体的卸载或隐藏）
	/// </summary>
	void OnHide();

	void OnDestroy();

	Dictionary<uint, System.Object> GetNodes();

	LinkedListNode<T> GetLinkedListNode<T>(uint morton) where T : ISceneObject;

	void SetLinkedListNode<T>(uint morton, LinkedListNode<T> node);
}
