---@class entityMgr
local entityMgr = class("entityMgr")
require "entity.entity_types"
local player = require "entity.behaviour.player"
----------------------------------------------------------------
local aoiId = 0
---@type table<string, entity>
local entity_map = {}

---@param attrib RoleAttrib
---@param status RoleStatus
---@return player
function entityMgr:create_player(attrib, status)
    aoiId = aoiId + 1
    local player = player.New(attrib, status, aoiId)
    entity_map[aoiId] = player
    return player
end

function entityMgr:create_monster()

end


---@param args Sync_Pos
function entityMgr:sync_pos(aoiId, args)

end

---@return table<string, entity>
function entityMgr:get_sync_info()
    local aoi = {}
    for id, entity in pairs(entity_map) do
        aoi[id] = entity.syncData
    end
    return aoi
end

function entityMgr:remove_entity(aoiId)
    if entity_map[aoiId] then
        entity_map[aoiId] = nil
    end
end




return entityMgr