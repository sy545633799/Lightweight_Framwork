local entity = require "entity.entity"
---@class player:entity
local player = class("player", entity)
-----------------------------------------------------------
---@param status RoleTrans
---@param attrib RoleAttrib
function player:ctor(attrib, trans, aoiId)
    self.aoiData.aoiId = aoiId
    self.aoiData.paramId = attrib.roleId
    self.aoiData.modelId = attrib.modelId
    self.aoiData.name = attrib.name
    self.aoiData.type = entity_types.player
    --位置
    self.aoiData.trans = trans
    self.aoiData.trans.aoiId = aoiId
    self.aoiData.aoiId = aoiId
    self.aoiData.dirty = false
    --状态
end

---@param args Sync_Trans
function player:c2s_sync_trans(args)
    --TODO 距离校验
    self.aoiData.trans.pos_x = args.pos_x
    self.aoiData.trans.pos_y = args.pos_y
    self.aoiData.trans.pos_z = args.pos_z
    self.aoiData.trans.forward = args.forward
    self.aoiData.dirty = true
end



return player