--[[
    author:shenyi
    time:2018-12-26 15:06:03
]]

local skynet = require "skynet"

local assert = assert
local string = string

local slave_list = {}

local CMD = {}

function CMD.register(name, address)
    assert(not slave_list[name], string.format("Already register database slave %s.", name))
    slave_list[name] = address
end

function CMD.get(name)
    return assert(slave_list[name], string.format("No database slave %s.", name))
end

skynet.start(function()
	skynet.dispatch("lua", function(session, source, command, ...)
		local f = assert(CMD[command])
        skynet.retpack(f(...))
	end)
end)
