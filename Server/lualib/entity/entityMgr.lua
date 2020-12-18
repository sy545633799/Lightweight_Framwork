---@class entityMgr
local entityMgr = class("entityMgr")
require "entity.entity_types"
local player = require "entity.behaviour.player"
----------------------------------------------------------------
---@type table<string, entity>
local entity_map = {}

---@param attrib RoleAttrib
---@param status RoleStatus
function entityMgr:create_player(attrib, status)
    local player = player.New(status, attrib)
    entity_map[attrib.roleId] = player
end

---@return table<string, entity>
function entityMgr:get_aoi_data()
    local aoi = {}
    for id, entity in pairs(entity_map) do
        aoi[id] = entity.aoiData
    end
    return aoi
end

function entityMgr:remove_entity(roleId)
    if entity_map[roleId] then
        entity_map[roleId] = nil
    end
end



return entityMgr