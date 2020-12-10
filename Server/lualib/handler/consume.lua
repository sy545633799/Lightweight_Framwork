--[[
    author:shenyi
    time:2020.11.24
    desc:充值相关
]]

local skynet = require "skynet"
local snax = require "skynet.snax"
local handler = require "handler.handler"

---@type RPC
local RPC = {}
---@type CMD
local CMD = {}
handler = handler.New (RPC, CMD)
-------------------------------------------------------------
local user
local snax_mongod

--注册登陆方法
handler:onLogin (function (_user)
    user = _user
    snax_mongod = snax.uniqueservice("common/mongod")
end)

---@public 客户端购买金币
function RPC.req_buygold(args)

    
    -----增加
    --mongod.post.update("test", { userid = 1 }, { ["$set"] = { ["test.1003"] = { id = "1003", star = 1, level = 1} } })
    -----删除
    --mongod.post.update("test", { userid = 1 }, { ["$unset"] = { ["test.1003"] = 1 } })
    -----更改
    --mongod.post.update("test", { userid = 1 }, { ["$set"] = { ["test.1001.star"] = 3 } })

end

---@public 客户端购买钻石
function RPC.req_buydiamond(args)
    ---@type gameconfig
    local config = require(skynet.getenv("config"))
    if config.debug then


    else
        --- 真正支付流程
    end
end

--注册登出方法
handler:onLogout (function ()

end)

return handler