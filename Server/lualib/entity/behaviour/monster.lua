local entity = require "entity.entity"
---@class monster:entity
local monster = class("monster", entity)
-----------------------------------------------------------
---@param monsterConfig SceneElement
function monster:ctor(aoiId, elementId, monsterConfig)
    self.config = monsterConfig
    ---@type AOIAttrib
    local attrib = {}
    attrib.type = entity_types.monster
    attrib.paramId = tostring(elementId)
    attrib.modelId = monsterConfig.ModelID
    self.aoiData.attrib = attrib
    self.aoiData.trans = {}
    self:random_pos()
end

function monster:random_pos()
    local radius = self.config.RanRadius
    local trans = self.aoiData.trans
    trans.pos_x = self.config.PosX + math.random(-radius, radius)
    trans.pos_y = self.config.PosY
    trans.pos_z = self.config.PosZ + math.random(-radius, radius)
    trans.forward = self.config.ForwardY
    trans.dirty = false
end



return monster