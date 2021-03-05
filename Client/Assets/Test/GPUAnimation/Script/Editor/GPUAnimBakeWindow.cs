using System.Collections.Generic;
using System.IO;
using System.Linq;
using GPUAnim;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
public class GPUAnimBakeWindow : EditorWindow {
	public enum bakeType {
		SkeletonAnim,
		VertexAnim,
	}
	public bakeType type = bakeType.SkeletonAnim;
	GameObject go;
	BaseBaker baker;
	string savePath = "Assets/";
	SkinnedMeshRenderer skm;
	public int framesOfAll = 0;
	int vertexCount;
	Mesh mesh;
	Mesh bakedMesh;
	Animation animation;
	Animator animator;
	Material material;
	Vector3 translate;
	Vector3 rotation;
	Matrix4x4 matrices;
	Vector3 scale = new Vector3 (1, 1, 1);
	List<GPUAnimationData> animDatas = new List<GPUAnimationData> ();

	[MenuItem ("Window/GPUAnimBakeWindow")]
	public static void ShowWindow () {
		EditorWindow.GetWindow (typeof (GPUAnimBakeWindow));
	}

	void OnGUI () {
		go = EditorGUILayout.ObjectField ("With Animator/Animation:", go, typeof (GameObject), true) as GameObject;
		var t = (bakeType) EditorGUILayout.EnumPopup ("BakeType:", type);
		if (t != type) {
			if (t == bakeType.SkeletonAnim) rotation = Vector3.zero;
			else if (t == bakeType.VertexAnim) rotation = new Vector3 (-90f, 0, 0);
			type = t;
		}
		EditorGUILayout.LabelField ("Path: " + savePath);
		translate = EditorGUILayout.Vector3Field ("translate:", translate);
		rotation = EditorGUILayout.Vector3Field ("Rotation:", rotation);
		scale = EditorGUILayout.Vector3Field ("scale:", scale);
		if (GUILayout.Button ("bake")) {
			framesOfAll = 0;
			animDatas.Clear ();
			animator = go.GetComponentInChildren<Animator> ();
			animation = go.GetComponentInChildren<Animation> ();
			if (animator) {
				List<AnimationClip> animClips = animator.runtimeAnimatorController.animationClips.ToList ();
				foreach (var clip in animClips) animDatas.Add (new GPUAnimationData (clip));
				baker = new AnimatorBaker();
			} else if (animation) {
				List<AnimationState> stateClips = new List<AnimationState> (animation.Cast<AnimationState> ());
				foreach (var clip in stateClips) animDatas.Add (new GPUAnimationData (clip));
				baker = new AnimationBaker();
			} else {
				EditorUtility.DisplayDialog ("Err", "No Animator/Animation Component Attached！", "OK");
				return;
			}

			baker.window = this;
			skm = go.GetComponentInChildren<SkinnedMeshRenderer> ();
			mesh = skm.sharedMesh;
			material = skm.sharedMaterial;
			vertexCount = mesh.vertexCount;
			if (!bakedMesh) bakedMesh = new Mesh ();
			matrices = Matrix4x4.TRS (translate, Quaternion.Euler (rotation), scale);

			for (int i = 0; i < animDatas.Count; i++) baker.Bake (animDatas[i]);

			Save ();
		}
		if (GUILayout.Button ("clear")) {
			animDatas.Clear ();
		}
	}

