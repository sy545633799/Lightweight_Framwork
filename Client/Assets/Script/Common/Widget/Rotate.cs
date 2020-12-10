// ========================================================
// des：
// author: 
// time：2020-10-12 20:49:34
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class Rotate : MonoBehaviour
	{
		public float RotationSpeed = 1;

		// Update is called once per frame
		void Update()
	    {
			transform.Rotate(Vector3.back * RotationSpeed * Time.deltaTime, Space.Self); //物体自转
		}
	}
}
