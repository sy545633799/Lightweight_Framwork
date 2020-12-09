using UnityEngine;
using UnityEngine.Rendering;

namespace CatlikePipeline
{
	[CreateAssetMenu(menuName = "Rendering/Mine/CatlikePipeline")]
	public class CatlikePipelineAsset : RenderPipelineAsset
	{
		public RenderSetting renderSetting = new RenderSetting();

		protected override RenderPipeline CreatePipeline()
		{
			return new CatlikePipeline(renderSetting);
		}
	}
}
