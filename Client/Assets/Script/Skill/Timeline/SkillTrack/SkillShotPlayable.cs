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
		//[NonSerialized]
		//[Header("自己")]
		public ExposedReference<Transform> Self;
		//[NonSerialized]
		//[Header("目标")]
		public ExposedReference<Transform> Target;
		
		public SKillShotTargetType TargetType = SKillShotTargetType.Enemy;
		public SkillShotSelectType RangeType = SkillShotSelectType.Single;
		public SkillShotCastType CastType = SkillShotCastType.Once;

		public SkillPath Path = new LineSkillPath();
		public SkillShape Shape = new TriangleSkillShape();
		public SkillSelector Selector;

		private Vector3 direction;
		private Transform m_Self;
		private Transform m_Target;

		public override void OnGraphStart(Playable playable)
		{
			base.OnGraphStart(playable);
			m_Self = Self.Resolve(graph.GetResolver());
			m_Target = Target.Resolve(graph.GetResolver());
			direction = m_Self.forward;
		}

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			base.ProcessFrame(playable, info, playerData);
			float duration = (float)playable.GetDuration();
			float timeNow = (float)playable.GetTime();
			float deltaTime = (float)info.deltaTime;
			Vector3 start = (Vector3)m_Self?.transform.position + Offset;
			Vector3 end = (Vector3)m_Target?.transform.position;
			Path?.Update(start, end, direction, duration, timeNow, deltaTime);

			//设置特效位置
			//Effect?.transform.position = Path.Position;
		}

		public override void OnGraphStop(Playable playable)
		{
			base.OnGraphStop(playable);
			
		}

	}
}
