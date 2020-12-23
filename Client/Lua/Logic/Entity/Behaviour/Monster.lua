local base = Entity
---@class Monster:Entity
local Monster = BaseClass("Monster", base)
-----------------------------------------------------------
local monster_config = require("Logic/Config/Monster")

function Monster:ctor(aoiData, element)
    self.element = element
    self.config = monster_config[element.ConfigId]
end

function Monster:OnBodyCreate(componets)
    base.OnBodyCreate(self, componets)
    self.nameComp:SetName(self.config.NameId)
end




return Monster