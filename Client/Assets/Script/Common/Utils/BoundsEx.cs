using UnityEngine;

public static class BoundsEx
{
	/// <summary>
	/// 绘制包围盒
	/// </summary>
	/// <param name="bounds"></param>
	/// <param name="color"></param>
	public static void DrawBounds(this Bounds bounds, Color color)
	{
		Gizmos.color = color;

		Gizmos.DrawWireCube(bounds.center, bounds.size);
	}

	/// <summary>
	/// 判断包围盒是否包含另一个包围盒
	/// </summary>
	/// <param name="bounds"></param>
	/// <param name="compareTo"></param>
	/// <returns></returns>
	public static bool IsBoundsContainsAnotherBounds(this Bounds bounds, Bounds compareTo)
	{
		return bounds.Contains(compareTo.center +
							new Vector3(-compareTo.size.x / 2, compareTo.size.y / 2, -compareTo.size.z / 2)) &&
		bounds.Contains(compareTo.center +
							new Vector3(compareTo.size.x / 2, compareTo.size.y / 2, -compareTo.size.z / 2)) &&
		bounds.Contains(compareTo.center +
							new Vector3(compareTo.size.x / 2, compareTo.size.y / 2, compareTo.size.z / 2)) &&
		bounds.Contains(compareTo.center +
							new Vector3(-compareTo.size.x / 2, compareTo.size.y / 2, compareTo.size.z / 2)) &&
		bounds.Contains(compareTo.center +
							new Vector3(-compareTo.size.x / 2, -compareTo.size.y / 2, -compareTo.size.z / 2)) &&
		bounds.Contains(compareTo.center +
							new Vector3(compareTo.size.x / 2, -compareTo.size.y / 2, -compareTo.size.z / 2)) &&
		bounds.Contains(compareTo.center +
							new Vector3(compareTo.size.x / 2, -compareTo.size.y / 2, compareTo.size.z / 2)) &&
		bounds.Contains(compareTo.center +
							new Vector3(-compareTo.size.x / 2, -compareTo.size.y / 2, compareTo.size.z / 2));
	}
}
