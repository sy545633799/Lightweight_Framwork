using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
namespace GPUAnim {
	[RequireComponent (typeof (MeshFilter))]
	[RequireComponent (typeof (Renderer))]
	[CanEditMultipleObjects, CustomEditor (typeof (GPUAnimation))]
	public class GPUAnimEditor : Editor {
		GPUAnimation go;
		string currentClip;
		GPUAnimCfg cfg;
		int Idx = 0;
		string clip;

		public override void OnInspectorGUI () {
			go = (GPUAnimation) target;
			base.OnInspectorGUI ();
			if (GUILayout.Button ("Play Next: " + clip)) {
				if (cfg == null) cfg = go.animCfg;
				if (Idx >= cfg.AnimClips.Count - 1) Idx = 0;
				if (clip != null) go.Play (clip);
				clip = cfg.AnimClips[Idx++].clipName;
			}
		}
	}

	// [RequireComponent (typeof (MeshFilter))]
	// [RequireComponent (typeof (Renderer))]
	// [CanEditMultipleObjects, CustomEditor (typeof (GPUAnimation))]
	// public class GPUAnim_Editor : Editor {
	// 	GPUAnim_ go;
	// 	string currentClip;
	// 	GPUAnimCfg cfg;
	// 	int Idx = 0;
	// 	string clip;

	// 	public override void OnInspectorGUI () {
	// 		go = (GPUAnim_) target;
	// 		base.OnInspectorGUI ();
	// 		if (GUILayout.Button ("Play Next: " + clip)) {
	// 			if (cfg == null) cfg = go.animCfg;
	// 			if (Idx >= cfg.AnimClips.Count - 1) Idx = 0;
	// 			if (clip != null) go.Play (clip);
	// 			clip = cfg.AnimClips[Idx++].clipName;
	// 		}
	// 	}
	// }
}