using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text;
using System;

public class TerrainSlicing : ScriptableWizard
{
	//分割大小
	public static int SlicingSize = 1;
	public static int Resolution = 250;

	private static string assetName = "";
	private static string PrefabFolder = "Assets/Test/T4M/";
	private static Terrain terrain;

	void OnWizardOtherButton()
	{

		terrain = GameObject.FindObjectOfType<Terrain>();
		if (terrain == null)
		{
			Debug.LogError("找不到地形!");
			return;
		}
		if (System.IO.Directory.Exists(PrefabFolder))
			System.IO.Directory.Delete(PrefabFolder, true);
		System.IO.Directory.CreateDirectory(PrefabFolder);
		System.IO.Directory.CreateDirectory(PrefabFolder + "Terrains/");
		System.IO.Directory.CreateDirectory(PrefabFolder + "Terrains/Material/");
		System.IO.Directory.CreateDirectory(PrefabFolder + "Terrains/Texture/");
		System.IO.Directory.CreateDirectory(PrefabFolder + "Terrains/Meshes/");
		System.IO.Directory.CreateDirectory(PrefabFolder + "Terrains/Prefab/");

		if (SlicingSize == 1)
		{
			CreateTexture(terrain.terrainData, terrain.name, 0, 0);
			ConvertUTerrain(terrain.terrainData, terrain.name, 0, 0);
		}
		else if (SlicingSize > 1)
		{
			Slicing();
		}
		terrain.gameObject.SetActive(false);
	}

	protected override bool DrawWizardGUI()
	{
		SlicingSize = EditorGUILayout.IntField("切割份数", SlicingSize);
		Resolution = EditorGUILayout.IntField("Mesh质量", Resolution);
		return base.DrawWizardGUI();
	}

	[MenuItem("Tools/Terrain/切割Terrain为Mesh")]
	static void CreateDeSer()
	{
		ScriptableWizard.DisplayWizard<TerrainSlicing>("切割Terrain为Mesh", "取消", "切割");
	}


	//开始分割地形
	//[MenuItem("Terrain/Slicing")]
	private static void Slicing()
	{
		TerrainData terrainData = terrain.terrainData;

		//这里我分割的宽和长度是一样的.这里求出循环次数,TerrainLoad.SIZE要生成的地形宽度,长度相同
		//高度地图的分辨率只能是2的N次幂加1,所以SLICING_SIZE必须为2的N次幂
		//SlicingSize = (int)terrainData.size.x / 125;

		Vector3 oldSize = terrainData.size;

		//得到新地图分辨率
		int newHeightmapResolution = (terrainData.heightmapResolution - 1) / SlicingSize;
		int newAlphamapResolution = terrainData.alphamapResolution / SlicingSize;
		int newbaseMapResolution = terrainData.baseMapResolution / SlicingSize;
		TerrainLayer[] splatProtos = terrainData.terrainLayers;

		//循环宽和长,生成小块地形
		for (int x = 0; x < SlicingSize; ++x)
		{
			for (int y = 0; y < SlicingSize; ++y)
			{
				//创建资源
				TerrainData newData = new TerrainData();

				//设置分辨率参数
				newData.heightmapResolution = (terrainData.heightmapResolution - 1) / SlicingSize;
				newData.alphamapResolution = terrainData.alphamapResolution / SlicingSize;
				newData.baseMapResolution = terrainData.baseMapResolution / SlicingSize;

				//设置大小
				newData.size = new Vector3(oldSize.x / SlicingSize, oldSize.y, oldSize.z / SlicingSize);

				//设置地形原型
				TerrainLayer[] newSplats = new TerrainLayer[splatProtos.Length];

				for (int i = 0; i < splatProtos.Length; ++i)
				{
					newSplats[i] = new TerrainLayer();
					newSplats[i].diffuseTexture = splatProtos[i].diffuseTexture;
					newSplats[i].normalMapTexture = splatProtos[i].normalMapTexture;
					newSplats[i].tileSize = splatProtos[i].tileSize;

					float offsetX = (newData.size.x * x) % splatProtos[i].tileSize.x + splatProtos[i].tileOffset.x;
					float offsetY = (newData.size.z * y) % splatProtos[i].tileSize.y + splatProtos[i].tileOffset.y;
					newSplats[i].tileOffset = new Vector2(offsetX, offsetY);
				}
				
				newData.terrainLayers = newSplats;

				//设置混合贴图
				float[,,] alphamap = new float[newAlphamapResolution, newAlphamapResolution, splatProtos.Length];
				alphamap = terrainData.GetAlphamaps(x * newData.alphamapWidth, y * newData.alphamapHeight, newData.alphamapWidth, newData.alphamapHeight);
				newData.SetAlphamaps(0, 0, alphamap);

				//设置高度
				int xBase = terrainData.heightmapResolution / SlicingSize;
				int yBase = terrainData.heightmapResolution / SlicingSize;
				float[,] height = terrainData.GetHeights(xBase * x, yBase * y, xBase + 1, yBase + 1);
				newData.SetHeights(0, 0, height);


				Scene scene = SceneManager.GetActiveScene();
				assetName = $"{scene.name}_{x}_{y}";
				CreateTexture(newData, assetName, x, y);
				//生成Mesh
				ConvertUTerrain(newData, assetName, x, y);
			}
		}

		EditorUtility.ClearProgressBar();
	}


