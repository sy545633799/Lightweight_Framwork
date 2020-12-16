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

	local gate = skynet.newservice("game/gated", config.platform_id, config.server_id)
	skynet.call(gate, "lua", "open" , config.gateconfig)

	local mongod = snax.uniqueservice("common/mongod")
	snax.uniqueservice("common/uid")
	snax.uniqueservice("game/account")


	--skynet.newservice("debug_console", config.consoleport)
	--local moniter = skynet.uniqueservice ("common/moniter")
	--skynet.call (moniter, "lua", "open")
	--skynet.uniqueservice ("common/cache")
	--skynet.uniqueservice("game/agent/agentpool")
	--
	--local gate = skynet.newservice("game/gated")
	--skynet.call(gate, "lua", "open" , config.gateconfig)

	--local s2c = require "proto.prototest"

	skynet.exit()
end)

