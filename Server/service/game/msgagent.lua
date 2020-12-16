local skynet = require "skynet"
local queue = require "skynet.queue"
local crypt = require "skynet.crypt"
local sproto = require "sproto"
local handlers = {
    "handler.login",
    "handler.role",
}

local msgProto
local id2ProtoDic = {}
local cs = queue()
local gate
local fd
---@class CMD
local CMD = {}
---@class RPC
local RPC = {}
local User

function CMD.connect(source, platform, server_id, fd1)
    gate = source
    fd = fd1
    User = {
        platform = platform,
        server_id = server_id,
        RPC = RPC,
        CMD = CMD,
    }
    for _,v in ipairs(handlers) do
        local handler = require(v)
        handler:register(User)
    end
end

function CMD.disconnect(source)
    fd = nil
    for _,v in ipairs(handlers) do
        local handler = require(v)
        handler:unregister(User)
    end
    User = nil
end

function CMD.doDisconnect(ret)
    skynet.send(gate, "lua", "doDisconnect", fd, ret)
    CMD.disconnect(nil)
end


function CMD.send(retId, ret)
    assert(retId and type(retId) == "number")
    local result = string.pack(">H", retId)
    if ret then
        result = result .. ret
    end
    skynet.send(gate, "lua", "send", fd, result)
end

local function dorequest(data)
    local protoId = string.unpack(">H", data)
    local protoName = id2ProtoDic[protoId]
    if protoName then
        local f = RPC[protoName]
        if not f then skynet.error("can't find protoName:" .. protoName) return end
        if protoId > 20000 then
            local retTb
            if #data > 4 then
                local str = string.sub(data, 5)
                retTb = f(msgProto:decode(protoName, str))
            else
                retTb = f()
            end
            local retId = protoId + 1
            local retName = id2ProtoDic[retId]
            local ret =  string.sub(data, 3, 4)
            if retTb and retName then
                ret = ret .. msgProto:encode(retName, retTb)
            end
            CMD.send(retId, ret)
        else
            if #data > 2 then
                local str = string.sub(data, 3)
                f(msgProto:decode(protoName, str))
            else
                f()
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
        local f = assert(CMD[command])
        cs(skynet.ret, skynet.pack(f(source, ...)))
    end)

    skynet.dispatch("client", function(session, source, data)
        cs(dorequest, data)
    end)
end)
