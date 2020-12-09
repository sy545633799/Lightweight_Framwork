using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using XLua.LuaDLL;

[LuaCallCSharp]
public class XluaTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		LuaEnv luaenv = new LuaEnv();
		LuaTable luaTable = luaenv.NewTable();

		luaenv.Dispose();
	}


}
