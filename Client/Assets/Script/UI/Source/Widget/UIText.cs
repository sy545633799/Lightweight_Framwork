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
	public class UIText: TextMeshProUGUI
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

				LocationTable table = LocationAsset.GetTable(m_ID);
				if (table == null)
					this.text = "";
				else
					this.text = table.Text;
#if UNITY_EDITOR
				//SetVerticesDirty();
				//SetLayoutDirty();
				UpdateGeometry();
				OnRebuildRequested();
#endif
			}
		}
        public new CanvasRenderer canvasRenderer;

        protected override void Awake()
		{
			base.Awake();
            canvasRenderer = GetComponent<CanvasRenderer>();
            LocationTable table = LocationAsset.GetTable(m_ID);
			if (table == null) return;
			this.text = table.Text;
        }

    }
}