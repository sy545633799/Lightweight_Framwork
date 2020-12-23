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
    public class GameTcpManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Game.TcpManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 21, 10, 9);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Init", _m_Init_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Update", _m_Update_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "FixedUpdate", _m_FixedUpdate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReConnect", _m_ReConnect_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Connect", _m_Connect_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OnConnect", _m_OnConnect_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SendBytes", _m_SendBytes_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddListener", _m_AddListener_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RemoveListener", _m_RemoveListener_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OnRead", _m_OnRead_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OnDisconnected", _m_OnDisconnected_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OnWrite", _m_OnWrite_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OnReceive", _m_OnReceive_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RemainingBytes", _m_RemainingBytes_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OnReceivedMessage", _m_OnReceivedMessage_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "StopReconnect", _m_StopReconnect_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Close", _m_Close_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Dispose", _m_Dispose_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MinLuaNetSessionID", Game.TcpManager.MinLuaNetSessionID);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MaxLuaNetSessionID", Game.TcpManager.MaxLuaNetSessionID);
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "LogMessageInConsole", _g_get_LogMessageInConsole);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ServerTime", _g_get_ServerTime);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "HeartTime", _g_get_HeartTime);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "NetworkDelayInMs", _g_get_NetworkDelayInMs);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Heart", _g_get_Heart);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "OnConnectEventCallBack", _g_get_OnConnectEventCallBack);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "OnReceiveMsgCallBack", _g_get_OnReceiveMsgCallBack);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "timeoutMiliSecond", _g_get_timeoutMiliSecond);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "maxReconnectTime", _g_get_maxReconnectTime);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "TimeStamp", _g_get_TimeStamp);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ServerTime", _s_set_ServerTime);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "HeartTime", _s_set_HeartTime);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "NetworkDelayInMs", _s_set_NetworkDelayInMs);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "Heart", _s_set_Heart);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "OnConnectEventCallBack", _s_set_OnConnectEventCallBack);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "OnReceiveMsgCallBack", _s_set_OnReceiveMsgCallBack);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "timeoutMiliSecond", _s_set_timeoutMiliSecond);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "maxReconnectTime", _s_set_maxReconnectTime);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "TimeStamp", _s_set_TimeStamp);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Game.TcpManager gen_ret = new Game.TcpManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Game.TcpManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        System.Threading.Tasks.Task gen_ret = Game.TcpManager.Init(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.TcpManager.Update(  );
                    
                    
                    
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
                    
                    Game.TcpManager.FixedUpdate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReConnect_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _host = LuaAPI.lua_tostring(L, 1);
                    int _port = LuaAPI.xlua_tointeger(L, 2);
                    int _timeoutMiliSecondx = LuaAPI.xlua_tointeger(L, 3);
                    int _maxReconnectTimex = LuaAPI.xlua_tointeger(L, 4);
                    
                    Game.TcpManager.ReConnect( _host, _port, _timeoutMiliSecondx, _maxReconnectTimex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Connect_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _host = LuaAPI.lua_tostring(L, 1);
                    int _port = LuaAPI.xlua_tointeger(L, 2);
                    int _timeoutMiliSecondx = LuaAPI.xlua_tointeger(L, 3);
                    
                    Game.TcpManager.Connect( _host, _port, _timeoutMiliSecondx );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    string _host = LuaAPI.lua_tostring(L, 1);
                    int _port = LuaAPI.xlua_tointeger(L, 2);
                    
                    Game.TcpManager.Connect( _host, _port );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.TcpManager.Connect!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnConnect_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.IAsyncResult _asr = (System.IAsyncResult)translator.GetObject(L, 1, typeof(System.IAsyncResult));
                    
                    Game.TcpManager.OnConnect( _asr );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendBytes_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    ushort _protoID = (ushort)LuaAPI.xlua_tointeger(L, 1);
                    byte[] _message = LuaAPI.lua_tobytes(L, 2);
                    ushort _rpcId = (ushort)LuaAPI.xlua_tointeger(L, 3);
                    
                    Game.TcpManager.SendBytes( _protoID, _message, _rpcId );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    ushort _protoID = (ushort)LuaAPI.xlua_tointeger(L, 1);
                    byte[] _message = LuaAPI.lua_tobytes(L, 2);
                    
                    Game.TcpManager.SendBytes( _protoID, _message );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Game.TcpManager.SendBytes!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddListener_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    ushort _msgId = (ushort)LuaAPI.xlua_tointeger(L, 1);
                    System.Action<byte[]> _callback = translator.GetDelegate<System.Action<byte[]>>(L, 2);
                    
                    Game.TcpManager.AddListener( _msgId, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveListener_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    ushort _msgId = (ushort)LuaAPI.xlua_tointeger(L, 1);
                    
                    Game.TcpManager.RemoveListener( _msgId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnRead_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.IAsyncResult _asr = (System.IAsyncResult)translator.GetObject(L, 1, typeof(System.IAsyncResult));
                    
                    Game.TcpManager.OnRead( _asr );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDisconnected_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Game.NetStateEvent _netState;translator.Get(L, 1, out _netState);
                    string _msg = LuaAPI.lua_tostring(L, 2);
                    
                    Game.TcpManager.OnDisconnected( _netState, _msg );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnWrite_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.IAsyncResult _r = (System.IAsyncResult)translator.GetObject(L, 1, typeof(System.IAsyncResult));
                    
                    Game.TcpManager.OnWrite( _r );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnReceive_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    byte[] _bytes = LuaAPI.lua_tobytes(L, 1);
                    int _length = LuaAPI.xlua_tointeger(L, 2);
                    
                    Game.TcpManager.OnReceive( _bytes, _length );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemainingBytes_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        long gen_ret = Game.TcpManager.RemainingBytes(  );
                        LuaAPI.lua_pushint64(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnReceivedMessage_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    ushort _protoID = (ushort)LuaAPI.xlua_tointeger(L, 1);
                    ushort _RPCID = (ushort)LuaAPI.xlua_tointeger(L, 2);
                    byte[] _cmd_byte = LuaAPI.lua_tobytes(L, 3);
                    
                    Game.TcpManager.OnReceivedMessage( _protoID, _RPCID, _cmd_byte );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopReconnect_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.TcpManager.StopReconnect(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Close_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.TcpManager.Close(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Dispose_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.TcpManager.Dispose(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LogMessageInConsole(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, Game.TcpManager.LogMessageInConsole);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ServerTime(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushint64(L, Game.TcpManager.ServerTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HeartTime(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushint64(L, Game.TcpManager.HeartTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_NetworkDelayInMs(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushint64(L, Game.TcpManager.NetworkDelayInMs);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Heart(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, Game.TcpManager.Heart);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnConnectEventCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Game.TcpManager.OnConnectEventCallBack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnReceiveMsgCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Game.TcpManager.OnReceiveMsgCallBack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_timeoutMiliSecond(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, Game.TcpManager.timeoutMiliSecond);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maxReconnectTime(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, Game.TcpManager.maxReconnectTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TimeStamp(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushint64(L, Game.TcpManager.TimeStamp);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ServerTime(RealStatePtr L)
        {
		    try {
                
			    Game.TcpManager.ServerTime = LuaAPI.lua_toint64(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_HeartTime(RealStatePtr L)
        {
		    try {
                
			    Game.TcpManager.HeartTime = LuaAPI.lua_toint64(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_NetworkDelayInMs(RealStatePtr L)
        {
		    try {
                
			    Game.TcpManager.NetworkDelayInMs = LuaAPI.lua_toint64(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Heart(RealStatePtr L)
        {
		    try {
                
			    Game.TcpManager.Heart = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnConnectEventCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Game.TcpManager.OnConnectEventCallBack = translator.GetDelegate<System.Action<ushort, ushort, byte[]>>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnReceiveMsgCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Game.TcpManager.OnReceiveMsgCallBack = translator.GetDelegate<System.Action<ushort, ushort, byte[]>>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_timeoutMiliSecond(RealStatePtr L)
        {
		    try {
                
			    Game.TcpManager.timeoutMiliSecond = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_maxReconnectTime(RealStatePtr L)
        {
		    try {
                
			    Game.TcpManager.maxReconnectTime = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TimeStamp(RealStatePtr L)
        {
		    try {
                
			    Game.TcpManager.TimeStamp = LuaAPI.lua_toint64(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
