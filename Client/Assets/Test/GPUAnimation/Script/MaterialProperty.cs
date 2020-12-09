using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GPUAnim {
	public class MaterialProperty : IDisposable {
		public int length;
		public Dictionary<string, float[]> floatProperty = new Dictionary<string, float[]> ();
		public Dictionary<string, Vector4[]> Vector4Property = new Dictionary<string, Vector4[]> ();
		public MaterialPropertyBlock block;

		public MaterialProperty (int length) {
			this.length = length;
			block = new MaterialPropertyBlock ();
		}

		public float[] RegistFloatProperty (string name) {
			if (floatProperty.ContainsKey (name)) return floatProperty[name];
			var propArrary = new float[length];
			floatProperty.Add (name, propArrary);
			return propArrary;
		}
		public void UnregistFloatProperty (string name) {
			var propArrary = new float[length];
			Vector4Property.Remove (name);
		}
		public Vector4[] RegistVector4Property (string name) {
			if (Vector4Property.ContainsKey (name)) return Vector4Property[name];
			var propArrary = new Vector4[length];
			Vector4Property.Add (name, propArrary);
			return propArrary;
		}
		public void UnregistVector4Property (string name) {
			Vector4Property.Remove (name);
		}
		public void UpdateProperty () {
			foreach (KeyValuePair<string, float[]> prop in floatProperty) {
				block.SetFloatArray (prop.Key, prop.Value);
			}
			foreach (KeyValuePair<string, Vector4[]> prop in Vector4Property) {
				block.SetVectorArray (prop.Key, prop.Value);
			}
		}

		public void Dispose () {
			floatProperty.Clear ();
			Vector4Property.Clear ();
		}

	}
}