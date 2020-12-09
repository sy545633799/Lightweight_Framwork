using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPUAnim {

	public class GPUAnimObj : IGPUAnim {
		public GPUAnimGroup group;
		public GPUAnim.MaterialProperty property;
		public InstancingBatch manager { get; set; }
		Vector3 _position;
		Quaternion _rotation;
		Vector3 _scale;
		float[] _Speed;
		float[] _AnimID;
		float[] _Basis;
		float _speed = 1f;
		float currentAnim;
		float basis;
		public Bounds _bounds;
		public Vector3 position {
			get {
				return _position;
			}
			set {
				_position = value;
				_bounds.center = _position;
				_needUpdateTransform = true;
			}
		}
		public Quaternion rotation {
			get {
				return _rotation;
			}
			set {
				_rotation = value;
				_needUpdateTransform = true;
			}
		}
		public Vector3 scale {
			get {
				return _scale;
			}
			set {
				_scale = value;
				_bounds.size = Vector3.Scale (_bounds.size, _scale);
				_needUpdateTransform = true;
			}
		}
		public int _index { get; set; }
		public bool isActive { get; set; }
		public bool _isVisible;
		public float speed {
			get {
				return _speed;
			}
			set {
				_speed = value;
				_needUpdateProperty = true;
			}
		}
		public bool _needUpdateProperty;
		public bool _needUpdateTransform;

		public bool IsPlaying (string name) { return currentAnim == group.GetAnimID (name); }
		public void Play (string animName, WrapMode wrap = WrapMode.Loop, float speed = 1, float basisTime = 0) {
			var anim = group.GetAnimID (animName);
			this.currentAnim = anim;
			this.speed = speed;
			this.basis = basisTime;
			this._needUpdateProperty = true;
		}
		public void UpdateProperty () {
			if (_AnimID == null) _AnimID = property.RegistFloatProperty ("_AnimID");
			if (_Basis == null) _Basis = property.RegistFloatProperty ("_Basis");
			if (_Speed == null) _Speed = property.RegistFloatProperty ("_Speed");
			_AnimID[_index] = currentAnim;
			_Basis[_index] = basis;
			_Speed[_index] = speed;
			_needUpdateProperty = false;
		}
		public void UpdateTransform () {
			manager.matrices[_index].SetTRS (_position, _rotation, _scale);
			_needUpdateTransform = false;
		}

		public string[] GetAllAnimClipNmae () {
			return group.GetAllAnim ();
		}

		public void SetBounds (Bounds b) {
			_bounds = b;
		}

		public void HideSelf () {
			isActive = false;
			manager.HideObject (this);
		}
		public void ShowSelf () {
			isActive = true;
			manager.ShowObject (this);
		}
		public GPUAnimObj (Vector3 pos, Quaternion rot) {
			this.position = pos;
			this.rotation = rot;
			this.scale = Vector3.one;
			this.isActive = true;
			this._isVisible = true;
			this._needUpdateProperty = true;
			this._needUpdateTransform = true;
		}
		public void Stop () { speed = 0; }
		public void Destory () {
			group.RemoveObject (this);
		}
	}
}