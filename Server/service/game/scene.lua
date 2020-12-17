local skynet = require "skynet"
local snax = require "skynet.snax"

local config
local snax_mongod, snax_uid


local sceneId, sceneName, sceneConfig

function init( ... )
    config = require(skynet.getenv("config"))
    snax_uid = snax.uniqueservice("common/uid")
    snax_mongod = snax.uniqueservice("common/mongod")

    local start_arge = {...}
    sceneId = start_arge[1]
    sceneName = start_arge[2]
    sceneConfig = require("config/" .. sceneName)
end


function response.role_enter_scene(account)

end

function response.role_leave_scene()

end

function exit( ... )

end
