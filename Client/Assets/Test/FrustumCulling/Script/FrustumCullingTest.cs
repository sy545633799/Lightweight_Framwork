using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[ExecuteInEditMode]
public class FrustumCullingTest : MonoBehaviour
{
	public MeshRenderer cube;
	public Camera camera1;
	private Bounds bounds;
	
    void Start()
    {
		bounds = cube.bounds;

		float3 test = new float3(-1, -1, -1);
	}

    void Update()
    {
		if (camera1.IsBoundsInCamera(cube.bounds))
			cube.sharedMaterial.color = new Color(0, 0.5f, 0.8f);
		else
			cube.sharedMaterial.color = new Color(1, 1, 1);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(cube.bounds.center, cube.bounds.size);
	}
}
