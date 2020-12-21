---@class entityMgr
local entityMgr = class("entityMgr")
require "entity.entity_types"
require "entity.common.aoi_properties"
local player = require "entity.behaviour.player"
----------------------------------------------------------------
local aoiId = 0
---@type table<number, entity>
local entity_map = {}
---@type table<number, AOIData>
local aoi_map = {}
---@type table<number, AOIData>
local create_map = {}
---@type table<number, number>
local delete_map = {}

---@param attrib RoleAttrib
---@param status RoleTrans
---@return player
function entityMgr:create_player(attrib, trans)
    aoiId = aoiId + 1
    ---@type player
    local player = player.New(attrib, trans, aoiId)
    entity_map[aoiId] = player
    aoi_map[aoiId] = player.aoiData
    create_map[aoiId] = player.aoiData
    return player
end

function entityMgr:create_monster()

end

---@param args Sync_Trans
function entityMgr:c2s_sync_trans(aoiId, args)
    ---@type player
    local player = entity_map[aoiId]
    if not player then return end
    player:c2s_sync_trans(args)
end

---@return table<number, entity>
function entityMgr:get_all_aoiData()
    return aoi_map
end

---@return table<number, entity>
function entityMgr:get_sync_info()
    local info = {}
    for id, entity in pairs(entity_map) do
        if entity.aoiData.trans.dirty then
            info[id] = entity.aoiData
            entity.aoiData.trans.dirty = false
        end
    end
    return info
end

---@public 获取一次就置空
---@return table<number, AOIData>
function entityMgr:get_create_map()
    local t = create_map
    create_map = {}
    return t
end

---@public 获取一次就置空
---@return table<number, number>
function entityMgr:get_delete_map()
    local t = delete_map
    delete_map = {}
    return t
end

function entityMgr:remove_entity(aoiId)
    if entity_map[aoiId] then
        entity_map[aoiId] = nil
        aoi_map[aoiId] = nil
        table.insert(delete_map, aoiId)
    end
end




return entityMgr