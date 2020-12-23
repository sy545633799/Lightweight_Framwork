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
    public class GameEntityBehaviorManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Game.EntityBehaviorManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 9, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Init", _m_Init_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateEntity", _m_CreateEntity_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Update", _m_Update_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "FixedUpdate", _m_FixedUpdate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DestroyEntity", _m_DestroyEntity_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DestroyAllEntity", _m_DestroyAllEntity_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetEntity", _m_GetEntity_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Cleanup", _m_Cleanup_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Game.EntityBehaviorManager gen_ret = new Game.EntityBehaviorManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Game.EntityBehaviorManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        System.Threading.Tasks.Task gen_ret = Game.EntityBehaviorManager.Init(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateEntity_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 7&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& translator.Assignable<System.Action<XLua.LuaTable>>(L, 7)) 
                {
                    int _sceneid = LuaAPI.xlua_tointeger(L, 1);
                    int _aoiId = LuaAPI.xlua_tointeger(L, 2);
                    int _resId = LuaAPI.xlua_tointeger(L, 3);
                    UnityEngine.Vector3 _bornPos;translator.Get(L, 4, out _bornPos);
                    float _orientation = (float)LuaAPI.lua_tonumber(L, 5);
                    int _entityType = LuaAPI.xlua_tointeger(L, 6);
                    System.Action<XLua.LuaTable> _onBodyCreated = translator.GetDelegate<System.Action<XLua.LuaTable>>(L, 7);
                    
                        Game.EntityBehavior gen_ret = Game.EntityBehaviorManager.CreateEntity( _sceneid, _aoiId, _resId, _bornPos, _orientation, _entityType, _onBodyCreated );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    int _sceneid = LuaAPI.xlua_tointeger(L, 1);
                    int _aoiId = LuaAPI.xlua_tointeger(L, 2);
                    int _resId = LuaAPI.xlua_tointeger(L, 3);
                    UnityEngine.Vector3 _bornPos;translator.Get(L, 4, out _bornPos);
                    float _orientation = (float)LuaAPI.lua_tonumber(L, 5);
                    int _entityType = LuaAPI.xlua_tointeger(L, 6);
                    
                        Game.EntityBehavior gen_ret = Game.EntityBehaviorManager.CreateEntity( _sceneid, _aoiId, _resId, _bornPos, _orientation, _entityType );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.EntityBehaviorManager.CreateEntity!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.EntityBehaviorManager.Update(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FixedUpdate_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.EntityBehaviorManager.FixedUpdate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DestroyEntity_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _aoiId = LuaAPI.xlua_tointeger(L, 1);
                    
                    Game.EntityBehaviorManager.DestroyEntity( _aoiId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DestroyAllEntity_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.EntityBehaviorManager.DestroyAllEntity(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetEntity_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _uid = LuaAPI.xlua_tointeger(L, 1);
                    
                        Game.EntityBehavior gen_ret = Game.EntityBehaviorManager.GetEntity( _uid );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Cleanup_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.EntityBehaviorManager.Cleanup(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
