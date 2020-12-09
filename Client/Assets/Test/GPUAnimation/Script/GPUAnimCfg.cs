using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GPUAnim {
	public class GPUAnimCfg : ScriptableObject {
		public List<AnimClipInfo> AnimClips = new List<AnimClipInfo> ();
		// public Texture2D animTex;
	}

	[Serializable]
	public class AnimClipInfo {
		public string clipName;
		public float animLength; //time	
		public int startFrame;
		public int frameCount; //帧数
	}
	public class GPUAnimationData : AnimClipInfo {
		public AnimationClip animClip;
		public AnimationState stateClip;
		public Color[][] cols;
		public Matrix4x4[][] boneMatrices;
		// public Texture2D tex;
		public GPUAnimationData (AnimationClip clip) {
			this.animClip = clip;
			this.clipName = clip.name;
			this.animLength = clip.length;
			this.frameCount = (int) (clip.frameRate * clip.length);
		}
		public GPUAnimationData (AnimationState state) {
			this.stateClip = state;
			this.clipName = state.name;
			this.animLength = state.clip.length;
			this.frameCount = (int) (state.clip.frameRate * state.length);
		}
	}
}