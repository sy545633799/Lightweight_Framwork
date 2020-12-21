---@class entityMgr
local entityMgr = {}
require "entity.entity_types"
require "entity.common.aoi_properties"
local player = require "entity.behaviour.player"
local monster = require "entity.behaviour.monster"
----------------------------------------------------------------
local entity_types = entity_types

local aoiId = 0
---@type table<number, entity> 当前激活(未死亡或者移除)的entity
local alive_map = {}
---@type table<number, AOIData> 但钱
local aoi_map = {}
---@type table<number, AOIData>
local create_map = {}
---@type table<number, number>
local delete_map = {}

---@param entity entity
local function onCreateEntity(entity)
    alive_map[aoiId] = entity
    aoi_map[aoiId] = entity.aoiData
    create_map[aoiId] = entity.aoiData
end

---@param attrib RoleAttrib
---@param status RoleTrans
---@return player
function entityMgr:create_player(attrib, trans)
    aoiId = aoiId + 1
    ---@type player
    local instance = player.New(aoiId, attrib, trans)
    onCreateEntity(instance)
    return instance
end

---@param scene_config table<number, SceneElement>
function entityMgr:create_elements(scene_config)
    for k, v in pairs(scene_config) do
        if v.Type == entity_types.monster then
            for _ = 1, v.Count do
                aoiId = aoiId + 1
                local instance = monster.New(aoiId, k, v)
                onCreateEntity(instance)
            end
        end
    end
end

---@param args Sync_Trans
function entityMgr:c2s_sync_trans(aoiId, args)
    ---@type player
    local player = alive_map[aoiId]
    if not player then return end
    player:c2s_sync_trans(args)
end

---@return table<number, entity>
function entityMgr:get_all_aoiData()
    return aoi_map
end

---@return table<number, entity>
function entityMgr:get_alive_map()
    return alive_map
end

---@return table<number, entity>
function entityMgr:get_sync_info()
    local info = {}
    for id, entity in pairs(alive_map) do
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

---@public 加入entity(比如重新激活的entity)
---@param entity entity
function entityMgr:add_entity(entity)
    if alive_map[entity.aoiData.aoiId] then
        alive_map[aoiId] = entity
        aoi_map[aoiId] = entity.aoiData
        table.insert(create_map, entity)
    end
end

---@public 移除entity
function entityMgr:remove_entity(aoiId)
    if alive_map[aoiId] then
        alive_map[aoiId] = nil
        aoi_map[aoiId] = nil
        table.insert(delete_map, aoiId)
    end
end



return entityMgr