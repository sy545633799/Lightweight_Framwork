local skynet = require "skynet"
local snax = require "skynet.snax"
local mc = require "skynet.multicast"

---@type entityMgr
local entityMgr

local config
local channel
local sceneConfig
---@class Scene_Req
local response = response
---@class Scene_Post
local accept = accept
---@class SceneParam
local sceneInfo = {}
---@type table <string, SceneRoleInfo>
local role_map = {}

local function update()
    while true do
        local entity_map = entityMgr:get_sync_info()
        if table.size(entity_map) > 0 then
            channel:publish(entity_map)
        end
        skynet.sleep(10)
    end
end

function init( ... )
    config = require(skynet.getenv("config"))
    entityMgr = require "entity.entityMgr".New()

    channel = mc.new()

    local start_arge = {...}
    sceneInfo.sceneId = start_arge[1]
    sceneInfo.sceneName = start_arge[2]
    sceneInfo.serviceName = SERVICE_NAME
    sceneInfo.handle = skynet.self()
    sceneInfo.channel = channel.channel

    sceneConfig = require("config/" .. sceneInfo.sceneName)



    skynet.fork(update)
end

function response.get_scene_param()
    return sceneInfo
end

---@param roleAttrib RoleAttrib
function response.role_enter_scene(agent, roleAttrib, status)
    local roleId = roleAttrib.roleId
    if role_map[roleId] then skynet.error("玩家已经在场景中") return false end
    local role = entityMgr:create_player(roleAttrib, status)
    ---@class SceneRoleInfo
    local roleInfo =
    {
        agent = agent,
        role = role
    }
    role_map[roleId] = roleInfo


    --通知其他玩家
    --channel:publish(entity_map)


    return true
end

function response.role_leave_scene(roleId)
    if not role_map[roleId] then skynet.error("玩家不在场景中") return false end
    role_map[roleId] = nil
    skynet.error(" exit game")
    return true
end

---@param args Sync_Pos
function accept.sync_pos(roleId, args)
    local roleInfo = role_map[roleId]
    if not roleInfo then skynet.error("玩家不在场景中") return false end
    entityMgr.sync_pos(roleInfo.role.aoiData.aoiId, args)
end


function exit( ... )

end
