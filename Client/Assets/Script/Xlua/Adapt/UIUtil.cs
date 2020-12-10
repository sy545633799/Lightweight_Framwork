using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using SuperScrollView;
using UnityEngine.EventSystems;

namespace Game
{
	public enum PointerType
	{
		None,
		UI,
		TerrainGeometry,
		Other,
	}

	public class UIUtil
    {
		/// <summary>
		/// 游戏点击事件 0 无点击 1 点击UI上 2 点击在地面上 3 点击不在地面 不在场景上
		/// 使用射线只有在寻路网格上点击才有效
		/// </summary>
		public static PointerType IsPointerOnUI()
		{
#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_IOS || UNITY_ANDROID)
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
#else
			if (Input.GetMouseButton(0))
#endif
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit))
				{

#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_IOS || UNITY_ANDROID)
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
					if (EventSystem.current.IsPointerOverGameObject())
#endif
					{
						return PointerType.UI;
					}
					else
					{
						if (hit.collider.CompareTag("TerrainGeometry"))
						{
							return PointerType.TerrainGeometry;
						}
						else
						{
							return PointerType.Other;
						}
					}
				}
				return PointerType.None;

				//#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_IOS || UNITY_ANDROID)
				//            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
				//#else
				//            //只有点击到 寻路网格上 才有用
				//            if (EventSystem.current.IsPointerOverGameObject())
				//#endif
				//			{
				//				return 1;
				//			}
				//            else
				//            {
				//                //Debug.LogError("tag : " + EventSystem.current.gameObject.tag);
				//                return 2;
				//            }
			}
			return 0;
		}

		/// <summary>
		/// 设置渲染层级
		/// </summary>
		/// <param name="go"></param>
		/// <param name="sortingLayerName"></param>
		/// <param name="order"></param>
		/// <param name="includeInactive"></param>
		public static void SetUIRenderLayer(GameObject go, string sortingLayerName, int order, bool includeInactive = false)
        {
            Renderer[] renders = go.GetComponentsInChildren<Renderer>(includeInactive);
            if (renders != null)
            {
                for (int i = 0; i < renders.Length; i++)
                {
                    renders[i].sortingLayerName = sortingLayerName;
                    renders[i].sortingOrder = order;
                }
            }
        }

        public static void SetUIRenderLayer(Transform trans, string sortingLayerName, int order, bool includeInactive = false)
        {
            SetUIRenderLayer(trans, sortingLayerName, order, includeInactive);
        }

        public static void SetImageColor(Image image, float x, float y, float z)
        {
            image.color = new Color(x / 255, y / 255, z / 255);
        }

        public static void SetTextColor(UIText text, float x, float y, float z)
        {
            text.color = new Color(x / 255, y / 255, z / 255);
        }

		/// <summary>
        /// 设置显隐
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="active"></param>
		public static void SetActive(RectTransform rect, bool active)
		{
			if (active)
				rect.localScale = Vector3.one;
			else
				rect.localScale = Vector3.zero;
		}

		/// <summary>
		/// 设置大小
		/// </summary>
		/// <param name="go"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public static void SetRectSize(GameObject go, float x, float y)
        {
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.SetRectSize(new Vector2(x, y));
        }

        public static void SetRectSize(Transform trans, float x, float y)
        {
            RectTransform rect = trans.GetComponent<RectTransform>();
            rect.SetRectSize(new Vector2(x, y));
        }

        public static void SetRectSize(RectTransform rect, float x, float y)
        {
            rect.SetRectSize(new Vector2(x, y));
        }

        /// <summary>
        /// 设置为Screen大小
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void SetAsSceneSize(GameObject go)
        {
            RectTransform rect = go.GetComponent<RectTransform>();
            SetAsSceneSize(rect);
        }

        public static void SetAsSceneSize(Transform trans)
        {
            RectTransform rect = trans.GetComponent<RectTransform>();
            SetAsSceneSize(rect);
        }

        public static void SetAsSceneSize(RectTransform rect)
        {
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = Vector2.zero;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
        }

        /// <summary>
        /// 设置轴心点
        /// </summary>
        /// <param name="go"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void SetRectPivot(GameObject go, float x, float y)
        {
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.SetRectPivot(new Vector2(x, y));
        }

        public static void SetRectPivot(Transform trans, float x, float y)
        {
            RectTransform rect = trans.GetComponent<RectTransform>();
            rect.SetRectPivot(new Vector2(x, y));
        }

        public static void SetRectPivot(RectTransform rect, float x, float y)
        {
            rect.SetRectPivot(new Vector2(x, y));
        }

        /// <summary>
        /// 设置锚点
        /// </summary>
        /// <param name="go"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public static void SetRectAnchor(GameObject go, float x1, float y1, float x2, float y2)
        {
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.SetRectAnchor(new Vector2(x1, y1), new Vector2(y2, y2));
        }

        public static void SetRectAnchor(Transform trans, float x1, float y1, float x2, float y2)
        {
            RectTransform rect = trans.GetComponent<RectTransform>();
            rect.SetRectAnchor(new Vector2(x1, y1), new Vector2(y2, y2));
        }

        public static void SetRectAnchor(RectTransform rect, float x1, float y1, float x2, float y2)
        {
            rect.SetRectAnchor(new Vector2(x1, y1), new Vector2(y2, y2));
        }

        /// <summary>
        /// 设置贴边
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="attach"></param>
        public static void SetRectAttachment(GameObject obj, string attach)
        {
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.SetRectAttachment(attach);
        }

        public static void SetRectAttachment(Transform trans, string attach)
        {
            RectTransform rect = trans.GetComponent<RectTransform>();
            rect.SetRectAttachment(attach);
        }

        public static void SetRectAttachment(RectTransform rect, string attach)
        {
            rect.SetRectAttachment(attach);
        }
        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="go"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void SetAnchoredPosition(GameObject go, float x, float y)
        {
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.SetAnchoredPosition(new Vector2(x, y));
        }

        public static void SetAnchoredPosition(Transform trans, float x, float y)
        {
            RectTransform rect = trans.GetComponent<RectTransform>();
            rect.SetAnchoredPosition(new Vector2(x, y));
        }

        public static void SetAnchoredPosition(RectTransform rect, float x, float y)
        {
            rect.SetAnchoredPosition(new Vector2(x, y));
        }

        /// <summary>
        /// 设置图片fill
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="attach"></param>
        public static void SetImageFillType(GameObject obj, string fillMethod, string origin)
        {
            Image img = obj.GetComponent<Image>();
            img.SetImageFillType(fillMethod, origin);
        }

        public static void SetImageFillType(Transform trans, string fillMethod, string origin)
        {
            Image img = trans.GetComponent<Image>();
            img.SetImageFillType(fillMethod, origin);
        }

        public static void SetImageFillType(Image img, string fillMethod, string origin)
        {
            img.SetImageFillType(fillMethod, origin);
        }

	}
}