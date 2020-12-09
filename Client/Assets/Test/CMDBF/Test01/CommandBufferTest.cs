using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CommandBufferTest : MonoBehaviour
{
	private CommandBuffer commandBuffer = null;
	public RenderTexture renderTexture = null;
	private Renderer targetRenderer = null;

	public Camera mainCamera = null;
	public GameObject Plane = null;
	public GameObject targetObject = null;
	public Material replaceMaterial = null;

	public Shader replaceShader = null;
	public Camera ShadowmapCamera;
	public RenderTexture ShadowmapTexture;

	private void Start()
	{
		
		targetRenderer = targetObject.GetComponentInChildren<Renderer>();
		//申请RT, 渲染纹理的深度位数(支持0, 16, 24/32)的精确度
		renderTexture = RenderTexture.GetTemporary(1024, 1024, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 4);
		//然后接受物体的材质使用这张RT作为主纹理
		Plane.GetComponent<Renderer>().sharedMaterial.mainTexture = renderTexture;

		commandBuffer = new CommandBuffer();
		//设置Command Buffer渲染目标为申请的RT
		commandBuffer.SetRenderTarget(renderTexture);
		//初始颜色设置为灰色
		commandBuffer.ClearRenderTarget(true, true, Color.gray);
		//绘制目标对象，如果没有替换材质，就用自己的材质
		//DrawRenderer的物体会被保存到RenderTarget中, 显示replaceMaterial材质的颜色
		commandBuffer.DrawRenderer(targetRenderer, replaceMaterial);
		//直接加入相机的CommandBuffer事件队列中
		mainCamera.AddCommandBuffer(CameraEvent.AfterForwardOpaque, commandBuffer);

		CommandBuffer commandBuffer1 = new CommandBuffer();
		//深度图测试
		ShadowmapCamera.SetReplacementShader(replaceShader, "RenderType");
		ShadowmapTexture = RenderTexture.GetTemporary(1024, 1024, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 4);
		ShadowmapCamera.targetTexture = ShadowmapTexture;
		ShadowmapCamera.AddCommandBuffer(CameraEvent.AfterForwardOpaque, commandBuffer1);
	}

	void OnDisable()
	{
		Renderer renderer = Plane.GetComponent<Renderer>();
		if (renderer)
			renderer.sharedMaterial.mainTexture = null;

		//移除事件，清理资源
		if (mainCamera && commandBuffer != null)
			mainCamera.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, commandBuffer);
		commandBuffer.Clear();
		renderTexture.Release();
		
	}
 
    //也可以在OnPreRender中直接通过Graphics执行Command Buffer，不过OnPreRender和OnPostRender只在挂在相机的脚本上才有作用！！！
    //void OnPreRender()
    //{
    //    //在正式渲染前执行Command Buffer
    //    Graphics.ExecuteCommandBuffer(commandBuffer);
    //}
}
