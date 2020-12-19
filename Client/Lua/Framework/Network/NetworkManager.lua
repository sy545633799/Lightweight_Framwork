--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by shenyi.
--- DateTime: 2020/6/26

require "Framework/Network/NetStateEvent"
require "Framework/Network/Proto/msgId"
local sproto = require "Framework/Network/Sproto/sproto"
local crypt = require "crypt"
---@class NetworkManager:Updatable
local NetworkManager = BaseClass("NetworkManager", Updatable)
local tcpManager = TcpManager

local msgProto
local Id2ProtoDic = {}
local rpcId = 1


function NetworkManager:ctor()
    self.loginLock = false -- 登录锁
    for protoName, id in pairs(NetMsgId) do
        Id2ProtoDic[id] = protoName
    end
    msgProto = sproto.parse(require("Framework/Network/Proto/msg"))
    tcpManager.OnConnectEventCallBack = function(state, count) self:OnConnectCallBack(state, count) end
    tcpManager.OnReceiveMsgCallBack = function(protoID, RPCID, bytes) self:OnReceiveMsgCallBack(protoID, RPCID, bytes) end
    self.srvReqHandler = {}
    self.sessionId2CBs = {}
end

---@登录
function NetworkManager:Login(ip, port)
    if self.loginLock then logError("已登录或正在登陆中") return end
    self.loginLock = true
    self.isConnect = false
    self.ip = ip
    self.port = port
    tcpManager.Connect(self.ip, self.port)
end

function NetworkManager:onDisConnect()
    self.isConnect = false
    self.loginLock = false
    --EventManager:Broadcast(EventNames.Login.DisConnect)
end

function NetworkManager:OnConnectCallBack(state, count)
    coroutine.start(function ()
        if state == NetStateEvent.ConnectSucess then
            logError("连接成功")
            --UIManager:UnLoadView(UIConfig.ReconnectionUI)
            --LoginController:HandSucess()
        elseif state == NetStateEvent.ConnectFailed then
            logError("连接失败")
            --UIManager:UnLoadView(UIConfig.ReconnectionUI)
            --TipsManager:ShowTips(TipsConfig.CommonTips, "连不上服务器")
            self:onDisConnect()
        elseif state == NetStateEvent.Exception then
            self:onDisConnect()
            --UIManager:LoadView(UIConfig.ReconnectionUI)
            logError("开始短线重连")
            self:Reconnect()
        elseif state == NetStateEvent.Disconnect then
            logError("断开连接")
        elseif state == NetStateEvent.ReConnectSucess then
            logNetMsg("重连成功")
            --UIManager:UnLoadView(UIConfig.ReconnectionUI)
            --LoginUIController:HandSucess()
        elseif state == NetStateEvent.ReConnectFailed then
            logError(string.format("当前重连失败第%d次数", count))
            if count == common_para.commonPara[39].figure then
                --UIManager:UnLoadView(UIConfig.ReconnectionUI)
            end
        end
        EventManager:Broadcast(EventNames.Network.NetworkEvent, state)
    end)
end

function NetworkManager:Close()
    tcpManager.Close()
end

---注册服务器单方向客户端发送的信息
function NetworkManager:RegSrvReqHandler(protoId, callback, handle)
    assert(type(protoId) == "number")
    assert(type(callback) == "function")
    self.srvReqHandler[protoId] = { callback = callback, handle = handle}
end

function NetworkManager:RemoveHandler(protoId)
    self.srvReqHandler[protoId] = nil
end



---客户端向服务端同步消息
function NetworkManager:SendMessage(protoId, args)
    assert(protoId and type(protoId) == "number")
    if protoId > 20000 then
        logError("protoId 大于 20000")
        return
    end
    local protoName = Id2ProtoDic[protoId]
    if protoName and args then
        local msg = msgProto:encode(protoName, args)
        tcpManager.SendBytes(protoId, msg, 0)
    else
        tcpManager.SendBytes(protoId, nil, 0)
    end
end

---客户端向服务端发送请求
function NetworkManager:SendRequest(protoId, args)
    assert(protoId and type(protoId) == "number")
    if protoId <= 20000 then
        logError("protoId 小于 20000")
        return
    end

    if rpcId == 65535 then
        rpcId = 1
    else
        rpcId = rpcId + 1
    end

    local protoName = Id2ProtoDic[protoId]
    if protoName and args then
        local msg = msgProto:encode(protoName, args)
        tcpManager.SendBytes(protoId, msg, rpcId)
    else
        tcpManager.SendBytes(protoId, nil, rpcId)
    end
    self.sessionId2CBs[rpcId] = coroutine.running()
    return coroutine.yield()
end

function NetworkManager:OnReceiveMsgCallBack(protoID, rpcId, bytes)
    local args
    if bytes and #bytes > 0 then
        local protoName = Id2ProtoDic[protoID]
        if not protoName then logError("can't find protoId" .. protoID) return end
        args = msgProto:decode(protoName, bytes)
        log("receive:" .. tostring(args))
    end

    if rpcId > 0 then
        local co = self.sessionId2CBs[rpcId]
        coroutine.resumeex(co, args)
        self.sessionId2CBs[rpcId] = nil
    else
        local handler = self.srvReqHandler[protoID]
        if handler then
            coroutine.start(function () if handler then handler(handler.handler, args) else handler(args) end end)
        end
    end

end

function NetworkManager:dtor()
    tcpManager.OnConnectEventCallBack = nil
    tcpManager.OnReceiveMsgCallBack = nil
end

return NetworkManager