--[[
    author:shneyi
    time:2019-02-09 14:06:37
]]

local skynet = require "skynet"
local snax = require "skynet.snax"
--local sharedata = require "skynet.sharedata"
local handler = require "handler.handler"

--------------------------------------------------------------------
---@type RPC
local RPC = {}
---@type CMD
local CMD = {}
handler = handler.New (RPC, CMD)
----------------------------------------------
local User
---@type accountd
local snax_account

--玩家属性
local role_attrib

handler:OnRegister (function (user)
    User = user
    snax_account = snax.uniqueservice("game/account")

    --create_role = snax.uniqueservice ("game/role/create_role")
    --uuid_inst = uuid.New()
    -- local account = mongod.req.findOne("account", { userid = user.account })
    -- if not account then
    --     mongod.post.insert("account", { userid = user.account, money = 123, age = 28 })
    --     account = mongod.req.findOne("account", { userid = user.account })
    -- end
    -- mongod.post.update("package", { userid = "127" }, { ["$set"] = { ["package.ChipPackage.10001"] = test} })
end)


handler:OnUnRegister (function ()
    User.uid = nil
    User.roleInfo = nil
end)

---@public 客户端点击登录调用, 返回玩家信息，如果没有，则返回空
function RPC.req_login(arg)
    User.uid = arg.uid
    User.roleInfo = snax_account.req.get_roleInfo(User.uid)
    return User.roleInfo
end

---@public 如果客户端得到的玩家信息为空，则调用这个方法注册，并返回玩家信息
function RPC.req_register(args)
    local ok, roleInfo = snax_account.req.create_role(User.uid, args.nickname)
    User.roleInfo = roleInfo
    local result = {}
    if ok then
        result.error = 0
        result.roleInfo = roleInfo
        role_attrib = roleInfo
    else
        result.error = 1
    end

    return result
end




return handler