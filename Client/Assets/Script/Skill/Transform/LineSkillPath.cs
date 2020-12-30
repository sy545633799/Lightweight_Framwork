// ========================================================
// des：
// author: 
// time：2020-12-30 20:12:45
// version：1.0
// ========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
	public class LineSkillPath : SkillPath
	{
		public override Vector3 OnUpdate(double time)
		{
			return StartPostion + Direction  * Speed * (float)time;
		}
	}
}
