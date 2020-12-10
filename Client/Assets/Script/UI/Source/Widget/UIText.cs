using UnityEngine;
using UnityEngine.UI;
using Game;
using TMPro;

namespace Game
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(CanvasRenderer))]
	[AddComponentMenu("UI/TextMeshPro - Text (UI)", 11)]
	[ExecuteAlways]
	public class UIText : TextMeshProUGUI
	{
		[SerializeField]
		private int m_ID = 0;
		public int ID
		{
			get
			{
				return m_ID;
			}
			set
			{
				if (m_ID == value) return;
				m_ID = value;

				UILocation table = UILocationAsset.Get(m_ID);
				if (table == null)
					this.text = "";
				else
				{
					this.text = m_AddColon ? table.Text + ":" : table.Text;
				}
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(this);
#endif
			}
		}

		[SerializeField]
		private bool m_showVertical = false;
		public bool showVertical
		{
			get
			{
				return m_showVertical;
			}
			set
			{
				if (m_showVertical == value) return;
				m_showVertical = value;
#if UNITY_EDITOR
				SetVerticesDirty();
				SetLayoutDirty();
				UnityEditor.EditorUtility.SetDirty(this);
#endif
			}
		}

		[SerializeField]
		private bool m_AddColon = false;
		public bool addColon
		{
			get
			{
				return m_AddColon;
			}
			set
			{
				if (m_AddColon == value) return;
				m_AddColon = value;
				UILocation table = UILocationAsset.Get(m_ID);
				if (table == null)
					this.text = "";
				else
				{
					this.text = m_AddColon ? table.Text + ":" : table.Text;
				}
#if UNITY_EDITOR
				SetVerticesDirty();
				SetLayoutDirty();
				UnityEditor.EditorUtility.SetDirty(this);
#endif
			}
		}

		protected override void Awake()
		{
			base.Awake();
			UILocation table = UILocationAsset.Get(m_ID);
			if (table == null) return;
			this.text = m_AddColon ? table.Text + ":" : table.Text;
		}


		protected override void OnEnable()
		{
			base.OnEnable();
			if (!raycastTarget)
				GraphicRegistry.UnregisterGraphicForCanvas(canvas, this);
		}

#if UNITY_EDITOR
		//raycast 提示
		private Vector3[] fourCorners = new Vector3[4];
		private void OnDrawGizmos()
		{
			if (raycastTarget)
			{
				rectTransform.GetWorldCorners(fourCorners);
				Gizmos.color = Color.blue;
				for (int i = 0; i < 4; i++)
					Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);
			}
		}
