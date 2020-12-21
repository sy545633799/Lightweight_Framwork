local skynet = require "skynet"
local snax = require "skynet.snax"
local mc = require "skynet.multicast"

local event_names = event_names
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
            channel:publish(event_names.scene.s2c_aoi_trans, entity_map)
        end

        local create_map = entityMgr:get_create_map()
        if table.size(create_map) > 0 then
            channel:publish(event_names.scene.create_entities, create_map)
        end

        local delete_map = entityMgr:get_delete_map()
        if table.size(delete_map) > 0 then
            channel:publish(event_names.scene.delete_entities, delete_map)
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
function response.role_enter_scene(agent, roleAttrib, trans)
    local roleId = roleAttrib.roleId
    
    if role_map[roleId] then skynet.error("玩家已经在场景中") return false end
    local role = entityMgr:create_player(roleAttrib, trans)
    ---@class SceneRoleInfo
    local roleInfo =
    {
        agent = agent,
        role = role
    }
    role_map[roleId] = roleInfo

    local aoi_map = entityMgr:get_all_aoiData()
    return true, role.aoiData.aoiId, aoi_map
end

function response.role_leave_scene(roleId)
    local roleInfo = role_map[roleId]
    if not roleInfo then skynet.error("玩家不在场景中") return false end
    local aoiId = roleInfo.role.aoiData.aoiId
    entityMgr:remove_entity(aoiId)
    role_map[roleId] = nil

    return true
end

---@param args Sync_Trans
function accept.c2s_sync_trans(roleId, args)
    local roleInfo = role_map[roleId]
    if not roleInfo then skynet.error("玩家不在场景中") return false end
    entityMgr:c2s_sync_trans(roleInfo.role.aoiData.aoiId, args)
end


function exit( ... )

    --TODO 处理channel
end
