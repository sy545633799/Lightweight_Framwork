local entity = require "entity.entity"
---@class monster:entity
local monster = class("monster", entity)
-----------------------------------------------------------
local monster_config = require("config/Monster")

---@param element SceneElement
function monster:ctor(aoiId, elementId, element)
    self.element = element
    self.monsterConfig = monster_config[element.ConfigId]
    ---@type AOIAttrib
    local attrib = {}
    attrib.type = entity_types.monster
    attrib.elementId = elementId
    attrib.modelId = self.monsterConfig.ModelId
    self.aoiData.attrib = attrib
    ---trans
    self.aoiData.trans = {}
    self:random_pos()
    ---status
    self.aoiData.status =
    {
        max_hp = self.monsterConfig.HP,
        hp = self.monsterConfig.HP,
        max_mp = self.monsterConfig.MP,
        mp = self.monsterConfig.MP,
        atn = self.monsterConfig.STR,
        int = self.monsterConfig.INT,
        def = self.monsterConfig.DEF,
        res = self.monsterConfig.RES,
        spd = self.monsterConfig.SPD,
        crt = self.monsterConfig.CRT,
    }
end

function monster:random_pos()
    local radius = self.element.RanRadius
    local trans = self.aoiData.trans
    trans.pos_x = self.element.PosX + math.random(-radius, radius)
    trans.pos_y = self.element.PosY
    trans.pos_z = self.element.PosZ + math.random(-radius, radius)
    trans.forward = self.element.ForwardY
    trans.dirty = false
end



return monster