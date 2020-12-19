---------------------------------------------------
--- 网络消息接收基类, 给Model使用
--- Created by shenyi
--- DateTime: 2019.6.6
---------------------------------------------------
---@class MessageListener:Updatable
MessageListener = BaseClass("MessageListener", Updatable)
local networkManager = NetworkManager

function MessageListener:ctor()
    self.message_callback = {}
end
---------------------------------------------------网络消息部分-------------------------------------------------------------
function MessageListener:AddMessageListener(protoID, callback, handler)
    if self.message_callback[protoID] then
        logError("重复添加id:" .. protoID)
        return
    end
    networkManager:RegSrvReqHandler(protoID, callback, handler)
    self.message_callback[protoID] = callback
end

function MessageListener:RemoveMessageListener(protoID)
    if self.message_callback[protoID] then
        networkManager:RemoveHandler(protoID)
        self.message_callback[protoID] = nil
    end
end

function MessageListener:RemoveAllMessageListener()
    for protoID, _ in pairs(self.message_callback) do
        networkManager:RemoveHandler(protoID)
    end
    self.message_callback = {}
end

function MessageListener:Send(protoID, args)
    networkManager:SendMessage(protoID, args)
end

function MessageListener:SendRequest(protoID, args)
    return networkManager:SendRequest(protoID, args)
end
--------------------------------------------------------------------------------------------------------------------------------
function MessageListener:dtor()
    self:RemoveAllMessageListener()
    self.message_callback = nil
end