// ========================================================
// des：
// author: 
// time：2020-09-18 16:37:32
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Game.Editor
{
	[CustomEditor(typeof(UIImage), true), CanEditMultipleObjects]
	public class UIImageEditor : ImageEditor
	{
		protected UIImage uiImage;
		protected override void OnEnable()
		{
			uiImage = target as UIImage;
			base.OnEnable();
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			bool empty = EditorGUILayout.Toggle("是否为透明空图", uiImage.Empty);
			if (empty != uiImage.Empty)
			{
				uiImage.Empty = empty;
				EditorUtility.SetDirty(uiImage);
			}
			Collider2D collider = (EditorGUILayout.ObjectField("自定义碰撞", uiImage.Collider2D, typeof(Collider2D), true)) as Collider2D;
			if (collider != uiImage.Collider2D)
			{
				uiImage.Collider2D = collider;
				EditorUtility.SetDirty(uiImage);
			}
		}

	}
}