local skynet = require "skynet"
local queue = require "skynet.queue"
local crypt = require "skynet.crypt"
local sprotoparser = require "sprotoparser"
local sproto = require "sproto"
local c2s = require "proto.protoc2s"
local s2c = require "proto.protos2c"
local handlers = {
    "handler.login",
    "handler.chat",
    "handler.role",
}

local cs = queue()
local gate
local user_info
---@class CMD
local CMD = {}
---@class RPC
local RPC = {}
local sproto_host, sproto_request

function CMD.login(source, uid, sid, secret, platform, server_id)
    skynet.error(string.format("%s is login", uid))
    gate = source
    user_info = {
        uid = uid,
        subid = sid,
        secret = secret,
        platform = platform,
        server_id = server_id,
        RPC = RPC,
        CMD = CMD,
        agent = skynet.self(),
        sendmessage = function(ret) skynet.send(gate, "lua", "send", user_info.uid, user_info.subid, ret) end
    }
    for _,v in ipairs(handlers) do
        local handler = require(v)
        handler:register(user_info)
    end
end

---玩家主动登出
function CMD.logout(source)
    for _,v in ipairs(handlers) do
        local handler = require(v)
        handler:unregister(user_info)
    end
    if gate then
        skynet.call(gate, "lua", "logout", user_info.uid, user_info.subid)
    end
end

---将玩家踢出登录
function CMD.afk(source)
    for _,v in ipairs(handlers) do
        local handler = require(v)
        handler:unregister(user_info)
    end
    skynet.error("玩家被踢离线")
end


local function dorequest(data)
    local ok, type, session, args, response = pcall(sproto_host.dispatch, sproto_host, data)
    if not ok then
        LOG_ERROR("sproto parser error") return
    end

    local f = RPC[session]
    if f then
        local r = f(args)
        if r and response then
            local ret = response(r)
            if ret then
                skynet.send(gate, "lua", "send", user_info.uid, user_info.subid, ret)
            end
        end
    else
        LOG_ERROR("request whith nil type : %s", session)
    end
end

skynet.register_protocol {
    name = "client",
    id = skynet.PTYPE_CLIENT,
    unpack = skynet.unpack,    --消息框架会自动释放msg和sz,无须调用skynet.trash
}

skynet.start(function()
    sproto_host = sproto.new(sprotoparser.parse(c2s)):host "package"
    sproto_request = sproto_host:attach(sproto.new(sprotoparser.parse(s2c)))
    skynet.dispatch("lua", function(session, source, command, ...)
        local f = assert(CMD[command])
        cs(skynet.ret, skynet.pack(f(source, ...)))
    end)

    skynet.dispatch("client", function(session, source, data)
        cs(dorequest, data)
    end)
end)
