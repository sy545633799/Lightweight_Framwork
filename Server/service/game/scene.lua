local skynet = require "skynet"
local snax = require "skynet.snax"
local mc = require "skynet.multicast"


---@type entityMgr
local entityMgr

local config
local snax_mongod, snax_uid
local channel

local sceneId, sceneName, sceneConfig

local role_map = {}

function init( ... )
    config = require(skynet.getenv("config"))
    snax_uid = snax.uniqueservice("common/uid")
    snax_mongod = snax.uniqueservice("common/mongod")

    channel = mc.new()

    entityMgr = require "entity.entityMgr".New()

    local start_arge = {...}
    sceneId = start_arge[1]
    sceneName = start_arge[2]
    sceneConfig = require("config/" .. sceneName)
end

function response.get_channel_id()
    return channel.channel
end

---@param roleAttrib RoleAttrib
function response.role_enter_scene(agent, roleAttrib, aoiData)
    local roleId = roleAttrib.roleId
    if role_map[roleId] then skynet.error("玩家已经在场景中") return false end
    local role = entityMgr:create_entity(roleAttrib, aoiData)


    role_map[roleId] = { agent = agent, role = role}
    skynet.error(roleAttrib.name .. " enter game")
    return true
end

function response.role_leave_scene(roleId)
    if not role_map[roleId] then skynet.error("玩家不在场景中") return false end
    role_map[roleId] = nil
    skynet.error(" exit game")
    return true
end


function exit( ... )

end
