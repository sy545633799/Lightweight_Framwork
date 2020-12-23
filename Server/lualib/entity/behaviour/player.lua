local entity = require "entity.entity"
---@class player:entity
local player = class("player", entity)
-----------------------------------------------------------
---@param roleInfo RoleInfo
function player:ctor(aoiId, roleInfo)
    ---@type AOIAttrib
    self.aoiData.attrib = roleInfo.attrib
    self.aoiData.attrib.roleId = roleInfo.roleId
    self.aoiData.attrib.name = roleInfo.name
    self.aoiData.attrib.type = entity_types.player
    ---@type AOITrans
    self.aoiData.trans = roleInfo.trans
    self.aoiData.trans.dirty = false
    ---状态
end

---@param args Sync_Trans
function player:c2s_sync_trans(args)
    --TODO 距离校验
    self.aoiData.trans = args.trans
    self.aoiData.trans.dirty = true
end



return player