local skynet = require "skynet"
local gateserver = require "snax.gateserver"
local netpack = require "skynet.netpack"
local crypt = require "skynet.crypt"
local socketdriver = require "skynet.socketdriver"
local assert = assert

local start_arge = {...}
local platform = tonumber(start_arge[1])
local server_id = tonumber(start_arge[2])

local connection = {}
local pool = {}

skynet.register_protocol {
    name = "client",
    id = skynet.PTYPE_CLIENT,
}

local handler = {}

function handler.command(cmd, source, ...)
    local f = assert(handler[cmd])
    return f(...)
end

function handler.open(source, gateconf)
    --local n = 10
    --for _ = 1, n do
    --    table.insert (pool, skynet.newservice "game/msgagent")
    --end
end

function handler.connect(fd, addr)
    gateserver.openclient(fd)

    connection[fd] = { fd = fd }
    if #pool == 0 then
        connection[fd].agent = skynet.newservice "game/msgagent"
    else
        connection[fd].agent = table.remove(pool, 1)
    end
    skynet.call(connection[fd].agent, "lua", "connect", platform, server_id, fd)
end

function handler.disconnect(fd)
    local c = connection[fd]
    if c then
        skynet.call(c.agent, "lua", "disconnect")
        table.insert(pool, c.agent)
        connection[fd] = nil
    end
end

function handler.doDisconnect(fd)
    local c = connection[fd]
    if c then
        gateserver.closeclient(c.fd)
        connection[fd] = nil
    end
end

function handler.message(fd, msg, sz)
    local message = netpack.tostring(msg, sz)
    local c = connection[fd]
    if c and c.agent then
        pcall(skynet.rawsend, c.agent, "client", skynet.pack(message))
    end
end

function handler.send(fd, msg)
    local c = connection[fd]
    if c and c.fd then
        pcall(socketdriver.send(c.fd, netpack.pack(msg)))
    end
end


gateserver.start(handler)

