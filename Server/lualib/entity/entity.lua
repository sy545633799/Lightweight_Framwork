

---@class entity
local entity = class("entity")
-----------------------------------------------------------

function entity:ctor(aoiId, ...)
    ---@type AOIData
    self.aoiData = {}
    self.aoiData.aoiId = aoiId
end


return entity