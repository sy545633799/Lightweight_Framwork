using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.Editor
{
    public class SpritePackerAuto : AssetPostprocessor
    {
        static private uint m_Version = 1;
        public override uint GetVersion() { return m_Version; }
        void OnPostprocessTexture(Texture2D texture)
        {
            string path = "Assets/Art/Picture";
            int pos = path.LastIndexOf("Assets");
            if (pos != -1)
                path = path.Substring(pos);
            if (assetPath.Contains(path))
            {
                string spritrName = "";
                spritrName = assetPath.Replace("\\", "/");
                int _position = spritrName.LastIndexOf(@"/");
                spritrName = spritrName.Substring(0, _position);
                spritrName = spritrName.Replace("/", "_");
                spritrName.ToUpper();
                TextureImporter textureImporter = assetImporter as TextureImporter;
                if (textureImporter.userData != GetVersion().ToString())
                {
                    textureImporter.textureType = TextureImporterType.Sprite;
                    textureImporter.spriteImportMode = UnityEditor.SpriteImportMode.Single;
                    textureImporter.spritePackingTag = spritrName;
                    textureImporter.mipmapEnabled = false;
                    textureImporter.spritePixelsPerUnit = 100;
                    textureImporter.userData = GetVersion().ToString();
                    textureImporter.alphaIsTransparency = textureImporter.DoesSourceTextureHaveAlpha();

                    TextureImporterPlatformSettings setting = textureImporter.GetDefaultPlatformTextureSettings();
                    setting.maxTextureSize = 1024;
                    textureImporter.SetPlatformTextureSettings(setting);
                    textureImporter.SaveAndReimport();
                    Debug.LogWarningFormat("[{0}]-->auto fix sprite and Named: {1}", assetPath, spritrName);
                }
                else
                    textureImporter.spritePackingTag = null;

                EditorUtility.SetDirty(textureImporter);

            }
        }
    }
}