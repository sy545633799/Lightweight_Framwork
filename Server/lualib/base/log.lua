--[[
    author:shenyi
    time:2018-12-11 15:20:54
]]
local skynet = require "skynet"

function LOG_DEBUG(fmt, ...)
	local msg = string.format(fmt, ...)
	local info = debug.getinfo(2)
	if info then
		msg = string.format("[%s:%d] %s", info.short_src, info.currentline, msg)
	end
	-- skynet.send("log", "lua", "debug", SERVICE_NAME, msg)
end

function LOG_INFO(fmt, ...)
	local msg = string.format(fmt, ...)
	local info = debug.getinfo(2)
	if info then
		msg = string.format("[%s:%d] %s", info.short_src, info.currentline, msg)
	end
	-- skynet.send("log", "lua", "info", SERVICE_NAME, msg)
end

function LOG_WARNING(fmt, ...)
	local msg = string.format(fmt, ...)
	local info = debug.getinfo(2)
	if info then
		msg = string.format("[%s:%d] %s", info.short_src, info.currentline, msg)
	end
	-- skynet.send("log", "lua", "warning", SERVICE_NAME, msg)
end

function LOG_ERROR(fmt, ...)
	local msg = string.format(fmt, ...)
	local info = debug.getinfo(2)
	if info then
		msg = string.format("[%s:%d] %s", info.short_src, info.currentline, msg)
	end
	skynet.error(msg)
	-- skynet.send("log", "lua", "error", SERVICE_NAME, msg)
end

function LOG_FATAL(fmt, ...)
	local msg = string.format(fmt, ...)
	local info = debug.getinfo(2) 
	if info then
		msg = string.format("[%s:%d] %s", info.short_src, info.currentline, msg)
	end
	-- skynet.send("log", "lua", "fatal", SERVICE_NAME, msg) 
end