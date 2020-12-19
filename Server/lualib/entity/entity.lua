-----------------------------------------------------------
------@class AOIStatus
local status = {
    aoiId = nil,
    ---状态
    pos_x = nil,
    pos_y = nil,
    pos_z = nil,
    forward = nil,
}
------@class AOIData
local aoiData =
{
    aoiId = nil,
    paramId = nil,--- 对于玩家来说是roleId, 对于配表单位来说是elementId
    modelId = nil,
    type = nil,
    name = nil,
    ---@type AOIStatus
    status = nil,

    ---服务端用，标记有没有修改
    dirty = false
}
-----------------------------------------------------------

---@class entity
local entity = class("entity")
-----------------------------------------------------------

function entity:ctor(...)
    ---@type AOIData
    self.aoiData = {}
end


return entity