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
		private GameObject m_GameObject;
		public GameObject gameObject
		{
			get
			{
				return m_GameObject;
			}
			set
			{
				Path?.OnStart(value.transform);
				m_GameObject = value;
			}
		}

		public SKillShotTargetType Target = SKillShotTargetType.Enemy;
		public SkillShotSelectType Range = SkillShotSelectType.Single;
		public SkillShotCastType Cast = SkillShotCastType.Once;

		public SkillPath Path = new LineSkillPath();
		public SkillShape Shape = new CircleSkillShape();
		public SkillSelector Selector;

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			base.ProcessFrame(playable, info, playerData);

			Vector3 position = (Vector3)Path?.OnUpdate(playable.GetTime());
			Shape?.OnUpdate(position);
		}

		public override void OnGraphStop(Playable playable)
		{
			base.OnGraphStop(playable);
			Path?.OnStop();
		}
	}
}
