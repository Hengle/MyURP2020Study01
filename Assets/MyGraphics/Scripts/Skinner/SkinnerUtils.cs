using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace MyGraphics.Scripts.Skinner
{
	public static class SkinnerShaderConstants
	{
		public static int SrcTex_ID = Shader.PropertyToID("_SrcTex");
		public static int SourcePositionTex0_ID = Shader.PropertyToID("_SourcePositionTex0");
		public static int SourcePositionTex1_ID = Shader.PropertyToID("_SourcePositionTex1");
		public static int PositionTex_ID = Shader.PropertyToID("_PositionTex");
		public static int VelocityTex_ID = Shader.PropertyToID("_VelocityTex");
		public static int RotationTex_ID = Shader.PropertyToID("_RotationTex");
		public static int OrthnormTex_ID = Shader.PropertyToID("_OrthnormTex");
		
		public static int ParticlePositionTex_ID = Shader.PropertyToID("_ParticlePositionTex");
		public static int ParticleVelocityTex_ID = Shader.PropertyToID("_ParticleVelocityTex");
		public static int ParticleRotationTex_ID = Shader.PropertyToID("_ParticleRotationTex");
		public static int ParticlePrevPositionTex_ID = Shader.PropertyToID("_ParticlePrevPositionTex");
		public static int ParticlePrevRotationTex_ID = Shader.PropertyToID("_ParticlePrevRotationTex");

		public static int TrailPositionTex_ID = Shader.PropertyToID("_TrailPositionTex");
		public static int TrailVelocityTex_ID = Shader.PropertyToID("_TrailVelocityTex");
		public static int TrailOrthnormTex_ID = Shader.PropertyToID("_TrailOrthnormTex");
		public static int TrailPrevPositionTex_ID = Shader.PropertyToID("_TrailPrevPositionTex");
		public static int TrailPrevVelocityTex_ID = Shader.PropertyToID("_TrailPrevVelocityTex");
		public static int TrailPrevOrthnormTex_ID = Shader.PropertyToID("_TrailPrevOrthnormTex");

		public static int RandomSeed_ID = Shader.PropertyToID("_RandomSeed");
		public static int Damper_ID = Shader.PropertyToID("_Damper");
		public static int Gravity_ID = Shader.PropertyToID("_Gravity");
		public static int Life_ID = Shader.PropertyToID("_Life");
		public static int Spin_ID = Shader.PropertyToID("_Spin");
		public static int NoiseParams_ID = Shader.PropertyToID("_NoiseParams");
		public static int NoiseOffset_ID = Shader.PropertyToID("_NoiseOffset");
		public static int Scale_ID = Shader.PropertyToID("_Scale");
		public static int SpeedLimit_ID = Shader.PropertyToID("_SpeedLimit");
		public static int Drag_ID = Shader.PropertyToID("_Drag");
		public static int LineWidth_ID = Shader.PropertyToID("_LineWidth");
	}

	public static class SkinnerUtils
	{
		public static void CreateRT(ref RenderTexture rt, int w, int h, string name = null)
		{
			CleanRT(ref rt);
			rt = new RenderTexture(w, h, 0, RenderTextureFormat.ARGBFloat)
			{
				filterMode = FilterMode.Point,
				wrapMode = TextureWrapMode.Clamp,
				name = name ?? Guid.NewGuid().ToString(),
			};
		}


		public static void CleanRT(ref RenderTexture rt)
		{
			CoreUtils.Destroy(rt);
			rt = null;
		}

		private static void Blit(CommandBuffer cmd, RenderTargetIdentifier src, RenderTargetIdentifier dest,
			Material mat, int pass, int mipmap = 0)
		{
			if (mipmap != 0)
			{
				dest = new RenderTargetIdentifier(dest, mipmap);
			}

			cmd.SetRenderTarget(dest, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);

			cmd.SetGlobalTexture(SkinnerShaderConstants.SrcTex_ID, src);

			CoreUtils.DrawFullScreen(cmd, mat, null, pass);
		}


		public static void DrawFullScreen(CommandBuffer cmd, RenderTargetIdentifier dst, Material mat, int pass = 0)
		{
			cmd.SetRenderTarget(dst, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
			CoreUtils.DrawFullScreen(cmd, mat, null, pass);
		}
	}
}