local entity = require "entity.entity"
---@class player:entity
local player = class("player", entity)
-----------------------------------------------------------
---@param status RoleStatus
---@param attrib RoleAttrib
function player:ctor(attrib, status, aoiId)
    self.aoiData.aoiId = aoiId
    self.aoiData.paramId = attrib.roleId
    self.aoiData.modelId = attrib.modelId
    self.aoiData.name = attrib.name
    self.aoiData.type = entity_types.player
    --状态
    self.aoiData.status = status
    self.aoiData.status.aoiId = aoiId
    self.aoiData.aoiId = aoiId
    self.aoiData.dirty = false
end

---@param args Sync_Pos
function player:sync_pos(args)
    --TODO 距离校验
    self.aoiData.status.pos_x = args.pos_x
    self.aoiData.status.pos_y = args.pos_y
    self.aoiData.status.pos_z = args.pos_z
    self.aoiData.status.forward = args.forward
    self.aoiData.dirty = true
end



return player