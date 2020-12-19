local skynet = require "skynet"
local snax = require "skynet.snax"

local event_names = event_names
local user = User
local rpc = RPC

---@public 客户端点击登录调用, 返回玩家信息，如果没有，则返回空
function RPC:req_login(arg)
    user.account = arg.uid
    ---@type RoleInfo
    user.roleInfo = user.account_req.get_roleInfo(arg.uid)
    return { roleInfo = user.roleInfo }
end

---@public 如果客户端得到的玩家信息为空，则调用这个方法注册，并返回玩家信息
function RPC:req_register(args)
    local ok, roleInfo = user.account_req.create_role(user.account, args.nickname)
    user.roleInfo = roleInfo
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
local function recvChannel(channel, source, eventId, data)
    --skynet.error("channel ID:",channel, "source:", skynet.address(source), "msg:",eventId)
    if eventId == event_names.scene.create_entities then
        if data[user.aoiId] then
            data[user.aoiId] = nil
        end
        if table.size(data) > 0 then
            rpc:sendmessage(NetMsgId.s2c_create_entities, { data = data })
        end

    elseif eventId == event_names.scene.delete_entities then
        if data[user.aoiId] then
            data[user.aoiId] = nil
        end
        if table.size(data) > 0 then
            rpc:sendmessage(NetMsgId.s2c_delete_entities, { id = data })
        end
    elseif eventId == event_names.scene.s2c_sync_trans then
        if data[user.aoiId] then
            user.roleInfo.status = data[user.aoiId]
        end
        --TODO 如果只是位置朝向发生改变，就不同步给客户端了
        rpc:sendmessage(NetMsgId.s2c_sync_trans, { list = data })
    end


end

function RPC:req_enter_game(args)
    local ok, aoiId, aoi_map, sceneInfo = user.world_req.role_enter_game(skynet.self(), user.roleInfo, user.roleInfo.attrib.sceneId)
    local channel = user.mc.new {
        channel = sceneInfo.channel,
        dispatch = recvChannel,
    }
    channel:subscribe()
    user.channels["scene"] = channel

    local snax_scene = snax.bind(sceneInfo.handle, sceneInfo.serviceName)
    user.aoiId = aoiId
    ---@type Scene_Req
    user.scene_req = snax_scene.req
    ---@type Scene_Post
    user.scene_post = snax_scene.post
    return { ok = ok, aoi_map = aoi_map }
end

function RPC:req_leave_game(args)
    user.channels["scene"]:unsubscribe()
    user.channels["scene"] = nil
    local ok = user.world_req.role_leave_game(user.roleInfo.attrib.roleId)
    return { ok = ok }
end

function RPC:req_switch_scene(args)
    local ok = user.world_req.role_switch_scene(user.roleInfo.attrib.roleId, args.sceneId)
    user.roleInfo.attrib.sceneId = args.sceneId
    return { ok = ok }
end
