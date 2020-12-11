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
			if (hasRotation) {
				OnRotation ();
			}
		}
		void OnRotation () {
			if (rotationY < 0) {
				rotationY += 360;
			}
			float curRotY = cacheTransform.rotation.eulerAngles.y;
			if (curRotY < 0) {
				curRotY += 360;
			}

			float angle = rotationY - curRotY;
			if (Mathf.Abs (angle) < 2) {
				hasRotation = false;
				return;
			}
			if (angle > 180) {
				angle = angle - 360;
			}
			if (angle < -180) {
				angle = angle + 360;
			}

			float rotate = angle * rotateSpeed * Time.deltaTime;
			if (angle > 0 && rotate > angle) {
				rotate = angle;
			}
			if (angle < 0 && rotate < angle) {
				rotate = angle;
			}

			cacheTransform.Rotate (Vector3.up, rotate);
		}

		void SyncRotation () {
			// if (syncComp == null) {
			// 	syncComp = behavior.GetEntityComp ("SyncComp") as SyncComp;
			// }
			// if (syncComp != null) {
			// 	syncComp.SyncSetRotation (cacheTransform.position, rotationY);
			// }
		}

		public void SetRotation (Quaternion rotation) {
			// if (behavior.IsOnSpurt) {
			// 	return;
			// }
			rotationY = rotation.eulerAngles.y;
			if (behavior.isSyncable) {
				SyncRotation ();
			}
			hasRotation = true;
		}
		public Quaternion GetRotation () {
			Quaternion q = Quaternion.Euler (Vector3.up * rotationY);
			return q;
		}
		public void SetLookAt (Vector3 target, bool smooth = false) {
			// if (behavior.IsOnSpurt) {
			// 	return;
			// }
			if (smooth) {
				Vector3 foward = target - cacheTransform.position;
				float angle = Mathf.Atan2 (foward.x, foward.z) * Mathf.Rad2Deg;
				Quaternion euler = Quaternion.Euler (Vector3.up * angle);
				rotationY = euler.eulerAngles.y;
				hasRotation = true;
			} else {
				//cacheTransform.LookAt(target);
				Vector3 foward = target - cacheTransform.position;
				float angle = Mathf.Atan2 (foward.x, foward.z) * Mathf.Rad2Deg;
				Quaternion q = Quaternion.Euler (Vector3.up * angle);
				cacheTransform.rotation = q;
				rotationY = cacheTransform.rotation.eulerAngles.y;
				hasRotation = false;
			}
			// 暂时不进行同步操作
			// if (behavior.isSyncable) {
			// 	SyncRotation ();
			// }
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