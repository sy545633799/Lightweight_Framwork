--[[
    author:shenyi
    time:2019-02-05 20:24:15
]]

local skynet = require "skynet"
local snax = require "skynet.snax"
--local share = require "skynet.sharedata"
local handler = require "handler.handler"


--------------------------------------------------------------------
local User
---@type RPC
local RPC = {}
---@type CMD
local CMD = {}
handler = handler.New (RPC, CMD)
---------------------------------------------------------------------

local mongod, snax_world

handler:OnRegister (function (user)
    User = user
    snax_world = snax.uniqueservice("game/world")
    -- mongod = snax.uniqueservice("common/mongod")
    -- local account = mongod.req.findOne("account", { userid = user.account })
    -- if not account then
    --     mongod.post.insert("account", { userid = user.account, money = 123, age = 28 })
    --     account = mongod.req.findOne("account", { userid = user.account })
    -- end
end)

handler:OnUnRegister (function ()

end)

function RPC.asyn_pos(args)
    snax_world.post.sync_pos(User.roleInfo.attrib.roleId, args)
end


return handler