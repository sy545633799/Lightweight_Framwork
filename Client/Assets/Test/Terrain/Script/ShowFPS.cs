using UnityEngine;
using System.Collections;

public class ShowFPS : MonoBehaviour
{
    
    public float f_UpdateInterval = 0.5F;

    private float f_LastInterval;

    private int i_Frames = 0;

    private float f_Fps;

    GUIStyle titleStyle2 = new GUIStyle();
    void Start()
    {
        Application.targetFrameRate = 360;
        //int width = Screen.width;
        //int high = Screen.height;
        //if (high > 1334)
        //{
        //    float aspect = (float)(high) / (float)width;
        //    high = 1334;
        //    width = (int)(1334.0f / aspect);
        //}
        //Screen.SetResolution(width, high, true);



        f_LastInterval = Time.realtimeSinceStartup;

        i_Frames = 0;

        
        titleStyle2.fontSize = 20;
        titleStyle2.normal.textColor = new Color(255f / 256f, 0f / 256f, 0f / 256f, 256f / 256f);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 100, 200, 200), "FPS:" + f_Fps.ToString("f2"), titleStyle2);
    }

    void Update()
    {
        ++i_Frames;

        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

            i_Frames = 0;

            f_LastInterval = Time.realtimeSinceStartup;
        }
    }
}