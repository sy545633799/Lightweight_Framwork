using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace CatlikePipeline
{
	public abstract class RendererBase: IDisposable
	{
		protected ScriptableRenderContext context;
		protected Camera camera;
		protected RenderSetting renderSetting;
		protected CullingResults cullingResults;
		protected CommandBuffer buffer;

		protected abstract void OnRender();
		public virtual void HandleDirectional(VisibleLight light, int index, CullingResults results) { }
		public virtual void HandlePoint(VisibleLight light, int index, CullingResults results) { }
		public virtual void HandleSport(VisibleLight light, int index, CullingResults results) { }
		public virtual void Dispose() { }

		public void Render(ScriptableRenderContext context, Camera camera,  RenderSetting renderSetting, CullingResults cullingResults)
		{
			this.context = context;
			this.camera = camera;
			this.renderSetting = renderSetting;
			this.cullingResults = cullingResults;
			buffer = CommandBufferPool.Get(GetType().Name);
			OnRender();
			CommandBufferPool.Release(buffer);
		}

		protected void ExecuteBufferAndClear()
		{
			if (context != null)
				context.ExecuteCommandBuffer(buffer);
			buffer.Clear();
		}

		protected void SetKeywords(string[] keywords, int enabledIndex)
		{
			for (int i = 0; i < keywords.Length; i++)
			{
				if (i == enabledIndex)
				{
					buffer.EnableShaderKeyword(keywords[i]);
				}
				else
				{
					buffer.DisableShaderKeyword(keywords[i]);
				}
			}
		}

		
	}
}
