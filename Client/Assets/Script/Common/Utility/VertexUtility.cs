// ========================================================
// des：
// author: 
// time：2020-10-28 16:56:45
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

namespace Game {

	public class VertexUtility
	{
		/// <summary>
		/// 镜像处理
		/// </summary>
		/// <param name="verts"></param>
		/// <param name="rect"></param>
		/// <param name="isHorizontal"></param>
		private static void Mirror(List<UIVertex> verts, Rect rect, bool isHorizontal)
		{
			int count = verts.Count;
			verts.ExtendCapacity(count * 2);
			for (int i = 0; i < count; i++)
			{
				UIVertex vertex = verts[i];
				Vector3 position = vertex.position;

				if (isHorizontal)
					position.x = rect.center.x * 2 - position.x;
				else
					position.y = rect.center.y * 2 - position.y;

				vertex.position = position;
				verts.Add(vertex);
			}
		}

		/// <summary>
		/// Simple缩放位移顶点（减半）
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="verts"></param>
		/// <param name="count"></param>
		public static void SimpleScale(Rect rect, List<UIVertex> verts, bool isHorizontal = true)
		{
			for (int i = 0; i < verts.Count; i++)
			{
				UIVertex vertex = verts[i];

				Vector3 position = vertex.position;
				if (isHorizontal)
					position.x = (position.x + rect.x) * 0.5f;
				else
					position.y = (position.y + rect.y) * 0.5f;

				vertex.position = position;
				verts[i] = vertex;
			}
		}

		/// <summary>
		/// Sample镜像
		/// </summary>
		/// <param name="verts"></param>
		/// <param name="rect"></param>
		/// <param name="isHorizontal"></param>
		public static void MirrorSample(List<UIVertex> verts, Rect rect, bool isHorizontal = true)
		{
			//先进行顶点缩放
			SimpleScale(rect, verts, isHorizontal);
			//再镜像
			Mirror(verts, rect, isHorizontal);
		}

		/// <summary>
		/// Sliced缩放位移顶点（减半）
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="verts"></param>
		/// <param name="count"></param>
		private static void SlicedScale(Rect rect, List<UIVertex> verts, Vector4 border, bool isHorizontal = true)
		{
			//矫正过的范围
			for (int axis = 0; axis <= 1; axis++)
			{
				float combinedBorders = border[axis] + border[axis + 2];
				if (rect.size[axis] < combinedBorders && combinedBorders != 0)
				{
					float borderScaleRatio = rect.size[axis] / combinedBorders;
					border[axis] *= borderScaleRatio;
					border[axis + 2] *= borderScaleRatio;
				}
			}

			float halfWidth = rect.width * 0.5f;
			float halfHeight = rect.height * 0.5f;

			int count = verts.Count;
			for (int i = 0; i < count; i++)
			{
				UIVertex vertex = verts[i];

				Vector3 position = vertex.position;

				if (isHorizontal)
				{
					if (halfWidth < border.x && position.x >= rect.center.x)
					{
						position.x = rect.center.x;
					}
					else if (position.x >= border.x)
					{
						position.x = (position.x + rect.x) * 0.5f;
					}
				}
				else
				{
					if (halfHeight < border.y && position.y >= rect.center.y)
					{
						position.y = rect.center.y;
					}
					else if (position.y >= border.y)
					{
						position.y = (position.y + rect.y) * 0.5f;
					}
				}

				vertex.position = position;

				verts[i] = vertex;
			}
		}

		/// <summary>
		/// 清理掉不能成三角面的顶点
		/// </summary>
		/// <param name="verts"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		private static void SliceExcludeVerts(List<UIVertex> verts)
		{
			int count = verts.Count;
			int realCount = count;

			int i = 0;

			while (i < realCount)
			{
				UIVertex v1 = verts[i];
				UIVertex v2 = verts[i + 1];
				UIVertex v3 = verts[i + 2];

				if (v1.position == v2.position || v2.position == v3.position || v3.position == v1.position)
				{
					verts[i] = verts[realCount - 3];
					verts[i + 1] = verts[realCount - 2];
					verts[i + 2] = verts[realCount - 1];

					realCount -= 3;
					continue;
				}

				i += 3;
			}

			if (realCount < count)
			{
				verts.RemoveRange(realCount, count - realCount);
			}
		}

		public static void MirrorSliced(List<UIVertex> verts, Rect rect, Vector4 border, bool isHorizontal = true)
		{
			//先进行顶点缩放
			SlicedScale(rect, verts, border, isHorizontal);
			//清理掉不能成三角面的顶点
			SliceExcludeVerts(verts);
			//再镜像
			Mirror(verts, rect, isHorizontal);
		}


		/// <summary>
		/// 返回三个点的中心点
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="p3"></param>
		/// <returns></returns>
		private static float GetCenter(float p1, float p2, float p3)
		{
			float max = Mathf.Max(Mathf.Max(p1, p2), p3);

			float min = Mathf.Min(Mathf.Min(p1, p2), p3);

			return (max + min) / 2;
		}

		/// <summary>
		/// 返回翻转UV坐标
		/// </summary>
		/// <param name="uv"></param>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <param name="isHorizontal"></param>
		/// <returns></returns>
		private static Vector2 GetOverturnUV(Vector2 uv, float start, float end, bool isHorizontal = true)
		{
			if (isHorizontal)
			{
				uv.x = end - uv.x + start;
			}
			else
			{
				uv.y = end - uv.y + start;
			}

			return uv;
		}

		/// <summary>
		/// 绘制Tiled版
		/// </summary>
		/// <param name="output"></param>
		/// <param name="count"></param>
		public static void DrawTiled(List<UIVertex> verts, Rect rect, Vector4 inner, Vector2 size, bool isHorizontal = true)
		{
			int count = verts.Count;
			int len = count / 3;
			for (int i = 0; i < len; i++)
			{
				UIVertex v1 = verts[i * 3];
				UIVertex v2 = verts[i * 3 + 1];
				UIVertex v3 = verts[i * 3 + 2];

				float centerX = GetCenter(v1.position.x, v2.position.x, v3.position.x);

				float centerY = GetCenter(v1.position.y, v2.position.y, v3.position.y);

				if (isHorizontal)
				{
					//判断三个点的水平位置是否在偶数矩形内，如果是，则把UV坐标水平翻转
					if (Mathf.FloorToInt((centerX - rect.xMin) / size.x) % 2 == 1)
					{
						v1.uv0 = GetOverturnUV(v1.uv0, inner.x, inner.z, true);
						v2.uv0 = GetOverturnUV(v2.uv0, inner.x, inner.z, true);
						v3.uv0 = GetOverturnUV(v3.uv0, inner.x, inner.z, true);
					}
				}
				else
				{
					//判断三个点的垂直位置是否在偶数矩形内，如果是，则把UV坐标垂直翻转
					if (Mathf.FloorToInt((centerY - rect.yMin) / size.y) % 2 == 1)
					{
						v1.uv0 = GetOverturnUV(v1.uv0, inner.y, inner.w, false);
						v2.uv0 = GetOverturnUV(v2.uv0, inner.y, inner.w, false);
						v3.uv0 = GetOverturnUV(v3.uv0, inner.y, inner.w, false);
					}
				}

				verts[i * 3] = v1;
				verts[i * 3 + 1] = v2;
				verts[i * 3 + 2] = v3;
			}
		}

		
	}

}
