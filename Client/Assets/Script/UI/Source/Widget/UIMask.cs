// ========================================================
// des：
// author: 
// time：2020-11-20 20:53:39
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
	public class UIMask : MonoBehaviour
	{
		public RectMask2D mask;
		private void Awake()
		{
			
			
		}

		private void Update()
		{
			if (mask != null)
			{
				Vector3[] wc = new Vector3[4];
				mask.GetComponent<RectTransform>().GetWorldCorners(wc);        // 计算world space中的点坐标
				var clipRect = new Vector4(wc[0].x, wc[0].y, wc[2].x, wc[2].y);// 选取左下角和右上角
				Shader.SetGlobalVector("_ClipRect", clipRect);                           // 设置裁剪区域
			}
		}
	}
}
