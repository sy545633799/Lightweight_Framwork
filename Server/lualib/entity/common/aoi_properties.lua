-----------------------------------------------------------
------@class AOIStatus
AOIStatus = {
    aoiId = "aoiId",
    ---状态
    pos_x = "pos_x",
    pos_y = "pos_y",
    pos_z = "pos_z",
    forward = "forward",
}

------@class AOIData
AOIData =
{
    aoiId = "aoiId",
    paramId = "paramId",--- 对于玩家来说是roleId, 对于配表单位来说是elementId
    modelId = "modelId",
    type = "type",
    name = "name",
    ---@type AOIStatus
    status = "status",

    ---服务端用，标记有没有修改
    dirty = false
}