	private static void CreateTexture(TerrainData child, string assetName, int x1, int y1)
	{
		string path = PrefabFolder + "Terrains/Texture/" + assetName + ".png";
		string AssetName = AssetDatabase.GetAssetPath(terrain.terrainData) as string;
		UnityEngine.Object[] AssetName2 = AssetDatabase.LoadAllAssetsAtPath(AssetName);
		if (AssetName2 != null && AssetName2.Length > 1)
		{
			for (var b = 0; b < AssetName2.Length; b++)
			{
				if (AssetName2[b].name == "SplatAlpha 0")
				{
					Texture2D texture = AssetName2[b] as Texture2D;
					byte[] bytes = texture.EncodeToPNG();
					File.WriteAllBytes(path, bytes);
					AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
				}
			}
		}
		else
		{
			path = PrefabFolder + "Terrains/Texture/" + assetName + ".png";
			Texture2D NewMaskText = new Texture2D(512, 512, TextureFormat.ARGB32, true);
			Color[] ColorBase = new Color[512 * 512];
			for (var t = 0; t < ColorBase.Length; t++)
			{
				ColorBase[t] = new Color(1, 0, 0, 0);
			}
			NewMaskText.SetPixels(ColorBase);
			byte[] data = NewMaskText.EncodeToPNG();
			File.WriteAllBytes(path, data);
			AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
		}
		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

		UpdateProgress();

		TextureImporter TextureI = AssetImporter.GetAtPath(path) as TextureImporter;
		TextureI.textureType = TextureImporterType.Default;
		TextureImporterPlatformSettings tSetting = new TextureImporterPlatformSettings();
		tSetting.overridden = true;
		tSetting.format = TextureImporterFormat.RGBA32;
		TextureI.SetPlatformTextureSettings(tSetting);
		TextureI.isReadable = true;
		TextureI.anisoLevel = 9;
		TextureI.mipmapEnabled = false;
		TextureI.wrapMode = TextureWrapMode.Clamp;
		AssetDatabase.Refresh();
		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
		UpdateProgress();


		Material Tmaterial = new Material(Shader.Find("Cartoon/Ulit/Terrain"));
		AssetDatabase.CreateAsset(Tmaterial, $"{PrefabFolder}Terrains/Material/{assetName}.mat");
		AssetDatabase.ImportAsset($"{PrefabFolder}Terrains/Material/{assetName}.mat", ImportAssetOptions.ForceUpdate);
		AssetDatabase.Refresh();
		string texPath = $"{PrefabFolder}Terrains/Texture/{assetName}.png";
		Texture test = (Texture)AssetDatabase.LoadAssetAtPath(texPath, typeof(Texture));
		Tmaterial.SetTexture("_Control", test);
		float scale = 1 / (float)SlicingSize;
		Tmaterial.SetTextureScale("_Control", new Vector2(scale, scale));
		Tmaterial.SetTextureOffset("_Control", new Vector2(scale * x1, scale * y1));

		TerrainLayer[] texts = child.terrainLayers;
		for (int e = 0; e < texts.Length; e++)
		{
			if (e < 4)
			{
				Tmaterial.SetTexture("_Splat" + (e), texts[e].diffuseTexture);
				Tmaterial.SetTextureScale("_Splat" + (e), texts[e].tileSize * 8.9f);
				Tmaterial.SetTexture("_Normal" + (e), texts[e].normalMapTexture);
				Tmaterial.SetTextureScale("_Normal" + (e), texts[e].tileSize * 8.9f);
			}
		}

		UpdateProgress();
	}



