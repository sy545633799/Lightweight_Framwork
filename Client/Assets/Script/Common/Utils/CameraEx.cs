using UnityEngine;
using Unity.Mathematics;
using static Unity.Mathematics.math;


public static class CameraEx
{

	public static bool IsBoundsInCamera(Matrix4x4 matrix, Bounds bounds, float leftex = 0, float rightex = 0, float downex = 0, float upex = 0)
	{
		int code =
			matrix.ComputeOutCode(new Vector4(bounds.min.x, bounds.min.y, bounds.min.z, 1), leftex, rightex, downex, upex);
		code &=
			matrix.ComputeOutCode(new Vector4(bounds.min.x, bounds.min.y, bounds.max.z, 1), leftex, rightex, downex, upex);
		code &=
			matrix.ComputeOutCode(new Vector4(bounds.min.x, bounds.max.y, bounds.min.z, 1), leftex, rightex, downex, upex);
		code &=
			matrix.ComputeOutCode(new Vector4(bounds.min.x, bounds.max.y, bounds.max.z, 1), leftex, rightex, downex, upex);
		code &=
			matrix.ComputeOutCode(new Vector4(bounds.max.x, bounds.min.y, bounds.min.z, 1), leftex, rightex, downex, upex);
		code &=
			matrix.ComputeOutCode(new Vector4(bounds.max.x, bounds.min.y, bounds.max.z, 1), leftex, rightex, downex, upex);
		code &=
			matrix.ComputeOutCode(new Vector4(bounds.max.x, bounds.max.y, bounds.min.z, 1), leftex, rightex, downex, upex);
		code &=
			matrix.ComputeOutCode(new Vector4(bounds.max.x, bounds.max.y, bounds.max.z, 1), leftex, rightex, downex, upex);
		return code == 0;
	}

	/// <summary>
	/// 使用碰撞检测判断
	/// </summary>
	/// <param name="bounds"></param>
	/// <param name="camera"></param>
	/// <returns></returns>
	public static bool IsBoundsInCamera(this Camera camera, Bounds bounds, float leftex = 0, float rightex = 0, float downex = 0, float upex = 0)
	{
		Matrix4x4 matrix = camera.cullingMatrix;
		return IsBoundsInCamera(matrix, bounds, leftex, rightex, downex, upex);
	}

	private static System.Action<Plane[], Matrix4x4> _calculateFrustumPlanes_Imp;
	private static Plane[] planes = new Plane[6];
	public static Plane[] GetPlanes(this Camera camera)
	{
		if (_calculateFrustumPlanes_Imp == null)
		{
			var meth = typeof(GeometryUtility).GetMethod("Internal_ExtractPlanes", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic, null, new System.Type[] { typeof(Plane[]), typeof(Matrix4x4) }, null);
			if (meth == null) throw new System.Exception("Failed to reflect internal method. Your Unity version may not contain the presumed named method in GeometryUtility.");

			_calculateFrustumPlanes_Imp = System.Delegate.CreateDelegate(typeof(System.Action<Plane[], Matrix4x4>), meth) as System.Action<Plane[], Matrix4x4>;
			if (_calculateFrustumPlanes_Imp == null) throw new System.Exception("Failed to reflect internal method. Your Unity version may not contain the presumed named method in GeometryUtility.");
		}

		Matrix4x4 matrix = camera.cullingMatrix;
		_calculateFrustumPlanes_Imp.Invoke(planes, matrix);
		return planes;
	}

	/**********************************************************************************************************/
}