#endif

		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			if (!raycastTarget)
				GraphicRegistry.UnregisterGraphicForCanvas(canvas, this);
		}


		protected override void OnCanvasHierarchyChanged()
		{
			base.OnCanvasHierarchyChanged();
			if (!raycastTarget)
				GraphicRegistry.UnregisterGraphicForCanvas(canvas, this);
		}

		public float GetWidth()
		{
			ForceMeshUpdate();
			return renderedWidth;
		}

		/// <summary>
		/// Store vertex attributes into the appropriate TMP_MeshInfo.
		/// </summary>
		/// <param name="i"></param>
		/// <param name="index_X4"></param>
		protected override void FillCharacterVertexBuffers(int i, int index_X4)
		{
			int materialIndex = m_textInfo.characterInfo[i].materialReferenceIndex;
			index_X4 = m_textInfo.meshInfo[materialIndex].vertexCount;

			// Make sure buffers allocation are sufficient to hold the vertex data
			//if (m_textInfo.meshInfo[materialIndex].vertices.Length < index_X4 + 4)
			//    m_textInfo.meshInfo[materialIndex].ResizeMeshInfo(Mathf.NextPowerOfTwo(index_X4 + 4));


			TMP_CharacterInfo[] characterInfoArray = m_textInfo.characterInfo;
			m_textInfo.characterInfo[i].vertexIndex = index_X4;

			if (m_showVertical)
			{
				#region �Զ������
				float x = m_rectTransform.sizeDelta.x / 2;
				float y = m_rectTransform.sizeDelta.y / 2;

				float BLx = characterInfoArray[i].vertex_BL.position.x + x;
				float BLy = y - characterInfoArray[i].vertex_BL.position.y;
				float BLy1 = y - BLx;
				float BLx1 = x - BLy;
				Vector3 TL = new Vector3(BLx1, BLy1, 0);

				float TLx = characterInfoArray[i].vertex_TL.position.x + x;
				float TLy = y - characterInfoArray[i].vertex_TL.position.y;
				float TLy1 = y - TLx;
				float TLx1 = x - TLy;
				Vector3 TR = new Vector3(TLx1, TLy1, 0);

				float TRx = characterInfoArray[i].vertex_TR.position.x + x;
				float TRy = y - characterInfoArray[i].vertex_TR.position.y;
				float TRy1 = y - TRx;
				float TRx1 = x - TRy;
				Vector3 BR = new Vector3(TRx1, TRy1, 0);

				float BRx = characterInfoArray[i].vertex_BR.position.x + x;
				float BRy = y - characterInfoArray[i].vertex_BR.position.y;
				float BRy1 = y - BRx;
				float BRx1 = x - BRy;
				Vector3 BL = new Vector3(BRx1, BRy1, 0);

				Vector3 center = Vector3.Lerp(BL, TR, 0.5f);
				Vector3 TL2 = new Vector3(center.y - TL.y + center.x, TL.x - center.x + center.y, 0);
				Vector3 TR2 = new Vector3(center.y - TR.y + center.x, TR.x - center.x + center.y, 0);
				Vector3 BR2 = new Vector3(center.y - BR.y + center.x, BR.x - center.x + center.y, 0);
				Vector3 BL2 = new Vector3(center.y - BL.y + center.x, BL.x - center.x + center.y, 0);

				//Setup Vertices for Characters
				m_textInfo.meshInfo[materialIndex].vertices[0 + index_X4] = TL2;
				m_textInfo.meshInfo[materialIndex].vertices[1 + index_X4] = TR2;
				m_textInfo.meshInfo[materialIndex].vertices[2 + index_X4] = BR2;
				m_textInfo.meshInfo[materialIndex].vertices[3 + index_X4] = BL2;
				#endregion
			}
			else
			{
				// Setup Vertices for Characters
				m_textInfo.meshInfo[materialIndex].vertices[0 + index_X4] = characterInfoArray[i].vertex_BL.position;
				m_textInfo.meshInfo[materialIndex].vertices[1 + index_X4] = characterInfoArray[i].vertex_TL.position;
				m_textInfo.meshInfo[materialIndex].vertices[2 + index_X4] = characterInfoArray[i].vertex_TR.position;
				m_textInfo.meshInfo[materialIndex].vertices[3 + index_X4] = characterInfoArray[i].vertex_BR.position;
			}


			// Setup UVS0
			m_textInfo.meshInfo[materialIndex].uvs0[0 + index_X4] = characterInfoArray[i].vertex_BL.uv;
			m_textInfo.meshInfo[materialIndex].uvs0[1 + index_X4] = characterInfoArray[i].vertex_TL.uv;
			m_textInfo.meshInfo[materialIndex].uvs0[2 + index_X4] = characterInfoArray[i].vertex_TR.uv;
			m_textInfo.meshInfo[materialIndex].uvs0[3 + index_X4] = characterInfoArray[i].vertex_BR.uv;


			// Setup UVS2
			m_textInfo.meshInfo[materialIndex].uvs2[0 + index_X4] = characterInfoArray[i].vertex_BL.uv2;
			m_textInfo.meshInfo[materialIndex].uvs2[1 + index_X4] = characterInfoArray[i].vertex_TL.uv2;
			m_textInfo.meshInfo[materialIndex].uvs2[2 + index_X4] = characterInfoArray[i].vertex_TR.uv2;
			m_textInfo.meshInfo[materialIndex].uvs2[3 + index_X4] = characterInfoArray[i].vertex_BR.uv2;


			// Setup UVS4
			//m_textInfo.meshInfo[0].uvs4[0 + index_X4] = characterInfoArray[i].vertex_BL.uv4;
			//m_textInfo.meshInfo[0].uvs4[1 + index_X4] = characterInfoArray[i].vertex_TL.uv4;
			//m_textInfo.meshInfo[0].uvs4[2 + index_X4] = characterInfoArray[i].vertex_TR.uv4;
			//m_textInfo.meshInfo[0].uvs4[3 + index_X4] = characterInfoArray[i].vertex_BR.uv4;


			// setup Vertex Colors
			m_textInfo.meshInfo[materialIndex].colors32[0 + index_X4] = characterInfoArray[i].vertex_BL.color;
			m_textInfo.meshInfo[materialIndex].colors32[1 + index_X4] = characterInfoArray[i].vertex_TL.color;
			m_textInfo.meshInfo[materialIndex].colors32[2 + index_X4] = characterInfoArray[i].vertex_TR.color;
			m_textInfo.meshInfo[materialIndex].colors32[3 + index_X4] = characterInfoArray[i].vertex_BR.color;

			m_textInfo.meshInfo[materialIndex].vertexCount = index_X4 + 4;
		}

		#region 设置程序文本
		/// <summary>
		/// 原则上UIText都由Lua控制，所以传入的string都是可回收的
		/// </summary>
		public void SetCodeText(string text)
		{
			SetText(text, false);
			//立即回收text
		}

		public void SetCodeText(int id)
		{
			UICodeText text = UICodeTextAsset.Get(id);
			SetText(text.Text, false);
		}

		public void SetCodeTextZ(int id, zstring z1)
		{
			using (zstring.Block())
			{
				UICodeText text = UICodeTextAsset.Get(id);
				SetText(zstring.Format(text.Text, z1), false);
			}
		}
		public void SetCodeText(int id, bool z1) => SetCodeTextZ(id, z1);
		public void SetCodeText(int id, int z1) => SetCodeTextZ(id, z1);
		//public void SetCodeText(int id, long z1) => SetCodeTextZ(id, z1);
		//public void SetCodeText(int id, float z1) => SetCodeTextZ(id, z1);
		//public void SetCodeText(int id, string z1)
		//{
		//	SetCodeTextZ(id, z1);
		//	//立即回收z1
		//}



		#endregion

	}
}