--[[
    author:{author}
    time:2018-12-24 19:26:02
]]

-- node1 节点配置
return {

    nodename = "game001",
	--日志端口
	consoleport = 5001,
    --gate服务配置
    gateconfig = {
        port = 8002,
        maxclient = 64,
        serverid = 1,
        servername = "game001"
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