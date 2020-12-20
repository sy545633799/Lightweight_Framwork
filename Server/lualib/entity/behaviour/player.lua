local entity = require "entity.entity"
---@class player:entity
local player = class("player", entity)
-----------------------------------------------------------
---@param status RoleTrans
---@param attrib RoleAttrib
function player:ctor(attrib, trans, aoiId)
    self.aoiData.aoiId = aoiId
    ---@type AOIAttrib
    self.aoiData.attrib = attrib
    self.aoiData.attrib.paramId = attrib.roleId
    self.aoiData.attrib.type = entity_types.player
    ---@type AOITrans
    self.aoiData.trans = trans
    self.aoiData.trans.dirty = false
    --状态
end

---@param args Sync_Trans
function player:c2s_sync_trans(args)
    --TODO 距离校验
    self.aoiData.trans = args.trans
    self.aoiData.trans.dirty = true
end



return player