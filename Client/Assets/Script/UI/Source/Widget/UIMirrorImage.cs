// ========================================================
// des：Modified from https://github.com/L-Lawliet/UGUIExtend
// author: shenyi
// time：2020-10-28 14:20:25
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace Game {
	public class UIMirrorImage : UIModifierImage
	{
		public enum MirrorType
		{
			None = 0,

			/// <summary>
			/// 水平
			/// </summary>
			Horizontal,

			/// <summary>
			/// 垂直
			/// </summary>
			Vertical,

			/// <summary>
			/// 四分之一
			/// 相当于水平，然后再垂直
			/// </summary>
			Quarter,
		}

		/// <summary>
		/// 镜像类型
		/// </summary>
		[SerializeField]
		private MirrorType m_MirrorType = MirrorType.None;

		public MirrorType mirrorType
		{
			get { return m_MirrorType; }
			set
			{
				if (m_MirrorType != value)
				{
					m_MirrorType = value;
					SetVerticesDirty();
				}
			}
		}

		/// <summary>
		/// 设置原始尺寸
		/// </summary>
		public override void SetNativeSize()
		{
			if (m_MirrorType == MirrorType.None)
			{
				if (overrideSprite != null)
				{
					float w = overrideSprite.rect.width / pixelsPerUnit;
					float h = overrideSprite.rect.height / pixelsPerUnit;
					rectTransform.anchorMax = rectTransform.anchorMin;

					switch (m_MirrorType)
					{
						case MirrorType.Horizontal:
							rectTransform.sizeDelta = new Vector2(w * 2, h);
							break;
						case MirrorType.Vertical:
							rectTransform.sizeDelta = new Vector2(w, h * 2);
							break;
						case MirrorType.Quarter:
							rectTransform.sizeDelta = new Vector2(w * 2, h * 2);
							break;
					}
					SetVerticesDirty();
				}
			}
			else
				base.SetNativeSize();
		}

		public override void ModifyMesh(VertexHelper vh)
		{
			if (m_MirrorType == MirrorType.None || !IsActive()){ return; }

			var output = ListPool<UIVertex>.Get();
			//获取所有的顶点数据
			vh.GetUIVertexStream(output);
			switch (type)
			{
				case Image.Type.Simple:
					DrawSimple(output);
					break;
				case Image.Type.Sliced:
					DrawSliced(output);
					break;
				case Image.Type.Tiled:
					DrawTiled(output);
					break;
				case Image.Type.Filled:
					break;
			}
			vh.Clear();
			//写入顶点数据
			vh.AddUIVertexTriangleStream(output);
			ListPool<UIVertex>.Recycle(output);
		}

		/// <summary>
		/// 绘制简单版
		/// </summary>
		/// <param name="output"></param>
		/// <param name="count"></param>
		protected void DrawSimple(List<UIVertex> output)
		{
			Rect rect = GetPixelAdjustedRect();
			//先缩放原顶点
			switch (m_MirrorType)
			{
				case MirrorType.Horizontal:
					VertexUtility.MirrorSample(output, rect, true);
					break;
				case MirrorType.Vertical:
					VertexUtility.MirrorSample(output, rect, false);
					break;
				case MirrorType.Quarter:
					VertexUtility.MirrorSample(output, rect, true);
					VertexUtility.MirrorSample(output, rect, false);
					break;
			}
		}

		/// <summary>
		/// 绘制Sliced版
		/// </summary>
		/// <param name="output"></param>
		/// <param name="count"></param>
		protected void DrawSliced(List<UIVertex> output)
		{
			if (!hasBorder)
			{
				DrawSimple(output);
				return;
			}

			Rect rect = GetPixelAdjustedRect();
			Vector4 border = overrideSprite.border / pixelsPerUnit;

			switch (m_MirrorType)
			{
				case MirrorType.Horizontal:
					VertexUtility.MirrorSliced(output, rect, border, true);
					break;
				case MirrorType.Vertical:
					VertexUtility.MirrorSliced(output, rect, border, false);
					break;
				case MirrorType.Quarter:
					VertexUtility.MirrorSliced(output, rect, border, true);
					VertexUtility.MirrorSliced(output, rect, border, false);
					break;
			}
		}


		/// <summary>
		/// 绘制Sliced版
		/// </summary>
		/// <param name="output"></param>
		/// <param name="count"></param>
		protected void DrawTiled(List<UIVertex> output)
		{
			Rect rect = GetPixelAdjustedRect();
			//此处使用inner是因为Image绘制Tiled时，会把透明区域也绘制了。
			Vector4 inner = DataUtility.GetInnerUV(overrideSprite);
			Vector2 size = new Vector2(overrideSprite.rect.width / pixelsPerUnit, overrideSprite.rect.height / pixelsPerUnit);
			switch (m_MirrorType)
			{
				case MirrorType.Horizontal:
					VertexUtility.DrawTiled(output, rect, inner, size, true);
					break;
				case MirrorType.Vertical:
					VertexUtility.DrawTiled(output, rect, inner, size, false);
					break;
				case MirrorType.Quarter:
					VertexUtility.DrawTiled(output, rect, inner, size, true);
					VertexUtility.DrawTiled(output, rect, inner, size, false);
					break;
			}
		}

	}
}
