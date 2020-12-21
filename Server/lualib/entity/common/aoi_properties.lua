
------@class AOIAttrib
AOIAttrib = {
    paramId = "paramId",--- 对于玩家来说是roleId, 对于配表单位来说是elementId
    modelId = "modelId",
    type = "type",
    name = "name",
    ---服务端用，标记有没有修改
    dirty = false
}

------@class AOITrans
AOITrans = {
    ---状态
    pos_x = "pos_x",
    pos_y = "pos_y",
    pos_z = "pos_z",
    forward = "forward",
    ---服务端用，标记有没有修改
    dirty = false
}

------@class AOIData
AOIData =
{
    aoiId = "aoiId",
    ---@type AOIAttrib
    attrib = "attrib",
    ---@type AOITrans
    trans = "trans",
}