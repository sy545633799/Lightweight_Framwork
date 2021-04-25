using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Linq;
namespace Game.Editor
{
	public partial class ExportTerrain : ScriptableWizard
	{
		private enum TextureChannel
		{
			R = 0,
			G,
			B,
			A
		}

		private enum ExportType
		{
			Default,
			DiffuseAndNormal,
			IndexAndControl,
		}

		private enum TextureArrayType
		{
			Diffuse,
			Normal,
		}

		private class IndexAndControl
		{
			public int Index;
			public float TexIndex;
			public float TexBlend;
		}
		private static int DefaultSize = 1024;
		private static string DiffusePath = null;
		private static string NormalPath = null;
		private static string IndexPath = null;
		private static string ControlPath = null;
		private static string MaterialPath = null;
		private static string TextureTempDir = "Assets/TextureTemplate/";

		public static Vector4[] ExportTexAndMat(TerrainData data, string assetName, int x1, int y1)
		{
			DiffusePath = $"{TextureFolder}/TerrainTexture_Diffuse.asset";
			NormalPath = $"{TextureFolder}TerrainTexture_Normal.asset";
			IndexPath = $"{TextureFolder}TerrainTexture_Index.png";
			ControlPath = $"{TextureFolder}TerrainTexture_Control.png";
			MaterialPath = $"{MaterialFolder}{assetName}.mat";

			Material Tmaterial = null;
			//if (data.alphamapTextures.Length == 1)
			//{
			//Tmaterial = new Material(Shader.Find("Cartoon/Ulit/Terrain"));
			//string texPath = $"{PrefabFolder}Terrains/Texture/{assetName}_0.png";
			//Texture test = (Texture)AssetDatabase.LoadAssetAtPath(texPath, typeof(Texture));
			//Tmaterial.SetTexture($"_Control", test);
			//float scale = 1 / (float)SlicingSize;
			//Tmaterial.SetTextureScale($"_Control", new Vector2(scale, scale));
			//Tmaterial.SetTextureOffset($"_Control", new Vector2(scale * x1, scale * y1));
			//Tmaterial.SetTexture("_Splat" + (e), texts[e].diffuseTexture);
			//Tmaterial.SetTextureScale("_Splat" + (e), newTilling);
			//Tmaterial.SetTexture("_Normal" + (e), texts[e].normalMapTexture);
			//Tmaterial.SetTextureScale("_Normal" + (e), newTilling);

			//}
			//else
			//{

				if (Directory.Exists(TextureTempDir))
					Directory.Delete(TextureTempDir, true);
				Directory.CreateDirectory(TextureTempDir);

				ExportDiffuseAndNormalMap(data);
				ExportIndexAndControlMap(data);
				float scale = 1 / (float)SlicingSize;
				Tmaterial = new Material(Shader.Find("Cartoon/Ulit/Terrain-Advance"));
				Texture controlTex = (Texture)AssetDatabase.LoadAssetAtPath(ControlPath, typeof(Texture));
				Tmaterial.SetTexture("_Control", controlTex);
				
				Tmaterial.SetTextureScale("_Control", new Vector2(scale, scale));
				Tmaterial.SetTextureOffset("_Control", new Vector2(scale * x1, scale * y1));
				Texture indexTex = (Texture)AssetDatabase.LoadAssetAtPath(IndexPath, typeof(Texture));
				Tmaterial.SetTexture("_Index", indexTex);
				Tmaterial.SetTextureScale("_Index", new Vector2(scale, scale));
				Tmaterial.SetTextureOffset("_Index", new Vector2(scale * x1, scale * y1));
				Texture2DArray diffuseArray = (Texture2DArray)AssetDatabase.LoadAssetAtPath(DiffusePath, typeof(Texture2DArray));
				Tmaterial.SetTexture("_Diffuse", diffuseArray);
				Texture2DArray normalArray = (Texture2DArray)AssetDatabase.LoadAssetAtPath(NormalPath, typeof(Texture2DArray));
				Tmaterial.SetTexture("_Normal", normalArray);

				Vector4[] uvs = new Vector4[8];
				TerrainLayer[] texts = data.terrainLayers;
				for (int e = 0; e < 8; e++)
				{
					Vector4 uv;
					if (e < texts.Length)
					{
						Vector2 tilling = texts[e].tileSize;
						Vector2 offset = texts[e].tileOffset;
						uv = new Vector4((float)Resolution / (SlicingSize * tilling.x), (float)Resolution /  (SlicingSize * tilling.y), offset.x, offset.y);

					}
					else
						uv = new Vector4(1, 1, 0, 0);
					uvs[e] = uv;
				}
				Tmaterial.SetVectorArray("_UV_STArray", uvs);
				Tmaterial.SetInt("_MaxSubTexCount", texts.Length);
				Directory.Delete(TextureTempDir, true);
			//}



			AssetDatabase.CreateAsset(Tmaterial, MaterialPath);
			AssetDatabase.ImportAsset(MaterialPath, ImportAssetOptions.ForceUpdate);
			AssetDatabase.Refresh();

			return uvs;
		}

