// ========================================================
// des：
// author: shenyi
// time：2020-07-01 09:35:28
// version：1.0
// ========================================================

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System;

namespace Game
{

	[System.Serializable]
	public struct PrefabInfo
	{
		public Vector3 Position;//坐标
		public Vector3 Rotation;//旋转
		public Vector3 Scale;//缩放
	}

	[System.Serializable]
	public struct CameraInfo
	{
		///* Camera 参数 */
		public Vector3 Position;                  // 位置
		public Vector3 Rotation;                  // 旋转  
        public float Depth;                       // 层级
		public float FieldOfView;                 // 角度
		/* CameraController 参数 */
		public Vector3 PivotOffset;             // 焦点偏移
		public bool FixDistance;                // 是否固定距离
		public float MinDistance;               // 相机最小距离
		public float MaxDistance;               // 相机最大距离
		public float Distance;                  // 相机人物实际距离
		public float HidePlayerDistance;        // 隐藏人物距离
		public bool AllowXTilt;                 // 是否允许X轴旋转
		public bool AllowYTilt;                 // 是否允许Y轴旋转
		public float YMinLimit;                 // Y轴最小角度限制
		public float YMaxLimit;                 // Y轴最大角度限制
		public float XMinLimit;                 // x轴最小角度限制
		public float XMaxLimit;                 // x轴最大角度限制
		public float XSpeed;                    // X轴速度
		public float YSpeed;                    // Y轴速度
		public float TargetX;                   // 相机X轴角度
		public float TargetY;                   // 相机Y轴角度
		public float MinY;                      // 相机最小高度(防止看到地下)

	}

    //public struct 

	[System.Serializable]
    public struct LightData
    {
        public Vector3 position;
        public Vector3 rotation;

        public Color color;
        public float intensity;
        public float bounceIntensity;
        public float range;

        public LightShadows shadows;
        public float shadowStrength;
        public LightShadowResolution shadowResolution;
        public float shadowNormalBias;
        public float shadowNearPlane;

        public LightRenderMode renderMode;
        public int cullingMask;
    }

	[System.Serializable]
	public struct PreloadElement
	{
        public int ResId;//路径
        public Vector3 position;//坐标
		public Vector3 rotation;//旋转
		public Vector3 scale;//缩放
	}

	[System.Serializable]
	public struct GrassGroup
	{
		public int eRes;
        public Mesh mesh;
        public Material mat;
		public Matrix4x4[] matrixList;
	}

	[System.Serializable]
    public struct SceneElement
    {
		[SerializeField]
		public int ID; //ID
        public int UID
        {
            get { return ID; }
        }
        public int order;//加载优先级
        public int ResId;//路径
        public Vector3 position;//坐标
        public Vector3 rotation;//旋转
        public Vector3 scale;//缩放
        public List<int> lightmapIndex;//光照索引
        public List<Vector4> lightmapOffset;//光照偏移
        
        [SerializeField]
        private Bounds m_Bounds;//物体包围盒
        public Bounds Bounds
        {
            get { return m_Bounds; }
            set { m_Bounds = value; }
        }
    }

    //场景总资源
    public class SceneElementAsset : ScriptableObject
    {
        public string SceneName;
        public Bounds Bounds;                   //总包围盒大小
        /// <summary>
        /// 总资源列表
        /// </summary>
        public List<PreloadElement> preload;
		/// <summary>
		/// 草地资源
		/// </summary>
		public List<GrassGroup> grass;
		/// <summary>
		/// 总资源列表
		/// </summary>
		public List<SceneElement> content;
        /// <summary>
        /// 资源路径列表
        /// </summary>
        public List<string> pathList;

        public void Add(SceneElement element)
        {
            if (content == null)
                content = new List<SceneElement>();
            content.Add(element);
        }

        public void Add(PreloadElement element)
        {
            if (preload == null)
                preload = new List<PreloadElement>();
            preload.Add(element);
        }

		public void Add(GrassGroup element)
		{
			if (grass == null)
                grass = new List<GrassGroup>();
            grass.Add(element);
		}

		public int GetPathIndex(string path)
        {
            if (pathList == null)
            {
                pathList = new List<string>();
            }
            if (!pathList.Contains(path))
            {
                pathList.Add(path);
            }
            return pathList.IndexOf(path);
        }
    }
}