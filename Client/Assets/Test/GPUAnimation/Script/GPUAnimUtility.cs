using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
// using UnityEditor;
using UnityEngine;
namespace GPUAnim {

    public class GPUAnimUtility {
        static GPUAnimUtility _instance;
        public static GPUAnimUtility Instance {
            get {
                if (_instance == null) {
                    _instance = new GPUAnimUtility ();
                }
                return _instance;
            }
        }
        static HashSet<Mesh> meshes = new HashSet<Mesh> ();
        Vector4[] animInfo = new Vector4[12]; // set to 12 in shader "Unlit/GPUAnimSkinning"
        static HashSet<Material> materials = new HashSet<Material> ();
        System.Action<Plane[], Matrix4x4> _calculateFrustumPlanes_Imp;
        // /https://forum.unity.com/threads/calculatefrustumplanes-without-allocations.371636/
        public void CalculateFrustumPlanes (ref Plane[] planes, ref Matrix4x4 worldToProjectMatrix) { //用反射算相机裁剪，避免GC
            if (planes == null) throw new System.ArgumentNullException ("planes");
            if (planes.Length < 6) throw new System.ArgumentException ("Output array must be at least 6 in length.", "planes");

            if (_calculateFrustumPlanes_Imp == null) {
                var meth = typeof (GeometryUtility).GetMethod ("Internal_ExtractPlanes", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic, null, new System.Type[] { typeof (Plane[]), typeof (Matrix4x4) }, null);
                if (meth == null) throw new System.Exception ("Failed to reflect internal method. Your Unity version may not contain the presumed named method in GeometryUtility.");

                _calculateFrustumPlanes_Imp = System.Delegate.CreateDelegate (typeof (System.Action<Plane[], Matrix4x4>), meth) as System.Action<Plane[], Matrix4x4>;
                if (_calculateFrustumPlanes_Imp == null) throw new System.Exception ("Failed to reflect internal method. Your Unity version may not contain the presumed named method in GeometryUtility.");
            }

            _calculateFrustumPlanes_Imp (planes, worldToProjectMatrix);
        }
        public void SetupMesh (Mesh mesh) {
            if (meshes.Contains (mesh)) return;
            var weights = mesh.boneWeights;
            Vector2[] uv2 = new Vector2[mesh.vertexCount];
            Vector2[] uv3 = new Vector2[mesh.vertexCount];
            Vector2[] uv4 = new Vector2[mesh.vertexCount];
            Color[] colors = new Color[mesh.vertexCount];
            for (int i = 0; i < mesh.vertexCount; i++) {
                BoneWeight boneWeight = weights[i];
                uv2[i].x = i;

                uv3[i].x = boneWeight.boneIndex0;
                uv3[i].y = boneWeight.boneIndex1;
                uv4[i].x = boneWeight.boneIndex2;
                uv4[i].y = boneWeight.boneIndex3;
                colors[i].r = boneWeight.weight0;
                colors[i].g = boneWeight.weight1;
                colors[i].b = boneWeight.weight2;
                colors[i].a = boneWeight.weight3;
                // Debug.Log (boneWeight.boneIndex0 + " : " + boneWeight.weight0 + " / " + boneWeight.boneIndex1 + " : " + boneWeight.weight1 + " / " + boneWeight.boneIndex2 + " : " + boneWeight.weight2 + " / " + boneWeight.boneIndex3 + " : " + boneWeight.weight3);
            }
            mesh.uv2 = uv2;
            mesh.uv3 = uv3;
            mesh.uv4 = uv4;
            mesh.colors = colors; //四根骨骼及其权重
            meshes.Add (mesh);
        }

        internal void OnDispose () {
            if (meshes != null) meshes.Clear ();
            if (materials != null) materials.Clear ();
        }

