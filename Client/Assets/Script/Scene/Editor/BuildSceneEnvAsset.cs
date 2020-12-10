// ========================================================
// des：
// author: 
// time：2020-12-10 14:46:38
// version：1.0
// ========================================================

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Editor {
	public class BuildSceneEnvAsset
	{
		public static string SceneInfoAssetPath = "Assets/Art/Scenes";

		[MenuItem("Tools/场景相关/导出场景环境信息", priority = 102)]
		public static void Build()
		{
			string sceneName = SceneManager.GetActiveScene().name;
			SceneEnvAsset holder = ScriptableObject.CreateInstance<SceneEnvAsset>();
			GameObject lightGo = GameObject.Find("Env/sun");
			if (lightGo != null)
			{
				Light light = lightGo.GetComponent<Light>();
				if (light)
				{
					holder.lightData = new LightData();
					holder.lightData.rotation = light.transform.eulerAngles;
					holder.lightData.position = light.transform.position;

					holder.lightData.color = light.color;
					holder.lightData.intensity = light.intensity;
					holder.lightData.bounceIntensity = light.bounceIntensity;
					holder.lightData.range = light.range;

					holder.lightData.shadows = light.shadows;
					holder.lightData.shadowStrength = light.shadowStrength;
					holder.lightData.shadowResolution = light.shadowResolution;
					holder.lightData.shadowNormalBias = light.shadowNormalBias;
					holder.lightData.shadowNearPlane = light.shadowNearPlane;

					holder.lightData.renderMode = light.renderMode;
					holder.lightData.cullingMask = light.cullingMask;
				}
				else
					Debug.LogError("light_zhu上没有设置灯光");
			}
			else
				Debug.LogError("没有找到light_zhu!!!!!");

			//光照贴图
			int iCount = LightmapSettings.lightmaps.Length;
			holder.lightmapColor = new Texture2D[iCount];
			holder.lightmapDir = new Texture2D[iCount];
			holder.shadowMask = new Texture2D[iCount];
			for (int i = 0; i < iCount; ++i)
			{
				// 这里把直接把lightmap纹理存起来
				holder.lightmapColor[i] = LightmapSettings.lightmaps[i].lightmapColor;
				holder.lightmapDir[i] = LightmapSettings.lightmaps[i].lightmapDir;
				holder.shadowMask[i] = LightmapSettings.lightmaps[i].shadowMask;
			}
			GameObject cameraGo = GameObject.Find("Env/camera");
			if (cameraGo)
			{
				Camera camera = cameraGo.GetComponent<Camera>();
				if (camera)
				{
					holder.CameraInfo.Depth = camera.depth;
					holder.CameraInfo.FieldOfView = camera.fieldOfView;
					holder.CameraInfo.Position = camera.transform.position;
					holder.CameraInfo.Rotation = camera.transform.eulerAngles;
					CameraController defaultCameraController = camera.GetComponent<CameraController>();
					if (defaultCameraController)
					{
						holder.CameraInfo.PivotOffset = defaultCameraController.pivotOffset;
						holder.CameraInfo.FixDistance = defaultCameraController.fixDistance;
						holder.CameraInfo.MinDistance = defaultCameraController.minDistance;
						holder.CameraInfo.MaxDistance = defaultCameraController.maxDistance;
						holder.CameraInfo.Distance = defaultCameraController.distance;
						holder.CameraInfo.HidePlayerDistance = defaultCameraController.HidePlayerDistance;
						holder.CameraInfo.AllowXTilt = defaultCameraController.allowXTilt;
						holder.CameraInfo.AllowYTilt = defaultCameraController.allowYTilt;
						holder.CameraInfo.YMinLimit = defaultCameraController.yMinLimit;
						holder.CameraInfo.YMaxLimit = defaultCameraController.yMaxLimit;
						holder.CameraInfo.XMinLimit = defaultCameraController.xMinLimit;
						holder.CameraInfo.XMaxLimit = defaultCameraController.xMaxLimit;
						holder.CameraInfo.XSpeed = defaultCameraController.xSpeed;
						holder.CameraInfo.YSpeed = defaultCameraController.ySpeed;
						holder.CameraInfo.TargetX = defaultCameraController.targetX;
						holder.CameraInfo.TargetY = defaultCameraController.targetY;
						holder.CameraInfo.MinY = defaultCameraController.minY;
					}
				}
			}



			//环境光
			holder.ambientMode = RenderSettings.ambientMode;
			holder.ambientIntensity = RenderSettings.ambientIntensity;
			holder.ambientLight = RenderSettings.ambientLight;
			holder.ambientSkyColor = RenderSettings.ambientSkyColor;
			holder.ambientGroundColor = RenderSettings.ambientGroundColor;
			holder.ambientEquatorColor = RenderSettings.ambientEquatorColor;
			//环境反射
			holder.skyBox = RenderSettings.skybox;
			holder.reflectionMode = RenderSettings.defaultReflectionMode;
			holder.customReflection = RenderSettings.customReflection;
			holder.reflectionIntensity = RenderSettings.reflectionIntensity;
			holder.reflectionBounces = RenderSettings.reflectionBounces;
			//雾效
			holder.fog = RenderSettings.fog;
			holder.fogColor = RenderSettings.fogColor;
			holder.fogMode = RenderSettings.fogMode;
			holder.fogStartDistance = RenderSettings.fogStartDistance;
			holder.fogEndDistance = RenderSettings.fogEndDistance;
			holder.haloStrength = RenderSettings.haloStrength;
			holder.flareFadeSpeed = RenderSettings.flareFadeSpeed;
			holder.flareStrength = RenderSettings.flareStrength;

			string sceneInfoPath = Path.Combine(SceneInfoAssetPath, $"{sceneName}/{sceneName}");
			if (!Directory.Exists(sceneInfoPath))
				Directory.CreateDirectory(sceneInfoPath);
			AssetDatabase.CreateAsset(holder, Path.Combine(sceneInfoPath, "sceneenv.asset"));
			AssetDatabase.Refresh();
		}

	}
}
