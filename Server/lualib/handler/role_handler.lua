--[[
    author:shenyi
    time:2019-02-05 20:24:15
]]

local skynet = require "skynet"
local snax = require "skynet.snax"
--local share = require "skynet.sharedata"
local handler = require "handler.handler"


--------------------------------------------------------------------
local RPC = {}
local CMD = {}
handler = handler.New (RPC, CMD)
---------------------------------------------------------------------

local user
local mongod

handler:onInit (function (user) 
    user = user
    -- mongod = snax.uniqueservice("common/mongod")
    -- local account = mongod.req.findOne("account", { userid = user.account })
    -- if not account then
    --     mongod.post.insert("account", { userid = user.account, money = 123, age = 28 })
    --     account = mongod.req.findOne("account", { userid = user.account })
    -- end
end)

--注册登陆方法
handler:onLogin (function () 
	skynet.error("role login")
end)

--注册登出方法
handler:onLogout (function () 
	skynet.error("role logout")
end)



-- function RPC.signup(args)
--     skynet.error("user signin:", args.userid)
--     skynet.sleep(300)
--     skynet.error("user signin:", args.userid)
--     -- local test = share.query("test")
--     -- print(test)

--     return { ok = true }
-- end



return handler