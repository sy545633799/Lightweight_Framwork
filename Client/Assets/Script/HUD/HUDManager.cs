// ========================================================
// des：
// author: shenyi
// time：2020-12-21 17:54:44
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;

namespace Game {
	public class HUDManager
	{
		public static CommandBuffer Command { get; private set; }

		public static Camera MainCamera;
		private static HUDConfigAsset m_HUDConfig;
		private static Dictionary<string, HUDNumberAnimation> m_HUDNumberAnimationAttributes = new Dictionary<string, HUDNumberAnimation>();
		private static Dictionary<HUDNumberType, HUDNumberAttributes> m_HUDNumberAttributes = new Dictionary<HUDNumberType, HUDNumberAttributes>();
		public static RecyclePool<HUDBaseRender> BloodPool = new RecyclePool<HUDBaseRender>(() => new HUDBloodRender());

		private static Mesh m_mesh;
		private static Material m_material;
		private static CombineInstance[] m_combineInstances;
		private static Matrix4x4 m_cameraRotationMatrix;

		private	static List<HUDBaseRender> m_disposeList = new List<HUDBaseRender>();
		private	static List<HUDBaseRender> m_staticRenderList = new List<HUDBaseRender>();
		private	static List<HUDBaseRender> m_dynamicRenderList = new List<HUDBaseRender>();

		public static void Init()
		{
			MainCamera = Camera.main;
			Command = new CommandBuffer();
			m_HUDConfig = HUDConfigAsset.Get();
			List<HUDNumberAnimation> list = HUDConfigAsset.Get().HUDNumberAnimationAttributesList;
			for (int i = 0; i < list.Count; i++)
				m_HUDNumberAnimationAttributes.Add(list[i].Name, list[i]);
			List<HUDNumberAttributes> list2 = HUDConfigAsset.Get().HUDNumberAttributesList;
			for (int i = 0; i < list2.Count; i++)
				m_HUDNumberAttributes.Add(list2[i].Type, list2[i]);

			m_material = new Material(Shader.Find("UI/UIHUD"));
			m_material.SetInt("unity_GUIZTestMode", (int)CompareFunction.Always);
			MainCamera.AddCommandBuffer(CameraEvent.AfterImageEffects, Command);
			m_mesh = new Mesh();
		}

		public static Sprite GetSpritesByName(string name) => m_HUDConfig.Atlas.GetSprite(name);

		public static void AddHUDRender(HUDBaseRender render)
		{
			if (render is HUDBloodRender)
				m_staticRenderList.Add(render);
		}

		

		public static void Update()
		{
			Command.Clear();
			if (m_staticRenderList.Count > 0 || m_dynamicRenderList.Count > 0)
			{
				//DepthSort(ref m_staticRenderList);
				//DepthSort(ref m_dynamicRenderList);
				for (int i = 0; i < m_staticRenderList.Count; i++)
				{
					Texture _texture = m_staticRenderList.Count <= 0 ? m_dynamicRenderList[0].HUDSprite.texture : m_staticRenderList[0].HUDSprite.texture;
					m_material.SetTexture("_MainTex", _texture);
					Command.DrawMesh(m_staticRenderList[i].Merge(), Matrix4x4.identity, m_material);
				}

				if (m_disposeList.Count > 0)
				{
					for (int i = 0; i < m_disposeList.Count; i++)
					{
						// 暂时只有动态列表里有需要被计时释放的网格
						m_dynamicRenderList.Remove(m_disposeList[i]);
						m_disposeList[i].Recycle();
					}
					m_disposeList.Clear();
				}
			}
		}

		#region 合并网格和排序
		public static void DepthSort(ref List<HUDBaseRender> list)
		{
			for (int i = 0; i < list.Count - 1; i++)
			{
				for (int j = 0; j < list.Count - 1 - i; j++)
				{
					if (list[j].depth < list[j + 1].depth)
					{
						HUDBaseRender temp = list[j];
						list[j] = list[j + 1];
						list[j + 1] = temp;
					}
				}
			}
		}
		#endregion

		public static void Dispose()
		{
			MainCamera.RemoveCommandBuffer(CameraEvent.AfterImageEffects, Command);
		}

	}
}
