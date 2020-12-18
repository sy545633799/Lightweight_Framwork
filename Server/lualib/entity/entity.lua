
---@class entity
local entity = class("entity")
-----------------------------------------------------------
---@class AOIData
local aoiData =
{
    roleId = nil,
    type = nil,
    name = nil,
    pos_x = nil,
    pos_y = nil,
    pos_z = nil,
    foward = nil
}

function entity:ctor(status)
    ---@type AOIData
    self.aoiData = status
end




return entity