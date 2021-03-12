// ========================================================
// des：
// author: 
// time：2021-03-04 16:32:20
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static ShaderGUIUtil;
using static UnityEditor.Rendering.Universal.ShaderGUI.LitGUI;

namespace Game.Editor
{
	public class SceneGUI : ShaderGUI
	{
		MaterialProperty blendMode = null;
		MaterialProperty cullMode = null;
		MaterialProperty albedoMap = null;
		MaterialProperty albedoColor = null;
		MaterialProperty alphaCutoff = null;

		MaterialProperty shininess = null;
		MaterialProperty specularColor = null;
		MaterialProperty metallicMap = null;
		MaterialProperty metallic = null;
		MaterialProperty smoothness = null;
		MaterialProperty bumpScale = null;
		MaterialProperty bumpMap = null;
		MaterialProperty occlusionStrength = null;
		MaterialProperty emissionColorForRendering = null;
		MaterialProperty emissionMap = null;

		public void FindProperties(MaterialProperty[] props)
		{
			blendMode = FindProperty("_Mode", props);
			cullMode = FindProperty("_Cull", props);
			albedoMap = FindProperty("_BaseMap", props);
			albedoColor = FindProperty("_BaseColor", props);
			alphaCutoff = FindProperty("_Cutoff", props);
			//bump
			bumpScale = FindProperty("_BumpScale", props);
			bumpMap = FindProperty("_BumpMap", props);
			//spec
			shininess = FindProperty("_Shininess", props, false);
			specularColor = FindProperty("_SpecColor", props, false);
			//pbr
			metallicMap = FindProperty("_MetallicGlossMap", props, false);
			metallic = FindProperty("_Metallic", props, false);
			smoothness = FindProperty("_Smoothness", props);
			//emission
			emissionColorForRendering = FindProperty("_EmissionColor", props);
			emissionMap = FindProperty("_EmissionMap", props);

			//occlusionStrength = FindProperty("_OcclusionStrength", props);
		}

		public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
		{
			FindProperties(props);
			Material material = materialEditor.target as Material;
			EditorGUIUtility.labelWidth = 0f;

			EditorGUI.BeginChangeCheck();
			{
				var lod = ShaderGUIUtil.DrawLODLevelPopup(materialEditor, material, ShaderGUIHelper.LodNames);
				var blend = ShaderGUIUtil.DrawBlendModePopup(material, materialEditor, blendMode);
				if (blend == ShaderGUIUtil.BlendMode.Cutout)
					materialEditor.ShaderProperty(alphaCutoff, alphaCutoff.displayName, 1);
				materialEditor.ShaderProperty(cullMode, "Cull Mode");

				EditorGUILayout.LabelField("主贴图");
				materialEditor.TexturePropertySingleLine(new GUIContent("Albedo Map (RGB)"), albedoMap, albedoColor);
				materialEditor.TextureScaleOffsetProperty(albedoMap);

				if (lod == ShaderLOD.High || (lod == ShaderLOD.None && Shader.globalMaximumLOD >= 300))
				{
					materialEditor.TexturePropertySingleLine(new GUIContent("Metallic(R), AlphaTest(G), (B未定), Smooth(A)"), metallicMap);
					if (metallicMap.textureValue == null)
					{
						material.DisableKeyword("_METALLICGLOSSMAP");
						materialEditor.RangeProperty(metallic, "金属度");
					}
					else
					{
						material.EnableKeyword("_METALLICGLOSSMAP");
					}
					materialEditor.RangeProperty(smoothness, "光滑度");
				}

				if (lod == ShaderLOD.Low || lod == ShaderLOD.Middle || (lod == ShaderLOD.None && Shader.globalMaximumLOD <= 200))
				{
					materialEditor.RangeProperty(shininess, "高光强度");
					materialEditor.ColorProperty(specularColor, "高光颜色");
				}

				if (lod == ShaderLOD.Middle || lod == ShaderLOD.High || (lod == ShaderLOD.None && Shader.globalMaximumLOD >= 200))
				{
					materialEditor.TexturePropertySingleLine(new GUIContent("法线贴图"), bumpMap);
					//materialEditor.TextureScaleOffsetProperty(bumpMap);
					if (bumpMap.textureValue != null)
					{
						material.EnableKeyword("_NORMALMAP");
						materialEditor.ShaderProperty(bumpScale, "法线强度", 1);
					}
					else
						material.DisableKeyword("_NORMALMAP");
				}

				materialEditor.TexturePropertySingleLine(new GUIContent("自发光贴图"), emissionMap, emissionColorForRendering);
				if (emissionMap.textureValue != null)
					material.EnableKeyword("_EMISSION");
				else
					material.DisableKeyword("_EMISSION");

				materialEditor.RenderQueueField();
			}
			if (EditorGUI.EndChangeCheck()) { }

			materialEditor.EnableInstancingField();
			materialEditor.DoubleSidedGIField();
		}

	}
}
