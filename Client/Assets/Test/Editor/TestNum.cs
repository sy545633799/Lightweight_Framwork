using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestNum
{
	[MenuItem("Tools/物体数量")]
	static void Nums()
	{
		GameObject[] goes = GameObject.FindObjectsOfType<GameObject>();
		Debug.LogError(goes.Length);
	}
}
