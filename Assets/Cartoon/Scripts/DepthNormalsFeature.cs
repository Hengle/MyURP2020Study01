using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Cartoon
{
	public class DepthNormalsFeature : ScriptableRendererFeature
	{
		private DepthNormalsPass depthNormalPass;
		private RenderTargetHandle depthNormalsTexture;
		private Material depthNormalsMaterial;


		public override void Create()
		{
			//其实这里也可以自己写depth normals 加密
			//但是替换材质球 可以一次性全部替换成自己想要的
			depthNormalsMaterial = CoreUtils.CreateEngineMaterial("MyRP/Cartoon/DepthNormals");
			depthNormalPass = new DepthNormalsPass(RenderQueueRange.opaque, -1, depthNormalsMaterial);
			depthNormalPass.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;
			depthNormalsTexture.Init("_CameraDepthNormalsTexture");
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			depthNormalPass.Setup(depthNormalsTexture);
			renderer.EnqueuePass(depthNormalPass);
		}
	}
}