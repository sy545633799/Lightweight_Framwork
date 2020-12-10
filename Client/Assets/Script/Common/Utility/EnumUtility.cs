// ========================================================
// des：
// author: 
// time：2020-10-24 16:58:06
// version：1.0
// ========================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Game {
	public class EnumUtility
	{

		/// <summary>
		/// 动态创建枚举
		/// </summary>
		/// <param name="enumDictionary">枚举元素列表</param>
		/// <param name="enumName">枚举名</param>
		/// <returns>Enum枚举</returns>
		public static Enum CreateEnum(Dictionary<string, int> enumDictionary, string enumName = "DefalutEnum")
		{
			if (enumDictionary == null || enumDictionary.Count <= 0)
				return null;

			AppDomain currentDomain = AppDomain.CurrentDomain;
			AssemblyName aName = new AssemblyName("Game");
			AssemblyBuilder ab = currentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
			ModuleBuilder mb = ab.DefineDynamicModule(aName.Name);
			if (string.IsNullOrEmpty(enumName))
			{
				enumName = "DefalutEnum";
			}
			EnumBuilder eb = mb.DefineEnum(enumName, TypeAttributes.Public, typeof(int));

			foreach (var item in enumDictionary)
			{
				eb.DefineLiteral(item.Key, item.Value);
			}

			Type finished = eb.CreateType();
			Enum eEnum = Activator.CreateInstance(finished) as Enum;
			return eEnum;
		}

		/// <summary>
		/// 动态创建枚举
		/// </summary>
		/// <param name="enumDictionary">枚举元素列表</param>
		/// <param name="enumName">枚举名</param>
		/// <returns>Enum枚举</returns>
		public static Enum CreateEnum(List<string> enumList, string enumName = "DefalutEnum")
		{
			if (enumList == null || enumList.Count <= 0)
				return null;
			
			AppDomain currentDomain = AppDomain.CurrentDomain;
			AssemblyName aName = new AssemblyName("Game");
			AssemblyBuilder ab = currentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
			ModuleBuilder mb = ab.DefineDynamicModule(aName.Name);
			if (string.IsNullOrEmpty(enumName))
			{
				enumName = "DefalutEnum";
			}
			EnumBuilder eb = mb.DefineEnum(enumName, TypeAttributes.Public, typeof(int));

			for (int i = 0; i < enumList.Count; i++)
			{
				eb.DefineLiteral(enumList[i], i);
			}
			Type finished = eb.CreateType();
			Enum eEnum = Activator.CreateInstance(finished) as Enum;
			return eEnum;
		}
	}
}
