require "Logic.Global.Global"

GameMain = {}

function GameMain.Start()
    print("GameMain start...")

    SceneManager:SwitchScene(SceneConfig.LoginScene)
end

-- 场景切换通知
function GameMain.OnLevelWasLoaded(level)
    
    collectgarbage("collect")
end

function GameMain.OnApplicationQuit()
    UIManager:UnLoadAllView(true)
    NetworkManager.Dispose()
    --XluaUtil.print_func_ref_by_csharp()
end

return GameMain