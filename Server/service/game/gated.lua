local msgserver = require "snax.msg_server"
local crypt = require "skynet.crypt"
local skynet = require "skynet"

local start_arge = {...}
local loginservice = tonumber(start_arge[1])
local platform = tonumber(start_arge[2])
local server_id = tonumber(start_arge[3])

local server = {}
local users = {}
local username_map = {}
local internal_id = 0
local pool = {}

-- login server disallow multi login, so login_handler never be reentry
-- call by login server
function server.login_handler(uid, secret)
    if users[uid] then
        error(string.format("%s is already login", uid))
    end

    internal_id = internal_id + 1
    local id = internal_id
    local username = msgserver.username(uid, id, servername)
    if username_map[username] then
        error(string.format("%s is already login", username))
    end

    users[uid] = {
        username = username,
        uid = uid,
        subid = id,
    }

    if #pool == 0 then
        users[uid].agent = skynet.newservice "game/msgagent"
    else
        users[uid].agent = table.remove(pool, 1)
    end

    skynet.call(users[uid].agent, "lua", "login", uid, id, secret, platform, server_id)
    username_map[username] = users[uid]
    msgserver.login(username, secret)
    return id
end

-- call by agent
function server.logout_handler(uid, subid)
    local u = users[uid]
    if u then
        local username = msgserver.username(uid, subid, servername)
        assert(u.username == username)
        msgserver.logout(u.username)
        users[uid] = nil
        skynet.call(loginservice, "lua", "logout", uid, subid)
        if username_map[u.username] and username_map[u.username].agent then
            table.insert(pool, username_map[u.username].agent)
        end
        username_map[u.username] = nil
    end
end

-- call by login server
function server.kick_handler(uid, subid)
    local u = users[uid]
    if u then
        local username = msgserver.username(uid, subid, servername)
        assert(u.username == username)
        msgserver.logout(u.username)
        users[uid] = nil
        pcall(skynet.call, u.agent, "lua", "afk")
        if username_map[u.username] and username_map[u.username].agent then
            table.insert(pool, username_map[u.username].agent)
        end
        username_map[u.username] = nil
    end
end

-- call by self (when socket disconnect)
function server.disconnect_handler(username)
    local u = username_map[username]
    if u then
        msgserver.logout(u.username)
        users[u.uid] = nil
        skynet.call(u.agent, "lua", "afk")
        if username_map[u.username] and username_map[u.username].agent then
            table.insert(pool, username_map[u.username].agent)
        end
        username_map[u.username] = nil
    end
end

--给客户端发消息
function server.send_handler(uid, subid, msg)
    local u = users[uid]
    if u then
        local username = msgserver.username(uid, subid, servername)
        msgserver.sendmessage(username, msg)
    end
end

-- call by self (when recv a request from client)
function server.request_handler(username, msg)
    local u = username_map[username]
    if u and users[u.uid] then
        pcall(skynet.rawsend, u.agent, "client", skynet.pack(msg))
    end
end

-- call by self (when gate open)
function server.register_handler(name)
    servername = name
    skynet.call(loginservice, "lua", "register_gate", servername, skynet.self())

    local n = 10
    for _ = 1, n do
        table.insert (pool, skynet.newservice "game/msgagent")
    end
end

msgserver.start(server)
