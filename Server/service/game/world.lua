local skynet = require "skynet"
local snax = require "skynet.snax"

local config
local snax_mongod, snax_uid
local all_scenes = {}
local online_roles = {}

function init( ... )
    config = require(skynet.getenv("config"))
    snax_uid = snax.uniqueservice("common/uid")
    snax_mongod = snax.uniqueservice("common/mongod")

    local worldConfig = require("config/World")
    for id, config in pairs(worldConfig) do
        local sceneName = config.Resource
        local service = snax.newservice("game/scene", id, sceneName)
        all_scenes[id] = service
    end
end

---@param roleInfo RoleInfo
---@param sceneId number
function response.role_enter_game(roleInfo, sceneId)
    local service = all_scenes[sceneId]
    if not service then
        return false
    end
    online_roles[roleInfo.attrib.roleId] = service
    local ok = service.req.role_enter_scene(roleInfo)
    return ok
end

function response.role_leave_game(roleInfo)
    local service = all_scenes[roleInfo.attrib.scene]
    if not service then
        return false
    end
    online_roles[roleInfo.attrib.roleId] = nil
    local ok = service.req.role_leave_scene(roleInfo)
    return ok
end

function response.role_switch_scene(roleInfo, sceneId)

end


function exit( ... )

end