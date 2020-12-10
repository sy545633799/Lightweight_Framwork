// ========================================================
// des：
// author: 
// time：2020-07-17 17:04:22
// version：1.0
// ========================================================

using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
    public class Empty4Raycast : MaskableGraphic
    {
        protected Empty4Raycast()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}
