---------------------------------------------------
--- 网络消息接收基类, 给Model使用
--- Created by shenyi
--- DateTime: 2019.6.6
---------------------------------------------------
---@class MessageListener:Updatable
MessageListener = BaseClass("MessageListener", Updatable)

function MessageListener:ctor()
    self.message_callaback = {}
end
---------------------------------------------------网络消息部分-------------------------------------------------------------
function MessageListener:AddMessageListener(funcname,authority)
    MessageRPCManager:AddHandler(self, funcname,authority)
    self.message_callaback[funcname] = true
end

function MessageListener:RemoveMessageListener(funcname)
    MessageRPCManager:RemoveHandler(self, funcname)
    self.message_callaback[funcname] = nil
end

function MessageListener:RemoveAllMessageListener()
    if not self.message_callaback then return end
    for funcname, _ in pairs(self.message_callaback) do
        MessageRPCManager:RemoveHandler(self, funcname)
    end
    self.message_callaback = {}
end
--------------------------------------------------------------------------------------------------------------------------------
function MessageListener:dtor()
    self:RemoveAllMessageListener()
    self.message_callaback = nil
end