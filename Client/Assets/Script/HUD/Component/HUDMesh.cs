// ========================================================
// des：四个顶点两个三角面的一个基本mesh
// author: shenyi
// time：2020-12-24 13:38:29
// version：1.0
// ========================================================

using UnityEngine;
using UnityEngine.Sprites;

namespace Game
{
	public class HUDMesh
	{
		public Transform transform;

		public Mesh mesh;
		public Sprite sprite;

		public Vector4 spriteUv = Vector4.zero;
		public Color32 spriteColor = Color.white;

		public Vector3[] Vertices = new Vector3[4];
		public Color32[] Colors = new Color32[4];
		public Vector2[] Uv = new Vector2[4];
		public Vector2[] Uv2 = new Vector2[4];
		public int[] Triangles = new int[] { 0, 1, 2, 0, 2, 3 };
		//  3|----|2
		//   |  / |
		//   | /  |
		//  0|----|1

		public float Width { get; private set; }
		public float Heigth { get; private set; }
		public float OffsetX { get; private set; }
		public float OffsetY { get; private set; }
		public float Order { get; private set; }
		public bool Dirty { get; private set; } = false;
		public bool HideOrShow { get; private set; } = true;

		public float depth
		{
			get
			{
				var _dir = transform.position - HUDManager.MainCamera.transform.position;
				return Mathf.Pow(_dir.x, 2) + Mathf.Pow(_dir.y, 2);
			}
		}

		public HUDMesh()
		{
			mesh = HUDMeshPool.CreateMesh();
		}

		public void ReSet(Transform transform, Sprite sprite, float width, float heigth, float offsetX = 0, float offsetY = 0)
		{
			Width = width;
			Heigth = heigth;
			OffsetX = offsetX;
			OffsetY = offsetY;
			this.sprite = sprite;
			this.transform = transform;

			spriteUv = DataUtility.GetOuterUV(this.sprite);
			Vector3 _position = transform.position;

			Vertices[0].Set(_position.x, _position.y, _position.z);
			Vertices[1].Set(_position.x, _position.y, _position.z);
			Vertices[2].Set(_position.x, _position.y, _position.z);
			Vertices[3].Set(_position.x, _position.y, _position.z);

			Uv2[0].Set( - (Width * 0.5f - OffsetX) * 0.01f, - (Heigth * 0.5f - OffsetY) * 0.01f);
			Uv2[1].Set((Width * 0.5f + OffsetX) * 0.01f, - (Heigth * 0.5f - OffsetY) * 0.01f);
			Uv2[2].Set((Width * 0.5f + OffsetX) * 0.01f, (Heigth * 0.5f + OffsetY) * 0.01f);
			Uv2[3].Set(- (Width * 0.5f - OffsetX) * 0.01f, (Heigth * 0.5f + OffsetY) * 0.01f);

			Uv[0].Set(spriteUv.x, spriteUv.y);
			Uv[1].Set(spriteUv.z, spriteUv.y);
			Uv[2].Set(spriteUv.z, spriteUv.w);
			Uv[3].Set(spriteUv.x, spriteUv.w);

			Colors[0] = spriteColor;
			Colors[1] = spriteColor;
			Colors[2] = spriteColor;
			Colors[3] = spriteColor;
			
			UpdateMesh();
		}

		public void UpdateVertices()
		{
			Vector3 _position = transform.position;
			Vertices[0].Set(_position.x, _position.y, _position.z);
			Vertices[1].Set(_position.x, _position.y, _position.z);
			Vertices[2].Set(_position.x, _position.y, _position.z);
			Vertices[3].Set(_position.x, _position.y, _position.z);
			UpdateMesh();
			Dirty = false;
		}

		public void ResetSprite(Sprite sprite)
		{
			this.sprite = sprite;
			spriteUv = DataUtility.GetOuterUV(this.sprite);
			Vector2 _uv = Vector2.zero;
			Uv[0].Set(spriteUv.x, spriteUv.y);
			Uv[1].Set(spriteUv.z, spriteUv.y);
			Uv[2].Set(spriteUv.z, spriteUv.w);
			Uv[3].Set(spriteUv.x, spriteUv.w);
			UpdateMesh();
		}

		public void ResetColorAlpha(float alpha)
		{
			if (mesh.vertices.Length != 4)
				return;

			spriteColor.a = (byte)Mathf.Clamp01(alpha);
			Colors[0] = spriteColor;
			Colors[1] = spriteColor;
			Colors[2] = spriteColor;
			Colors[3] = spriteColor;
			UpdateMesh();
		}

		public void ResetFillLeftToRight(float normal)
		{
			normal = Mathf.Clamp01(normal);
			Vector3 _position = Vector3.zero;
			float _offset = Width * (1 - normal) * 0.01f;
			Vertices[1].x = _position.x + (Width * 0.5f + OffsetX) * 0.01f - _offset;
			Vertices[2].x = _position.x + (Width * 0.5f + OffsetX) * 0.01f - _offset;

			Vector2 _uv = Vector2.zero;
			float z = spriteUv.x + (spriteUv.z - spriteUv.x) * normal;
			Uv[0].Set(spriteUv.x, spriteUv.y);
			Uv[1].Set(z, spriteUv.y);
			Uv[2].Set(z, spriteUv.w);
			Uv[3].Set(spriteUv.x, spriteUv.w);
			UpdateMesh();
		}

		public void ResetFillRightToLeft(float normal)
		{
			normal = Mathf.Clamp01(normal);
			Vector3 _position = Vector3.zero;
			float _offset = Width * normal * 0.01f;
			Vertices[0].x = _position.x - (Width * 0.5f - OffsetX) * 0.01f + _offset;
			Vertices[3].x = _position.x - (Width * 0.5f - OffsetX) * 0.01f + _offset;

			Vector2 _uv = Vector2.zero;
			float x = spriteUv.x + (spriteUv.z - spriteUv.x) * normal;
			Uv[0].Set(x, spriteUv.y);
			Uv[1].Set(spriteUv.z, spriteUv.y);
			Uv[2].Set(spriteUv.z, spriteUv.w);
			Uv[3].Set(x, spriteUv.w);

			UpdateMesh();
		}

		public void SetOffsetX(float offsetX)
		{
			OffsetX = offsetX;
			Dirty = true;
		}

		public void SetOffsetY(float offsetY)
		{
			OffsetY = offsetY;
			Dirty = true;
		}

		public void SetOffset(float offsetX, float offsetY)
		{
			OffsetX = offsetX;
			OffsetY = offsetY;
			Dirty = true;
		}

		public void MoveOffsetX(float offsetX)
		{
			OffsetX += offsetX;
			Dirty = true;
		}

		public void MoveOffsetY(float offsetY)
		{
			OffsetY += offsetY;
			Dirty = true;
		}

		public void MoveOffset(float offsetX, float offsetY)
		{
			OffsetY += offsetY;
			OffsetY += offsetY;
			Dirty = true;
		}

		private void UpdateMesh()
		{
			if (mesh != null)
			{
				mesh.Clear();
				mesh.vertices = Vertices;
				mesh.colors32 = Colors;
				mesh.uv = Uv;
				mesh.triangles = Triangles;
				mesh.RecalculateBounds();
				mesh.RecalculateNormals();
			}
		}

		public void Dispose()
		{
			HUDMeshPool.RecoveryMesh(mesh);
		}
	}
}
