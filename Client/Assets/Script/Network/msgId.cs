// ========================================================
// des: 消息id
// 0-->10000 服务器->客户端
// 10000-->20000 客户端->服务器
// >20000    RPC消息(ack = req + 1)
// author: shenyi
// time：2020-12-18 10:11:20
// version：1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class msgId
	{
		public const ushort sync_pos = 10101;
		public const ushort sync_status = 10102;
	}
}
