// ========================================================
// des：
// author: 
// time：2020-12-23 11:51:01
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
	public class NameComp : EntityComp
	{
		protected override int compIndex => CompIndex.Name;

		public override void OnAdd()
		{
			
		}

		public void SetName(int nameId) => SetName(UINamesAsset.Get(nameId).Text);

		public void SetName(string name)
		{
			Debug.LogError(name);

		}
	}
}
