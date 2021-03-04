// ========================================================
// des：
// author: 
// time：2021-03-04 16:26:27
// version：1.0
// ========================================================

using System;
using UnityEngine;
using UnityEditor;

public class ShaderGUIUtil : ShaderGUI
{
    public static bool shaderDebugFoldout;
    public static bool shaderRawView;
	

	public enum BlendMode
    {
        Opaque = 0,
        Cutout,
        Transparent,
    }

	public static BlendMode DrawBlendModePopup(Material material, MaterialEditor materialEditor, MaterialProperty blendMode)
    {
        var mode = (BlendMode)blendMode.floatValue;
        EditorGUI.BeginChangeCheck();
        mode = (BlendMode)EditorGUILayout.Popup("Blend Mode", (int)mode, Enum.GetNames(typeof(BlendMode)));
        if (EditorGUI.EndChangeCheck())
        {
            materialEditor.RegisterPropertyChangeUndo("Blend Mode");
            blendMode.floatValue = (int)mode;
        }

        SetupMaterialWithBlendMode(material, mode, false);
        return mode;
    }

    public static BlendMode DrawBlendModePopup(Material material, MaterialEditor materialEditor, MaterialProperty blendMode, MaterialProperty customRenderQueue)
    {
        var mode = (BlendMode)blendMode.floatValue;
        EditorGUI.BeginChangeCheck();
        mode = (BlendMode)EditorGUILayout.Popup("Blend Mode", (int)mode, Enum.GetNames(typeof(BlendMode)));
        if (EditorGUI.EndChangeCheck())
        {
            materialEditor.RegisterPropertyChangeUndo("Blend Mode");
            blendMode.floatValue = (int)mode;
        }

        customRenderQueue.floatValue = GUILayout.Toggle(customRenderQueue.floatValue == 1, new GUIContent("RenderQueue")) ? 1 : 0;
        SetupMaterialWithBlendMode(material, mode, customRenderQueue.floatValue == 1);
        return mode;
    }

    public static BlendMode DrawBlendModePopup(Material material, MaterialEditor materialEditor, MaterialProperty blendMode, MaterialProperty customRenderQueue, MaterialProperty renderQueue)
    {
        var mode = (BlendMode)blendMode.floatValue;
        EditorGUI.BeginChangeCheck();
        mode = (BlendMode)EditorGUILayout.Popup("Blend Mode", (int)mode, Enum.GetNames(typeof(BlendMode)));
        if (EditorGUI.EndChangeCheck())
        {
            materialEditor.RegisterPropertyChangeUndo("Blend Mode");
            blendMode.floatValue = (int)mode;
        }

        customRenderQueue.floatValue = GUILayout.Toggle(customRenderQueue.floatValue == 1, new GUIContent("RenderQueue")) ? 1 : 0;
        SetupMaterialWithBlendMode(material, mode, customRenderQueue.floatValue == 1);
        
        if(customRenderQueue.floatValue == 1)
        {
            materialEditor.ShaderProperty(renderQueue, renderQueue.displayName);
            material.renderQueue = (int)renderQueue.floatValue;
        }

        return mode;
    }

