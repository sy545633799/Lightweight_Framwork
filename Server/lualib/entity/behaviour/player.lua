local entity = require "entity.entity"
---@class player:entity
local player = class("player", entity)
-----------------------------------------------------------
---@param status RoleStatus
---@param attrib RoleAttrib
function player:ctor(attrib, status, aoiId)
    ---@type AOIData
    self.aoiData = {}
    self.aoiData.aoiId = aoiId
    self.aoiData.paramId = attrib.roleId
    self.aoiData.name = attrib.name
    self.aoiData.type = entity_types.player

    ---@type SyncData
    self.entityData = status
    self.entityData.aoiId = aoiId


end

return player