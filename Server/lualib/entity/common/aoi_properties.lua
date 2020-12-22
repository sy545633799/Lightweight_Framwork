
---@class AOIAttrib
AOIAttrib = {
    paramId = 1,--- 对于玩家来说是roleId, 对于配表单位来说是elementId
    modelId = 2,
    type = 3,
    name = 4,
    ---服务端用，标记有没有修改
    dirty = false
}

---@class AOIStatus
AOIStatus = {
    str = 1,
    mag = 2,
    dex = 3,
    max_hp = 4,
    hp = 5,
    max_mp = 6,
    mp = 7,
    atn = 8,
    int = 9,
    def = 10,
    res = 11,
    spd = 12,
    crt = 13,
    ---服务端用，标记有没有修改
    dirty = false
}

------@class AOITrans
AOIStatus = {
    ---状态
    pos_x = 1,
    pos_y = 2,
    pos_z = 3,
    forward = 4,
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