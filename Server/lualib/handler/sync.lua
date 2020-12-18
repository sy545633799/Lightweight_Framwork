local skynet = require "skynet"

local user = User

---@class Sync_Pos
local sync_pos =
{
    pos_x = nil,
    pos_y = nil,
    pos_z = nil,
    fowrad = nil
}

---@param args AOIStatus
function RPC:asyn_pos(args)
    user.scene_post.sync_pos(user.roleInfo.attrib.roleId, args)
end
