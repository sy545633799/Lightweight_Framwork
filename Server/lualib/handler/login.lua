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
local snax_account, anax_world
local channel

handler:OnRegister (function (user)
    User = user
    snax_account = snax.uniqueservice("game/account")
    anax_world = snax.uniqueservice("game/world")
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
    if User.roleInfo then
        channel:unsubscribe()
        anax_world.req.role_leave_game(User.roleInfo)

        --TODO:保存玩家数据到数据库
    end
    ---@type RoleInfo
    User.roleInfo = nil
end)

---@public 客户端点击登录调用, 返回玩家信息，如果没有，则返回空
function RPC.req_login(arg)
    User.account = arg.uid
    User.roleInfo = snax_account.req.get_roleInfo(arg.uid)
    return { roleInfo = User.roleInfo }
end

---@public 如果客户端得到的玩家信息为空，则调用这个方法注册，并返回玩家信息
function RPC.req_register(args)
    local ok, roleInfo = snax_account.req.create_role(User.account, args.nickname)
    User.roleInfo = roleInfo
    local result = {}
    if ok then
        result.error = 0
        result.roleInfo = roleInfo
    else
        result.error = 1
    end

    return result
end

---推送给客户端的消息
local function recvChannel(channel, source, msg, ...)
    --skynet.error("channel ID:",channel, "source:", skynet.address(source), "msg:",msg)


end

function RPC.req_enter_game(args)
    local ok, channelId = anax_world.req.role_enter_game(skynet.self(), User.roleInfo, User.roleInfo.attrib.sceneId)
    channel = User.mc.new {
        channel = channelId,
        dispatch = recvChannel,
    }
    channel:subscribe()

    return { ok = ok }
end

function RPC.req_leave_game(args)
    skynet.error("leave")
    channel:unsubscribe()
    local ok = anax_world.req.role_leave_game(User.roleInfo)
    return { ok = ok }
end

function RPC.req_switch_scene(args)

    local ok = anax_world.req.role_switch_scene(User.roleInfo, args.sceneId)
    User.roleInfo.attrib.sceneId = args.sceneId
    return { ok = ok }
end

return handler