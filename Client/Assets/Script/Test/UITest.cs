using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class UITest : MonoBehaviour
    {
        void Start()
        {
            GetComponent<UIButton>().ondoubleClick.AddListener((time) => print(time));
        }

        void Update()
        {

        }
    }
}