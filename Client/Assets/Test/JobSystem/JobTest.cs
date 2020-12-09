
using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct MyJobSystem0 : IJob
{
	public int a;
	public int b;
	public NativeArray<int> result;

	public void Execute()
	{
		var index = 0;
		for (int i = 0; i < 1000000; i++)
		{
			index++;
		}
		result[0] = a + b;
	}
}

public struct MyJobSystem1 : IJob
{
	public int a;

	public NativeArray<int> result;

	public void Execute()
	{
		var index = 0;
		for (int i = 0; i < 1000000; i++)
		{
			index++;
		}
		result[0] = a + 1;
	}
}

public class JobTest
{
	private static Stopwatch stopwatch = new Stopwatch();

	public static void Test04()
	{
		NativeArray<int> result0 = new NativeArray<int>(1, Allocator.TempJob);
		NativeArray<int> result1 = new NativeArray<int>(1, Allocator.TempJob);

		stopwatch.Start();

		//Job0
		MyJobSystem0 job0 = new MyJobSystem0();
		job0.a = 0;
		job0.b = 1;
		job0.result = result0;
		var handle0 = job0.Schedule();
		//Job1
		MyJobSystem1 job1 = new MyJobSystem1();
		job1.a = 100;
		job1.result = result1;
		var handle1 = job1.Schedule(handle0);
		handle1.Complete();

		stopwatch.Stop();

		UnityEngine.Debug.Log($"task {stopwatch.ElapsedMilliseconds}");
		UnityEngine.Debug.Log($"result0: {result0[0]}, result1: {result1[0]}");

		result0.Dispose();
		result1.Dispose();
	}


	/// <summary>
	/// 并行job
	/// </summary>
	public static void Test03()
	{
		NativeArray<int> result0 = new NativeArray<int>(1, Allocator.TempJob);
		NativeArray<int> result1 = new NativeArray<int>(1, Allocator.TempJob);

		stopwatch.Start();

		//Job0
		MyJobSystem0 job0 = new MyJobSystem0();
		job0.a = 0;
		job0.b = 1;
		job0.result = result0;
		var handle0 = job0.Schedule();
		//Job1
		MyJobSystem1 job1 = new MyJobSystem1();
		job1.a = 100;
		job1.result = result1;
		var handle1 = job1.Schedule();

		handle0.Complete();
		handle1.Complete();

		stopwatch.Stop();
		UnityEngine.Debug.Log($"task {stopwatch.ElapsedMilliseconds}");

		result0.Dispose();
		result1.Dispose();
	}

	/// <summary>
	/// 使用Job依赖造成不并行
	/// </summary>
	public static void Test02()
	{
		NativeArray<int> result0 = new NativeArray<int>(1, Allocator.TempJob);

		stopwatch.Start();

		//Job0
		MyJobSystem0 job0 = new MyJobSystem0();
		job0.a = 0;
		job0.b = 1;
		job0.result = result0;
		var handle0 = job0.Schedule();
		//Job1
		MyJobSystem1 job1 = new MyJobSystem1();
		job1.a = 100;
		job1.result = result0;
		var handle1 = job1.Schedule(handle0);

		handle0.Complete();
		handle1.Complete();

		stopwatch.Stop();
		UnityEngine.Debug.Log($"task {stopwatch.ElapsedMilliseconds} : {result0[0]}");
		result0.Dispose();
	}

	/// <summary>
	/// 比较多线程和直接执行的消耗
	/// </summary>
	public static void Start()
	{
		NativeArray<int> result = new NativeArray<int>(2, Allocator.TempJob);
		MyJobSystem0 job0 = new MyJobSystem0();
		job0.a = 0;
		job0.b = 1;
		job0.result = result;

		stopwatch.Start();
		JobHandle handle = job0.Schedule();
		handle.Complete();
		stopwatch.Stop();
		UnityEngine.Debug.Log($"task {stopwatch.ElapsedMilliseconds} : {result[0]} : {result[1]}");
		UnityEngine.Debug.LogError(result.Length);
		result.Dispose();

		//stopwatch.Reset();
		//stopwatch.Start();
		//var index = 0;
		//for (int i = 0; i < 1000000; i++)
		//{
		//	index++;
		//}
		//stopwatch.Stop();
		//UnityEngine.Debug.Log($"task {stopwatch.ElapsedMilliseconds}");
	}
}