using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
namespace GPUAnim {
	public class GPUAnimManager : MonoBehaviour, IDisposable {
		static GPUAnimManager _instance;
		Dictionary<string, GPUAnimGroup> GroupDic = new Dictionary<string, GPUAnimGroup> ();
		public bool supportsInstancing;
		Camera _mainCamera;
		public Camera mainCamera {
			get {
				if (_mainCamera == null) _mainCamera = Camera.main;
				return _mainCamera;
			}
			set {
				_mainCamera = value;
			}
		}
		public Plane[] camPlanes = new Plane[6];
		public static GPUAnimManager Instance {
			get {
				if (_instance == null) {
					GameObject manager = new GameObject ("GPUAnimManager");
					DontDestroyOnLoad (manager); //not necessary
					_instance = manager.AddComponent<GPUAnimManager> ();
					_instance.supportsInstancing = SystemInfo.supportsInstancing;
				}
				return _instance;
			}
		}
		public IGPUAnim Instantiate (GameObject prefab, Vector3 pos, Quaternion rot) {
			if (supportsInstancing) {
				GPUAnimGroup group = null;
				GroupDic.TryGetValue (prefab.name, out group);
				if (group == null) {
					GameObject o = GameObject.Instantiate (prefab, transform);
					o.name = prefab.name;
					GPUAnimation gpuAnim = o.GetComponentInChildren<GPUAnimation> ();
					gpuAnim._gameObject = o;
					group = new GPUAnimGroup (gpuAnim);
                    Debug.LogError(o.name);
					GroupDic.Add (o.name, group);
					o.SetActive (false);
					// DestroyImmediate (o);
				}
				var animObj = new GPUAnimObj (pos, rot);
				group.AddObject (animObj);
				return animObj as IGPUAnim;
			} else {
				GameObject o = GameObject.Instantiate (prefab, pos, rot);
				GPUAnimation gpuAnim = o.GetComponentInChildren<GPUAnimation> ();
				// if (!gpuAnim) return o.GetComponent<GPUAnim_> ();
				gpuAnim._gameObject = o;
				return gpuAnim as IGPUAnim;
			}
		}
		void LateUpdate () {
			if (!supportsInstancing) return;
			// camPlanes = GeometryUtility.CalculateFrustumPlanes (mainCamera); // caused 136B GC each frame!!!
			CalculateFrustumPlanes (mainCamera);
			foreach (KeyValuePair<string, GPUAnimGroup> item in GroupDic) {
				item.Value.Render ();
			}
		}
		void CalculateFrustumPlanes (Camera cam) {
			var mat = cam.projectionMatrix * cam.worldToCameraMatrix;
			GPUAnimUtility.Instance.CalculateFrustumPlanes (ref camPlanes, ref mat);
		}

		public void Dispose () {
			GroupDic.Clear ();
			GPUAnimUtility.Instance.OnDispose ();
		}
	}
}