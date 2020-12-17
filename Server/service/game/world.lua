local skynet = require "skynet"
local snax = require "skynet.snax"

local config
local snax_mongod, snax_uid
local scene_services = {}

function init( ... )
    config = require(skynet.getenv("config"))
    snax_uid = snax.uniqueservice("common/uid")
    snax_mongod = snax.uniqueservice("common/mongod")

    local worldConfig = require("config/World")
    for id, config in pairs(worldConfig) do
        local sceneName = config.Resource
        local service = snax.newservice("game/scene", id, sceneName)
        scene_services[id] = service
    end
end


function response.role_enter_game(account)
    
end

function response.role_leave_game()

end


function exit( ... )

end