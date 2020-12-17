--[[
    author:shenyi
    time:2019-02-09 13:48:27
]]

local handler = class("handler")

function handler:__init (rpc, cmd)
    self.rpc = rpc
    self.cmd = cmd
    self.register_func = {}
    self.unregister_func = {}
end

function handler:onInit (f)
	table.insert (self.register_func, f)
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

function handler:OnRegister (f)
	table.insert (self.register_func, f)
end


function handler:OnUnRegister (f)
	table.insert (self.unregister_func, f)
end


function handler:register (user)
	for _, f in pairs (self.register_func) do
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
	for _, f in pairs (self.unregister_func) do
		f (user)
	end
	clean (user.RPC, self.rpc)
	clean (user.CMD, self.cmd)
end

return handler