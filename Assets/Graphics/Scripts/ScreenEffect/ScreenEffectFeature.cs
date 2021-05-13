using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Graphics.Scripts.ScreenEffect
{
	public class ScreenEffectFeature : ScriptableRendererFeature
	{
		private ScreenEffectPass screenEffectPass;

		public override void Create()
		{
			screenEffectPass = new ScreenEffectPass();
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			var settings = VolumeManager.instance.stack.GetComponent<ScreenEffectPostProcess>();
			if (settings.IsActive() && settings.effectMat.value != null)
			{
				screenEffectPass.Setup(settings);
				screenEffectPass.renderPassEvent = settings.renderPassEvent.value;
				renderer.EnqueuePass(screenEffectPass);
			}
		}
	}
}