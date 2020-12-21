--[[
    author:{author}
    time:2018-12-24 19:26:02
]]

-- node1 节点配置
---@class gameconfig
local config = {
    --调试模式
    debug = true,
    nodename = "game001",
	consoleport = 5001,
    platform_id = 1,
    server_id = 1,

    ---帧数，每秒
    fps = 10,

    ---gate服务配置
    gateconfig = {
        port = 8890,
        maxclient = 512,
        servername = "DevelopServer",

    },

    db = {
        dbname = "game",
        host = "127.0.0.1",
        port = 27017,
        -- username
        -- password
    },

    consoleport = 8802 ,
}

return config