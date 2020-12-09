using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class InitialLevel : MonoBehaviour
{
	public RenderPipelineAsset RenderPipelineAsset;

	void Start()
	{
		if (GraphicsSettings.renderPipelineAsset != RenderPipelineAsset)
			GraphicsSettings.renderPipelineAsset = RenderPipelineAsset;
	}

}
