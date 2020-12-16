// ========================================================
// des：
// author: 
// time：2020-12-11 13:30:06
// version：1.0
// ========================================================

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
	public class MoveHelper
	{
		/// <summary>
		/// 将普通V3坐标映射到Navimesh上  [validateXZ 是否验证xz方向]
		/// （可以用来获得地形高度，但是最大距离不要太大，注意效率）
		/// </summary>
		public static Vector3 Util_SamplePosition(Vector3 pos, bool validateXZ = false)
		{
			UnityEngine.AI.NavMeshHit hit;
			if (UnityEngine.AI.NavMesh.SamplePosition(pos, out hit, 20, UnityEngine.AI.NavMesh.AllAreas))
			{
				if (validateXZ)
				{
					return hit.position;
				}
				else
				{
					pos.y = hit.position.y;
					return pos;
				}
			}
			return pos;
		}

		public static float MoveTowards(float current, float target, float maxDelta, float minOffset)
		{
			float val = Mathf.MoveTowards(current, target, maxDelta);
			if (Mathf.Abs(val - target) < minOffset) val = target;
			return val;
		}

		public static float Lerp(float current, float target, float maxDelta, float minOffset)
		{
			float val = Mathf.Lerp(current, target, maxDelta);
			if (Mathf.Abs(val - target) < minOffset) val = target;
			return val;
		}
	}
}
