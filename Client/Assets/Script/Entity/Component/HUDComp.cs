// ========================================================
// des: 血条
// author: shenyi
// time：2020-12-23 11:36:32
// version：1.0
// ========================================================

using UnityEngine;
using XLua;

namespace Game
{
	public class HUDComp : EntityComp
	{
		public override void OnAdd()
		{
			LuaTable table = behavior.StatusTable;

			//Debug.LogError($"{table.Get<int, int>(1)}:{table.Get<int, int>(2)}:{table.Get<int, int>(3)}:{table.Get<int, int>(4)}");


		}


		
	}
}
