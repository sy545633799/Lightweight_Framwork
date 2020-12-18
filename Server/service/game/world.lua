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
        local channelId = service.req.get_channel_id()
        all_scenes[id] = { service = service, channelId = channelId}
    end
end

---@param roleInfo RoleInfo
---@param sceneId number
function response.role_enter_game(agent, roleInfo, sceneId)
    local sceneInfo = all_scenes[sceneId]
    if not sceneInfo then
        return false
    end
    online_roles[roleInfo.attrib.roleId] = { sceneId = sceneId, sceneInfo = sceneInfo, agent = agent}
    local ok = sceneInfo.service.req.role_enter_scene(agent, roleInfo.attrib, roleInfo.status)
    return ok, sceneInfo.channelId
end

function response.role_leave_game(roleInfo)
    local sceneInfo = all_scenes[roleInfo.attrib.sceneId]
    if not sceneInfo then

        return false
    end
    online_roles[roleInfo.attrib.roleId] = nil
    local ok = sceneInfo.service.req.role_leave_scene(roleInfo.attrib.roleId)

    return ok
end

function response.role_switch_scene(roleInfo, sceneId)

end

function accept.sync_pos(roleId, info)
    local roleInfo = online_roles[roleId]
    if not roleInfo then return end
    roleInfo.sceneInfo.service.post.sync_pos(roleId, info)
end



function exit( ... )

end