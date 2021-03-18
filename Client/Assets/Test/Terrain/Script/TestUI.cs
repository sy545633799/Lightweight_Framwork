using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
	public Button Btn_Terrain;
	public Text Text_Terrain;
	
	public GameObject Terrain;
	public GameObject Mesh;

	public Slider slider;
	public SkinnedMeshRenderer render;

	private void Start()
	{
		Btn_Terrain.onClick.AddListener(() =>
		{
			if (Terrain.activeSelf)
			{
				Terrain.SetActive(false);
				Mesh.SetActive(true);
				Text_Terrain.text = "显示Terrain";
			}
			else
			{
				Terrain.SetActive(true);
				Mesh.SetActive(false);
				Text_Terrain.text = "显示Mesh";
			}
		});

		slider.onValueChanged.AddListener(val =>
		{
			render.sharedMaterial.SetFloat("_Rampthreshold", val);
		});
	}
}
