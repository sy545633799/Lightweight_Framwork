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
		public GameObject Effect;
		private GameObject m_Effect;

		public int Num;
		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			base.ProcessFrame(playable, info, playerData);

			
		}
	}
}
