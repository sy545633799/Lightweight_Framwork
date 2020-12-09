using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GPUAnim {
	public class GPUAnimGroup : IDisposable {
		public GameObject prefab;

		public Mesh mesh;
		public Material material;
		public GPUAnimCfg animCfg;
		CullingGroup cullGroup;

		List<InstancingBatch> batches = new List<InstancingBatch> ();
		public List<InstancingBatch> combineList = new List<InstancingBatch> ();
		List<GPUAnimObj> objs = new List<GPUAnimObj> ();

		public GPUAnimGroup (GPUAnimation o) {
			this.prefab = o._gameObject;
			this.mesh = o.mesh;
			this.material = o.material;
			this.animCfg = o.animCfg;
			GPUAnimUtility.Instance.SetupMesh (mesh);
		}
		public void AddObject (GPUAnimObj o) {
			objs.Add (o);
			o.group = this;
			o._bounds = mesh.bounds; //to do :calculat the real bounds
			// o.bounds = new Bounds (o.position, new Vector3 (0.1f, 0.1f, 0.1f));
			o._bounds.center = o.position;
			var batch = batches.Find ((x) => !x.isfull);
			if (batch == null) {
				batch = new InstancingBatch (this);
				batches.Add (batch);
			}
			batch.AddObject (o);
		}
		public void RemoveObject (GPUAnimObj o) {
			if (!objs.Contains (o)) return;
			o.manager.RemoveObject (o);
			objs.Remove (o);
		}
		public string[] GetAllAnim () {
			string[] o = new string[animCfg.AnimClips.Count];
			for (int i = 0; i < animCfg.AnimClips.Count; i++) {
				o[i] = animCfg.AnimClips[i].clipName;
			}
			return o;
		}
		public AnimClipInfo GetAnimInfo (string name) {
			return animCfg.AnimClips.Find ((x) => x.clipName.Equals (name));
		}

		public int GetAnimID (string name) {
			var anim = animCfg.AnimClips.Find ((x) => x.clipName.Equals (name));
			return animCfg.AnimClips.IndexOf (anim);
		}
		public void Render ()
		{
			Culling();
			ZSort ();
			// TryCombineBatch ();
			for (int i = 0; i < batches.Count; i++) {
				if (batches[i].isValid) batches[i].DrawBatch ();
			}
		}

		void Culling () {
			for (int i = 0; i < objs.Count; i++) {
				var o = objs[i];
				if (o.isActive) {
					
					//bool visible = GPUAnimManager.Instance.mainCamera.IsBoundsInCamera(o._bounds);
					bool visible = GeometryUtility.TestPlanesAABB(GPUAnimManager.Instance.camPlanes, o._bounds);
					if (o._isVisible) {
						if (!visible) {
							o.manager.HideObject (o);
						}
					} else {
						if (visible) {
							o.manager.ShowObject (o);
						}
					}
				}
			}
		}

		void ZSort () { //to do

		}

		void TryCombineBatch () { //to do: caused heavy GC
			// combineList = batches.FindAll ((x) => x.canCombineBatch == true);
			if (combineList.Count >= 2) {
				for (int i = 0; i < combineList.Count - 1; i += 2) {
					combineList[i].CombineBatch (combineList[i + 1]);
					// RemoveBatch (combineList[i + 1]);
				}
			}
		}
		void RemoveBatch (InstancingBatch batch) {
			batches.Remove (batch);
		}
		public void Dispose () {
			prefab = null;
			material = null;
			mesh = null;
			batches.Clear ();
			objs.Clear ();
		}
	}
}