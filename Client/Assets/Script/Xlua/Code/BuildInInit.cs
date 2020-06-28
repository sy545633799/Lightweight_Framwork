namespace XLua.LuaDLL
{
	using System.Diagnostics;
	using System.Runtime.InteropServices;

    public partial class Lua
    {
        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_rapidjson(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadRapidJson(System.IntPtr L)
        {
            return luaopen_rapidjson(L);
        }

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_lpeg(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadLpeg(System.IntPtr L)
        {
            return luaopen_lpeg(L);
        }

        // [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        // public static extern int luaopen_protobuf_c(System.IntPtr L);

        // [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        // public static int LoadProtobufC(System.IntPtr L)
        // {
        //     return luaopen_protobuf_c(L);
        // }

        // [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        // public static extern int luaopen_pb(System.IntPtr L);

        // [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        // public static int LoadPb(System.IntPtr L)
        // {
        //     return luaopen_pb(L);
        // }

		[DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern int luaopen_sproto_core(System.IntPtr L);

		[MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
		public static int LoadSproto(System.IntPtr L)
		{
			return luaopen_sproto_core(L);
		}

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern int luaopen_crypt(System.IntPtr L);

		[MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
		public static int LoadCrypt(System.IntPtr L)
		{
			return luaopen_crypt(L);
		}
    }
}