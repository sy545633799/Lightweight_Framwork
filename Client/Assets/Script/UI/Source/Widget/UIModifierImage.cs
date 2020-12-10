// ========================================================
// des：
// author: shenyi
// time：2020-10-28 14:38:18
// version：1.0
// ========================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game {

	[ExecuteAlways]
	public abstract class UIModifierImage : UIImage, IMeshModifier
	{
		protected override void OnEnable()
		{
			base.OnEnable();
			SetVerticesDirty();
		}

		protected override void OnDisable()
		{
			SetVerticesDirty();
			base.OnDisable();
		}

		/// <summary>
		/// Called from the native side any time a animation property is changed.
		/// </summary>
		protected override void OnDidApplyAnimationProperties()
		{
			SetVerticesDirty();
			base.OnDidApplyAnimationProperties();
		}

#if UNITY_EDITOR
		protected override void OnValidate()
		{
			base.OnValidate();
			SetVerticesDirty();
		}

#endif

		public virtual void ModifyMesh(Mesh mesh)
		{
			using (var vh = new VertexHelper(mesh))
			{
				ModifyMesh(vh);
				vh.FillMesh(mesh);
			}
		}

		public abstract void ModifyMesh(VertexHelper verts);
	}
}