		private static void OnCreateTextureArray(List<Texture2D> textureList, string path, TextureArrayType textureType)
		{

			Texture2DArray textureArray = textureArray = new Texture2DArray(DefaultSize, DefaultSize, textureList.Count, TextureFormat.ETC2_RGB, true)
			{

				anisoLevel = textureList[0].anisoLevel,
				filterMode = textureList[0].filterMode,
				wrapMode = textureList[0].wrapMode
			};

			for (int i = 0; i < textureList.Count; i++)
			{
				Texture2D dstTexture = new Texture2D(DefaultSize, DefaultSize, TextureFormat.RGB24, true);
				int width = textureList[i].width;
				int height = textureList[i].height;
				for (int j = 0; j < DefaultSize; j++)
				{
					for (int k = 0; k < DefaultSize; k++)
					{
						int x = (int)((float)j * (float)width / DefaultSize);
						int y = (int)((float)k * (float)height / DefaultSize);
						dstTexture.SetPixel(j, k, textureList[i].GetPixel(x, y));
					}
				}

				//TODO试验直接压缩的方法
				//EditorUtility.CompressTexture(dstTexture, TextureFormat.ETC_RGB4, TextureCompressionQuality.Normal);
				//这种情况下只能在android和ios平台各打一份
				{
					byte[] bytes = dstTexture.EncodeToPNG();
					string newPath = $"{TextureTempDir}{textureType.ToString()}{i}.png";
					File.WriteAllBytes(newPath, bytes);
					AssetDatabase.ImportAsset(newPath);
					AssetDatabase.Refresh();
					TextureImporter import = AssetImporter.GetAtPath(newPath) as TextureImporter;
					var setting = import.GetPlatformTextureSettings("Android");
					setting.format = TextureImporterFormat.ETC2_RGB4;
					setting.overridden = true;
					setting.compressionQuality = 2;
					setting.allowsAlphaSplitting = false;
					setting.maxTextureSize = DefaultSize;
					import.SetPlatformTextureSettings(setting);
					setting = import.GetPlatformTextureSettings("iPhone");
					setting.format = TextureImporterFormat.PVRTC_RGB4;
					setting.overridden = true;
					setting.compressionQuality = 2;
					setting.allowsAlphaSplitting = false;
					setting.maxTextureSize = DefaultSize;
					import.SetPlatformTextureSettings(setting);
					//setting = import.GetPlatformTextureSettings("Standalone");
					//setting.format = TextureImporterFormat.RGB24;
					//setting.overridden = true;
					//setting.compressionQuality = 3;
					//setting.allowsAlphaSplitting = false;
					//setting.maxTextureSize = 2048;
					//import.SetPlatformTextureSettings(setting);
					import.SaveAndReimport();

					dstTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(newPath);
				}
				
				for (int m = 0; m < dstTexture.mipmapCount; m++)
					Graphics.CopyTexture(dstTexture, 0, m, textureArray, i, m);
			}

			var existing = AssetDatabase.LoadAssetAtPath<Texture2DArray>(path);
			if (existing != null)
			{
				EditorUtility.CopySerialized(textureArray, existing);
			}
			else
			{
				AssetDatabase.CreateAsset(textureArray, path);
			}

			AssetDatabase.ImportAsset(path);
			AssetDatabase.Refresh();
		}

		private static string GetSelectedPath()
		{
			string path = "Assets";
			foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
			{
				path = AssetDatabase.GetAssetPath(obj);
				if (!string.IsNullOrEmpty(path) && File.Exists(path))
				{
					path = Path.GetDirectoryName(path);
					break;
				}
			}
			return path;
		}

