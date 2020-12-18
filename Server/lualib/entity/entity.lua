---@class SyncData
local syncData =
{
    aoiId = nil,
    pos_x = nil,
    pos_y = nil,
    pos_z = nil,
    foward = nil
}

-----------------------------------------------------------
---@class AOIData
local aoiData =
{
    aoiId = nil,
    --- 对于玩家来说是roleId, 对于配表单位来说是elementId
    paramId = nil,
    type = nil,
    name = nil,

}
-----------------------------------------------------------

---@class entity
local entity = class("entity")
-----------------------------------------------------------

function entity:ctor(...)
    --self.aoiData = nil
    --self.syncData = nil
end


return entity