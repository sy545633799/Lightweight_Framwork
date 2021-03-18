// ========================================================
// des：
// author: 
// time：2021-03-08 11:47:58
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Editor {
	public class CharacterGUI : ShaderGUI
	{
		MaterialProperty blendMode = null;
		MaterialProperty cullMode = null;
		MaterialProperty albedoMap = null;
		MaterialProperty albedoColor = null;
		MaterialProperty alphaCutoff = null;
		//mask
		MaterialProperty maskMap = null;
		//outline 
		MaterialProperty outlineWidth = null;
		MaterialProperty outlineColor = null;
		MaterialProperty outlineScaledMaxDistance = null;
		//shadow
		MaterialProperty brightSideColor = null;
		MaterialProperty darkSideColor = null;
		MaterialProperty rampthreshold = null;
		MaterialProperty showdowRange = null;
		MaterialProperty faceShadowRange = null;
		//specular
		MaterialProperty shininess = null;
		MaterialProperty specTrail = null;
		MaterialProperty specular = null;
		MaterialProperty specularColor = null;
		//rim
		MaterialProperty rimThreshold = null;
		MaterialProperty rimPower = null;
		MaterialProperty rimColor = null;
		//emission
		MaterialProperty emissionColorForRendering = null;
		MaterialProperty emissionMap = null;

		GUIStyle headStyle;

		public void FindProperties(MaterialProperty[] props)
		{
			blendMode = FindProperty("_Mode", props);
			cullMode = FindProperty("_Cull", props);
			albedoMap = FindProperty("_BaseMap", props);
			albedoColor = FindProperty("_BaseColor", props);
			alphaCutoff = FindProperty("_Cutoff", props);
			//mask
			maskMap = FindProperty("_MaskMap", props);
			outlineWidth = FindProperty("_OutlineWidth", props);
			outlineColor = FindProperty("_OutlineColor", props);
			outlineScaledMaxDistance = FindProperty("_OutlineScaledMaxDistance", props);
			//shadow
			brightSideColor = FindProperty("_BrightSideColor", props);
			darkSideColor = FindProperty("_DarkSideColor", props);
			rampthreshold = FindProperty("_Rampthreshold", props);
			showdowRange = FindProperty("_ShadowRange", props);
			faceShadowRange = FindProperty("_FaceShadowRange", props);
			//specular
			shininess = FindProperty("_Shininess", props);
			specTrail = FindProperty("_SpecTrail", props);
			specular = FindProperty("_Specular", props);
			specularColor = FindProperty("_SpecularColor", props);
			//rim
			rimThreshold = FindProperty("_RimThreshold", props);
			rimPower = FindProperty("_RimPower", props);
			rimColor = FindProperty("_RimColor", props);
			//emission
			emissionColorForRendering = FindProperty("_EmissionColor", props);
			emissionMap = FindProperty("_EmissionMap", props);


			//outline 
			headStyle = new GUIStyle();
			headStyle.fontSize = 16;
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

				if (lod != ShaderLOD.Low)
				{
					materialEditor.TexturePropertySingleLine(new GUIContent("遮罩图"), maskMap);
					if (maskMap.textureValue != null)
						material.EnableKeyword("_MASKMAP");
					else
						material.DisableKeyword("_MASKMAP");
				}

				EditorGUILayout.LabelField("描边", headStyle);
				materialEditor.ColorProperty(outlineColor, "描边颜色");
				materialEditor.RangeProperty(outlineWidth, "描边宽度");
				materialEditor.RangeProperty(outlineScaledMaxDistance, "描边距离系数");
				EditorGUILayout.LabelField("阴影", headStyle);
				materialEditor.ColorProperty(brightSideColor, "明部颜色");
				materialEditor.ColorProperty(darkSideColor, "暗部颜色");
				materialEditor.RangeProperty(rampthreshold, "明暗部系数调整");
				materialEditor.RangeProperty(showdowRange, "阴影边界范围");
				materialEditor.RangeProperty(faceShadowRange, "脸部与阴影边界范围");

				if (lod != ShaderLOD.Low)
				{
					EditorGUILayout.LabelField("高光", headStyle);
					materialEditor.ColorProperty(specularColor, "高光颜色");
					materialEditor.RangeProperty(shininess, "高光范围");
					materialEditor.RangeProperty(specTrail, "高光拖尾");
					materialEditor.FloatProperty(specular, "高光强度");
				}

				if (lod != ShaderLOD.Low && lod != ShaderLOD.Middle)
				{
					EditorGUILayout.LabelField("边光", headStyle);
					materialEditor.ColorProperty(rimColor, "边缘光颜色");
					materialEditor.RangeProperty(rimThreshold, "范围宽度");
					materialEditor.RangeProperty(rimPower, "边缘正面影响参数");
				}

				EditorGUILayout.LabelField("自发光", headStyle);
				materialEditor.TexturePropertyWithHDRColor(new GUIContent("自发光贴图"), emissionMap, emissionColorForRendering, true);
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
