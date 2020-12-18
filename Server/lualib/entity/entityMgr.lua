---@class entityMgr
local entityMgr = class("entityMgr")
require "entity.entity_types"
local player = require "entity.behaviour.player"
----------------------------------------------------------------
local aoiId = 0
---@type table<number, entity>
local entity_map = {}
---@type table<number, AOIData>
local aoi_map = {}
---@param attrib RoleAttrib
---@param status RoleStatus
---@return player
function entityMgr:create_player(attrib, status)
    aoiId = aoiId + 1
    ---@type player
    local player = player.New(attrib, status, aoiId)
    entity_map[aoiId] = player
    aoi_map[aoiId] = player.aoiData
    return player
end

function entityMgr:create_monster()
    
end

---@param args Sync_Pos
function entityMgr:sync_pos(aoiId, args)
    ---@type player
    local player = entity_map[aoiId]
    if not player then return end
    player:sync_pos(args)
end

---@return table<number, entity>
function entityMgr:get_all_aoiData()
    return aoi_map
end

---@return table<number, entity>
function entityMgr:get_sync_info()
    local info = {}
    for id, entity in pairs(entity_map) do
        if entity.aoiData.dirty then
            info[id] = entity.aoiData.status
            entity.aoiData.dirty = false
        end
    end
    return info
end

function entityMgr:remove_entity(aoiId)
    if entity_map[aoiId] then
        entity_map[aoiId] = nil
        aoi_map[aoiId] = nil
    end
end




return entityMgr