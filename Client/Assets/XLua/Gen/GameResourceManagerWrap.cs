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


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class GameResourceManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Game.ResourceManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 11, 2, 2);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadPrefab", _m_LoadPrefab_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LuaLoadPrefab", _m_LuaLoadPrefab_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnloadPrefab", _m_UnloadPrefab_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadPrefabFromePool", _m_LoadPrefabFromePool_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RecyclePrefab", _m_RecyclePrefab_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadAsset", _m_LoadAsset_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LuaLoadUI", _m_LuaLoadUI_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnloadAsset", _m_UnloadAsset_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadSprite", _m_LoadSprite_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Cleanup", _m_Cleanup_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "m_Pools", _g_get_m_Pools);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "m_Id2PoolDic", _g_get_m_Id2PoolDic);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "m_Pools", _s_set_m_Pools);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "m_Id2PoolDic", _s_set_m_Id2PoolDic);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Game.ResourceManager gen_ret = new Game.ResourceManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Game.ResourceManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadPrefab_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    
                        System.Threading.Tasks.Task<UnityEngine.GameObject> gen_ret = Game.ResourceManager.LoadPrefab( _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LuaLoadPrefab_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    
                        UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.GameObject> gen_ret = Game.ResourceManager.LuaLoadPrefab( _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadPrefab_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& translator.Assignable<UnityEngine.GameObject>(L, 1)) 
                {
                    UnityEngine.GameObject _object = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    
                        bool gen_ret = Game.ResourceManager.UnloadPrefab( _object );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1&& translator.Assignable<UnityEngine.Transform>(L, 1)) 
                {
                    UnityEngine.Transform _transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    
                    Game.ResourceManager.UnloadPrefab( _transform );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.ResourceManager.UnloadPrefab!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadPrefabFromePool_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    
                        System.Threading.Tasks.Task<UnityEngine.GameObject> gen_ret = Game.ResourceManager.LoadPrefabFromePool( _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.GameObject>>(L, 2)) 
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    System.Action<UnityEngine.GameObject> _action = translator.GetDelegate<System.Action<UnityEngine.GameObject>>(L, 2);
                    
                    Game.ResourceManager.LoadPrefabFromePool( _path, _action );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.ResourceManager.LoadPrefabFromePool!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RecyclePrefab_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _object = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    
                    Game.ResourceManager.RecyclePrefab( _object );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAsset_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    
                        System.Threading.Tasks.Task<UnityEngine.Object> gen_ret = Game.ResourceManager.LoadAsset( _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.Object>>(L, 2)) 
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    System.Action<UnityEngine.Object> _action = translator.GetDelegate<System.Action<UnityEngine.Object>>(L, 2);
                    
                    Game.ResourceManager.LoadAsset( _path, _action );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.ResourceManager.LoadAsset!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LuaLoadUI_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 1);
                    
                        XLua.LuaTable gen_ret = Game.ResourceManager.LuaLoadUI( _id );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadAsset_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Object _object = (UnityEngine.Object)translator.GetObject(L, 1, typeof(UnityEngine.Object));
                    
                    Game.ResourceManager.UnloadAsset( _object );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadSprite_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.Sprite>>(L, 3)) 
                {
                    string _atlasName = LuaAPI.lua_tostring(L, 1);
                    string _spriteName = LuaAPI.lua_tostring(L, 2);
                    System.Action<UnityEngine.Sprite> _action = translator.GetDelegate<System.Action<UnityEngine.Sprite>>(L, 3);
                    
                    Game.ResourceManager.LoadSprite( _atlasName, _spriteName, _action );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _atlasName = LuaAPI.lua_tostring(L, 1);
                    string _spriteName = LuaAPI.lua_tostring(L, 2);
                    
                    Game.ResourceManager.LoadSprite( _atlasName, _spriteName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.ResourceManager.LoadSprite!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Cleanup_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.ResourceManager.Cleanup(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_Pools(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Game.ResourceManager.m_Pools);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_Id2PoolDic(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Game.ResourceManager.m_Id2PoolDic);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_Pools(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Game.ResourceManager.m_Pools = (System.Collections.Generic.Dictionary<string, System.Collections.Generic.Queue<UnityEngine.Object>>)translator.GetObject(L, 1, typeof(System.Collections.Generic.Dictionary<string, System.Collections.Generic.Queue<UnityEngine.Object>>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_Id2PoolDic(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Game.ResourceManager.m_Id2PoolDic = (System.Collections.Generic.Dictionary<int, string>)translator.GetObject(L, 1, typeof(System.Collections.Generic.Dictionary<int, string>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
