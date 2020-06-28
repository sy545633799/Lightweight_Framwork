--[[
    author:shneyi
    time:2019-02-09 14:06:37
]]

local skynet = require "skynet"
local snax = require "skynet.snax"
--local sharedata = require "skynet.sharedata"
local handler = require "handler.handler"
local uuid = require "utils.uuid"
local errors = require "proto.errcode"

--------------------------------------------------------------------
local RPC = {}
local CMD = {}
handler = handler.New (RPC, CMD)
---------------------------------------------------------------------

local user
local mongod
local create_role
local uuid_inst

handler:onInit (function (user_info)
    user = user_info
    --mongod = snax.uniqueservice("common/mongod")
    --create_role = snax.uniqueservice ("game/role/create_role")
    --uuid_inst = uuid.New()
    -- local account = mongod.req.findOne("account", { userid = user.account })
    -- if not account then
    --     mongod.post.insert("account", { userid = user.account, money = 123, age = 28 })
    --     account = mongod.req.findOne("account", { userid = user.account })
    -- end
    -- mongod.post.update("package", { userid = "127" }, { ["$set"] = { ["package.ChipPackage.10001"] = test} })
end)

--注册登陆方法
handler:onLogin (function () 
	
end)

--注册登出方法
handler:onLogout (function () 
	
end)

function RPC.req_register(args)
    return { ok = true }
    --local result = create_role.req.create(user.account, args)
    --return result
end


function RPC.req_login(args)
    --local account = mongod.req.findOne("account", { Account = user.account })
    --if not account then
    --    return { ok = false, errCode = ERROR_CODE.Account_UnRegister }
    --end
    --local roleAttrib = mongod.req.findOne("role_attrib", { RoleId = account.RoleId })
    --local heroPackage = mongod.req.findOne("hero_package", { RoleId = account.RoleId })
    --local itemPackage = mongod.req.findOne("item_package", { RoleId = account.RoleId })
    --
    --if roleAttrib and heroPackage and itemPackage then
    --    user.send_request("ack_roleInfo", {
    --
    --        Attrib = roleAttrib,
    --        HeroPackage = heroPackage.Package,
    --        ItemPackage = itemPackage.Package
    --    })
    --
    --    return { ok = true }
    --else
    --    return { ok = false, errCode = ERROR_CODE.Account_UnRegister }
    --end
    
end


return handler