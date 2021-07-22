using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Graphics.Scripts.GPUDrivenTerrain
{
	[CreateAssetMenu(menuName = "GPUDrivenTerrainLearn/TerrainAsset")]
	public class TerrainAsset : ScriptableObject
	{
		public const uint MAX_NODE_ID = 34124; //5x5+10x10+20x20+40x40+80x80+160x160 - 1
		public const int MAX_LOD = 5;

		/// <summary>
		/// MAX LOD下，世界由5x5个区块组成
		/// </summary>
		public const int MAX_LOD_NODE_COUNT = 5;

		private static Mesh _patchMesh;
		private static Mesh _unitCubeMesh;

		[SerializeField] private Vector3 _worldSize = new Vector3(10240, 2048, 10240);

		[SerializeField] private Texture2D _albedoMap;

		[SerializeField] private Texture2D _heightMap;

		[SerializeField] private Texture2D _normalMap;

		[SerializeField] private Texture2D[] _minMaxHeightMaps;

		[SerializeField] private Texture2D[] _quadTreeMaps;

		[SerializeField] private ComputeShader _terrainCompute;

		[SerializeField] private Material _terrainMaterial;

		[SerializeField] private Shader _boundsDebugShader;


		private RenderTexture _quadTreeMap;
		private RenderTexture _minMaxHeightMap;
		private Material _boundsDebugMaterial;

		public static Mesh patchMesh
		{
			get
			{
				if (!_patchMesh)
				{
					_patchMesh = MeshUtility.CreatePlaneMesh(16);
				}

				return _patchMesh;
			}
		}

		public static Mesh unitCubeMesh
		{
			get
			{
				if (!_unitCubeMesh)
				{
					_unitCubeMesh = MeshUtility.CreateCube(1);
				}

				return _unitCubeMesh;
			}
		}

		public Vector3 worldSize => _worldSize;

		public Texture2D albedoMap => _albedoMap;

		public Texture2D heightMap => _heightMap;

		public Texture2D normalMap => _normalMap;

		public ComputeShader computeShader => _terrainCompute;

		public Material terrainMaterial => _terrainMaterial;

		public RenderTexture quadTreeMap
		{
			get
			{
				if (!_quadTreeMap)
				{
					_quadTreeMap =
						TextureUtility.CreateRenderTextureWithMipTextures(_quadTreeMaps, RenderTextureFormat.R16);
				}

				return _quadTreeMap;
			}
		}

		public RenderTexture minMaxHeightMap
		{
			get
			{
				if (!_minMaxHeightMap)
				{
					_minMaxHeightMap =
						TextureUtility.CreateRenderTextureWithMipTextures(_minMaxHeightMaps, RenderTextureFormat.RG32);
				}

				return _minMaxHeightMap;
			}
		}

		public Material boundsDebugMaterial
		{
			get
			{
				if (!_boundsDebugMaterial)
				{
					_boundsDebugMaterial = new Material(_boundsDebugShader);
				}

				return _boundsDebugMaterial;
			}
		}


		public void OnDestroy()
		{
			CoreUtils.Destroy(_quadTreeMap);
			CoreUtils.Destroy(_minMaxHeightMap);
			CoreUtils.Destroy(_boundsDebugMaterial);
		}
	}
}