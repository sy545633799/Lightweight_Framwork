// ========================================================
// des：贝塞尔曲线
// author: shenyi
// time：2020-12-31 13:38:20
// version：1.0
// ========================================================

using System;
using UnityEngine;


namespace Game
{
	[Serializable]
	public class Bezier
	{
		public Vector3 start_position;
		public Vector3 end_position;
		public Vector3 p1;
		public Vector3 p2;
		public float rotate;
		public float ti = 0f;

		private Vector3 b0 = Vector3.zero;
		private Vector3 b1 = Vector3.zero;
		private Vector3 b2 = Vector3.zero;
		private Vector3 b3 = Vector3.zero;
		private float Ax;
		private float Ay;
		private float Az;
		private float Bx;
		private float By;
		private float Bz;
		private float Cx;
		private float Cy;
		private float Cz;


		//public Bezier(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, float rotate)
		//{
		//    this.start_position = v0;
		//    this.p1 = v1;
		//    this.p2 = v2;
		//    this.end_position = v3;
		//    this.rotate = rotate;
		//}

		public Vector3 GetPointAtTime(float t)
		{
			this.CheckConstant();
			t = Mathf.Clamp(t, 0, 1);
			float t2 = Mathf.Pow(t, 2);
			float t3 = Mathf.Pow(t, 3);

			float x = this.Ax * t3 + this.Bx * t2 + this.Cx * t + start_position.x;
			float y = this.Ay * t3 + this.By * t2 + this.Cy * t + start_position.y;
			float z = this.Az * t3 + this.Bz * t2 + this.Cz * t + start_position.z;

			// 新增旋转
			Vector3 resulVector = new Vector3(x, y, z);
			Quaternion rotation = Quaternion.AngleAxis(this.rotate, this.end_position - this.start_position);
			resulVector = rotation * resulVector - (rotation * this.start_position - this.start_position);

			return resulVector;
		}

		private void SetConstant()
		{
			this.Cx = 3f * ((this.start_position.x + this.p1.x) - this.start_position.x);
			this.Bx = 3f * ((this.end_position.x + this.p2.x) - (this.start_position.x + this.p1.x)) - this.Cx;
			this.Ax = this.end_position.x - this.start_position.x - this.Cx - this.Bx;
			this.Cy = 3f * ((this.start_position.y + this.p1.y) - this.start_position.y);
			this.By = 3f * ((this.end_position.y + this.p2.y) - (this.start_position.y + this.p1.y)) - this.Cy;
			this.Ay = this.end_position.y - this.start_position.y - this.Cy - this.By;
			this.Cz = 3f * ((this.start_position.z + this.p1.z) - this.start_position.z);
			this.Bz = 3f * ((this.end_position.z + this.p2.z) - (this.start_position.z + this.p1.z)) - this.Cz;
			this.Az = this.end_position.z - this.start_position.z - this.Cz - this.Bz;
		}

		private void CheckConstant()
		{
			if (this.start_position != this.b0 || this.p1 != this.b1 || this.p2 != this.b2 || this.end_position != this.b3)
			{
				this.SetConstant();
				this.b0 = this.start_position;
				this.b1 = this.p1;
				this.b2 = this.p2;
				this.b3 = this.end_position;
			}
		}
	}
}