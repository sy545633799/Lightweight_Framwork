// ========================================================
// des：
// author: 
// time：2020-10-29 17:12:17
// version：1.0
// ========================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game {
	public class UIBaseImage : MaskableGraphic, ISerializationCallbackReceiver, ILayoutElement, ICanvasRaycastFilter
	{
		[FormerlySerializedAs("m_Frame")]
		[SerializeField]
		private Sprite m_Sprite;
		public Sprite sprite { get { return m_Sprite; } set { if (SetPropertyUtility.SetClass(ref m_Sprite, value)) SetAllDirty(); } }

		[NonSerialized]
		private Sprite m_OverrideSprite;
		public Sprite overrideSprite
		{
			get
			{
				return m_OverrideSprite == null ? sprite : m_OverrideSprite;
			}
			set
			{
				if (SetPropertyUtility.SetClass(ref m_OverrideSprite, value))
					SetAllDirty();
			}
		}
		private Sprite activeSprite
		{
			get
			{
				return m_OverrideSprite != null ? m_OverrideSprite : sprite;
			}
		}

		/// <summary>
		/// Image's texture comes from the UnityEngine.Image.
		/// </summary>
		public override Texture mainTexture
		{
			get
			{
				return overrideSprite == null ? s_WhiteTexture : overrideSprite.texture;
			}
		}

		public float pixelsPerUnit
		{
			get
			{
				float spritePixelsPerUnit = 100;
				if (sprite)
					spritePixelsPerUnit = sprite.pixelsPerUnit;

				float referencePixelsPerUnit = 100;
				if (canvas)
					referencePixelsPerUnit = canvas.referencePixelsPerUnit;
				return spritePixelsPerUnit / referencePixelsPerUnit;
			}
		}

		/// <summary>
		/// Whether the Sprite of the image has a border to work with.
		/// </summary>

		public bool hasBorder
		{
			get
			{
				if (activeSprite != null)
				{
					Vector4 v = activeSprite.border;
					return v.sqrMagnitude > 0f;
				}
				return false;
			}
		}

		/// <summary>
		/// 子类需要重写该方法来自定义Image形状
		/// </summary>
		/// <param name="vh"></param>
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			base.OnPopulateMesh(vh);
		}

		#region ISerializationCallbackReceiver
		public void OnAfterDeserialize()
		{
		}

		//
		// 摘要: 
		//     Implement this method to receive a callback after unity serialized your object.
		public void OnBeforeSerialize()
		{
		}
		#endregion

		#region ILayoutElement
		public virtual void CalculateLayoutInputHorizontal() { }
		public virtual void CalculateLayoutInputVertical() { }

		public virtual float minWidth { get { return 0; } }

		public virtual float preferredWidth
		{
			get
			{
				if (overrideSprite == null)
					return 0;
				return overrideSprite.rect.size.x / pixelsPerUnit;
			}
		}

		public virtual float flexibleWidth { get { return -1; } }

		public virtual float minHeight { get { return 0; } }

		public virtual float preferredHeight
		{
			get
			{
				if (overrideSprite == null)
					return 0;
				return overrideSprite.rect.size.y / pixelsPerUnit;
			}
		}

		public virtual float flexibleHeight { get { return -1; } }

		public virtual int layoutPriority { get { return 0; } }
		#endregion

		#region ICanvasRaycastFilter
		public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			return true;
		}
		#endregion

		#region 点击优化
#if UNITY_EDITOR
		//raycast 提示
		private Vector3[] fourCorners = new Vector3[4];
		private void OnDrawGizmos()
		{
			if (raycastTarget)
			{
				rectTransform.GetWorldCorners(fourCorners);
				Gizmos.color = Color.blue;
				for (int i = 0; i < 4; i++)
					Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);
			}
		}
#endif

		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			if (!raycastTarget)
				GraphicRegistry.UnregisterGraphicForCanvas(canvas, this);
		}


		protected override void OnCanvasHierarchyChanged()
		{
			base.OnCanvasHierarchyChanged();
			if (!raycastTarget)
				GraphicRegistry.UnregisterGraphicForCanvas(canvas, this);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (!raycastTarget)
				GraphicRegistry.UnregisterGraphicForCanvas(canvas, this);
		}
		#endregion

	}
}
