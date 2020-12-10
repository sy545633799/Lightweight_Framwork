// ========================================================
// des：
// author: 
// time：2020-10-18 14:36:54
// version：1.0
// ========================================================

using System;
using System.Runtime.InteropServices;

namespace XLua {
	[StructLayout(LayoutKind.Sequential)]
	public struct RawLuaTable
	{
		//CommonHeader
		public IntPtr next;
		public byte tt;
		public byte marked;

		public byte flags;
		public byte lsizenode;
		public uint sizearray;
		public IntPtr array;
		public IntPtr node;
		public IntPtr lastfree;
		public IntPtr metatable;
		public IntPtr gclist;

		public unsafe int GetInt(int index)
		{
			if (index > 0 && index <= sizearray)
			{
				index = index - 1;
				LuaTValue* tv = (LuaTValue*)(array) + index;
				if (tv->tt_ == LuaEnvValues.LUA_TNUMINT)
				{
					return (int)tv->i;
				}
				else
				{
					return (int)tv->n;
				}
			}
			else
			{
				return 0;
			}
		}

		public unsafe void SetInt(int index, int Value)
		{
			if (index > 0 && index <= sizearray)
			{
				index = index - 1;
				LuaTValue* v = (LuaTValue*)(array) + index;
				v->i = Value;
				v->tt_ = LuaEnvValues.LUA_TNUMINT;
			}
		}

		public unsafe double GetDouble(int index)
		{
			if (index > 0 && index <= sizearray)
			{
				index = index - 1;
				LuaTValue* tv = (LuaTValue*)(array) + index;
				if (tv->tt_ == LuaEnvValues.LUA_TNUMINT)
				{
					return (double)tv->i;
				}
				else
				{
					return tv->n;
				}
			}
			else
			{
				return 0;
			}
		}

		public unsafe void SetDouble(int index, double Value)
		{
			if (index > 0 && index <= sizearray)
			{
				index = index - 1;
				LuaTValue* v = (LuaTValue*)(array) + index;
				v->n = Value;
				v->tt_ = LuaEnvValues.LUA_TNUMFLT;
			}
		}
	}
}
