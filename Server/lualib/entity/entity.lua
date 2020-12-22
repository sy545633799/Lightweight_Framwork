--[[
    author:shenyi
    des: entity 本质上是个fsmmanager
    time:2020/12/21
]]

---@class entity
local entity = class("entity")
-----------------------------------------------------------

function entity:ctor(aoiId, ...)
    ---@type AOIData
    self.aoiData = {}
    self.aoiData.aoiId = aoiId

end

function entity:update(deltaTime)

end

function entity:is_dead()

end


return entity