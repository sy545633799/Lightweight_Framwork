using UnityEngine;

public class CubeDraw
{

	public static void Draw(Material pureColorMaterial, Mesh cubeMesh)
	{
		//pureColorMaterial.color = new Color(0, 0.5f, 0.8f);
		pureColorMaterial.SetPass(0);
		for (int i = -5; i <= 5; i++)
		{
			for (int j = -5; j <= 5; j++)
			{
				Vector3 postion = new Vector3(i + i % 2, j + j % 2, (i + j) % 2);
				Matrix4x4 mt = Matrix4x4.TRS(postion, Quaternion.identity, 0.5f * Vector3.one);
				Graphics.DrawMeshNow(cubeMesh, mt);
			}
		}
	}
}