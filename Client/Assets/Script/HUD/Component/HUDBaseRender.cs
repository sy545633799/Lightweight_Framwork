// ========================================================
// des：保存多个HUDMesh并渲染
// author: shenyi
// time：2020-12-24 19:09:47
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class HUDBaseRender : RecycleObject<HUDBaseRender>
	{
		public Sprite HUDSprite;
		private Mesh MergeMesh = new Mesh();
		protected List<HUDMesh> m_hudmeshs = new List<HUDMesh>(256);
		protected VaryingVector3List mVerts = new VaryingVector3List(1024);
		protected VaryingVector2List mUvs = new VaryingVector2List(1024);
		protected VaryingVector2List mUv2s = new VaryingVector2List(1024);
		protected VaryingColor32List mCols = new VaryingColor32List(1024);
		protected VaryingIntList mIndices = new VaryingIntList(1536);

		public Transform transform { get; private set; }
		public bool HideOrShow = true;
		public Vector2 m_offset = Vector2.zero;
		public Vector3 m_viewportPosition;

		public float depth
		{
			get
			{
				var _dir = transform.position - HUDManager.MainCamera.transform.position;
				return Mathf.Pow(_dir.x, 2) + Mathf.Pow(_dir.y, 2) + Mathf.Pow(_dir.z, 2);
			}
		}

		public override void OnAlloc()
		{
			HUDManager.AddHUDRender(this);
		}

		public void SetTransform(Transform transform) { this.transform = transform; }

		public HUDMesh PushSprite(Sprite sprite, float width, float heigth, float offsetX = 0, float offsetY = 0)
		{
			HUDMesh _mesh = new HUDMesh();
			_mesh.ReSet(transform, sprite, width, heigth, offsetX, offsetY);
			m_hudmeshs.Add(_mesh);
			HUDSprite = sprite;
			return _mesh;
		}

		public void ResetColorAlpha(float alpha)
		{
			for (int i = 0; i < m_hudmeshs.Count; i++)
			{
				m_hudmeshs[i].ResetColorAlpha(alpha);
			}
		}

		public virtual Mesh Merge()
		{
			mVerts.Clear();
			mUvs.Clear();
			mUv2s.Clear();
			mCols.Clear();
			mIndices.Clear();
			if (m_hudmeshs.Count > 0)
			{
				mVerts.SetArrayLength(1024);
				mUvs.SetArrayLength(1024);
				mUv2s.SetArrayLength(1024);
				mCols.SetArrayLength(1024);
				mIndices.SetArrayLength(1536);

				for (int i = 0; i < m_hudmeshs.Count; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						mVerts.Add(m_hudmeshs[i].Vertices[j]);
						mUvs.Add(m_hudmeshs[i].Uv[j]);
						mUv2s.Add(m_hudmeshs[i].Uv2[j]);
						mCols.Add(m_hudmeshs[i].Colors[j]);
					}
					for (int j = 0; j < 6; j++)
					{
						mIndices.Add(m_hudmeshs[i].Triangles[j] + 6 * i);
					}
				}
				mVerts.SetArrayLength((ulong)m_hudmeshs.Count * 4);
				mUvs.SetArrayLength((ulong)m_hudmeshs.Count * 4);
				mUv2s.SetArrayLength((ulong)m_hudmeshs.Count * 4);
				mCols.SetArrayLength((ulong)m_hudmeshs.Count * 4);
				mIndices.SetArrayLength((ulong)m_hudmeshs.Count * 6);

				MergeMesh.vertices = mVerts.buffer;
				MergeMesh.uv = mUvs.buffer;
				MergeMesh.uv2 = mUv2s.buffer;
				MergeMesh.colors32 = mCols.buffer;
				MergeMesh.triangles = mIndices.buffer;
			}
			return MergeMesh;
		}

		public override void Downcast()
		{
			foreach (var mesh in m_hudmeshs)
			{
				mesh.Dispose();
			}
			m_hudmeshs.Clear();
			HUDMeshPool.RecoveryMesh(MergeMesh);
		}
	}
}
