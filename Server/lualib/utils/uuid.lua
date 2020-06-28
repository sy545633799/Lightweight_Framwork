local core = require "uuid.core"
local skynet = require "skynet"
local skynet_timeout = skynet.timeout

-- [[uuid format : (33bits self.timestamp)(6bits harbor)(15bits self.service)(10bits self.sequence)]]
local uuid = class("uuid")

function uuid:__init()
	self.sequence = 0

	local sid = core.sid()
	local config = require(skynet.getenv "config")
	local serverid = config.gateconfig.serverid
	self.service = ((serverid & 0x3f) << 25) | ((sid & 0xffff) << 10)
end

function uuid:Gen ()
	if not self.timestamp then
		self.timestamp = (os.time () << 31) | self.service
		self.sequence = 0
		skynet_timeout (100, function ()
			self.timestamp = nil
		end)
	end

	self.sequence = self.sequence + 1
	assert (self.sequence < 1024)

	return (self.timestamp | self.sequence)
end

function uuid:Split (id)
	local timestamp = id >> 31
	local serverid = (id & 0x7fffffff) >> 25
	local service = (id & 0x1ffffff) >> 10
	local sequence = id & 0x3ff
	return timestamp, serverid, service, sequence
end

return uuid

