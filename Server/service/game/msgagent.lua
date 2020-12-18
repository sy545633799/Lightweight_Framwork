local skynet = require "skynet"
local snax = require "skynet.snax"
local queue = require "skynet.queue"
local crypt = require "skynet.crypt"
local sproto = require "sproto"
local mc = require "skynet.multicast"

---@class User
User = {}
---@class CMD : User
CMD = {}
---@class RPC : User
RPC = {}
require "handler.login"
require "handler.sync"

local user = User
local cmd = CMD
local rpc = RPC
local msgProto
local id2ProtoDic = {}
local cs = queue()
local gate
local fd

function CMD:connect(source, platform, server_id, fd1)
    gate = source
    fd = fd1
    user.platform = platform
    user.server_id = server_id
    user.mc = mc
    user.channels = {}
    local account = snax.uniqueservice("game/account")
    ---@type Account_Req
    user.account_req = account.req
    ---@type Account_Post
    user.account_post = account.post
    local world = snax.uniqueservice("game/world")
    ---@type World_Req
    user.world_req = world.req
    ---@type World_Post
    user.world_post = world.post
end

function CMD:disconnect(source)
    for name, channel in pairs(user.channels) do
        channel:unsubscribe()
    end
    user.world_req.role_leave_game(user.roleInfo)

    for k, v in pairs(user) do
        if not type(v) == "function" then
            user[k] = nil
        end
    end
    user.roleInfo = nil
end

function CMD:doDisconnect(ret)
    skynet.send(gate, "lua", "doDisconnect", fd, ret)
    self:disconnect(nil)
end


function RPC:send(retId, ret)
    assert(retId and type(retId) == "number")
    local result = string.pack(">H", retId)
    if ret then
        result = result .. ret
    end
    skynet.send(gate, "lua", "send", fd, result)
end

function RPC:dorequest(data)
    local protoId = string.unpack(">H", data)
    local protoName = id2ProtoDic[protoId]
    if protoName then
        local f = self[protoName]
        if not f then skynet.error("can't find protoName:" .. protoName) return end
        if protoId > 20000 then
            local retTb
            if #data > 4 then
                local str = string.sub(data, 5)
                retTb = f(self, msgProto:decode(protoName, str))
            else
                retTb = f(self)
            end
            local retId = protoId + 1
            local retName = id2ProtoDic[retId]
            local ret =  string.sub(data, 3, 4)
            if retTb and retName then
                --skynet.error(tostring(retTb))
                ret = ret .. msgProto:encode(retName, retTb)
            end
            self:send(retId, ret)
        else
            if #data > 2 then
                local str = string.sub(data, 3)
                f(self, msgProto:decode(protoName, str))
            else
                f(self)
            end
        end
    else
        skynet.error("can't find protoId:" .. protoId)
    end
end

skynet.register_protocol {
    name = "client",
    id = skynet.PTYPE_CLIENT,
    unpack = skynet.unpack,    --消息框架会自动释放msg和sz,无须调用skynet.trash
}

skynet.start(function()
    for protoName, id in pairs(NetMsgId) do
        id2ProtoDic[id] = protoName
    end
    msgProto = sproto.parse(require("proto.msg"))
    skynet.dispatch("lua", function(session, source, command, ...)
        local f = assert(cmd[command])
        cs(skynet.ret, skynet.pack(f(cmd, source, ...)))
    end)

    skynet.dispatch("client", function(session, source, data)
        cs(rpc.dorequest, rpc, data)
    end)
end)
