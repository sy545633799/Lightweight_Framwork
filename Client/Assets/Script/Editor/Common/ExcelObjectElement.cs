// ========================================================
// des：
// author: 
// time：2020-09-21 20:35:46
// version：1.0
// ========================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Editor {
	public struct ExcelObjectElement
	{
		public string FieldType;
		public string FieldValue;

		public object GetValue()
		{
			string _type = string.Empty;
			if (FieldType == "int")
				try { return int.Parse(FieldValue); } catch (Exception) { return 0; }
			if (FieldType == "float")
				try { return float.Parse(FieldValue); } catch (Exception) { return 0f; }
			if (FieldType == "string")
				return FieldValue;
			return null;
			// 类型的处理(可以再封装)
		}
	}
}
