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

		    Utils.BeginClassRegister(type, L, __CreateInstance, 23, 9, 8);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Init", _m_Init_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddEvent", _m_AddEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Update", _m_Update_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "FixedUpdate", _m_FixedUpdate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Connect", _m_Connect_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OnConnect", _m_OnConnect_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SendBytes", _m_SendBytes_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SendBytesWithoutSize", _m_SendBytesWithoutSize_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OnRead", _m_OnRead_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OnReadLine", _m_OnReadLine_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OnDisconnected", _m_OnDisconnected_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PrintBytes", _m_PrintBytes_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OnWrite", _m_OnWrite_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OnReceiveLine", _m_OnReceiveLine_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OnReceive", _m_OnReceive_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RemainingBytes", _m_RemainingBytes_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OnReceivedMessageLine", _m_OnReceivedMessageLine_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OnReceivedMessage", _m_OnReceivedMessage_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Close", _m_Close_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Dispose", _m_Dispose_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MinLuaNetSessionID", Game.TcpManager.MinLuaNetSessionID);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MaxLuaNetSessionID", Game.TcpManager.MaxLuaNetSessionID);
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "OnConnectCallBack", _g_get_OnConnectCallBack);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "OnDisConnectCallBack", _g_get_OnDisConnectCallBack);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "OnReceiveLineCallBack", _g_get_OnReceiveLineCallBack);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "OnReceiveMsgCallBack", _g_get_OnReceiveMsgCallBack);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "TimeStamp", _g_get_TimeStamp);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "onConnectCallBack", _g_get_onConnectCallBack);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "onDisConnectCallBack", _g_get_onDisConnectCallBack);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "onReceiveLineCallBack", _g_get_onReceiveLineCallBack);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "onReceiveMsgCallBack", _g_get_onReceiveMsgCallBack);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "OnConnectCallBack", _s_set_OnConnectCallBack);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "OnDisConnectCallBack", _s_set_OnDisConnectCallBack);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "OnReceiveLineCallBack", _s_set_OnReceiveLineCallBack);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "OnReceiveMsgCallBack", _s_set_OnReceiveMsgCallBack);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "onConnectCallBack", _s_set_onConnectCallBack);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "onDisConnectCallBack", _s_set_onDisConnectCallBack);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "onReceiveLineCallBack", _s_set_onReceiveLineCallBack);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "onReceiveMsgCallBack", _s_set_onReceiveMsgCallBack);
            
			
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
        static int _m_AddEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action<byte[]> __event = translator.GetDelegate<System.Action<byte[]>>(L, 1);
                    byte[] _data = LuaAPI.lua_tobytes(L, 2);
                    
                    Game.TcpManager.AddEvent( __event, _data );
                    
                    
                    
                    return 0;
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
        static int _m_Connect_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _host = LuaAPI.lua_tostring(L, 1);
                    int _port = LuaAPI.xlua_tointeger(L, 2);
                    Game.NetPackageType _type;translator.Get(L, 3, out _type);
                    
                    Game.TcpManager.Connect( _host, _port, _type );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
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
            
            
            
                
                {
                    byte[] _message = LuaAPI.lua_tobytes(L, 1);
                    
                    Game.TcpManager.SendBytes( _message );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendBytesWithoutSize_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    byte[] _message = LuaAPI.lua_tobytes(L, 1);
                    
                    Game.TcpManager.SendBytesWithoutSize( _message );
                    
                    
                    
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
        static int _m_OnReadLine_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.IAsyncResult _asr = (System.IAsyncResult)translator.GetObject(L, 1, typeof(System.IAsyncResult));
                    
                    Game.TcpManager.OnReadLine( _asr );
                    
                    
                    
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
            
            
            
                
                {
                    int _dis = LuaAPI.xlua_tointeger(L, 1);
                    byte[] _bytes = LuaAPI.lua_tobytes(L, 2);
                    string _msg = LuaAPI.lua_tostring(L, 3);
                    
                    Game.TcpManager.OnDisconnected( _dis, _bytes, _msg );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PrintBytes_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Game.TcpManager.PrintBytes(  );
                    
                    
                    
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
        static int _m_OnReceiveLine_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    byte[] _bytes = LuaAPI.lua_tobytes(L, 1);
                    int _length = LuaAPI.xlua_tointeger(L, 2);
                    
                    Game.TcpManager.OnReceiveLine( _bytes, _length );
                    
                    
                    
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
        static int _m_OnReceivedMessageLine_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    byte[] _cmd_byte = LuaAPI.lua_tobytes(L, 1);
                    
                    Game.TcpManager.OnReceivedMessageLine( _cmd_byte );
                    
                    
                    
                    return 0;
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
                    byte[] _cmd_byte = LuaAPI.lua_tobytes(L, 1);
                    
                    Game.TcpManager.OnReceivedMessage( _cmd_byte );
                    
                    
                    
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
        static int _g_get_OnConnectCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Game.TcpManager.OnConnectCallBack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnDisConnectCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Game.TcpManager.OnDisConnectCallBack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnReceiveLineCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Game.TcpManager.OnReceiveLineCallBack);
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
        static int _g_get_TimeStamp(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, Game.TcpManager.TimeStamp);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onConnectCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Game.TcpManager.onConnectCallBack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onDisConnectCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Game.TcpManager.onDisConnectCallBack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onReceiveLineCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Game.TcpManager.onReceiveLineCallBack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onReceiveMsgCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Game.TcpManager.onReceiveMsgCallBack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnConnectCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Game.TcpManager.OnConnectCallBack = translator.GetDelegate<System.Action<byte[]>>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnDisConnectCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Game.TcpManager.OnDisConnectCallBack = translator.GetDelegate<System.Action<byte[]>>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnReceiveLineCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Game.TcpManager.OnReceiveLineCallBack = translator.GetDelegate<System.Action<byte[]>>(L, 1);
            
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
			    Game.TcpManager.OnReceiveMsgCallBack = translator.GetDelegate<System.Action<byte[]>>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onConnectCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Game.TcpManager.onConnectCallBack = translator.GetDelegate<System.Action<byte[]>>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onDisConnectCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Game.TcpManager.onDisConnectCallBack = translator.GetDelegate<System.Action<byte[]>>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onReceiveLineCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Game.TcpManager.onReceiveLineCallBack = translator.GetDelegate<System.Action<byte[]>>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onReceiveMsgCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Game.TcpManager.onReceiveMsgCallBack = translator.GetDelegate<System.Action<byte[]>>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
