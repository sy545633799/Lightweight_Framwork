// ========================================================
// des：
// author: 
// time：2020-12-10 16:01:27
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	[System.Serializable]
	public struct CameraInfo
	{
		///* Camera 参数 */
		public Vector3 Position;                  // 位置
		public Vector3 Rotation;                  // 旋转  
		public float Depth;                       // 层级
		public float FieldOfView;                 // 角度
												  /* CameraController 参数 */
		public Vector3 PivotOffset;             // 焦点偏移
		public bool FixDistance;                // 是否固定距离
		public float MinDistance;               // 相机最小距离
		public float MaxDistance;               // 相机最大距离
		public float Distance;                  // 相机人物实际距离
		public float HidePlayerDistance;        // 隐藏人物距离
		public bool AllowXTilt;                 // 是否允许X轴旋转
		public bool AllowYTilt;                 // 是否允许Y轴旋转
		public float YMinLimit;                 // Y轴最小角度限制
		public float YMaxLimit;                 // Y轴最大角度限制
		public float XMinLimit;                 // x轴最小角度限制
		public float XMaxLimit;                 // x轴最大角度限制
		public float XSpeed;                    // X轴速度
		public float YSpeed;                    // Y轴速度
		public float TargetX;                   // 相机X轴角度
		public float TargetY;                   // 相机Y轴角度
		public float MinY;                      // 相机最小高度(防止看到地下)

	}
}
