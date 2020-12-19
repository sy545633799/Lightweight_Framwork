// ========================================================
// des：同步位置, 朝向
// author: shenyi
// time：2020-12-19 17:57:21
// version：1.0
// ========================================================

using UnityEngine;

namespace Game
{
	public class SyncTransComp : EntityComp
	{
		
		public void Sync(aoi_trans status)
		{
			Debug.LogError(status.forward);
		}
	}
}
