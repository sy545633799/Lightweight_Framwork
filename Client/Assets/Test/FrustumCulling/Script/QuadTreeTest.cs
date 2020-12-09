using System;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class QuadTreeTest : MonoBehaviour
{
	[BurstCompile(CompileSynchronously = true)]
	struct VelocityJob : IJobParallelFor
	{
		[ReadOnly]
		public NativeArray<Matrix4x4> matrix;
		[ReadOnly]
		public NativeArray<Bounds> bounds;
		//[ReadOnly]
		public NativeArray<bool> results;

		public void Execute(int index)
		{
			results[index] = CameraEx.IsBoundsInCamera(matrix[0], bounds[index]);
		}
	}

	public static int TESTCOUNT = 10000;
	System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

	void Update()
	{
		Test02();
	}

	public void TreeTest()
	{
		
	}
	

	public void Test01()
	{
		Camera camera = this.GetComponent<Camera>();
		Bounds[] bounds = new Bounds[TESTCOUNT];

		for (int i = 0; i < bounds.Length; i++)
			bounds[i] = new Bounds();
		stopwatch.Reset();
		stopwatch.Start();
		for (int i = 0; i < bounds.Length; i++)
			camera.IsBoundsInCamera(bounds[i]);
		stopwatch.Stop();
		UnityEngine.Debug.Log($"task {stopwatch.ElapsedMilliseconds}");
	}

	public void Test02()
	{
		Camera camera = this.GetComponent<Camera>();
		NativeArray<Matrix4x4> matrix = new NativeArray<Matrix4x4>(1, Allocator.TempJob);
		matrix[0] = camera.cullingMatrix;
		NativeArray<Bounds> bounds = new NativeArray<Bounds>(TESTCOUNT, Allocator.TempJob);
		for (int i = 0; i < TESTCOUNT; i++)
			bounds[i] = new Bounds();


		stopwatch.Reset();
		stopwatch.Start();
		NativeArray<bool> results = new NativeArray<bool>(TESTCOUNT, Allocator.TempJob);
		VelocityJob velocity = new VelocityJob() { matrix = matrix, bounds = bounds, results = results };
		JobHandle jobHandle = velocity.Schedule(TESTCOUNT, 1);
		jobHandle.Complete();

		stopwatch.Stop();
		UnityEngine.Debug.Log($"task {stopwatch.ElapsedMilliseconds}");

		matrix.Dispose();
		bounds.Dispose();
		results.Dispose();
	}

	public unsafe void Test03()
	{
		Camera camera = this.GetComponent<Camera>();
		Bounds* boundsex = (Bounds*)Marshal.AllocHGlobal(Marshal.SizeOf<Bounds>() * TESTCOUNT).ToPointer();
		ZeroMemory((byte*)boundsex, Marshal.SizeOf<Bounds>() * TESTCOUNT);

		for (int i = 0; i < TESTCOUNT; i++)
			boundsex[i] = new Bounds();
		stopwatch.Start();
		for (int i = 0; i < TESTCOUNT; i++)
			camera.IsBoundsInCamera(boundsex[i]);
		stopwatch.Stop();
		UnityEngine.Debug.Log($"task {stopwatch.ElapsedMilliseconds}");

		Marshal.FreeHGlobal(new IntPtr(boundsex));
	}

	public unsafe static void ZeroMemory(byte* ptr, int size)
	{
		for (int num = size - 1; num >= 0; num--)
		{
			ptr[num] = 0;
		}
	}
}
