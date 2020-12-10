// ========================================================
// des：
// author: 
// time：2020-10-18 15:11:56
// version：1.0
// ========================================================

using System;
using System.Runtime.InteropServices;

namespace XLua
{
	public static class LuaEnvValues
	{
		public const int LUA_TNUMBER = 3;
		public const int LUA_TTABLE = 5;

		public const int LUA_TNUMFLT = (LUA_TNUMBER | (0 << 4));
		public const int LUA_TNUMINT = (LUA_TNUMBER | (1 << 4));
	}

	[StructLayout(LayoutKind.Explicit, Size = 12)]
	public struct LuaTValue
	{
		[FieldOffset(0)]
		public IntPtr p;
		[FieldOffset(0)]
		public UInt64 u64;
		[FieldOffset(0)]
		public double n;
		[FieldOffset(0)]
		public long i;
		[FieldOffset(8)]
		public int tt_;
	}
}
