--[[
    author:shenyi
    time:2019-02-09 13:48:27
]]

local handler = class("handler_base")

function handler:__init (rpc, cmd)
    self.init_func = {}
    self.rpc = rpc
    self.cmd = cmd
    self.login_func = {}
    self.logout_func = {}
end

function handler:onInit (f)
	table.insert (self.init_func, f)
end

local function merge (dest, t) -- 复制表元素
	if not dest or not t then return end
	for k, v in pairs (t) do
		dest[k] = v
	end
end

local function merge_safe (dest, t) -- 复制表元素，并检查
    if not dest or not t then return end
    for k, v in pairs (t) do
        assert(dest[k] == nil, string.format("handler 有个方法重名:%s", k))
        dest[k] = v
    end
end

function handler:onLogin (f)
	table.insert (self.login_func, f)
end

function handler:login ()
	for _, f in pairs (self.login_func) do
		f ()
	end
end

function handler:onLogout (f)
	table.insert (self.logout_func, f)
end

function handler:logout ()
	for _, f in pairs (self.logout_func) do
		f ()
	end
end

function handler:register (user)
	for _, f in pairs (self.init_func) do
		f (user)
	end

	merge_safe (user.RPC, self.rpc)
	merge_safe (user.CMD, self.cmd)
end

local function clean (dest, t)
	if not dest or not t then return end
	for k, _ in pairs (t) do
		dest[k] = nil
	end
end

function handler:unregister (user)
	clean (user.RPC, self.rpc)
	clean (user.CMD, self.cmd)
end

return handler