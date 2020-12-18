local entity = require "entity.entity"
---@class player:entity
local player = class("player", entity)
-----------------------------------------------------------
---@param status RoleStatus
---@param attrib RoleAttrib
function entity:ctor(status, attrib)
    self.aoiData.roleId = attrib.roleId
    self.aoiData.name = attrib.name
    self.aoiData.type = entity_types.player

end


return player