local skynet = require "skynet"

local user = User

function RPC:asyn_pos(args)
    user.scene_post.sync_pos(user.roleInfo.attrib.roleId, args)
end
