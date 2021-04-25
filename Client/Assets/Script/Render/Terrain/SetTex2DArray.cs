// ========================================================
// des：
// author: 
// time：2021-04-06 18:07:27
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	
	[ExecuteAlways]
	[RequireComponent(typeof(MeshRenderer))]
	public class SetTex2DArray : MonoBehaviour
	{
		//[HideInInspector]
		public Vector4[] UV_STArray;

		void Start()
	    {
			SetArray();
			
		}
#if UNITY_EDITOR
		private void Update()
		{
			SetArray();
		}
#endif

		public void SetArray()
		{
			if (UV_STArray != null)
			{
				MeshRenderer renderer = GetComponent<MeshRenderer>();
				renderer.sharedMaterial.SetVectorArray("_UV_STArray", UV_STArray);
			}
		}

	} 
}
