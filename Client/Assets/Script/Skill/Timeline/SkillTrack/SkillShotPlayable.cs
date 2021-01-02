// ========================================================
// des：
// author: 
// time：2020-12-29 15:56:42
// version：1.0
// ========================================================

using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Game
{
    [Serializable]
    public class SkillShotPlayable : PlayableBehaviour
    {
		[NonSerialized]
		public PlayableGraph graph;

		[Header("特效")]
		public GameObject Effect;
		[Header("技能施放初始偏移")]
		public Vector3 Offset;
		[Header("自己")]
		public ExposedReference<Transform> Self;
		[Header("目标")]
		public ExposedReference<Transform> Target;

		public SKillShotTargetType TargetType = SKillShotTargetType.Enemy;
		public SkillShotSelectType RangeType = SkillShotSelectType.Single;
		public SkillShotCastType CastType = SkillShotCastType.Once;

		public SkillPath Path = new FollowSkillPath();
		public SkillShape Shape = new SectorSkillShape();
		public SkillSelector Selector;

		private Transform m_Self;
		private Transform m_Target;

		public override void OnGraphStart(Playable playable)
		{
			base.OnGraphStart(playable);
			m_Self = Self.Resolve(graph.GetResolver());
			m_Target = Target.Resolve(graph.GetResolver());
			Path?.Start(m_Self, Offset);
		}

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			base.ProcessFrame(playable, info, playerData);
			float duration = (float)playable.GetDuration();
			float timeNow = (float)playable.GetTime();
			float deltaTime = (float)info.deltaTime;
			Vector3 potition = (Vector3)Path?.Update(m_Target.transform, duration, timeNow, deltaTime);

			//设置特效位置
			//Effect?.transform.position = potition;
		}

		public override void OnGraphStop(Playable playable)
		{
			base.OnGraphStop(playable);
		}

	}
}