	Texture2D CreateSkeletonTexture () { //存骨骼动画
		Texture2D tex = new Texture2D (ClosestPowerOfTwo (framesOfAll), ClosestPowerOfTwo (skm.bones.Length * 3), TextureFormat.RGBAHalf, false);
		tex.name = go.name;
		for (int i = 0; i < animDatas.Count; i++) {
			for (int j = 0; j < animDatas[i].frameCount; j++) {
				for (int k = 0; k < skm.bones.Length; k++) { //第四行始终为(0,0,0,1)，省一个像素不传
					tex.SetPixel (animDatas[i].startFrame + j, k * 3 + 0, new Vector4 (animDatas[i].boneMatrices[j][k].m00, animDatas[i].boneMatrices[j][k].m01, animDatas[i].boneMatrices[j][k].m02, animDatas[i].boneMatrices[j][k].m03));
					tex.SetPixel (animDatas[i].startFrame + j, k * 3 + 1, new Vector4 (animDatas[i].boneMatrices[j][k].m10, animDatas[i].boneMatrices[j][k].m11, animDatas[i].boneMatrices[j][k].m12, animDatas[i].boneMatrices[j][k].m13));
					tex.SetPixel (animDatas[i].startFrame + j, k * 3 + 2, new Vector4 (animDatas[i].boneMatrices[j][k].m20, animDatas[i].boneMatrices[j][k].m21, animDatas[i].boneMatrices[j][k].m22, animDatas[i].boneMatrices[j][k].m23));
					// Debug.Log (string.Format ("{0}/{1}/{2}/{3}", animDatas[i].boneMatrices[j][k].m30, animDatas[i].boneMatrices[j][k].m31, animDatas[i].boneMatrices[j][k].m32, animDatas[i].boneMatrices[j][k].m33));
				}
			}
		}
		tex.Apply ();
		return tex;
	}
	Texture2D CreateVertexTexture () { //存顶点动画
		Texture2D tex = new Texture2D (ClosestPowerOfTwo (framesOfAll), ClosestPowerOfTwo (vertexCount), TextureFormat.RGBAHalf, false);
		for (int i = 0; i < animDatas.Count; i++) {
			for (int j = 0; j < animDatas[i].frameCount; j++) {
				for (int k = 0; k < vertexCount; k++) {
					tex.SetPixel (animDatas[i].startFrame + j, k, animDatas[i].cols[j][k]);
				}
			}
		}
		tex.Apply ();
		return tex;
	}
	private void Save () {
		string s = string.Format ("{0}/", EditorUtility.OpenFolderPanel ("选择导出路径", savePath, ""));
		if (s.Contains ("Assets")) {
			savePath = s.Substring (s.LastIndexOf ("Assets"));
			var o = new GameObject (go.name);
			var gpuAnim = o.AddComponent<GPUAnimation> ();
			var cfg = ScriptableObject.CreateInstance (typeof (GPUAnimCfg)) as GPUAnimCfg;
			o.name = go.name;
			Material mat = null;
			Texture2D combinedTex = null;
			if (type == bakeType.SkeletonAnim) {
				mat = new Material (Shader.Find ("Unlit/GPUAnimSkinning"));
				mat.name = go.name + "_skinMat";
				combinedTex = CreateSkeletonTexture ();
				combinedTex.name = go.name + "_skinTex";
				// savePath += "/" + o.name + "_Skeleton" + "/";
			} else if (type == bakeType.VertexAnim) {
				mat = new Material (Shader.Find ("Unlit/GPUAnimVertex"));
				mat.name = go.name + "_vertMat";
				combinedTex = CreateVertexTexture ();
				combinedTex.name = go.name + "_vertTex";
				// savePath += "/" + o.name + "_Vertex" + "/";
			}
			mat.enableInstancing = true;

			foreach (var info in animDatas) {
				info.startFrame += 1; //部分动画闪烁，去掉头尾就可以吃了...
				info.frameCount -= 2; //...Orz
				cfg.AnimClips.Add (info as AnimClipInfo);
			}

			if (!AssetDatabase.IsValidFolder (savePath + o.name)) AssetDatabase.CreateFolder (savePath.TrimEnd ('/'), o.name);
			savePath += o.name + "/";
			AssetDatabase.CreateAsset (cfg, Path.Combine (savePath, o.name + "_animInfo.asset"));
			AssetDatabase.CreateAsset (combinedTex, Path.Combine (savePath, combinedTex.name + ".asset"));
			// SaveAsPng (s, combinedTex);
			mat.SetTexture ("_MainTex", material.GetTexture ("_MainTex"));
			mat.SetTexture ("_AnimMap", combinedTex);
			AssetDatabase.CreateAsset (mat, Path.Combine (savePath, mat.name + ".mat"));
			gpuAnim.animCfg = cfg;
			gpuAnim.GetComponent<MeshFilter> ().sharedMesh = mesh;
			gpuAnim.GetComponent<Renderer> ().sharedMaterial = mat;
			if (type == bakeType.SkeletonAnim) PrefabUtility.CreatePrefab (Path.Combine (savePath, o.name + "_skeletonPrefab.prefab"), o);
			else if (type == bakeType.VertexAnim) PrefabUtility.CreatePrefab (Path.Combine (savePath, o.name + "_vertexPrefab.prefab"), o);
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();

			DestroyImmediate (o);
		}
	}
	Texture2D SaveAsPng (string path, Texture2D tex) {
		if (tex == null) return null;
		var b = tex.EncodeToPNG ();
		var p = Path.Combine (path.Substring (savePath.LastIndexOf ("Assets")), tex.name + ".png");
		File.WriteAllBytes (p, b);
		AssetDatabase.Refresh ();
		return AssetDatabase.LoadAssetAtPath<Texture2D> (p);
	}
	int ClosestPowerOfTwo (int n) {
		int t = Mathf.ClosestPowerOfTwo (n);
		return t >= n? t : t * 2;
	}
	/////////////////////////////////////////////////////////////////////////////////////////////////////
	class BaseBaker {
		public GPUAnimBakeWindow window { get; set; }
		protected virtual void PlayAnim (string name) { }
		protected virtual void SetAnimTime (GPUAnimationData anim, float f) { }
		public void Bake (GPUAnimationData anim) {
			if (window.type == bakeType.SkeletonAnim) BakeSkeleton (anim);
			if (window.type == bakeType.VertexAnim) BakeVertex (anim);
		}
		public void BakeSkeleton (GPUAnimationData anim) {
			float sampleTime = 0;
			float perFrameTime = 0;
			perFrameTime = anim.animLength / anim.frameCount;
			sampleTime += perFrameTime / 2;
			PlayAnim (anim.clipName);
			anim.boneMatrices = new Matrix4x4[anim.frameCount][];
			for (int i = 0; i < anim.frameCount; i++) {
				SetAnimTime (anim, sampleTime);
				// window.skm.BakeMesh (window.bakedMesh); //to do combine mesh
				anim.boneMatrices[i] = CalculateSkinMatrix (window.go.transform, window.skm.bones, window.skm.sharedMesh.bindposes); //根骨骼指定,window.skm.rootBone ?
				sampleTime += perFrameTime;
			}
			anim.startFrame = window.framesOfAll;
			window.framesOfAll += anim.frameCount;
		}
		Matrix4x4[] CalculateSkinMatrix (Transform rootBone, Transform[] bones, Matrix4x4[] bindposes) {
			var matrices = new Matrix4x4[bones.Length];
			for (int i = 0; i < bones.Length; i++) {
				matrices[i] = Matrix4x4.TRS (bones[i].localPosition, bones[i].localRotation, bones[i].localScale);
				matrices[i] = matrices[i] * bindposes[i];
				var p = bones[i];
				if (p == rootBone) continue;
				do {
					p = p.parent;
					matrices[i] = Matrix4x4.TRS (p.localPosition, p.localRotation, p.localScale) * matrices[i]; //累乘至根骨骼
				} while (p != rootBone);
				matrices[i] = window.matrices * matrices[i];
			}
			return matrices;
		}
		public void BakeVertex (GPUAnimationData anim) {
			float sampleTime = 0;
			float perFrameTime = 0;
			Vector3 v = new Vector3 ();
			perFrameTime = anim.animLength / anim.frameCount;
			anim.cols = new Color[anim.frameCount][];
			PlayAnim (anim.clipName);
			for (int i = 0; i < anim.frameCount; i++) {
				SetAnimTime (anim, sampleTime);
				window.skm.BakeMesh (window.bakedMesh);
				anim.cols[i] = new Color[window.vertexCount];
				for (int j = 0; j < window.vertexCount; j++) {
					v = window.bakedMesh.vertices[j];
					v = window.matrices.MultiplyPoint3x4 (v);
					anim.cols[i][j] = new Color (v.x, v.y, v.z, 1);
				}
				sampleTime += perFrameTime;
			}
			anim.startFrame = window.framesOfAll;
			window.framesOfAll += anim.frameCount;
		}
	}
	class AnimatorBaker : BaseBaker {
		protected override void PlayAnim (string name) {
			window.animator.Play (name);
		}
		protected override void SetAnimTime (GPUAnimationData anim, float sampleTime) {
			anim.animClip.SampleAnimation (window.animator.gameObject, sampleTime);
		}
	}
	class AnimationBaker : BaseBaker {
		protected override void PlayAnim (string name) {
			window.animation.Play (name);
		}
		protected override void SetAnimTime (GPUAnimationData anim, float sampleTime) {
			anim.stateClip.time = sampleTime;
			window.animation.Sample ();
		}
	}
}