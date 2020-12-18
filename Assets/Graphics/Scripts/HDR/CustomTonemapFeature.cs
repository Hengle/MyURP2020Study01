using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Graphics.Scripts.HDR
{
	public class CustomTonemapFeature : ScriptableRendererFeature
	{
		[SerializeField]
		private Shader customTonemapShader;

		private CustomTonemapPass customTonemapPass;
		private Material customTonemapMaterial;

		public override void Create()
		{
			customTonemapShader = Shader.Find("MyRP/HDR/CustomTonemap");
			customTonemapMaterial = CoreUtils.CreateEngineMaterial(customTonemapShader);
			customTonemapPass = new CustomTonemapPass(customTonemapMaterial);
			customTonemapPass.renderPassEvent =
				RenderPassEvent.BeforeRenderingPostProcessing; //AfterRenderingPostProcessing;
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (renderingData.postProcessingEnabled)
			{
				var settings = VolumeManager.instance.stack.GetComponent<CustomTonemapSettings>();
				if (settings != null && settings.IsActive())
				{
					customTonemapPass.Setup(renderer.cameraColorTarget, settings);
					renderer.EnqueuePass(customTonemapPass);
				}
			}
		}
	}
}