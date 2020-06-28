using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public static class UIExtensions
    {
        /// <summary>
        /// 设置大小
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="size"></param>
        public static void SetRectSize(this RectTransform rect, Vector2 size)
        {
            rect.sizeDelta = size;
        }

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="go"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void SetAnchoredPosition(this RectTransform rect, Vector2 pos)
        {
            if (rect == null)
                return;
            rect.anchoredPosition = pos;
        }

        /// <summary>
        /// 设置轴心点，并重新调整坐标(保持相对位置不变)
        /// </summary>
        /// <param name="go"></param>
        /// <param name="pivot"></param>
        public static void SetRectPivot(this RectTransform rect, Vector2 pivot)
        {
            if (rect == null)
            {
                return;
            }
            Vector2 pos = rect.anchoredPosition;
            Vector2 piv = rect.pivot;

            rect.pivot = pivot;
            float dx = pivot.x - piv.x;
            float dy = pivot.y - piv.y;

            Vector2 adjustPos = new Vector2(pos.x + rect.sizeDelta.x * dx, pos.y + rect.sizeDelta.y * dy);
            rect.anchoredPosition = adjustPos;
        }


        /// <summary>
        /// 设置锚点
        /// </summary>
        /// <param name="tans"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static void SetRectAnchor(this RectTransform rect, Vector2 min, Vector2 max)
        {
            if (rect == null)
            {
                return;
            }
            Vector2 pos = rect.anchoredPosition;

            Vector2 anchorMin = rect.anchorMin;
            Vector2 anchorMax = rect.anchorMax;

            rect.anchorMin = min;
            rect.anchorMax = max;

            if (anchorMin.x == anchorMax.x && anchorMin.y == anchorMax.y &&
                rect.anchorMin.x == rect.anchorMax.x && rect.anchorMin.y == rect.anchorMax.y)
            {
                float wid = 1920;// rect.sizeDelta.x;
                float hei = 1080;// rect.sizeDelta.y;

                float dx = rect.anchorMin.x - anchorMin.x;
                float dy = rect.anchorMin.y - anchorMin.y;

                Vector2 adjustPos = new Vector2(pos.x - wid * dx, pos.y - hei * dy);
                rect.anchoredPosition = adjustPos;
            }
        }

        /// <summary>
        /// 设置贴边
        /// </summary>
        /// <param name="tans"></param>
        /// <param name="attach"></param>
        public static void SetRectAttachment(this RectTransform rect, string attach)
        {
            if (rect == null)
            {
                return;
            }
            SetRectAnchor(rect, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
            switch (attach)
            {
                //case "left":
                case "l":
                    SetRectAnchor(rect, new Vector2(0, 0.5f), new Vector2(0, 0.5f));
                    break;
                //case "left_bottom":
                case "lb":
                    SetRectAnchor(rect, new Vector2(0, 0), new Vector2(0, 0));
                    break;
                //case "bottom":
                case "b":
                    SetRectAnchor(rect, new Vector2(0.5f, 0), new Vector2(0.5f, 0));
                    break;
                //case "right_bottom":
                case "rb":
                    SetRectAnchor(rect, new Vector2(1, 0), new Vector2(1, 0));
                    break;
                //case "right":
                case "r":
                    SetRectAnchor(rect, new Vector2(1, 0.5f), new Vector2(1, 0.5f));
                    break;
                //case "right_top":
                case "rt":
                    SetRectAnchor(rect, new Vector2(1, 1), new Vector2(1, 1));
                    break;
                //case "top":
                case "t":
                    SetRectAnchor(rect, new Vector2(0.5f, 1), new Vector2(0.5f, 1));
                    break;
                //case "left_top":
                case "lt":
                    SetRectAnchor(rect, new Vector2(0, 1), new Vector2(0, 1));
                    break;
            }
        }


        /// <summary>
        /// 设置图片fill
        /// </summary>
        /// <param name="go"></param>
        /// <param name="fillMethod"></param>
        /// <param name="origin"></param>
        public static void SetImageFillType(this Image image, string fillMethod, string origin)
        {
            if (image != null)
            {
                image.type = Image.Type.Filled;
                if (fillMethod == "horizontal")
                {
                    image.fillMethod = Image.FillMethod.Horizontal;
                    if (origin == "left") image.fillOrigin = 0;
                    else if (origin == "right") image.fillOrigin = 1;
                }
                else if (fillMethod == "vertical")
                {
                    image.fillMethod = Image.FillMethod.Vertical;
                    if (origin == "bottom") image.fillOrigin = 0;
                    else if (origin == "top") image.fillOrigin = 1;
                }
                else if (fillMethod == "radial90")
                {
                    image.fillMethod = Image.FillMethod.Radial90;
                    if (origin == "bottom_left") image.fillOrigin = 0;
                    else if (origin == "top_left") image.fillOrigin = 1;
                    else if (origin == "top_right") image.fillOrigin = 2;
                    else if (origin == "bottom_right") image.fillOrigin = 3;
                    image.fillClockwise = false;
                }
                else if (fillMethod == "radial180")
                {
                    image.fillMethod = Image.FillMethod.Radial180;
                    if (origin == "bottom") image.fillOrigin = 0;
                    else if (origin == "left") image.fillOrigin = 1;
                    else if (origin == "top") image.fillOrigin = 2;
                    else if (origin == "right") image.fillOrigin = 3;
                    image.fillClockwise = false;
                }
                else if (fillMethod == "radial360")
                {
                    image.fillMethod = Image.FillMethod.Radial360;
                    if (origin == "bottom") image.fillOrigin = 0;
                    else if (origin == "right") image.fillOrigin = 1;
                    else if (origin == "top") image.fillOrigin = 2;
                    else if (origin == "left") image.fillOrigin = 3;
                    image.fillClockwise = false;
                }
            }
        }

        /// <summary>
        /// 增加outline滤镜
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="color"></param>
        public static void AddTextOutline(this Outline outline, Color color)
        {
            outline.effectColor = color;
        }

        /// <summary>
        /// 设置文本框居中格式
        /// </summary>
        /// <param name="text"></param>
        /// <param name="anchor"></param>
        public static void SetTextAlignment(this UIText text, int anchor)
        {
            if (text == null)
            {
                return;
            }
            text.alignment = (TextAlignmentOptions)anchor;
        }
    }
}