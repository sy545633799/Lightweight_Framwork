using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RenderPipelineTest : MonoBehaviour
{
	public Mesh cubeMesh;
	public Material pureColorMaterial;
	public Material skyboxMaterial;

	private Camera cam;
	private RenderTexture rt;

	private void Start()
	{

		//使用0时，渲染纹理不会创建Z缓冲区。
		//16表示至少16位Z缓冲区，没有模板缓冲区。24或32表示至少24位Z缓冲区和一个模板缓冲区。
		rt = new RenderTexture(Screen.width, Screen.height, 24);
		cam = GetComponent<Camera>();
	}

	//void OnPreRender()
	//{

	//}

	//private void OnRenderImage(RenderTexture source, RenderTexture destination)
	//{

	//}

	//MSAA和HDR会激活unity内部的渲染流程必须走OnRenderImage（存疑）,所以事先关闭
	private void OnPostRender()
	{
		Graphics.SetRenderTarget(rt);
		GL.Clear(true, true, Color.grey);

		CubeDraw.Draw(pureColorMaterial, cubeMesh);
		SkyboxDraw.Draw(cam, skyboxMaterial);

		Graphics.Blit(rt, cam.targetTexture);
	}

}
