// ========================================================
// des：
// author: 
// time：2020-12-19 13:59:20
// version：1.0
// ========================================================

using UnityEngine;
using System.Threading.Tasks;
using Sproto;
using System;
using System.Collections.Generic;

namespace Game
{
	public class AOIManager
	{
		private static s2c_sync_trans sync_Status = new s2c_sync_trans();

		public async static Task Init()
		{
			TcpManager.AddListener(msgId.s2c_sync_trans, Sync_Status);
		}

		private static void Sync_Status(byte[] obj)
		{
			try
			{
				sync_Status.init(obj);
				Dictionary<long, EntityComp> comps = EntityCompFactory.Instance.GetComponentsInUse<SyncTransComp>();
				foreach (var aoiData in sync_Status.status.Values)
				{
					EntityComp comp = null;
					if (comps.TryGetValue(aoiData.aoiId, out comp))
						(comp as SyncTransComp).Sync(aoiData);
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}

		public static void Dispose()
		{
			TcpManager.RemoveListener(msgId.s2c_sync_trans);
		}
	}
}
