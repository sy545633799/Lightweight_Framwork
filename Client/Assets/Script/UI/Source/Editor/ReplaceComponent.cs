// ========================================================
// des：
// author: 
// time：2020-09-19 17:36:17
// version：1.0
// ========================================================

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection;
using static UnityEngine.UI.Image;

namespace Game.Editor
{

	public class ReplaceTextComponent
	{

		[ExecuteInEditMode]
		[MenuItem("Tools/UI/Replace/替换所有Image为UIImage")]
		private static void RecordPointAddFlame()
		{
			
			string[] ids = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets/AssetRes/UI" });
			for (int i = 0; i < ids.Length; i++)
			{
				string path = AssetDatabase.GUIDToAssetPath(ids[i]);
				
				GameObject originTwoCube = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
				string assetPath = AssetDatabase.GetAssetPath(originTwoCube);

				GameObject newGo = GameObject.Instantiate(originTwoCube);
				Image[] images = newGo.GetComponentsInChildren<Image>(true);
				for (int j = images.Length - 1; j >= 0 ; j--)
				{
					Image image = images[j];
					GameObject go = image.gameObject;
					Sprite sprite = image.sprite;
					Color color = image.color;
					Material material = image.material;
					bool raycast = image.raycastTarget;
					FillMethod fillMethod = image.fillMethod;
					Image.Type type = image.type;

					GameObject.DestroyImmediate(image);
					UIImage uIImage = go.AddComponent<UIImage>();
					uIImage.sprite = sprite;
					uIImage.color = color;
					uIImage.material = material;
					uIImage.raycastTarget = raycast;
					uIImage.fillMethod = fillMethod;
					uIImage.type = type;
				}
				if (images.Length > 0)
				{
					PrefabUtility.SaveAsPrefabAsset(newGo, assetPath);
				}
				
				GameObject.DestroyImmediate(newGo);

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

			}
		}

		[ExecuteInEditMode]
		[MenuItem("Tools/UI/Replace/替换所有Image为UISlider")]
		private static void ReplceUISlider()
		{

			string[] ids = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets/AssetRes/UI" });
			for (int i = 0; i < ids.Length; i++)
			{
				string path = AssetDatabase.GUIDToAssetPath(ids[i]);

				GameObject originTwoCube = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
				string assetPath = AssetDatabase.GetAssetPath(originTwoCube);

				GameObject newGo = GameObject.Instantiate(originTwoCube);
				Slider[] sliders = newGo.GetComponentsInChildren<Slider>(true);
				for (int j = sliders.Length - 1; j >= 0; j--)
				{
					Slider slider = sliders[j];
					GameObject go = slider.gameObject;
					UIImage image = slider.image as UIImage;
					RectTransform fill = slider.fillRect;
					RectTransform handle = slider.handleRect;
					GameObject.DestroyImmediate(slider);
					UISlider uISlider = go.AddComponent<UISlider>();
					uISlider.image = image;
					uISlider.fillRect = fill;
					uISlider.handleRect = handle;
				}
				if (sliders.Length > 0)
				{
					PrefabUtility.SaveAsPrefabAsset(newGo, assetPath);
				}

				GameObject.DestroyImmediate(newGo);

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

			}
		}
	}
}
