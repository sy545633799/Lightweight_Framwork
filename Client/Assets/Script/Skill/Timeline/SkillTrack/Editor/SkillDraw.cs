// ========================================================
// des：
// author: 
// time：2020-12-29 16:20:20
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Game.Editor {
	
	public class SkillDraw
	{
		[DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.InSelectionHierarchy)]
		private static void Draw(PlayableDirector director, GizmoType gizmoType)
		{
			PlayableAsset asset = director.playableAsset;
			IEnumerable<PlayableBinding> bindings = asset.outputs;
			foreach (var item in bindings)
			{
				SkillTrack skillTrack = item.sourceObject as SkillTrack;
				if (skillTrack != null)
				{
					foreach (var item2 in skillTrack.GetClips())
					{
						//Debug.LogError($"{playable.time} >= {item2.start}");
						
						if (director.time >= item2.start && director.time <= item2.end)
						{
							var skillShot = item2.asset as SkillShot;
							SkillShotPlayable shotPlayable = skillShot.template;
							Vector3 position = (Vector3)shotPlayable.Path?.Position;
							Vector3 direction = (Vector3)shotPlayable.Path?.Direction;
							shotPlayable.Shape?.DrawGizmos(position, direction);

						
						}
					}
				}
			}

		}


	}
}
