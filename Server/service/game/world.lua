local skynet = require "skynet"
local snax = require "skynet.snax"

local config

local snax_mongod, snax_uid
---@type table<number, SceneServiceInfo>
local all_scene_service = {}
---@type table<string, SceneRoleInfo>
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
        local scene_info = service.req.get_scene_info()
        ---@class SceneServiceInfo
        local sceneInfo =
        {
            scene_info = scene_info,
            service_req = service.req,
            service_post = service.post
        }
        all_scene_service[id] = sceneInfo
    end
end

---@param roleInfo RoleInfo
---@param sceneId number
---@return boolean, SceneInfo
function response.role_enter_game(agent, roleInfo, sceneId)
    local serv_scene_info = all_scene_service[sceneId]
    if not serv_scene_info then
        return false
    end

    ---@class SceneRoleInfo
    local sceneRole =
    {
        sceneId = sceneId,
        sceneInfo = serv_scene_info,
        agent = agent
    }

    online_roles[roleInfo.attrib.roleId] = sceneRole
    local ok = serv_scene_info.service_req.role_enter_scene(agent, roleInfo.attrib, roleInfo.status)
    return ok, serv_scene_info.scene_info
end

---@param roleInfo RoleInfo
function response.role_leave_game(roleInfo)
    local serv_scene_info = all_scene_service[roleInfo.attrib.sceneId]
    if not serv_scene_info then

        return false
    end
    online_roles[roleInfo.attrib.roleId] = nil
    local ok = serv_scene_info.service_req.role_leave_scene(roleInfo.attrib.roleId)

    return ok
end

---@param roleInfo RoleInfo
function response.role_switch_scene(roleInfo, sceneId)

end


function exit( ... )

end