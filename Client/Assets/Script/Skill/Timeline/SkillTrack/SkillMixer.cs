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
    public class SkillMixer : PlayableBehaviour
    {
        public GameObject m_GameObject;

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (m_GameObject == null)
                m_GameObject = (GameObject)playerData;;
            if (m_GameObject == null)
                return;
            int inputCount = playable.GetInputCount();
            for (int i = 0; i < inputCount; i++)
            {
                float _weight = playable.GetInputWeight(i);
                ScriptPlayable<SkillShotPlayable> _shotPlayable = (ScriptPlayable<SkillShotPlayable>)playable.GetInput(i);
                SkillShotPlayable _shotbehaviour = _shotPlayable.GetBehaviour();
				
                float normalizedTime = (float)(_shotPlayable.GetTime() / _shotPlayable.GetDuration());
            }
        }
    }
}
