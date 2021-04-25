// ========================================================
// des：
// author: 
// time：2021-04-07 13:47:28
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.Editor {
	public partial class ExportTerrain : ScriptableWizard
	{
		private static void ExportDetails(TerrainData data, GameObject terrainGo)
		{
			GameObject[] trees = new GameObject[data.treePrototypes.Length];
			GameObject[] parents = new GameObject[data.treePrototypes.Length];
			TreePrototype[] prototype = data.treePrototypes;
			for (int i = 0; i < prototype.Length; i++)
			{
				trees[i] = prototype[i].prefab;
				parents[i] = new GameObject(trees[i].name);
				parents[i].transform.SetParent(terrainGo.transform);
			}
			for (int i = 0; i < data.treeInstances.Length; i++)
			{
				TreeInstance instance = data.treeInstances[i];
				GameObject tree = PrefabUtility.InstantiatePrefab(trees[instance.prototypeIndex]) as GameObject;
				tree.transform.position = Vector3.Scale(instance.position, data.size);

				tree.transform.eulerAngles = new Vector3(0, instance.rotation, 0);
				//tree.transform.widthScale = instance.widthScale;
				tree.transform.SetParent(parents[instance.prototypeIndex].transform);
			}
		}
	}
}
