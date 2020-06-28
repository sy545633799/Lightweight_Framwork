local skynet = require "skynet"
local snax = require "skynet.snax"
local cluster = require "skynet.cluster"
require "skynet.manager"

skynet.start(function()
	local harborname = skynet.getenv("harborname")
	cluster.open(harborname)

	local config = require(skynet.getenv("config"))
	if not skynet.getenv "daemon" then
		skynet.newservice("console")
	end

	local loginserver = skynet.newservice("game/logind")
	local platform_id = 1
	local server_id = 1
	local gate = skynet.newservice("game/gated", loginserver, platform_id, server_id)
	skynet.call(gate, "lua", "open" , {
		port = 8890,
		maxclient = 512,
		servername = "DevelopServer",
	})

	--skynet.newservice("debug_console", config.consoleport)
	--local moniter = skynet.uniqueservice ("common/moniter")
	--skynet.call (moniter, "lua", "open")
	--skynet.uniqueservice ("common/cache")
	--skynet.uniqueservice("game/agent/agentpool")
	--
	--local gate = skynet.newservice("game/gated")
	--skynet.call(gate, "lua", "open" , config.gateconfig)

	skynet.exit()
end)

