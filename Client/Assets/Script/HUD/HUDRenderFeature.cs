// ========================================================
// des：
// author: 
// time：2020-12-21 17:29:31
// version：1.0
// ========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


namespace Game {
	public class HUDRenderFeature : ScriptableRendererFeature
	{
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			renderer.EnqueuePass(hudRenderPass);
		}
		HUDRenderPass hudRenderPass;
		public override void Create()
		{
			hudRenderPass = new HUDRenderPass();
			hudRenderPass.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
		}
	}

	public class HUDRenderPass : ScriptableRenderPass
	{
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (renderingData.cameraData.renderType != CameraRenderType.Base) return;
			
		//CommandBuffer cmd = HUDTitleInfo.HUDTitleRender.Instance.m_cmdBuffer;
		//if (cmd != null)
		//	context.ExecuteCommandBuffer(cmd);
		//cmd = HUDNumberRender.Instance.s_cmdBuffer;
		//if (cmd != null)
		//	context.ExecuteCommandBuffer(cmd);

		}

	}
}
