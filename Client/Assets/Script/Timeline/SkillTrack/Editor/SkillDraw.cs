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
		[DrawGizmo(GizmoType.InSelectionHierarchy)]
		static void MyGizmo(PlayableDirector playable, GizmoType gizmoType)
		{
			//Gizmos.color = Color.red;   //绘制时颜色
			//Gizmos.DrawSphere(Vector3.zero, 1);  //参数1绘制坐标，参数2绘制半径


			PlayableAsset asset = playable.playableAsset;
			IEnumerable<PlayableBinding> bindings = asset.outputs;
			foreach (var item in bindings)
			{
				SkillTrack skillTrack = item.sourceObject as SkillTrack;
				if (skillTrack != null)
				{
					foreach (var item2 in skillTrack.GetClips())
					{
						var skillShot = item2.asset as SkillShot;
						SkillShotPlayable shotPlayable = skillShot.template;
						
					}
				}
			}

		}


	}
}
