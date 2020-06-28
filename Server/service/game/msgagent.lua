local skynet = require "skynet"
local queue = require "skynet.queue"
local crypt = require "skynet.crypt"
local sprotoparser = require "sprotoparser"
local sproto = require "sproto"
local c2s = require "proto.protoc2s"
local s2c = require "proto.protos2c"
local handlers = {
    "handler.login_handler",
    "handler.chat_handler",
    "handler.role_handler",
}

local cs = queue()
local gate
local user_info
local CMD = {}
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
        agent = skynet.self()
    }
    for _,v in ipairs(handlers) do
        local handler = require(v)
        handler:register(user_info)
    end
end

local function logout()
    if gate then
        skynet.call(gate, "lua", "logout", user_info.uid, user_info.sid)
    end
    skynet.exit()
end

function CMD.logout(source)
    skynet.error(string.format("%s is logout", user_info.uid))
    logout()
end

function CMD.afk(source)

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
            skynet.send(gate, "lua", "send", user_info.uid, user_info.subid, ret)
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
        skynet.ret(skynet.pack(f(source, ...)))
    end)

    skynet.dispatch("client", function(session, source, data)
        cs(dorequest, data)
    end)
end)
