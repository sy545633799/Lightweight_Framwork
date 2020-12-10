// ========================================================
// des：
// author: shenyi
// time：2020-05-01 13:40:17
// version：1.0
// ========================================================

#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;
using System;
using System.Text;

namespace Game
{
	using Utils = XLua.Utils;
	public class GameUtil
	{
		
		
		public static void __Register(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Game.GameUtil);
			Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);

			Utils.RegisterFunc(L, Utils.CLS_IDX, "SetText", SetText);

			Utils.EndClassRegister(type, L, translator);
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		static int __CreateInstance(RealStatePtr L)
		{
			return LuaAPI.luaL_error(L, "Game.GameUtil does not have a constructor!");
		}

		private static unsafe zstring lua_tozstring(RealStatePtr L, int index)
		{
			RealStatePtr strlen;
			IntPtr str = LuaAPI.lua_tolstring(L, index, out strlen);
			if (str != IntPtr.Zero)
			{
				int byteLen = strlen.ToInt32();
				int strLength = Encoding.UTF8.GetCharCount((byte*)str, byteLen);
				//从zstring对象池中获取string
				zstring zstr = zstring.get(strLength);
				string ret = zstr;
				fixed (char* chars = ret)
				{
					Encoding.UTF8.GetChars((byte*)str, byteLen, chars, strLength);
				}
				return zstr;
			}
			return null;
		}

		private static List<zstring> zstrings = new List<zstring>(10);
		private static void Resetzstrings(RealStatePtr L)
		{
			int gen_param_count = LuaAPI.lua_gettop(L);
			for (int index = 3; index <= gen_param_count; index++)
			{
				zstring zstr = null;
				LuaTypes type = LuaAPI.lua_type(L, index);
				if (LuaAPI.lua_isint64(L, index))
					zstr = LuaAPI.lua_toint64(L, index);
				else if (LuaAPI.lua_isinteger(L, index))
					zstr = LuaAPI.xlua_tointeger(L, index);
				else if (LuaAPI.lua_isnumber(L, index))
					zstr = (float)LuaAPI.lua_tonumber(L, index);
				else if (LuaTypes.LUA_TBOOLEAN == type)
					zstr = LuaAPI.lua_toboolean(L, index);
				else if (LuaTypes.LUA_TSTRING == type)
					zstr = lua_tozstring(L, index);
				zstrings[index - 3] = zstr;
			}
		}


		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		private static int SetText(RealStatePtr L)
		{
			try
			{
				ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				int gen_param_count = LuaAPI.lua_gettop(L);
				if (gen_param_count >=2 && gen_param_count <= 12)
				{
					if (!translator.Assignable<Game.UIText>(L, 1)) return LuaAPI.luaL_error(L, "invalid arguments to Game.GameUtil.SetText!");
					//获取Text
					Game.UIText _uIText = (Game.UIText)translator.GetObject(L, 1, typeof(Game.UIText));

					//当第二个参数是string时，表示要直接赋值
					if (LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)
					{
						using (zstring.Block())
						{
							zstring text = lua_tozstring(L, 2);
							_uIText.SetText(text, false);
						}
						return 0;
					}
					//第二个参数是int，表示要到UICodeText取值，如果参数数量大于3, 表示要需要格式化
					else if (LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TNUMBER)
					{
						int _id = LuaAPI.xlua_tointeger(L, 2);
						UICodeText codeText = UICodeTextAsset.Get(_id);
						if (gen_param_count == 2)
						{
							_uIText.SetText(codeText.Text, false);
						}
						//参数大于等于3，使用zstring格式化
						else
						{
							using (zstring.Block())
							{
								Resetzstrings(L);
								zstring text = null;
								if (gen_param_count == 3)
									text = zstring.Format(codeText.Text, zstrings[0]);
								if (gen_param_count == 4)
									text = zstring.Format(codeText.Text, zstrings[0], zstrings[1]);
								if (gen_param_count == 5)
									text = zstring.Format(codeText.Text, zstrings[0], zstrings[1], zstrings[2]);
								if (gen_param_count == 6)
									text = zstring.Format(codeText.Text, zstrings[0], zstrings[1], zstrings[2], zstrings[3]);
								if (gen_param_count == 7)
									text = zstring.Format(codeText.Text, zstrings[0], zstrings[1], zstrings[2], zstrings[3], zstrings[4]);
								if (gen_param_count == 8)
									text = zstring.Format(codeText.Text, zstrings[0], zstrings[1], zstrings[2], zstrings[3], zstrings[4], zstrings[5]);
								if (gen_param_count == 9)
									text = zstring.Format(codeText.Text, zstrings[0], zstrings[1], zstrings[2], zstrings[3], zstrings[4], zstrings[5], zstrings[6]);
								if (gen_param_count == 10)
									text = zstring.Format(codeText.Text, zstrings[0], zstrings[1], zstrings[2], zstrings[3], zstrings[4], zstrings[5], zstrings[6], zstrings[7]);
								if (gen_param_count == 11)
									text = zstring.Format(codeText.Text, zstrings[0], zstrings[1], zstrings[2], zstrings[3], zstrings[4], zstrings[5], zstrings[6], zstrings[7], zstrings[8]);
								if (gen_param_count == 12)
									text = zstring.Format(codeText.Text, zstrings[0], zstrings[1], zstrings[2], zstrings[3], zstrings[4], zstrings[5], zstrings[6], zstrings[7], zstrings[8], zstrings[9]);

								if (text != null)
									_uIText.SetText(text, false);
							}
						}

						return 0;
					}
					else
						return LuaAPI.luaL_error(L, "invalid arguments to Game.GameUtil.SetText!");
				}
				else
					return LuaAPI.luaL_error(L, "invalid arguments to Game.GameUtil.SetText!");
			}
			catch (System.Exception gen_e)
			{
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
		}

		

	}
}
