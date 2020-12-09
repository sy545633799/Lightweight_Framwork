using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GPUAnim {

	[RequireComponent (typeof (MeshFilter))]
	[RequireComponent (typeof (MeshRenderer))]
	public class GPUAnimation : MonoBehaviour, IGPUAnim {
		public GPUAnimCfg animCfg;
		[HideInInspector]
		public GameObject _gameObject; // root gamobject of this prefab;
		Mesh _mesh;
		Material _material;
		MeshRenderer _render;
		float _speed = 1f;
		MaterialPropertyBlock _block;
		string currentAnim;
		bool playOnceLock;
		public Vector3 position { get { return transform.position; } set { transform.position = value; } }
		public Quaternion rotation { get { return transform.rotation; } set { transform.rotation = value; } }
		public Vector3 scale { get { return transform.localScale; } set { transform.localScale = value; } }
		public int _index { get; set; }
		public bool isActive { get; set; }
		public MeshRenderer render {
			get {
				if (_render == null) _render = GetComponent<MeshRenderer> ();
				return _render;
			}
		}
		public Material material {
			get {
				if (_material == null) _material = render.sharedMaterial;
				return _material;
			}
		}
		public Mesh mesh {
			get {
				if (_mesh == null) _mesh = GetComponent<MeshFilter> ().sharedMesh;
				return _mesh;
			}
		}
		MaterialPropertyBlock block {
			get {
				if (_block == null) _block = new MaterialPropertyBlock ();
				return _block;
			}
		}
		public float speed {
			get {
				return _speed;
			}
			set {
				_speed = value;
				// render.material.SetFloat ("_Speed", _speed);
				block.Clear ();
				render.GetPropertyBlock (block);
				block.SetFloat ("_Speed", _speed);
				render.SetPropertyBlock (block);
			}
		}
		void Awake () {
			// SetupAnimInfo ();
			GPUAnimUtility.Instance.SetupAnimInfo (material, animCfg);
			GPUAnimUtility.Instance.SetupMesh (mesh);
		}

		public void Play (string animName, WrapMode wrap = WrapMode.Loop, float speed = 1, float basisTime = 0) {
			var anim = animCfg.AnimClips.Find ((x) => x.clipName.Equals (animName));
			int animId = animCfg.AnimClips.IndexOf (anim);
			basisTime = basisTime % anim.animLength;
			if (anim != null) {
				render.GetPropertyBlock (block);
				block.SetFloat ("_AnimID", animId);
				block.SetFloat ("_Basis", Time.timeSinceLevelLoad + basisTime);
				block.SetFloat ("_Speed", speed);
				render.SetPropertyBlock (block);
				currentAnim = animName;
			}
		}

		public bool IsPlaying (string name) {
			return currentAnim.Equals (name);
		}

		public string[] GetAllAnimClipNmae () {
			string[] o = new string[animCfg.AnimClips.Count];
			for (int i = 0; i < animCfg.AnimClips.Count; i++) {
				o[i] = animCfg.AnimClips[i].clipName;
			}
			return o;
		}
		public void Destory () {
			Destroy (_gameObject);
		}
		public void Stop () {
			speed = 0;
		}

		public void HideSelf () {
			if (_gameObject) _gameObject.SetActive (false);
		}

		public void ShowSelf () {
			if (_gameObject) _gameObject.SetActive (true);
		}
	}
}