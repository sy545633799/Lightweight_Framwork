

---@class entity
local entity = class("entity")
-----------------------------------------------------------

function entity:ctor(...)
    ---@type AOIData
    self.aoiData = {}
end


return entity