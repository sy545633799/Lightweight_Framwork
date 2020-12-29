// ========================================================
// des：
// author: 
// time：2020-12-29 15:56:42
// version：1.0
// ========================================================

using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Game
{
    [Serializable]
    [TrackClipType(typeof(SkillShot))]
    [TrackBindingType(typeof(GameObject))]
    [TrackColor(1f, 1f, 1f)]
    public class SkillTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject owner, int inputCount)
        {
            ScriptPlayable<SkillMixer> behaviour = ScriptPlayable<SkillMixer>.Create(graph, inputCount);
            return behaviour;
        }
    }
}
