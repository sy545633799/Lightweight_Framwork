local skynet = require "skynet"

local user = User

---@class Sync_Trans
local Sync_Trans =
{
    pos_x = "pos_x",
    pos_y = "pos_y",
    pos_z = "pos_z",
    forward = "forward"
}

---@param args AOITrans
function RPC:c2s_sync_trans(args)
    user.scene_post.c2s_sync_trans(user.roleInfo.roleId, args)
end
