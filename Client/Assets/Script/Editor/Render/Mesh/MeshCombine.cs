// ========================================================
// des：
// author: 
// time：2021-04-14 14:21:44
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Game;
using System.IO;

namespace Game.Editor {
	public class MeshCombine : ScriptableWizard
	{
		public GameObject Root;

		//void OnWizardUpdate() { }

		//void OnWizardCreate() { }

		void OnWizardOtherButton()
		{
			SkinnedMeshRenderer[] meshRenderer = Root.GetComponentsInChildren<SkinnedMeshRenderer>(true);

			SkinnedMeshRenderer r = MeshUtility.CombineObject(Root, meshRenderer, true);
			AssetDatabase.CreateAsset(r.sharedMesh, "Assets/Test/mesh.asset");
			Texture2D src = r.material.mainTexture as Texture2D;
			Texture2D texture = new Texture2D(2048, 2048, TextureFormat.RGB24, true);

			for (int i = 0; i < MeshUtility.COMBINE_TEXTURE_MAX; i++)
			{
				for (int j = 0; j < MeshUtility.COMBINE_TEXTURE_MAX; j++)
				{
					texture.SetPixel(i, j, src.GetPixel(i, j));
				}
			}
			File.WriteAllBytes("Assets/Test/chan.png", texture.EncodeToPNG());
			
		}

		[MenuItem("Tools/Render/合并SkinMeshRender")]
		static void CreateDeSer()
		{
			ScriptableWizard.DisplayWizard<MeshCombine>("合并SkinMeshRender", "取消", "设置");
		}

	}
}
