--[[
    author:shenyi
    time:2019-02-05 20:24:15
]]

local skynet = require "skynet"
local snax = require "skynet.snax"
--local share = require "skynet.sharedata"
local handler = require "handler.handler"


--------------------------------------------------------------------
---@type RPC
local RPC = {}
---@type CMD
local CMD = {}
handler = handler.New (RPC, CMD)
---------------------------------------------------------------------

local user
local mongod

handler:OnRegister (function ()

    -- mongod = snax.uniqueservice("common/mongod")
    -- local account = mongod.req.findOne("account", { userid = user.account })
    -- if not account then
    --     mongod.post.insert("account", { userid = user.account, money = 123, age = 28 })
    --     account = mongod.req.findOne("account", { userid = user.account })
    -- end
end)

handler:OnRegister (function () end)

function RPC.req_changeName(args)
	--TODO 验证名字合法性

end

function RPC.req_changeHead(args)

end

function RPC.req_changeHeadFrame(args)

end

function RPC.req_signup(args)
	--skynet.error("user signin:", args.userid)
	--skynet.sleep(300)
	--skynet.error("user signin:", args.userid)
	---- local test = share.query("test")
	---- print(test)		--
	--return { ok = true }
end

return handler