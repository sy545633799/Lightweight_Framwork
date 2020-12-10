--[[
    author:shenyi
    time:2018-12-23 11:49:19
]]
local skynet = require "skynet"
local snax = require "skynet.snax"
local handler = require "handler.handler"

-----------------------------------------------------------
local RPC = {}
local CMD = {}
handler = handler.New (RPC, CMD)
-------------------------------------------------------------
local user

--注册登陆方法
handler:onLogin (function () 

end)

--注册登出方法
handler:onLogout (function () 

end)

return handler