// ========================================================
// des：
// author: 
// time：2020-12-24 20:52:45
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Game {
	public class HUDTest : MonoBehaviour
	{
		public PlayableDirector director;

		public Transform Target1;
		public Transform Target2;
		public Transform Target3;

		CombineInstance[] combines;
		private Mesh m_mesh;
		private async void Start()
	    {

			await HUDConfigAsset.Load();
			HUDManager.Init();

			HUDBloodRender render = HUDManager.BloodPool.Alloc() as HUDBloodRender;
			
			render.SetTransform(Target1);
			render.PushBlood();

			//list.Add
			//m_mesh = new Mesh();
			//combines = new CombineInstance[10];
			//Mesh mesh = new Mesh();
			//var _combineInstance = new CombineInstance();
			//_combineInstance.mesh = mesh;
			//_combineInstance.transform = Matrix4x4.identity;
			//for (int i = 0; i < combines.Length; i++)
			//	combines[i] = _combineInstance;

			//TimelineAsset asset = null;
			//director.playableAsset = asset;
			//IEnumerable<PlayableBinding> bindings = asset.outputs;
			//foreach (var item in bindings)
			//{
			//	SkillTrack skillTrack = item.sourceObject as SkillTrack;
			//	if (skillTrack != null)
			//	{
			//		foreach (var item2 in skillTrack.GetClips())
			//		{
			//			var skillShot = item2.asset as SkillShot;
			//			var explore = new ExposedReference<Transform>();
			//			explore.defaultValue = new GameObject().transform;
			//			skillShot.template.Self = explore;
			//		}
			//	}
			//}
			//director.Play();
			//director.Stop();
		}

		
		private void Update()
		{
			HUDManager.Update();
		}

		private void OnApplicationQuit()
		{
			HUDManager.Dispose();
		}
	}
}