	private static void ConvertUTerrain(TerrainData child, string assetName, int x1, int y1)
	{
		
		AssetDatabase.Refresh();
		int w = child.heightmapResolution;
		int h = child.heightmapResolution;
		float tRes = w / Resolution;

		Vector3 meshScale = child.size;
		meshScale = new Vector3(meshScale.x / (h - 1) * tRes, meshScale.y, meshScale.z / (w - 1) * tRes);
		Vector2 uvScale = new Vector2((float)(1.0 / (w - 1)), (float)(1.0 / (h - 1)));
		float[,] tData = child.GetHeights(0, 0, w, h);
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
		
		FileUtil.CopyFileOrDirectory(assetName + ".obj", PrefabFolder + "Terrains/Meshes/" + assetName + ".obj");
		FileUtil.DeleteFileOrDirectory(assetName + ".obj");
		AssetDatabase.ImportAsset(PrefabFolder + "Terrains/Meshes/" + assetName + ".obj", ImportAssetOptions.ForceUpdate);
		UpdateProgress();
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();


		//Instance du T4M
		GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(PrefabFolder + "Terrains/Meshes/" + assetName + ".obj", typeof(GameObject));

		AssetDatabase.Refresh();

		Vector3 position = terrain.transform.position + new Vector3(x1 * child.size.z, 0, y1 * child.size.z);
		GameObject forRotate = (GameObject)GameObject.Instantiate(prefab, position, Quaternion.identity) as GameObject;
		Transform childCheck = forRotate.transform.Find("default");
		GameObject Child = childCheck.gameObject;
		forRotate.transform.DetachChildren();
		GameObject.DestroyImmediate(forRotate);
		Child.name = assetName;
		Child.AddComponent<T4MObjSC>();
		Child.transform.rotation = Quaternion.Euler(0, 90, 0);

		UpdateProgress();

		Material Tmaterial = AssetDatabase.LoadAssetAtPath<Material>($"{PrefabFolder}Terrains/Material/{assetName}.mat");
		//Application des Parametres sur le Script
		Child.GetComponent<T4MObjSC>().T4MMaterial = Tmaterial;
		Child.GetComponent<T4MObjSC>().ConvertType = "UT";

		//Regalges Divers
		int vertexInfo = 0;
		int partofT4MObj = 0;
		int trisInfo = 0;
		int countchild = Child.transform.childCount;
		if (countchild > 0)
		{
			Renderer[] T4MOBJPART = Child.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < T4MOBJPART.Length; i++)
			{
				if (!T4MOBJPART[i].gameObject.AddComponent<MeshCollider>())
					T4MOBJPART[i].gameObject.AddComponent<MeshCollider>();
				T4MOBJPART[i].gameObject.isStatic = true;
				T4MOBJPART[i].material = Tmaterial;
				T4MOBJPART[i].gameObject.layer = 30;
				T4MOBJPART[i].gameObject.AddComponent<T4MPartSC>();
				Child.GetComponent<T4MObjSC>().T4MMesh = T4MOBJPART[0].GetComponent<MeshFilter>();
				partofT4MObj += 1;
				vertexInfo += T4MOBJPART[i].gameObject.GetComponent<MeshFilter>().sharedMesh.vertexCount;
				trisInfo += T4MOBJPART[i].gameObject.GetComponent<MeshFilter>().sharedMesh.triangles.Length / 3;
			}
		}
		else
		{
			Child.AddComponent<MeshCollider>();
			Child.isStatic = true;
			Child.GetComponent<Renderer>().material = Tmaterial;
			Child.layer = 30;
			vertexInfo += Child.GetComponent<MeshFilter>().sharedMesh.vertexCount;
			trisInfo += Child.GetComponent<MeshFilter>().sharedMesh.triangles.Length / 3;
			partofT4MObj += 1;
		}


		UpdateProgress();


		GameObject BasePrefab2 = PrefabUtility.CreatePrefab(PrefabFolder + "Terrains/Prefab/" + assetName + ".prefab", Child);
		AssetDatabase.ImportAsset(PrefabFolder + "Terrains/Prefab/" + assetName + ".prefab", ImportAssetOptions.ForceUpdate);
		GameObject forRotate2 = (GameObject)PrefabUtility.InstantiatePrefab(BasePrefab2) as GameObject;
		GameObject.DestroyImmediate(Child.gameObject);
		Child = forRotate2.gameObject;
		EditorUtility.SetSelectedRenderState(Child.GetComponent<Renderer>(), EditorSelectedRenderState.Wireframe);


		EditorUtility.ClearProgressBar();

		AssetDatabase.DeleteAsset(PrefabFolder + "Terrains/Meshes/Materials");
		AssetDatabase.StartAssetEditing();
		//Modification des attribut du mesh avant de le préfabriquer
		ModelImporter OBJI = ModelImporter.GetAtPath(PrefabFolder + "Terrains/Meshes/" + assetName + ".obj") as ModelImporter;
		OBJI.globalScale = 1;
		OBJI.splitTangentsAcrossSeams = true;
		OBJI.normalImportMode = ModelImporterTangentSpaceMode.Calculate;
		OBJI.tangentImportMode = ModelImporterTangentSpaceMode.Calculate;
		OBJI.generateAnimations = ModelImporterGenerateAnimations.None;
		OBJI.meshCompression = ModelImporterMeshCompression.Off;
		OBJI.normalSmoothingAngle = 180f;
		//AssetDatabase.ImportAsset (T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj", ImportAssetOptions.TryFastReimportFromMetaData);
		AssetDatabase.ImportAsset(PrefabFolder + "Terrains/Meshes/" + assetName + ".obj", ImportAssetOptions.ForceSynchronousImport);
		AssetDatabase.StopAssetEditing();
		PrefabUtility.ResetToPrefabState(Child);
	}

	

	private static int counter;
	private static int tCount;
	private static int totalCount;
	private static float progressUpdateInterval = 10000;
	private static void UpdateProgress()
	{
		if (counter++ == progressUpdateInterval)
		{
			counter = 0;
			EditorUtility.DisplayProgressBar("Generate...", "", Mathf.InverseLerp(0, totalCount, ++tCount));
		}
	}
}