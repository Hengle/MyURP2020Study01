using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Graphics.Scripts.ScreenEffect
{
	[Serializable]
	public sealed class RenderPassEventParameter : VolumeParameter<RenderPassEvent>
	{
		public RenderPassEventParameter(RenderPassEvent value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}

	[Serializable]
	public sealed class MaterialParameter : VolumeParameter<Material>
	{
		public MaterialParameter(Material value, bool overrideState = false)
			: base(value, overrideState)
		{
		}
	}

	[Serializable, VolumeComponentMenu("My/ScreenEffect")]
	public class ScreenEffectPostProcess : VolumeComponent, IPostProcessComponent
	{
		public BoolParameter enableEffect = new BoolParameter(false);

		public RenderPassEventParameter renderPassEvent =
			new RenderPassEventParameter(RenderPassEvent.BeforeRenderingPostProcessing);

		public MaterialParameter effectMat = new MaterialParameter(null);

		public bool IsActive() => enableEffect.value;

		public bool IsTileCompatible() => false;
	}
}