        public void SetupAnimInfo (Material mat, GPUAnimCfg animCfg) {
            if (materials.Contains (mat)) return;
            if (animCfg) {
                for (int i = 0; i < animCfg.AnimClips.Count; i++) {
                    animInfo[i].x = animCfg.AnimClips[i].animLength;
                    animInfo[i].y = animCfg.AnimClips[i].startFrame;
                    animInfo[i].z = animCfg.AnimClips[i].frameCount;
                    animInfo[i].w = 0;
                }
                mat.SetVectorArray ("_AnimInfo", animInfo); //store anim information to material
                mat.SetFloat ("_Speed", 1);
                materials.Add (mat);
            }
        }
    }
    // 	public static void FillVectory4 (ref Vector4 v, float x, float y, float z, float w) {
    // 		v.x = x;
    // 		v.y = y;
    // 		v.z = z;
    // 		v.w = w;
    // 	}
    // 	public static void FastMatrixCopy (ref Matrix4x4 dst, ref Matrix4x4 src) {
    // 		dst.m00 = src.m00;
    // 		dst.m01 = src.m01;
    // 		dst.m02 = src.m02;
    // 		dst.m03 = src.m03;
    // 		dst.m10 = src.m10;
    // 		dst.m11 = src.m11;
    // 		dst.m12 = src.m12;
    // 		dst.m13 = src.m13;
    // 		dst.m20 = src.m20;
    // 		dst.m21 = src.m21;
    // 		dst.m22 = src.m22;
    // 		dst.m23 = src.m23;
    // 	}
    // 	public static void FastMatrix4x4Swap (ref Matrix4x4 dst, ref Matrix4x4 src) { // 忽略最后一行
    // 		float t;
    // 		t = dst.m00;
    // 		dst.m00 = src.m00;
    // 		src.m00 = t;
    // 		t = dst.m01;
    // 		dst.m01 = src.m01;
    // 		src.m01 = t;
    // 		t = dst.m02;
    // 		dst.m02 = src.m02;
    // 		src.m02 = t;
    // 		t = dst.m03;
    // 		dst.m03 = src.m03;
    // 		src.m03 = t;
    // 		t = dst.m10;
    // 		dst.m10 = src.m10;
    // 		src.m10 = t;
    // 		t = dst.m11;
    // 		dst.m11 = src.m11;
    // 		src.m11 = t;
    // 		t = dst.m12;
    // 		dst.m12 = src.m12;
    // 		src.m12 = t;
    // 		t = dst.m13;
    // 		dst.m13 = src.m13;
    // 		src.m13 = t;
    // 		t = dst.m20;
    // 		dst.m20 = src.m20;
    // 		src.m20 = t;
    // 		t = dst.m21;
    // 		dst.m21 = src.m21;
    // 		src.m21 = t;
    // 		t = dst.m22;
    // 		dst.m22 = src.m22;
    // 		src.m22 = t;
    // 		t = dst.m23;
    // 		dst.m23 = src.m23;
    // 		src.m23 = t;
    // 	}
    // 	public static void FastVector4Swap (ref Vector4 dst, ref Vector4 src) {
    // 		float t;
    // 		t = dst.x;
    // 		dst.x = src.x;
    // 		src.x = t;
    // 		t = dst.y;
    // 		dst.y = src.y;
    // 		src.y = t;
    // 		t = dst.z;
    // 		dst.z = src.z;
    // 		src.z = t;
    // 		t = dst.w;
    // 		dst.w = src.w;
    // 		src.w = t;
    // 	}

    // 	public static void SwapFloat (ref float a, ref float b) {
    // 		float t = a;
    // 		b = a;
    // 		a = b;
    // 	}
    // 	public static Texture2D SaveAsPng (string path, Texture2D tex) {
    // 		if (tex == null) return null;
    // 		var b = tex.EncodeToPNG ();
    // 		var p = Path.Combine (path, tex.name + ".png");
    // 		File.WriteAllBytes (p, b);
    // 		AssetDatabase.Refresh ();
    // 		return AssetDatabase.LoadAssetAtPath<Texture2D> (p);
    // 	}

}