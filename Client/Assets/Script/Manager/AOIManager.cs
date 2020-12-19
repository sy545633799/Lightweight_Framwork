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

namespace Game
{
	public class AOIManager
	{
		private static sync_status sync_Status = new sync_status();

		public async static Task Init()
		{
			TcpManager.AddListener(msgId.sync_status, Sync_Status);
		}

		private static void Sync_Status(byte[] obj)
		{
			try
			{
				//sync_Status.init(obj);
				
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
			
		}

		public static void Dispose()
		{
			TcpManager.RemoveListener(msgId.sync_status);
		}
	}
}
