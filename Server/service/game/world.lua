local skynet = require "skynet"
local snax = require "skynet.snax"

local config

local snax_mongod, snax_uid
---@type table<number, WorldSceneInfo>
local scene_services = {}
---@type table<string, WorldRoleInfo>
local online_roles = {}
---@class World_Req
local response = response
---@class World_Post
local accept = accept

function init( ... )
    config = require(skynet.getenv("config"))
    snax_uid = snax.uniqueservice("common/uid")
    snax_mongod = snax.uniqueservice("common/mongod")

    local worldConfig = require("config/World")
    for id, config in pairs(worldConfig) do
        local sceneName = config.Resource
        local service = snax.newservice("game/scene", id, sceneName)
        local scene_param = service.req.get_scene_param()
        ---@class WorldSceneInfo
        local sceneInfo =
        {
            scene_param = scene_param,
            ---@type Scene_Req
            service_req = service.req,
            ---@type Scene_Post
            service_post = service.post
        }
        scene_services[id] = sceneInfo
    end
end

---@param roleInfo RoleInfo
---@param sceneId number
---@return boolean, SceneParam
function response.role_enter_game(agent, roleInfo, sceneId)
    local scene_Info = scene_services[sceneId]
    if not scene_Info then
        return false
    end

    ---@class WorldRoleInfo
    local sceneRole =
    {
        sceneId = sceneId,
        sceneInfo = scene_Info,
        agent = agent
    }

    online_roles[roleInfo.attrib.roleId] = sceneRole
    local ok, aoiId, aoi_map = scene_Info.service_req.role_enter_scene(agent, roleInfo.attrib, roleInfo.status)
    return ok, aoiId, aoi_map, scene_Info.scene_param
end

---@param roleInfo RoleInfo
function accept.role_leave_game(roleId)
    local roleInfo = online_roles[roleId]
    if not roleInfo then
        return false
    end

    local sceneInfo = roleInfo.sceneInfo
    local ok = sceneInfo.service_req.role_leave_scene(roleId)
    online_roles[roleId] = nil
    return ok
end

---@param roleInfo RoleInfo
function response.role_switch_scene(roleId, sceneId)

end


function exit( ... )

end