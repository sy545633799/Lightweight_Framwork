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

namespace Game {
	public class HUDTest : MonoBehaviour
	{
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