		private static void ExportDiffuseAndNormalMap(TerrainData terrainData)
		{
			TerrainLayer[] terrainLayerArray = terrainData.terrainLayers;

			List<Texture2D> diffuselist = new List<Texture2D>();
			List<Texture2D> normalMaplist = new List<Texture2D>();
			for (int i = 0; i < terrainLayerArray.Length; i++)
			{
				if (terrainLayerArray[i].diffuseTexture != null)
				{
					diffuselist.Add(terrainLayerArray[i].diffuseTexture);
				}
				if (terrainLayerArray[i].normalMapTexture)
				{
					normalMaplist.Add(terrainLayerArray[i].normalMapTexture);
				}
				else
				{
					Color color = new Color(0.5f, 0.5f, 1);
					Texture2D normalTexture = new Texture2D(diffuselist[i].width, diffuselist[i].height, TextureFormat.RGB24, false, true);
					for (int j = 0; j < normalTexture.width; j++)
					{
						for (int k = 0; k < normalTexture.height; k++)
						{
							normalTexture.SetPixel(k, j, color);
						}
					}
					normalMaplist.Add(normalTexture);
				}
			}

			if (diffuselist.Count <= 0)
			{
				Debug.LogError("The number of terrain splat must be greater than 0.");
				return;
			}

			OnCreateTextureArray(diffuselist, DiffusePath, TextureArrayType.Diffuse);
			if (diffuselist.Count != normalMaplist.Count)
				Debug.LogError("The number of diffuse and the number of normal are inconsistent.");
			else
				OnCreateTextureArray(normalMaplist, NormalPath, TextureArrayType.Normal);
		}

		private static void ExportIndexAndControlMap(TerrainData terrainData)
		{
			TerrainLayer[] terrainLayArray = terrainData.terrainLayers;
			Texture2D[] alphaMapArray = terrainData.alphamapTextures;
			int textureNum = terrainLayArray.Length;
			float maxTexCount = textureNum - 1;
			int witdh = alphaMapArray[0].width;
			int height = alphaMapArray[0].height;

			Texture2D indexTex = new Texture2D(witdh, height, TextureFormat.RGBA32, false, true);
			Texture2D blendTex = new Texture2D(witdh, height, TextureFormat.RGBA32, false, true);

			List<IndexAndControl> indexAndblendList = new List<IndexAndControl>(textureNum);
			for (int i = 0; i < textureNum; i++)
			{
				indexAndblendList.Add(new IndexAndControl());
			}

			for (int j = 0; j < witdh; j++)
			{
				for (int k = 0; k < height; k++)
				{
					
					for (int i = 0; i < textureNum; i++)
					{
						int controlMapNumber = i / 4;
						int controlChannelNum = i % 4;
						Color curColor = alphaMapArray[controlMapNumber].GetPixel(j, k);
						float index = (float)i / maxTexCount;

						switch ((TextureChannel)controlChannelNum)
						{
							case TextureChannel.R:
								indexAndblendList[i].Index = i;
								indexAndblendList[i].TexIndex = index;
								indexAndblendList[i].TexBlend = curColor.r;
								break;
							case TextureChannel.G:
								indexAndblendList[i].Index = i;
								indexAndblendList[i].TexIndex = index;
								indexAndblendList[i].TexBlend = curColor.g;
								break;
							case TextureChannel.B:
								indexAndblendList[i].Index = i;
								indexAndblendList[i].TexIndex = index;
								indexAndblendList[i].TexBlend = curColor.b;
								break;
							case TextureChannel.A:
								indexAndblendList[i].Index = i;
								indexAndblendList[i].TexIndex = index;
								indexAndblendList[i].TexBlend = curColor.a;
								break;
						}
					}

					//取当前像素内的混合值最大的4张贴图
					var indexAndblendPairs = from pair in indexAndblendList
											 orderby pair.TexBlend descending
											 select pair;
					var datas = indexAndblendPairs.Take(4).OrderBy(item => item.TexIndex);

					int colorChannel = 0;
					Color indexColor = Color.clear;
					Color blendColor = Color.clear;

					//保存这4张贴图的索引和权重值
					foreach (var indexAndblend in datas)
					{
						if (colorChannel < 4)
						{
							switch ((TextureChannel)colorChannel)
							{
								case TextureChannel.R:
									indexColor.r = indexAndblend.TexIndex;
									blendColor.r = indexAndblend.TexBlend;
									break;
								case TextureChannel.G:
									indexColor.g = indexAndblend.TexIndex;
									blendColor.g = indexAndblend.TexBlend;
									break;
								case TextureChannel.B:
									indexColor.b = indexAndblend.TexIndex;
									blendColor.b = indexAndblend.TexBlend;
									break;
								case TextureChannel.A:
									indexColor.a = indexAndblend.TexIndex;
									blendColor.a = indexAndblend.TexBlend;
									break;
							}
							colorChannel++;
						}
					}

					indexTex.SetPixel(j, k, indexColor);
					blendTex.SetPixel(j, k, blendColor);
				}
			}

			indexTex.Apply();
			byte[] bytes = indexTex.EncodeToPNG();
			File.WriteAllBytes(IndexPath, bytes);
			AssetDatabase.ImportAsset(IndexPath);

			blendTex.Apply();
			byte[] blendBytes = blendTex.EncodeToPNG();
			File.WriteAllBytes(ControlPath, blendBytes);
			AssetDatabase.ImportAsset(ControlPath);

			AssetDatabase.Refresh();
		}

	}
}