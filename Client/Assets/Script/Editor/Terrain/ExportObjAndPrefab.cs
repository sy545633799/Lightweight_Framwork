// ========================================================
// des：
// author: 
// time：2021-04-07 13:57:59
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System;

namespace Game.Editor {
	public partial class ExportTerrain : ScriptableWizard
	{
		private static GameObject ExportObjAndPrefab(TerrainData data, string assetName, int x1, int y1, Vector4[] tilling)
		{
			string objPath = $"{MeshFolder}{assetName}.obj";
			string matPath = $"{MaterialFolder}{assetName}.mat";
			string prefabPath = $"{PrefabFolder}{assetName}.prefab";


			int w = data.heightmapResolution;
			int h = data.heightmapResolution;
			float tRes = (float)w / (float)Resolution;

			Vector3 meshScale = data.size;
			meshScale = new Vector3(meshScale.x / (h - 1) * tRes, meshScale.y, meshScale.z / (w - 1) * tRes);
			Vector2 uvScale = new Vector2((float)(1.0 / (w - 1)), (float)(1.0 / (h - 1)));

			float[,] tData = data.GetHeights(0, 0, w, h);
			w = (int)((w - 1) / tRes + 1);
			h = (int)((h - 1) / tRes + 1);
			Vector3[] tVertices = new Vector3[w * h];
			Vector2[] tUV = new Vector2[w * h];
			int[] tPolys = new int[(w - 1) * (h - 1) * 6];
			int y = 0;
			int x = 0;
			for (y = 0; y < h; y++)
			{
				for (x = 0; x < w; x++)
				{
					tVertices[y * w + x] = Vector3.Scale(meshScale, new Vector3(x, tData[(int)(x * tRes), (int)(y * tRes)], y));
					tUV[y * w + x] = Vector2.Scale(new Vector2(y * tRes, x * tRes), uvScale);
				}
			}

			y = 0;
			x = 0;
			int index = 0;
			for (y = 0; y < h - 1; y++)
			{
				for (x = 0; x < w - 1; x++)
				{
					tPolys[index++] = (y * w) + x;
					tPolys[index++] = ((y + 1) * w) + x;
					tPolys[index++] = (y * w) + x + 1;

					tPolys[index++] = ((y + 1) * w) + x;
					tPolys[index++] = ((y + 1) * w) + x + 1;
					tPolys[index++] = (y * w) + x + 1;
				}
			}

			StreamWriter sw = new StreamWriter(assetName + ".obj");
			try
			{
				sw.WriteLine("# T4M File");
				System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
				counter = tCount = 0;
				totalCount = (int)((tVertices.Length * 2 + (tPolys.Length / 3)) / progressUpdateInterval);
				for (int i = 0; i < tVertices.Length; i++)
				{
					UpdateProgress();
					StringBuilder sb = new StringBuilder("v ", 20);
					sb.Append(tVertices[i].x.ToString()).Append(" ").
					   Append(tVertices[i].y.ToString()).Append(" ").
					   Append(tVertices[i].z.ToString());
					sw.WriteLine(sb);
				}

				for (int i = 0; i < tUV.Length; i++)
				{
					UpdateProgress();
					StringBuilder sb = new StringBuilder("vt ", 22);
					sb.Append(tUV[i].x.ToString()).Append(" ").
					   Append(tUV[i].y.ToString());
					sw.WriteLine(sb);
				}
				for (int i = 0; i < tPolys.Length; i += 3)
				{
					UpdateProgress();
					StringBuilder sb = new StringBuilder("f ", 43);
					sb.Append(tPolys[i] + 1).Append("/").Append(tPolys[i] + 1).Append(" ").
					   Append(tPolys[i + 1] + 1).Append("/").Append(tPolys[i + 1] + 1).Append(" ").
					   Append(tPolys[i + 2] + 1).Append("/").Append(tPolys[i + 2] + 1);
					sw.WriteLine(sb);
				}
			}
			catch (Exception err)
			{
				Debug.Log("Error saving file: " + err.Message);
			}
			sw.Close();

			//创建并导入mesh
			FileUtil.CopyFileOrDirectory(assetName + ".obj", objPath);
			FileUtil.DeleteFileOrDirectory(assetName + ".obj");
			AssetDatabase.ImportAsset(objPath, ImportAssetOptions.ForceUpdate);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			//创建并导入prefab
			GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(objPath, typeof(GameObject));
			Vector3 position = terrain.transform.position + new Vector3(x1 * data.size.z, 0, y1 * data.size.z);
			GameObject forRotate = (GameObject)GameObject.Instantiate(prefab, position, Quaternion.identity) as GameObject;
			Transform childCheck = forRotate.transform.Find("default");
			GameObject terrainGo = childCheck.gameObject;
			forRotate.transform.DetachChildren();
			GameObject.DestroyImmediate(forRotate);
			terrainGo.name = assetName;
			terrainGo.transform.rotation = Quaternion.Euler(0, 90, 0);
			Material Tmaterial = AssetDatabase.LoadAssetAtPath<Material>(matPath);
			terrainGo.AddComponent<MeshCollider>();
			terrainGo.isStatic = true;
			terrainGo.GetComponent<Renderer>().material = Tmaterial;
			SetTex2DArray setTex2D = terrainGo.AddComponent<SetTex2DArray>();
			setTex2D.UV_STArray = tilling;
			setTex2D.SetArray();
			GameObject BasePrefab2 = PrefabUtility.SaveAsPrefabAssetAndConnect(terrainGo, prefabPath, InteractionMode.AutomatedAction);
			AssetDatabase.ImportAsset(prefabPath, ImportAssetOptions.ForceUpdate);
			EditorUtility.SetSelectedRenderState(terrainGo.GetComponent<Renderer>(), EditorSelectedRenderState.Wireframe);

			AssetDatabase.StartAssetEditing();
			ModelImporter OBJI = ModelImporter.GetAtPath(objPath) as ModelImporter;
			OBJI.globalScale = 1;
			OBJI.importNormals = ModelImporterNormals.Calculate;
			OBJI.importTangents = ModelImporterTangents.CalculateLegacyWithSplitTangents;
			OBJI.generateAnimations = ModelImporterGenerateAnimations.None;
			OBJI.meshCompression = ModelImporterMeshCompression.Off;
			OBJI.isReadable = true; //地形碰撞需要
			OBJI.normalSmoothingAngle = 180f;
			AssetDatabase.ImportAsset(objPath, ImportAssetOptions.ForceSynchronousImport);
			AssetDatabase.StopAssetEditing();

			UpdateProgress();
			return terrainGo;
		}

	}
}
