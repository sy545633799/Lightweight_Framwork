using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text;
using System;

namespace Game.Editor
{
	public partial class ExportTerrain : ScriptableWizard
	{
		//分割大小
		public static int SlicingSize = 1;
		public static int Resolution = 250;

		private static string assetName = "";
		private static Terrain terrain;
		private static string BaseFolder = "Assets/Test/T4M/";
		private static string TextureFolder = null;
		private static string MaterialFolder = null;
		private static string MeshFolder = null;
		private static string PrefabFolder = null;
		

		void OnWizardOtherButton()
		{
			terrain = GameObject.FindObjectOfType<Terrain>();

			if (terrain == null)
			{
				Debug.LogError("找不到地形!");
				return;
			}
			string sceneName = SceneManager.GetActiveScene().name;
			if (!Directory.Exists(BaseFolder))
				Directory.CreateDirectory(BaseFolder);
			string sceneDir = $"{BaseFolder}/{sceneName}/";
			if (!Directory.Exists(sceneDir))
				Directory.CreateDirectory(sceneDir);
			string platformDir = string.Empty;
#if UNITY_ANDROID
			platformDir = $"{sceneDir}/Android/";
#elif UNITY_IOS
			platformDir = $"{sceneDir}/IOS/";
else
			platformDir = $"{sceneDir}/PC/";
#endif
			if (Directory.Exists(platformDir))
				Directory.Delete(platformDir, true);
			Directory.CreateDirectory(platformDir);

			TextureFolder = $"{platformDir}/Texture/";
			MaterialFolder = $"{platformDir}/Material/";
			MeshFolder = $"{platformDir}/Meshes/";
			PrefabFolder = $"{platformDir}/Prefab/";
			System.IO.Directory.CreateDirectory(TextureFolder);
			System.IO.Directory.CreateDirectory(MaterialFolder);
			System.IO.Directory.CreateDirectory(MeshFolder);
			System.IO.Directory.CreateDirectory(PrefabFolder);

			if (SlicingSize == 1)
			{
				//生成贴图和材质
				Vector4[] tilling = ExportTexAndMat(terrain.terrainData, terrain.name, 0, 0);
				//生成Mesh和prefab
				GameObject terrainGo = ExportObjAndPrefab(terrain.terrainData, terrain.name, 0, 0, tilling);
				//生成detail信息
				//ExportDetails(terrain.terrainData, terrainGo);
				EditorUtility.UnloadUnusedAssetsImmediate();
			}
			else if (SlicingSize > 1)
			{
				Debug.LogError("暂时不支持");
				//SlicingAndExport();
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
			ScriptableWizard.DisplayWizard<ExportTerrain>("切割Terrain为Mesh", "取消", "切割");
		}


		//开始分割地形
		//[MenuItem("Terrain/Slicing")]
		private static void SlicingAndExport()
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
					//生成贴图和材质
					ExportTexAndMat(newData, assetName, x, y);
					//UpdateProgress();
					////生成Mesh和prefab
					//GameObject terrainGo = ExportObjAndPrefab(newData, assetName, x, y);
					//UpdateProgress();
					////生成detail信息
					//ExportDetails(newData, terrainGo);
					//UpdateProgress();
				}
			}

			EditorUtility.ClearProgressBar();
		}

		private static int counter;
		private static int tCount;
		private static int totalCount;
		private static float progressUpdateInterval = 10000;
		private static void UpdateProgress()
		{
			//if (counter++ == progressUpdateInterval)
			//{
			//	counter = 0;
			//	EditorUtility.DisplayProgressBar("Generate...", "", Mathf.InverseLerp(0, totalCount, ++tCount));
			//}
		}
	}
}