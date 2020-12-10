// ========================================================
// des：
// author: 
// time：2020-12-10 12:27:44
// version：1.0
// ========================================================

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
	public class BuildSceneAsset
	{
		[MenuItem("Tools/场景相关/一键导出场景信息", priority = 200)]
		private static void BuildSceneInfo()
		{
			BuildPrefabPathAsset.Build();
			BuildSceneElementAsset.Build();
			BuildSceneEnvAsset.Build();
		}
	}
}