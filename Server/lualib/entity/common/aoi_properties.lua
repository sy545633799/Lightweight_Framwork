
---@class AOIAttrib
AOIAttrib = {
    roleId = 1,
    elementId = 2,
    modelId = 3,
    type = 4,
    name = 5,
    ---服务端用，标记有没有修改
    dirty = false
}

---@class AOIStatus
AOIStatus = {
    max_hp = 1,
    hp = 2,
    max_mp = 3,
    mp = 4,
    is_crit = 5,
    ---服务端用，标记有没有修改
    dirty = false
}

---@class AOITrans
AOITrans = {
    ---状态
    pos_x = 1,
    pos_y = 2,
    pos_z = 3,
    forward = 4,
    ---服务端用，标记有没有修改
    dirty = false
}

---@class AOIData
AOIData =
{
    aoiId = "aoiId",
    ---@type AOIAttrib
    attrib = "attrib",
    ---@type AOITrans
    trans = "trans",
    ---@type AOIStatus
    status = "trans",
    dirty = false
}