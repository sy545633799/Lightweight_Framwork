// ========================================================
// des：
// author: 
// time：2020-07-09 15:12:58
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class RotateComp : EntityComp {

		private Transform cacheTransform;

		public bool hasRotation { get; private set; }
		public float rotationY { get; private set; }

		public float rotateSpeed = 10;

		public override void OnUpdate (float deltaTime) {
			
		}

		void SyncRotation () {
			// if (syncComp == null) {
			// 	syncComp = behavior.GetEntityComp ("SyncComp") as SyncComp;
			// }
			// if (syncComp != null) {
			// 	syncComp.SyncSetRotation (cacheTransform.position, rotationY);
			// }
		}

		public Quaternion GetRotation () {
			Quaternion q = Quaternion.Euler (Vector3.up * rotationY);
			return q;
		}
		
		public void SetLookAt (Vector3 dir, float t, bool smooth = false) {
			if (smooth) {
				Vector3 foward = dir;
				float angle = Mathf.Atan2 (foward.x, foward.z) * Mathf.Rad2Deg;
				Quaternion euler = Quaternion.Euler (Vector3.up * angle);
				Quaternion target = Quaternion.RotateTowards(cacheTransform.rotation, euler, t);
				cacheTransform.rotation = target;
			} else {
				//cacheTransform.LookAt(target);
				Vector3 foward = dir;
				float angle = Mathf.Atan2 (foward.x, foward.z) * Mathf.Rad2Deg;
				Quaternion q = Quaternion.Euler (Vector3.up * angle);
				cacheTransform.rotation = q;
			}
		}

		public void SetForward (Vector3 foward) {
			// if (behavior.IsOnSpurt) {
			// 	return;
			// }
			float angle = Mathf.Atan2 (foward.x, foward.z) * Mathf.Rad2Deg;
			Quaternion euler = Quaternion.Euler (Vector3.up * angle);
			if (Mathf.Abs (rotationY - euler.eulerAngles.y) < 1) {
				return;
			}
			rotationY = euler.eulerAngles.y;

			// if (behavior.isSyncable) {
			// 	SyncRotation ();
			// }
			hasRotation = true;
		}
		public void SetRotateAngle (float y) {
			// if (behavior.IsOnSpurt) {
			// 	return;
			//}
			if (Mathf.Abs (rotationY - y) < 1) {
				return;
			}
			rotationY = y;
			hasRotation = true;
		}
		public override void OnAdd () {
			cacheTransform = behavior.transform;
			rotationY = cacheTransform.rotation.eulerAngles.y;
		}
		public override void OnRemove () {
			//syncComp = null;
			hasRotation = false;
		}
	}
}