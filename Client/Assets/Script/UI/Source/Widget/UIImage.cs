// ========================================================
// des：shenyi
// author: 
// time：2020-09-18 16:15:31
// version：1.0
// ========================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game {

	public class UIImage : Image
	{
		public bool Empty;
		public Collider2D Collider2D;
		protected override void OnPopulateMesh(VertexHelper toFill)
		{
			if (Empty)
				toFill.Clear();
			else
				base.OnPopulateMesh(toFill);
		}

		public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			if (Collider2D)
				return Collider2D.OverlapPoint(eventCamera.ScreenToWorldPoint(screenPoint));
			else
				return base.IsRaycastLocationValid(screenPoint, eventCamera);
		}

		public void SetGray(bool gray)
		{
			//if (gray)
			//	material = MaterialManager.GetSpecialMaterial(SpecialMatType.Gray);
			//else
			//	material = null;
		}

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
