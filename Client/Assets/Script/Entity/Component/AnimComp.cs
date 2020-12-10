// ========================================================
// des：
// author: 
// time：2020-07-09 13:14:43
// version：1.0
// ========================================================

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{

    public class AnimComp : EntityComp
    {
        public AnimComp () { }
		public AnimComp (EntityBehavior behavior) : base (behavior) { }

		public new AnimComp Downcast () {
			return this;
		}
		private MoveComp m_MoveComp = null;
		private MoveComp moveComp {
			get {
				if (m_MoveComp == null) {
					m_MoveComp = behavior.GetEntityComp<MoveComp>() as MoveComp;
				}
				return m_MoveComp;
			}
			set {
				m_MoveComp = value;
			}
		}

		/// <summary>
		/// entiity的tranfrom
		/// </summary>
		private Transform cacheTrans;

		/// <summary>
		/// 是否在跳跃
		/// </summary>
		public bool jumpFlag = false;

		#region // ********** 人物动画相关 ***********

		/// <summary>
		/// 融合时间
		/// </summary>
		private float crossFadeTime = 0.15f;

		/// <summary>
		/// Move层
		/// </summary>
		public string secondLayerName {
			private set { }
			get {
				return "Move";
			}
		}

		/// <summary>
		/// 主动画状态机
		/// </summary>
		private Animator animator = null;

		/// <summary>
		/// 保存主状态机里面所有的animationClip.name 与 animationClip 对应关系的字典 
		/// 如果 不同的层级存在相同的clip 则只记录一个
		/// </summary>
		private Dictionary<string, AnimationClip> animationClipMap = new Dictionary<string, AnimationClip> ();

		/// <summary>
		/// 各个身体部位名称 、Animator的对应关系	
		/// 这里是为了表情动画添加的 但目前来说貌似用不到这种形式
		/// <animator.trandform.name, animator> 这种形式
		/// </summary>
		private Dictionary<string, Animator> allPartAnimator = new Dictionary<string, Animator> ();

		/// <summary>
		/// 正在播放的state的name
		/// </summary>
		public string currentAnimStateName;

		/// <summary>
		/// 开始播放动画时的时间
		/// </summary>
		private float currentAnimTime;

		/// <summary>
		/// 当前在播放的循环动画 不存在时为string.Empty
		/// </summary>
		private string loopAnimName;

		/// <summary>
		/// 循环动画播放的时间 毫秒级
		/// </summary>
		private float loopAnimTime = 0;

		/// <summary>
		/// 默认的动画
		/// </summary>
		public string defaultAnimation = "idle";

		/// <summary>
		/// 人物移动时的动画
		/// </summary>
		public string runAnimation = "run";

		/// <summary>
		/// 死亡动画
		/// </summary>
		public string dieAnimation = "die";

		/// <summary>
		/// 受击动画
		/// </summary>
		public string behitAnimation = "hit";

		#endregion

		#region 一些临时处理的操作

		///--------------------------- 临时的处理
		/// <summary>
		/// 御剑时的默认动画
		/// </summary>
		private string rideSwordDefaultRunAnim = "yujianwalk";

		/// <summary>
		/// 御剑时的默认动画
		/// </summary>
		private string rideSwordDefaultIdelAnim = "yujianstand";

		/// <summary>
		/// 上御剑
		/// </summary>
		private string rideSwordJumpOnAnim = "yujianjumpon";

		/// <summary>
		/// 下御剑
		/// </summary>
		private string rideSwordJumoOffAnim = "yujianjumpoff";

		private string rideSwordUpAnim = "yujianup";
		private string rideSwordDownAnim = "yujiandown";

		#endregion

		#region 速度相关的

		/// <summary>
		/// 逻辑速度
		/// 直接用于给animator设置speed
		/// </summary>
		public float LogicSpeed {
			get {
				return behavior.GetLogicSpeed ();
			}
		}

		/// <summary>
		/// 攻击动画的速度 要乘上的系数 attackAnimSpeed
		/// </summary>
		private float attackAnimSpeed = 1f;

		/// <summary>
		/// 设置攻击动画的速度
		/// </summary>
		/// <param name="speed"></param>
		public void SetAttackAnimSpeed (float speed) {
			attackAnimSpeed = speed;
		}

		/// <summary>
		/// 设置包含指定clip的，主animator的速度，以及所有的身体部位的animator的速度
		/// </summary>
		/// <param name="stateName">指定的animatorState.name</param>
		/// <param name="speed"></param>
		public void SetAnimSpeed (string stateName, float speed) {
			if (animator != null) {
				if (animationClipMap.ContainsKey (stateName))
					animator.speed = speed;
				else
					return;
			}

			if (allPartAnimator != null && allPartAnimator.Count > 0) {
				foreach (var kv in allPartAnimator) {
					kv.Value.speed = speed;
				}
			}
		}

		/// <summary>
		/// 设置所有动画机的动画速度
		/// </summary>
		/// <param name="speed"></param>
		public void SetAllAnimSpeed (float speed) {
			if (allPartAnimator != null && allPartAnimator.Count > 0) {
				foreach (KeyValuePair<string, Animator> kv in allPartAnimator) {
					kv.Value.speed = speed;
				}
			}
		}

		/// <summary>
		/// 获取指定animator的动画速度
		/// </summary>
		/// <param name="stateName">animatorState name</param>
		/// <returns></returns>
		public float GetAnimSpeed (string stateName) {
			if (animator) {
				return animator.speed;
			}
			return -1;
		}

		#endregion

		#region 动画相关的其它方法

		/// <summary>
		/// 是否在移动
		/// </summary>
		public bool IsEntityMoving {
			get {
					if (moveComp != null) 
						return moveComp.IsMoving;
					return false;
			}
		}

		/// <summary>
		/// 根据 stateName 获取 animationClip 的 name
		/// 只有 animatorState 和 animationClip 一对一 的情况才能获取到 clipName
		/// </summary>
		/// <param name="stateName"></param>
		/// <returns></returns>
		public string GetAnimationClipName (string stateName) {
			if (animator) {
				if (animationClipMap.ContainsKey (stateName)) {
					return stateName; // 等于 clip.name
					//AnimationClip clip;
					//animationClipMap.TryGetValue(stateName, out clip);
					//return clip.name;
				}
			}
			return null;
		}

		/// <summary>
		/// 初始化 四个状态对应的动画片段
		/// </summary>
		/// <param name="table"></param>
		/// <param name="attackAnimSpeed"></param>
		/// <param name="reset"></param>
		public void InitAnims (XLua.LuaTable table, float attackAnimSpeed, bool reset) {
			this.defaultAnimation = table[1] as string;
			this.runAnimation = table[2] as string;
			this.dieAnimation = table[3] as string;
			this.behitAnimation = table[4] as string;
			this.attackAnimSpeed = attackAnimSpeed;
			if (reset) {
				Reset ();
			}
			table.Dispose ();
		}

		/// <summary>
		/// 是否存在对应的clip
		/// 仅对 animatorState 和 animationClip 一一对应的情况下有效
		/// </summary>
		/// <param name="stateName">stateName 也是 clipName </param>
		/// <returns></returns>
		public bool HasAnimationClip (string stateName) {
			return GetAnimationClip (stateName) != null;
		}

		/// <summary>
		/// 获取name对应的clip
		/// </summary>
		/// <param name="stateName"></param>
		/// <returns></returns>
		private AnimationClip GetAnimationClip (string stateName) {
			if (string.IsNullOrEmpty (stateName)) {
				return null;
			}

			if (animator != null && animationClipMap.ContainsKey (stateName)) {
				AnimationClip clip;
				if (animationClipMap.TryGetValue (stateName, out clip)) {
					return clip;
				}
			}
			return null;
		}

		/// <summary>
		/// 获取name对应的clip长度
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public float GetAnimationLength (string name) {
			AnimationClip clip = GetAnimationClip (name);
			if (clip) return clip.length;
			return 0;
		}

		/// <summary>
		/// 获取当前state对应的wrapMode
		/// </summary>
		/// <returns></returns>
		private bool IsCurrentAnimatorStateLoopWrapMode () {
			if (animator) {
				AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo (0);
				if (animatorStateInfo.loop) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 设置name 对应的 clip的wrapMode
		/// </summary>
		/// <param name="name"></param>
		/// <param name="warpMode"></param>
		public void SetAnimationWrapMode (string name, WrapMode warpMode) {
			AnimationClip clip = GetAnimationClip (name);
			if (clip)
				clip.wrapMode = warpMode;
		}

		/// <summary>
		/// 当前clip是否播放完
		/// 不再用此判断当前动画能否切换
		/// </summary>
		/// <returns></returns>
		private bool IsCurrentPlayOver () {

			if (animator != null) {
				float length = 0;
				if (IsAttackAnim (currentAnimStateName)) {
					if (currentAnimStateName.EndsWith ("_1")) {
						string tempName = currentAnimStateName.Remove (currentAnimStateName.Length - 1);
						//Debug.LogError("111 : " + tempName.Insert(tempName.Length, "1"));
						length = GetAnimationLength (tempName.Insert (tempName.Length, "1"));
						//Debug.LogError("222 : " + tempName.Insert(tempName.Length, "2"));
						length += (GetAnimationLength (tempName.Insert (tempName.Length, "2")));

					}
					//else if (currentAnimStateName.EndsWith("_2"))
					//{

					//}
					else {
						length = GetAnimationLength (currentAnimStateName);
					}
				} else {
					length = GetAnimationLength (currentAnimStateName);
				}

				if (Time.time >= currentAnimTime + (length / animator.speed - crossFadeTime)) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 当前片段是否在是攻击动画
		/// </summary>
		/// <param name="stateName"></param>
		/// <returns></returns>
		public bool IsAttackAnim (string stateName) {
			if (string.IsNullOrEmpty (stateName))
				return false;
			stateName = stateName.ToLower ();
			if (stateName.Contains ("attack") || stateName.Contains ("skill")) {
				return true;
			}
			return false;
		}

		/// <summary>
		/// 当前是否处于跳跃状态 根据动画状态判断
		/// </summary>
		public bool IsJumping {
			get {
				if (string.IsNullOrEmpty (currentAnimStateName))
					return false;

				string temp = currentAnimStateName.ToLower ();
				if (currentAnimStateName.Contains ("jump")) {
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// 当前是否在御剑 根据动画状态判断
		/// 上御剑 下御剑 御剑idle 御剑移动 都算作御剑
		/// <returns></returns>
		public bool IsRidingSword {
			get {
				if (string.IsNullOrEmpty (currentAnimStateName))
					return false;

				string temp = currentAnimStateName.ToLower ();
				if (currentAnimStateName.Contains ("yujian")) {
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// 播放当前默认动画 暂用于 跳跃、御剑状态结束时的切换  
		/// </summary>
		public void PlayDefaultAnimation () {
			PlayAnimation (defaultAnimation);
		}

		/// <summary>
		/// 播放动画 （包括 animation和animator） 注意 播放技能动画时
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="time">time>0 说明此动画是循环动画</param>
		/// <param name="bEvent">是否调用OnPlayAnimation回调</param>
		/// <param name="bEvent"/><param>
		/// <param name="layerName"/>播放的层级<param>
		public float PlayAnimation (string stateName, float time = 0, bool bEvent = true, string layerName = null) {
			if (animator == null)
				return 0;

			if (!HasAnimationClip (stateName)) {
				return 0;
			}

			int layerIndex;
			if (string.IsNullOrEmpty (layerName)) {
				layerIndex = 0;
			} else {
				layerIndex = animator.GetLayerIndex (layerName);
			}

			if (animator && animator.isActiveAndEnabled && animator.runtimeAnimatorController) {
				animator.CrossFade (stateName, crossFadeTime);
				//animator.Play(stateName, layerIndex);
				foreach (KeyValuePair<string, Animator> kv in allPartAnimator) {
					kv.Value.CrossFade (stateName, crossFadeTime);
					//kv.Value.Play(stateName, layerIndex);
				}
			}

			currentAnimStateName = stateName;
			currentAnimTime = Time.time;

			// if (OnPlayAnimation != null && bEvent)
			// {
			//     OnPlayAnimation(stateName);
			// }

			if (time > 0) {
				SetAnimationWrapMode (stateName, WrapMode.Loop);
				loopAnimName = stateName;
				loopAnimTime = time /= 1000;
				return time;
			} else {
				float length = GetAnimationLength (stateName);
				return length / animator.speed - crossFadeTime;
			}
		}

		/// <summary>
		/// 停止当前动画去播放默认动画
		/// </summary>
		/// <param name="name"></param>
		public void StopAnimation (string stateName) {
			if (currentAnimStateName == stateName && stateName != dieAnimation) // 死亡动作不允许中断
			{
				PlayAnimation (defaultAnimation);
			}
		}

		/// <summary>
		/// 主动中断当前技能
		/// 设置LoopMode
		/// 还有吗？停止当前动画？
		/// </summary>
		public void BreakCurrentSkill () {
			SetAnimationWrapMode (currentAnimStateName, WrapMode.Once); // 这个很有意思
			//PlayAnimation(runAnimation);
			//StopAnimation(currentAnim);
		}

		/// <summary>
		/// 设置animator的层级
		/// 这里的控制完全放在lua端来控制
		/// </summary>
		/// <param name="layerName"></param>
		/// <param name="weight"></param>
		public void SetAnimatorLayerWeight (string layerName, int weight) {
			if (animator && animator.isActiveAndEnabled && animator.runtimeAnimatorController) {
				int layer = animator.GetLayerIndex (layerName);
				animator.SetLayerWeight (layer, weight);
				Animator partAnimator;
				foreach (KeyValuePair<string, Animator> kv in allPartAnimator) {
					partAnimator = kv.Value;
					layer = partAnimator.GetLayerIndex (layerName);
					partAnimator.SetLayerWeight (layer, weight);
				}
			}
		}

		/// <summary>
		/// 直接播放对应动画，time对应的帧数
		/// 无法设置混合树的那些
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="time"></param>
		public void SetAnimation (string stateName, float time) {
			if (animator != null) {
				if (animationClipMap.ContainsKey (stateName)) {
					animator.PlayInFixedTime (stateName, 0, time);
				}
			}

			if (allPartAnimator != null && allPartAnimator.Count > 0) {
				foreach (var kv in allPartAnimator) {
					kv.Value.PlayInFixedTime (stateName, 0, time);
				}
			}
		}
		#endregion

		#region 走帧

		/// <summary>
		/// 重写的EntityComp中的方法 在EntityBehavior中调用
		/// </summary>
		/// <param name="deltaTime"></param>
		public override void OnUpdate (float deltaTime) {
			//控制循环动画的的停止
			if (loopAnimName != string.Empty) {
				if ((loopAnimTime -= deltaTime) <= 0) {
					// 停止循环
					loopAnimTime = 0;
					SetAnimationWrapMode (loopAnimName, WrapMode.Once);
					StopAnimation (loopAnimName);
					loopAnimName = string.Empty;
				}
			}

			// if (IsJumping) {
			// 	UpdateAnimJumping (deltaTime);
			// } else if (IsRidingSword) {
			// 	UpdateAnimRidingSword (deltaTime);
			// } else {
			UpdateAnimDefault (deltaTime);
			// }
		}

		/// <summary>
		/// 动画切换状态逻辑执行前的判断、循环动画的判断和速度设定
		/// </summary>
		/// <returns></returns>
		private bool UpdateAnimCheck () {
			if (animationClipMap.Count == 0) {
				return false;
			}

			if (currentAnimStateName == dieAnimation) //已死亡
			{
				return false;
			}

			bool iscurrentAttackAnim = IsAttackAnim (currentAnimStateName);
			if (iscurrentAttackAnim && !IsCurrentPlayOver ()) // 攻击还没有播完 
			{
				return false;
			}

			// 需要持续播放的动画
			// 持续动画的结束通过loopTime等变量控制
			// 这里通过的条件 =  一一对应 + 播放完毕 + 循环播放
			if (IsCurrentPlayOver () && IsCurrentAnimatorStateLoopWrapMode ()) {
				PlayAnimation (currentAnimStateName, 0, false);
				return false;
			}

			// 速度控制
			float aniSpeed = LogicSpeed;
			if (iscurrentAttackAnim) // 战斗状态 乘攻速
			{
				aniSpeed = LogicSpeed * attackAnimSpeed;
			}
			SetAnimSpeed (currentAnimStateName, aniSpeed);

			return true;
		}

		/// <summary>
		/// 默认状态下的动画切换逻辑
		/// </summary>
		private void UpdateAnimDefault (float deltaTime) {
			if (!UpdateAnimCheck ()) {
				return;
			}

			if (IsEntityMoving) {
				if (currentAnimStateName != runAnimation)
					PlayAnimation (runAnimation);

			} else {
				// 正在受击
				if (currentAnimStateName == behitAnimation) {
					if (IsCurrentPlayOver ()) { //结束受击
						PlayAnimation (defaultAnimation);
					}
				} else {
					if (currentAnimStateName != defaultAnimation)
						PlayAnimation (defaultAnimation, 0, false);
				}
			}

		}

		/// <summary>
		/// 跳跃状态下的动画切换逻辑
		/// </summary>
		/// <param name="deltaTime"></param>
		private void UpdateAnimJumping (float deltaTime) {
			if (!UpdateAnimCheck ()) {
				return;
			}

			// 未着地 + 当前播放完毕 + 当前播放不是 下落中
			if (behavior.characterController != null && !behavior.characterController.isGrounded && IsCurrentPlayOver () && !currentAnimStateName.Contains ("_")) // 这个 Contains临时处理
			{
				// 各个状态的配置表未操作 这里直接简单处理下
				PlayAnimation (currentAnimStateName + "_2"); // 临时处理
				return;
			}

			if (behavior.characterController != null && behavior.characterController.isGrounded && IsCurrentPlayOver () && currentAnimStateName.Contains ("_3")) // 这个 Contains临时处理
			{
				jumpFlag = false;
				if (IsEntityMoving) {
					PlayAnimation (runAnimation);
				} else {
					PlayAnimation (defaultAnimation);
				}
				return;
			}

		}

		/// <summary>
		/// 御剑状态下的动画切换逻辑
		/// </summary>
		/// <param name="deltaTime"></param>
		private void UpdateAnimRidingSword (float deltaTime) {
			if (!UpdateAnimCheck ()) {
				return;
			}

			if (IsEntityMoving) {

				if (currentAnimStateName != rideSwordDefaultRunAnim) {
					if (currentAnimStateName == rideSwordJumpOnAnim || currentAnimStateName == rideSwordJumoOffAnim) {
						if (IsCurrentPlayOver ()) {
							PlayAnimation (rideSwordDefaultRunAnim);
						}
					} else {
						PlayAnimation (rideSwordDefaultRunAnim);
					}
				}
			} else {
				if (currentAnimStateName != rideSwordDefaultIdelAnim) {
					if (currentAnimStateName == rideSwordUpAnim || currentAnimStateName == rideSwordDownAnim) {
						return;
					} else if (currentAnimStateName == rideSwordJumpOnAnim || currentAnimStateName == rideSwordJumoOffAnim) {
						if (IsCurrentPlayOver ()) {
							PlayAnimation (rideSwordDefaultIdelAnim);
						}
					} else {
						PlayAnimation (rideSwordDefaultIdelAnim);
					}
				}
			}
		}

		#endregion

		/// <summary>
		/// 起跳的瞬间
		/// 好像不太准确
		/// </summary>
		private void OnJump () {
		
		}

		/// <summary>
		/// 落地的瞬间 
		/// 好像不太准确
		/// </summary>
		private void OnLand () //async
		{
			if (IsJumping) {
				PlayAnimation (currentAnimStateName.Replace ("2", "3")); // 临时处理
				jumpFlag = true;
				Task task = Task.Run (async delegate {
					await Task.Delay (1300);
					jumpFlag = false;
				});
			} else if (!IsRidingSword) {
				PlayDefaultAnimation ();
			}
		}

		/// <summary>
		/// 添加组件时执行的逻辑
		/// 在外部逻辑被调用
		/// </summary>
		public override void OnAdd () {
			currentAnimStateName = null;
			behavior.onJump += OnJump;
			behavior.onLand += OnLand;
			jumpFlag = false;
			Reset (true);
		}

		/// <summary>
		/// 移除组件执行的逻辑
		/// </summary>
		public override void OnRemove () {
			currentAnimStateName = null;
			behavior.onJump -= OnJump;
			behavior.onLand -= OnLand;
			moveComp = null;
		}

		/// <summary>
		/// 重置 AnimComp 状态 功能类似一个Init方法
		/// </summary>
		/// <param name="OnAdd">是否在添加</param>
		public void Reset (bool OnAdd = false) {
			if (behavior.gameObject == null) {
				return;
			}
			behavior.gameObject.SetActive (true);

			if (behavior.Body != null) {
				cacheTrans = behavior.Body.transform;
			}
			if (cacheTrans == null)
				return;

			//获取动画组件
			animator = cacheTrans.GetComponent<Animator> ();
			Animator[] animators = cacheTrans.GetComponentsInChildren<Animator> (true);
			foreach (Animator partAnimator in animators) {
				if (animator != partAnimator && !allPartAnimator.ContainsKey (partAnimator.name)) {
					allPartAnimator.Add (partAnimator.name, partAnimator);
				}
			}

			if (animator == null) {
				return;
			} else if (OnAdd) //对 anim_ClipName 进行重新赋值
			{
				// 初始化clip容器
				animationClipMap.Clear ();

				if (animator && animator.isActiveAndEnabled && animator.runtimeAnimatorController) {
					// animatorClipMap
					AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
					if (clips != null) {
						for (int i = 0; i < clips.Length; i++) {
							/// 动画分层后 这里的clip 可能会重复 直接去重 不会有影响
							/// 这里不会添加 动画的混合树的部分
							if (!animationClipMap.ContainsKey (clips[i].name)) {
								animationClipMap.Add (clips[i].name, clips[i]);
							}
						}
					}
				}
			}

			// 播放默认动画 并记录时间
			StopAnimation (currentAnimStateName);
			currentAnimStateName = defaultAnimation;
			currentAnimTime = Time.time;
		}


    }
}