    public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode, bool customRenderQueue)
    {
		int opaque = (int)UnityEngine.Rendering.RenderQueue.Geometry;
		int cutout = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
		int transparent = (int)UnityEngine.Rendering.RenderQueue.Transparent;
		int overlay = (int)UnityEngine.Rendering.RenderQueue.Overlay;

		switch (blendMode)
        {
            case BlendMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ColorMask", (int)UnityEngine.Rendering.ColorWriteMask.All);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                if(!customRenderQueue && (material.renderQueue < opaque || material.renderQueue >= cutout))
                    material.renderQueue = opaque;
                break;
            case BlendMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ColorMask", (int)UnityEngine.Rendering.ColorWriteMask.All);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                if(!customRenderQueue && (material.renderQueue < cutout || material.renderQueue >= transparent))
					material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
                break;
            case BlendMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ColorMask", (int)(UnityEngine.Rendering.ColorWriteMask.All & ~UnityEngine.Rendering.ColorWriteMask.Alpha));
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                if(!customRenderQueue && (material.renderQueue < transparent || material.renderQueue >= overlay))
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
        }
    }

    public static ShaderLOD DrawLODLevelPopup(MaterialEditor materialEditor, Material material, string[] lodNames)
	{
		EditorGUI.BeginChangeCheck();

		ShaderLOD lodMode;
		if (material.shader.maximumLOD >= 300)
			lodMode = ShaderLOD.High;
		else if (material.shader.maximumLOD >= 200)
			lodMode = ShaderLOD.Middle;
		else if (material.shader.maximumLOD >= 100)
			lodMode = ShaderLOD.Low;
		else
			lodMode = ShaderLOD.None;

		ShaderLOD mode = (ShaderLOD)EditorGUILayout.Popup("ShaderLOD", (int)lodMode, lodNames);
		if (EditorGUI.EndChangeCheck())
		{
			materialEditor.RegisterPropertyChangeUndo("ShaderLOD");
			switch (mode)
			{
				case ShaderLOD.High: material.shader.maximumLOD = 300; break;
				case ShaderLOD.Middle: material.shader.maximumLOD = 200; break;
				case ShaderLOD.Low: material.shader.maximumLOD = 100; break;
				case ShaderLOD.None: material.shader.maximumLOD = 0; break;
			}

			//TODO:������ò��Ҳû��
			if (mode != lodMode)
			{
				//Debug.Log($"Set {material.name} maximumLOD: {material.shader.maximumLOD}");
				EditorUtility.SetDirty(material.shader);
				materialEditor.PropertiesChanged();
				AssetDatabase.SaveAssets();
			}
		}

		return mode;
    }

    public static void DrawKeywordList(Material material)
    {
        var keywords = material.shaderKeywords;
        EditorGUILayout.LabelField(new GUIContent("Keywords:"), EditorStyles.boldLabel);
        for (int i=0; i<keywords.Length; ++i)
        {
            EditorGUILayout.LabelField(new GUIContent(keywords[i]));
        }

        if (GUILayout.Button("Clear All Keyword"))
        {
            ClearAllKeyword(material);
        }
    }

    public static void ClearAllKeyword(Material material)
    {
        material.shaderKeywords = new string[0];
    }

    static void __ClearMaterialProperties(string propPath, Material material, SerializedObject serializedObj)
    {
        var array = serializedObj.FindProperty(propPath);
        for (int i = 0; i < array.arraySize;)
        {
            var data = array.GetArrayElementAtIndex(i);
            var key = data.FindPropertyRelative("first");
            if (!material.HasProperty(key.stringValue))
            {
                array.DeleteArrayElementAtIndex(i);
            }
            else
            {
                ++i;
            }
        }
        serializedObj.ApplyModifiedProperties();
    }

    public static void ClearMaterialProperties(Material material, SerializedObject serializedObj)
    {
        __ClearMaterialProperties("m_SavedProperties.m_TexEnvs.Array", material, serializedObj);
        __ClearMaterialProperties("m_SavedProperties.m_Floats.Array", material, serializedObj);
        __ClearMaterialProperties("m_SavedProperties.m_Colors.Array", material, serializedObj);
    }

    static void __DrawUnusedMaterialProperties(string propPath, string valuePropPath, Material material, SerializedObject serializedObj)
    {
        var array = serializedObj.FindProperty(propPath);
        for (int i = 0; i < array.arraySize; ++i)
        {
            var data = array.GetArrayElementAtIndex(i);
            var key = data.FindPropertyRelative("first");
            if (!material.HasProperty(key.stringValue))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(key.stringValue);
                var value = data.FindPropertyRelative(valuePropPath);
                if (value.propertyType == SerializedPropertyType.ObjectReference)
                {
                    EditorGUILayout.ObjectField(value, new GUIContent(""));
                }
                else if (value.propertyType == SerializedPropertyType.Color)
                {
                    EditorGUILayout.ColorField(value.colorValue);
                }
                else if (value.propertyType == SerializedPropertyType.Float)
                {
                    EditorGUILayout.FloatField(value.floatValue);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }

    public static void DrawUnusedMaterialProperties(Material material, SerializedObject serializedObj)
    {
        EditorGUILayout.LabelField(new GUIContent("Unused Material Properties:"), EditorStyles.boldLabel);
        __DrawUnusedMaterialProperties("m_SavedProperties.m_TexEnvs.Array", "second.m_Texture", material, serializedObj);
        __DrawUnusedMaterialProperties("m_SavedProperties.m_Colors.Array", "second", material, serializedObj);
        __DrawUnusedMaterialProperties("m_SavedProperties.m_Floats.Array", "second", material, serializedObj);

        if (GUILayout.Button("Clear Material Properties"))
        {
            ClearMaterialProperties(material, serializedObj);
        }
    }

    public static void DrawHeightFogArea(Material material, MaterialEditor materialEditor, MaterialProperty[] props)
    {
        //MaterialProperty _HeightFogColor = FindProperty("_HeightFogColor", props);
        //MaterialProperty _HeightFogParams = FindProperty("_HeightFogParams", props);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Height Fog"), EditorStyles.boldLabel);
        bool isHeightFogOn = material.IsKeywordEnabled("_HEIGHT_FOG");
        isHeightFogOn = EditorGUILayout.Toggle(isHeightFogOn);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        if (isHeightFogOn)
        {
            material.EnableKeyword("_HEIGHT_FOG");
            //materialEditor.ShaderProperty(_HeightFogColor, "Color");
            //Vector4 p = _HeightFogParams.vectorValue;
            //p.y = EditorGUILayout.FloatField("Height Max", p.y);
            //p.x = EditorGUILayout.FloatField("Height Min", p.x);

            //if (p.y - p.x < 0.01f)
            //{
            //    p.x = p.y - 0.01f;
            //}
            //p.z = 1f / (p.y - p.x);
            //p.w = 1f - p.y * p.z;
            //_HeightFogParams.vectorValue = p;
        }
        else
        {
            material.DisableKeyword("_HEIGHT_FOG");
        }
    }

    static public void DrawBakedArea(Material material, MaterialEditor materialEditor, MaterialProperty[] props)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent(""), EditorStyles.boldLabel);
        bool isReflectOn = material.IsKeywordEnabled("REFLECTDIRECTION");
        isReflectOn = EditorGUILayout.Toggle(isReflectOn);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        if (isReflectOn)
        {
            material.EnableKeyword("REFLECTDIRECTION");
        }
        else
        {
            material.DisableKeyword("REFLECTDIRECTION");
        }
    }
}
