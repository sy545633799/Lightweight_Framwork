using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public enum ShaderLod
{
    Global1000 = 1000,
    Low100 = 100,
    Middle200 = 200,
    High300 = 300
}

public class LODSetting : ScriptableWizard
{
    private ShaderLod ShaderLod = ShaderLod.Global1000;

    void OnWizardUpdate(){}

    void OnWizardCreate(){}

    void OnWizardOtherButton()
    {
		
        Shader.globalMaximumLOD = (int)ShaderLod;
		//Debug.LogError(Shader.globalMaximumLOD);

	}

    protected override bool DrawWizardGUI()
    {
        EditorGUILayout.LabelField("选择LOD:");
        ShaderLod = (ShaderLod)EditorGUILayout.EnumPopup(ShaderLod);
        return base.DrawWizardGUI();
    }

    [MenuItem("Tools/Render/ShaderLod")]
    static void CreateDeSer()
    {
        ScriptableWizard.DisplayWizard<LODSetting>("ShaderLod11", "取消", "设置");
    }

}
