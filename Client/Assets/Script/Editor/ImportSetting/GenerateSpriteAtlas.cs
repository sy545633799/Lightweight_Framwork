// ========================================================
// des：
// author: 
// time：2020-07-16 20:40:24
// version：1.0
// ========================================================

using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using Game.Editor;
using System.Collections.Generic;
using System.Text;

namespace Game.Editor
{
    public class AutoSetAtlasContent : UnityEditor.Editor
    {
		private const string AtlasCongigPath = "Assets/Art/Assets/Config/AtlasConfig.asset";
		private const string LuaAtlasConfigPath = "Lua/Framework/UI/AtlasConfig.lua";
		private const string OprPath = "Assets/Art/Picture/";//操作文件夹
        private const string TarPath = "Assets/Art/Atlas/";//目标文件夹
        //private const string BigTexPath = "Assets/AssetRes/BackUITexture/";//大图文件夹


        [MenuItem("Tools/UI/AutoSetAtlas", false, 2)]
        static public void AutoSetAtlasContents()
        {
            DirectoryInfo TarDirInfo = new DirectoryInfo(TarPath);
            var allFiles = TarDirInfo.GetFiles("*.spriteatlas", SearchOption.AllDirectories);
            foreach (FileInfo nFile in allFiles)
            {
                File.Delete(nFile.FullName);
            }

			DirectoryInfo rootDirInfo = new DirectoryInfo(OprPath);
			DirectoryInfo[] dirs = rootDirInfo.GetDirectories();
			
			AtlasConfigAsset configAsset;
			List<AtlasConfig> configs;
			if (File.Exists(AtlasCongigPath))
			{
				configAsset = AssetDatabase.LoadAssetAtPath<AtlasConfigAsset>(AtlasCongigPath);
				configs = new List<AtlasConfig>(configAsset.Configs);
			}
			else
			{
				configAsset = new AtlasConfigAsset();
				configs = new List<AtlasConfig>();
			}

			int time = TimeUtility.GetTimeStampEx();
            foreach (DirectoryInfo dirInfo in dirs)
            {
				GeneratePerAtlas(dirInfo);
				if (configs.Find(p => p.Name == dirInfo.Name) == null)
				{
					AtlasConfig config = new AtlasConfig();
					config.ID = time++;
					config.Name = dirInfo.Name;
					configs.Add(config);
				}
			}
			configAsset.Configs = configs.ToArray();
			if (File.Exists(AtlasCongigPath))
				EditorUtility.SetDirty(configAsset);
			else
				AssetDatabase.CreateAsset(configAsset, AtlasCongigPath);

			StringBuilder sb = new StringBuilder();
			sb.AppendLine("AtlasConfig =");
			sb.AppendLine("{");
			for (int i = 0; i < configs.Count; i++)
			{
				sb.AppendLine($"\t{configs[i].Name} = {configs[i].ID},");
			}
			sb.AppendLine("}");
			if (!File.Exists(LuaAtlasConfigPath))
				//File.Delete(LuaAtlasConfigPath);
				File.Create(LuaAtlasConfigPath);
			File.WriteAllText(LuaAtlasConfigPath, sb.ToString());

			//DirectoryInfo BigTexDirInfo = new DirectoryInfo(BigTexPath);
			//var allBigTexs = BigTexDirInfo.GetFiles("*.png", SearchOption.AllDirectories);
			//foreach (FileInfo pngFile in allBigTexs)
			//{
			//    string curName = pngFile.Name;
			//    int index = curName.IndexOf(".png");
			//    string ReName = "BigTex_" + curName.Substring(0, index);

			//    var FileName = TarPath + ReName;
			//    if (Directory.Exists(FileName))
			//    {
			//        Directory.Delete(FileName, true);
			//    }
			//    Directory.CreateDirectory(FileName);
			//    var AtlasName = FileName + "/" + ReName + ".spriteatlas";
			//    SpriteAtlas atlas = new SpriteAtlas();
			//    AssetDatabase.CreateAsset(atlas, AtlasName);
			//    AtlasSetting(atlas);
			//    string str = pngFile.FullName.Substring(pngFile.FullName.IndexOf("Assets"));
			//    atlas.Add(new[] { AssetDatabase.LoadAssetAtPath<Sprite>(str) });
			//    AssetDatabase.SaveAssets();
			//}
		}

        static void GeneratePerAtlas(DirectoryInfo dirInfo)
        {
            var DirName = TarPath + dirInfo.Name;
			if (!Directory.Exists(DirName))
				Directory.CreateDirectory(DirName);
			var AtlasName = DirName + "/" + dirInfo.Name + ".spriteatlas";
			if (File.Exists(AtlasName))
				Directory.Delete(AtlasName, true);

			SpriteAtlas atlas = new SpriteAtlas();
            AssetDatabase.CreateAsset(atlas, AtlasName);
            AtlasSetting(atlas);

            var allFiles = dirInfo.GetFiles("*.png", SearchOption.AllDirectories);
            foreach (FileInfo pngFile in allFiles)
            {
                string str = pngFile.FullName.Substring(pngFile.FullName.IndexOf("Assets"));
                atlas.Add(new[] { AssetDatabase.LoadAssetAtPath<Sprite>(str) });
            }
            //添加文件夹
            //Object obj = AssetDatabase.LoadAssetAtPath(OprPath + dirInfo.Name, typeof(Object));
            //atlas.Add(new[] { obj });

            AssetDatabase.SaveAssets();
        }

        // 设置参数
        static void AtlasSetting(SpriteAtlas atlas)
        {
            atlas.SetIncludeInBuild(false);
            SpriteAtlasPackingSettings packSetting = new SpriteAtlasPackingSettings()
            {
                blockOffset = 2,
                enableRotation = false,
                enableTightPacking = false,
                padding = 2,
            };
            atlas.SetPackingSettings(packSetting);

            SpriteAtlasTextureSettings textureSetting = new SpriteAtlasTextureSettings()
            {
                readable = false,
                generateMipMaps = false,
                sRGB = true,
                filterMode = FilterMode.Bilinear,
            };
            atlas.SetTextureSettings(textureSetting);

            TextureImporterPlatformSettings platformSetting = new TextureImporterPlatformSettings()
            {
                maxTextureSize = DefineSpritePacker.kDefaultMaxSprite,
                format = TextureImporterFormat.Automatic,
                textureCompression = TextureImporterCompression.Compressed,
                crunchedCompression = true,
                compressionQuality = DefineSpritePacker.kDefaultQuality,
            };
            atlas.SetPlatformSettings(platformSetting);
        }
    }
}