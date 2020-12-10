require "Logic/Global/Global"
local launchUI = CS.Game.LaunchUI
local update_handle
local modules = GetModule()
local index = 1

--local gametest = CS.Game.GameUtil
--gametest.Test()

GameMain = {}

function GameMain.Update()
    if index <= table.count(modules) then
        if modules[index].obj then
            require(modules[index].path).NewByTable(modules[index].obj)
        else
            require(modules[index].path)
        end
        index = index + 1
        launchUI.ShowProcess(index/table.count(modules) * 0.5 + 0.5)
    else
        index = 1
        UpdateBeat:RemoveListener(update_handle)
        coroutine.start(function ()
            SceneManager:SwitchScene(SceneConfig.LoginScene)
        end)
    end
end

function GameMain.Start()
    print("GameMain start...")

    update_handle = UpdateBeat:CreateListener(GameMain.Update)
    UpdateBeat:AddListener(update_handle)

end

-- 场景切换通知
function GameMain.OnLevelWasLoaded(level)
    coroutine.start(function ()
        collectgarbage("collect")
    end)
end

function GameMain.OnApplicationQuit()
    coroutine.start(function ()
        UIManager:Delete()
        SceneManager:Delete()
        NetworkManager:Delete()
    end)
end

return GameMain