// ========================================================
// des：
// author: 
// time：2020-12-29 15:56:42
// version：1.0
// ========================================================

using System;
using UnityEngine;
using UnityEngine.Playables;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
    [Serializable]
    public class SkillShot : PlayableAsset
    {
        public SkillShotPlayable template = new SkillShotPlayable();
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<SkillShotPlayable>.Create(graph, template);
            return playable;
        }

	}
}
