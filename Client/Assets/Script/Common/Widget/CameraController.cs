// ========================================================
// des：
// author: 
// time：2020-07-13 15:18:44
// version：1.0
// ========================================================

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game {

	[System.Serializable]
	public class CameraController : MonoSingleton<CameraController>
	{
		private Camera m_Camera;
		private Transform m_Target;
		private bool isLock = false;
	
		//新增相机参数
		[Header("焦点偏移")]
		public Vector3 pivotOffset = new Vector3(0,1.2f,0);
		[Header("是否固定距离")]
		public bool fixDistance = false;
		private float viewDistance = 1f;
		[Header("相机最小距离")]
		public float minDistance = 2f;
		[Header("相机最大距离")]
		public float maxDistance = 30f;
		[Header("相机人物实际距离")]
		public float distance = 10.0f;
	    [Header("隐藏人物距离")]
	    public float HidePlayerDistance = 1.0f;
		[Header("是否允许X轴旋转")]
		public bool allowXTilt = true;
		[Header("是否允许Y轴旋转")]
		public bool allowYTilt = true;
		[Header("Y轴最小角度限制")]
		public float yMinLimit = -10f;
		[Header("Y轴最大角度限制")]
		public float yMaxLimit = 80f;

        [Header("x轴最小角度限制")]
        public float xMinLimit = -360;
        [Header("x轴最大角度限制")]
        public float xMaxLimit = 360;

        [Header("X轴速度")]
		public float xSpeed = 45.0f;
		[Header("Y轴速度")]
		public float ySpeed = 20.0f;
	    [Header("复位速度")]
	    private float resetSpeed = 0.2f;
	    [Header("相机X轴角度")]
		public float targetX = 270f;
		[Header("相机Y轴角度")]
		public float targetY = 45f;
		[Header("相机最小高度(防止看到地下)")][Range(0.1f, 1)]
		public float minY = 0.1f;
		[Header("碰撞层级")]
		public LayerMask layerMask;

		private float x = 0.0f;
		private float y = 0.0f;
		private float xVelocity = 1f;
		private float yVelocity = 1f;
		private float targetDistance = 0f;
		private float distanceVelocity = 1f;
	    
	
	    private bool hadOnDown = false;
	    private bool hadOnDrag = false;
        public bool IsBattleModel = false;

	    void Awake()
		{
			yMaxLimit = yMaxLimit > 80 ? 80 : yMaxLimit;
	        m_Camera = GetComponent<Camera>();
			x = targetX;
			y = targetY;
			targetDistance = distance;
	
	
			//TODO
			Vector2 lastPos = Vector2.zero;
			InputManager.onDownScene.AddListener(pos =>
			{
	            hadOnDown = true;
	            lastPos = pos;
                SetInputing(true);
            });
	
			InputManager.onDragScene.AddListener(pos =>
			{
	            hadOnDrag = true;
	            if (lastPos == Vector2.zero) return;
	
				if (Math.Abs(pos.x - lastPos.x) > Math.Abs(pos.y - lastPos.y))
				{
					if (allowXTilt)
					{
						float deltaX = Mathf.Clamp(pos.x - lastPos.x,-10, 10);
						targetX += deltaX * xSpeed * 0.01f;
                        if (IsBattleModel)
                            targetX = ClampAngle(targetX, xMinLimit, xMaxLimit);
                    }
				}
				else if (allowYTilt)
				{
					float deltaY = Mathf.Clamp(pos.y - lastPos.y, -10, 10);
					targetY -= deltaY * ySpeed * 0.01f;
					targetY = ClampAngle(targetY, yMinLimit, yMaxLimit);
				}
	
				lastPos = pos;
                SetInputing(true);
            });
	
			InputManager.onUpScene.AddListener(pos =>
			{
	            if (hadOnDown && hadOnDrag)
	            {
	                //Util.CallMethod("Game", "CameraOnEndDrag");
	                hadOnDown = false;
	                hadOnDrag = false;
	            }
	            lastPos = Vector2.zero;
                SetInputing(false);
            });
		}
	
		private float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360) angle += 360;
			if (angle > 360) angle -= 360;
			return Mathf.Clamp(angle, min, max);
		}
	
		void LateUpdate()
	    {
	        if (m_Target == null || isLock)
	        {
	            return;
	        }
	
	        x = Mathf.SmoothDampAngle(x, targetX, ref xVelocity, resetSpeed);
	        if (allowYTilt)
	            y = Mathf.SmoothDampAngle(y, targetY, ref yVelocity, resetSpeed);
	        else
	            y = targetY;
	        //x = targetX;
	        //y = targetY;
	
	        SetPosByTarget(m_Target.transform.position);

            CheckCameraFloat();

        }

		public void SetTarget(Transform target)
		{
			m_Target = target;
		}

        public Transform GetTarget()
        {
            return m_Target;
        }
	    
		public void SetPosByTarget(Vector3 target)
		{
			Vector3 rolePos = target + pivotOffset;
			Quaternion rotation = Quaternion.Euler(y, x, 0);
			Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + rolePos;
	
			RaycastHit hit;
			if (Physics.Linecast(rolePos, position, out hit, layerMask))
			{
	            Vector3 hitPoint = hit.point;
	            targetDistance = Mathf.SmoothDampAngle(targetDistance, Vector3.Distance(rolePos, hitPoint), ref distanceVelocity, 0.05f);
	        }
			else
				targetDistance = Mathf.SmoothDampAngle(targetDistance, distance, ref distanceVelocity, 0.05f);
	
			position = rotation * new Vector3(0.0f, 0.0f, -targetDistance) + rolePos;
	
			//防止看到地底
			position = position + Vector3.up * minY;
			this.transform.position = position;
			this.transform.rotation = rotation;
	    }

        public void SetPosNow(Vector3 target, Vector3 eulerAngle)
        {
            x = targetX;
            y = targetY;
            Vector3 rolePos = target + pivotOffset;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, eulerAngle);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + rolePos;

            position = rotation * new Vector3(0.0f, 0.0f, -targetDistance) + rolePos;

            //防止看到地底
            position = position + Vector3.up * minY;
            this.transform.position = position;
            this.transform.rotation = rotation;
        }

		#region 【设置模块接口】
		//X轴计算数据设置
		public void SetXPos(int Xpos)
	    {
	        targetX = Xpos;
	    }
	    //镜头距离
	    public void SetDistance(float CurDistance)
	    {
	        distance = CurDistance;
	    }
	    //X灵敏度
	    public void SetXSensitivity(float Speed)
	    {
	        xSpeed = Speed;
	    }
	    //Y灵敏度
	    public void SetYSensitivity(float Speed)
	    {
	        ySpeed = Speed;
	    }
	    //复位灵敏度
	    public void SetResetSensitivity(int Speed)
	    {
	        float RealSpeed = Speed * 1.0f / 100;
	        resetSpeed = Mathf.Clamp(RealSpeed, 0.05f, 0.5f);
	    }
	
	    #endregion
	    public void LockToPos(Vector3 position, Vector3 angle)
		{
			isLock = true;
			this.transform.position = position;
			this.transform.eulerAngles = angle;
		}
	
		public void UnLock()
		{
			isLock = false;
	    }
	    public void Reset(bool _initial = true)
		{
			#region 注释
			//if (null == target_)
			//{
			//    return;
			//}
	
			//if (Followparent)
			//{
			//    Movingtransform = cacheCamera.transform.parent.transform;
			//    cacheCamera.transform.localPosition = Vector3.zero;
			//}
			//else
			//    Movingtransform = cacheCamera.transform;
	
			//if (mesh != null) {
			//    mesh = null;
			//}
			//offsetDistance = 0;
			//offsetRotateX = 0;
			//if(_initial)
			//{
			//    initial = false;
			//    initialTime = false;
			//}
			//bReset = true;
			//Movingtransform.position = GetDestination(target_);
			//smooth = true;
			#endregion
		}
	
		public Vector3 ConvertDirByCam(Vector2 joysticDir)
		{
			Vector3 forward = m_Camera.transform.TransformDirection(Vector3.forward);//前后移动
			forward = new Vector3(forward.x, 0, forward.z);
			Vector3 verticle = this.transform.TransformDirection(Vector3.right);//左右移动
			verticle = new Vector3(verticle.x, 0, verticle.z);
	
			//Vector3 targetDir = forward * joysticDir.y + verticle * joysticDir.x;
            Vector3 targetDir = forward * joysticDir.y + verticle * joysticDir.x;
            return targetDir.normalized;
		}
	
	    public Vector3 GetRotation()
	    {
	        return m_Camera.transform.rotation.eulerAngles;
	    }
	
		public Vector3 GetFarestPoint()
		{
			//offsetDistance = 5;
			//smooth = false;
			return Vector3.zero;
		}

	
		public void OnDestroy()
	    {
	        //SwitchUIState = null;
	    }


        #region 相机飘浮
        [Serializable]
        private class CameraFloatAnimationAttribute
        {
            public enum Status
            {
                Wait,
                Playing,
                Over,
            }

            [Header("相机左右漂浮的动画曲线")]
            public AnimationCurve m_curve;
            [Header("相机左右漂浮总时长"), Range(0.1f, 1000)]
            public float m_duration = 10f;
            [Header("自动进入相机左右漂浮的等待秒数")]
            public float m_waitForSecond;
            [HideInInspector]
            public float m_tick = 0f;
            [HideInInspector]
            public Status m_status = Status.Wait;
        }
        [SerializeField, Header("相机缓动相关参数")]
        private CameraFloatAnimationAttribute m_cameraFloatAnimationAttribute;
        [SerializeField, Header("相机缓动的一些值(仅保留查看, 不要改)")]
        private CameraFloatField m_currentCameraFloatField = new CameraFloatField();

        [Serializable]
        private class CameraFloatField
        {
            public float m_targetAngle_X;
            public float m_startAngle_X;
            public float m_duration;
            public float m_length;
            public float GetValue(float tick, AnimationCurve curve)
            {
                var _normalTick = Mathf.Clamp01(tick / m_duration);
                var _normalValue = Mathf.Clamp01(curve.Evaluate(_normalTick));
                var _increment = m_length * _normalValue;
                return _normalTick >= 1 ? -1 : m_startAngle_X + _increment;
            }
        }

        public void CheckCameraFloat()
        {
            if (!IsBattleModel || m_pause)
            {
                return;
            }
            if (m_inputing)
            {
                m_cameraFloatAnimationAttribute.m_tick = 0f;
                m_cameraFloatAnimationAttribute.m_status = CameraFloatAnimationAttribute.Status.Wait;
                return;
            }
            if (m_cameraFloatAnimationAttribute.m_status == CameraFloatAnimationAttribute.Status.Wait)
            {
                if (m_cameraFloatAnimationAttribute.m_tick >= m_cameraFloatAnimationAttribute.m_waitForSecond)
                {
                    m_cameraFloatAnimationAttribute.m_status = CameraFloatAnimationAttribute.Status.Playing;
                    m_cameraFloatAnimationAttribute.m_tick = 0f;
                    InitializeCurrentCameraFloatField();
                }
            }
            if (m_cameraFloatAnimationAttribute.m_status == CameraFloatAnimationAttribute.Status.Playing)
            {
                var _angleX = m_currentCameraFloatField.GetValue(m_cameraFloatAnimationAttribute.m_tick, m_cameraFloatAnimationAttribute.m_curve);
                if (_angleX > 0)
                    targetX = _angleX;
                else
                    m_cameraFloatAnimationAttribute.m_status = CameraFloatAnimationAttribute.Status.Over;
            }
            if (m_cameraFloatAnimationAttribute.m_status == CameraFloatAnimationAttribute.Status.Over)
            {
                m_cameraFloatAnimationAttribute.m_tick = 0f;
                m_cameraFloatAnimationAttribute.m_status = CameraFloatAnimationAttribute.Status.Wait;
            }
            m_cameraFloatAnimationAttribute.m_tick += Time.deltaTime;
        }

        public void InitializeCurrentCameraFloatField()
        {
            var _min = targetX - xMinLimit;
            var _max = xMaxLimit - targetX;
            var _difference = xMaxLimit - xMinLimit;
            float _curValue = _min < _max ? _min : _max;
            var _normal = Mathf.Clamp01(_curValue / _difference);
            float _targetAngle_X = _min > _max ? xMinLimit : xMaxLimit;
            m_currentCameraFloatField.m_startAngle_X = targetX;
            m_currentCameraFloatField.m_targetAngle_X = _targetAngle_X;
            m_currentCameraFloatField.m_duration = m_cameraFloatAnimationAttribute.m_duration * (1 - _normal);
            m_currentCameraFloatField.m_length = _targetAngle_X - targetX;
        }

        private bool m_inputing = false;
        public bool m_pause = false;
        public void SetInputing(bool isDown)
        {
            if (m_inputing != isDown)
                m_cameraFloatAnimationAttribute.m_tick = 0f;
            m_inputing = isDown;
        }

        internal void ClearCameraFloatField()
        {
            m_currentCameraFloatField = new CameraFloatField();
        }

        #endregion
    }
}
