using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//374.277
public class PrintPex : ScriptableWizard
{
	public int x;
	public int y;
	public Texture2D tex;

	void OnWizardUpdate() { }

	void OnWizardCreate() { }

	void OnWizardOtherButton()
	{

		Debug.LogError(tex.GetPixel(x, y));
	}

	//protected override bool DrawWizardGUI()
	//{
	//	EditorGUILayout.LabelField("选择LOD:");
	//	ShaderLod = (ShaderLod)EditorGUILayout.EnumPopup(ShaderLod);
	//	return base.DrawWizardGUI();
	//}

	[MenuItem("Tools/Render/Pex")]
	static void CreateDeSer()
	{
		ScriptableWizard.DisplayWizard<PrintPex>("ShaderLod11", "取消", "设置");
	}
}
