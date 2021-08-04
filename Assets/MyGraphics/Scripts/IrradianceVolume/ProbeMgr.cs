// using System;
// using UnityEngine;
// using UnityEngine.Experimental.Rendering;
// #if UNITY_EDITOR
// using System.Threading.Tasks;
// using UnityEditor;
// using UnityEngine.Rendering;
//
// #endif
//
// //copy from https://zhuanlan.zhihu.com/p/390648011
// namespace MyGraphics.Scripts.IrradianceVolume
// {
// 	[Serializable]
// 	public class ProbeData
// 	{
// 		public int index;
// 		public Vector3Int position;
// 		public Color[] colors;
// 	}
//
//
// 	[ExecuteAlways]
// 	public class ProbeMgr : MonoBehaviour
// 	{
// 		private const int k_size = 128;
//
// 		private static Vector3[] directions = new Vector3[]
// 		{
// 			new Vector3(-1, 0, 0),
// 			new Vector3(1, 0, 0),
// 			new Vector3(0, -1, 0),
// 			new Vector3(0, 1, 0),
// 			new Vector3(0, 0, -1),
// 			new Vector3(0, 0, 1),
// 		};
//
// 		public ComputeShader cs;
// 		public Vector3Int size;
// 		public float interval;
// 		public ProbeData[] datas;
// 		public Texture3D[] textures;
//
// 		private Vector3 position;
// 		private int progress;
//
// 		public bool IsBaking { get; private set; }
//
// 		private void Start()
// 		{
// 			AdjustPosition();
// 			SetValue();
// 		}
//
// #if UNITY_EDITOR
// 		protected void Update()
// 		{
// 			if (this.IsBaking)
// 			{
// 				EditorUtility.SetDirty(this);
// 			}
// 		}
// #endif
//
// 		private void OnDrawGizmosSelected()
// 		{
// 			if (datas == null)
// 			{
// 				return;
// 			}
//
// 			Gizmos.color = Color.black;
// 			var size = new Vector3(interval, interval, interval);
// 			var position = this.transform.position;
//
// 			for (int x = -this.size.x; x <= this.size.x; x++)
// 			{
// 				for (int y = -this.size.y; y <= this.size.y; y++)
// 				{
// 					for (int z = -this.size.z; z <= this.size.z; z++)
// 					{
// 						var pos = new Vector3(x, y, z) * this.interval;
// 						Gizmos.DrawWireCube(position + pos, size);
// 					}
// 				}
// 			}
//
// 			foreach (var data in datas)
// 			{
// 				var pos = this.GetProbePosition(data);
//
// 				for (int i = 0; i < data.colors.Length; i++)
// 				{
// 					Gizmos.color = data.colors[i];
// 					Gizmos.DrawSphere(pos + directions[i] * this.interval * 0.3f, this.interval * 0.1f);
// 				}
// 			}
// 		}
//
// #if UNITY_EDITOR
// 		public async void Bake()
// 		{
// 			Debug.Log("Baking...");
//
// 			IsBaking = true;
// 			Shader.EnableKeyword("_BAKING");
// 			progress = 0;
//
// 			if (textures != null)
// 			{
// 				foreach (var item in textures)
// 				{
// 					CoreUtils.Destroy(item);
// 				}
// 			}
//
// 			FlushProbe();
// 			textures = new Texture3D[6];
//
// 			for (int i = 0; i < textures.Length; i++)
// 			{
// 				var texture = new Texture3D(size.x * 2 + 1, size.y * 2 + 1, size.z * 2 + 1, DefaultFormat.HDR, 0)
// 				{
// 					wrapMode = TextureWrapMode.Clamp
// 				};
// 				this.textures[i] = texture;
// 			}
//
// 			for (int i = 0; i < this.datas.Length; i++)
// 			{
// 				this.CaptureProbe(this.datas[i]);
// 			}
//
// 			while (this.progress < this.datas.Length)
// 			{
// 				EditorUtility.SetDirty(this);
// 				await Task.Yield();
// 			}
//
// 			foreach (var item in textures)
// 			{
// 				item.Apply();
// 			}
//
// 			this.SetValue();
// 			Shader.DisableKeyword("_BAKING");
// 			IsBaking = false;
//
// 			print("Bake Finish");
// 		}
// #endif
//
// 		private Vector3 GetProbePosition(ProbeData data)
// 		{
// 			var pos = this.position;
// 			for (int i = 0; i < 3; i++)
// 			{
// 				pos[i] += interval * data.position[i];
// 			}
//
// 			return pos;
// 		}
//
// 		public Vector3Int GetPositionIndex(Vector3 pos)
// 		{
// 			pos -= this.position;
// 			pos /= this.interval;
//
// 			return new Vector3Int((int) pos.x, (int) pos.y, (int) pos.z);
// 		}
//
// 		private void AdjustPosition()
// 		{
// 			var inter = new Vector3(this.interval, this.interval, this.interval) * 0.5f;
// 			position = this.transform.position - ((Vector3) this.size * this.interval) - inter;
// 		}
//
// 		private void FlushProbe()
// 		{
// 			int max = (this.size.x * 2 + 1) * (this.size.y * 2 + 1) * (this.size.z * 2 + 1);
// 			this.datas = new ProbeData[max];
// 			int n = -1;
//
// 			for (int i = 0; i <= this.size.x * 2; i++)
// 			{
// 				for (int j = 0; j <= this.size.y * 2; j++)
// 				{
// 					for (int k = 0; k <= this.size.z * 2; k++)
// 					{
// 						n++;
// 						this.datas[n] = new ProbeData()
// 						{
// 							index = n,
// 							position = new Vector3Int(i, j, k),
// 							colors = new Color[6]
// 						};
// 					}
// 				}
// 			}
// 			
// 			this.AdjustPosition();
// 		}
// 	}
// }