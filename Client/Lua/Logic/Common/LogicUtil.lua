
function GetConfig(name)
    return require("Logic/Config/" .. name)
end

function ShowLoadProcess(process)
    EventManager:Broadcast(EventNames.Scene.LoadProcess, process)
end
