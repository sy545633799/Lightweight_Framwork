using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace Game
{
	public class UIDropDown : TMP_Dropdown
	{
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            Canvas[] canvases = transform.GetComponentsInChildren<Canvas>();
            for(int i=0;i< canvases.Length;i++)
            {
                canvases[i].overrideSorting = false;
            }
        }
    }

}