using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowFpsInfo : MonoBehaviour
{

    private const string DISPLAY_TEXT_FORMAT = "{0} msf\n({1} FPS)";
    private const string MSF_FORMAT = "#.#";
    private const float MS_PER_SEC = 1000f;

    private string textField;
    private float fps = 60;

    void Awake()
    {
    }

    void Start()
    {

    }

    void LateUpdate()
    {
        float interp = Time.deltaTime / (0.5f + Time.deltaTime);
        float currentFPS = 1.0f / Time.deltaTime;
        fps = Mathf.Lerp(fps, currentFPS, interp);
        float msf = MS_PER_SEC / fps;
        textField = string.Format(DISPLAY_TEXT_FORMAT,
            msf.ToString(MSF_FORMAT), Mathf.RoundToInt(fps));
    }
    GUIStyle style = new GUIStyle();
    void OnGUI()
    {
        style.fontSize = 30;
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(0, 0, 200, 100), textField, style);
    }

}
