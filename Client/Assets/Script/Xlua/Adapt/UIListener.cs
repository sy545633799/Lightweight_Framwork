using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Game
{
    public static class UIListener
    {
        public static void AddClick(UIButton button, UnityAction action)
        {
            button.onClick.AddListener(action);
        }

        public static void RemoveClick(UIButton button, UnityAction action)
        {
            button.onClick.RemoveListener(action);
        }

        public static void RemoveAllClick(UIButton button)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}