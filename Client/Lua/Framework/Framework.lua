---@type UnityEngine.Application
Application = CS.UnityEngine.Application
---@type CS.UnityEngine.RuntimePlatform
RuntimePlatform = CS.UnityEngine.RuntimePlatform
---@type UnityEngine.PlayerPrefs
PlayerPrefs = CS.UnityEngine.PlayerPrefs

---@type Game.MainCamera
MainCamera = CS.Game.MainCamera.Instance
---@type Game.CommonUtil
CommonUtil = CS.Game.CommonUtil
---@type Game.UIUtil
UIUtil = CS.Game.UIUtil
---@type Game.UIListener
UIListener = CS.Game.UIListener
---@type Game.InputManager
InputManager = CS.Game.InputManager
---@type Game.LoadManager
LoadManager = CS.Game.LoadManager
---@type Game.ResourceManager
ResourceManager = CS.Game.ResourceManager
---@type Game.TcpManager
TcpManager = CS.Game.TcpManager
---@type  Game.MapManager
MapManager = CS.Game.MapManager
---@type  Game.EntityBehaviorManager
EntityBehaviorManager = CS.Game.EntityBehaviorManager
---------------------------------------------------------------------------------------------
local modules = {}

function RegisterModule(path)
    local module ={ path = path }
    table.insert(modules, module)
end

function RegisterGlobalModule(path)
    local obj = {}
    local module ={ obj = obj, path = path}
    table.insert(modules, module)
    return obj
end

function GetModule()
    return modules
end

local registerModule = RegisterModule
local registerGlobalModule = RegisterGlobalModule
--Common
registerModule( "Framework/Common/BaseClass")
registerModule( "Framework/Common/ConstClass")
registerModule( "Framework/Common/DataClass")
registerModule( "Framework/Common/Updatable")
registerModule( "Framework/Common/Model")
registerModule( "Framework/Common/ModelListener")
registerModule( "Framework/Common/MessageListener")
registerModule( "Framework/Common/Controller")
registerModule( "Framework/Util/TimerUtil")
registerModule( "Framework/Util/CoroutineUtil")
--UI
registerModule( "Framework/UI/LayerGroup")
registerModule( "Framework/UI/UIConfig")
registerModule( "Framework/UI/TipsConfig")
registerModule( "Framework/UI/AtlasNames")
--Base
registerModule( "Framework/UI/Common/UIContent")
--Widget
registerModule( "Framework/UI/Widget/UIText")
registerModule( "Framework/UI/Widget/UIImage")
registerModule( "Framework/UI/Widget/UIButton")
registerModule( "Framework/UI/Widget/UIToggle")
registerModule( "Framework/UI/Widget/UIToggleGroup")
registerModule( "Framework/UI/Widget/UIDropDown")
registerModule( "Framework/UI/Widget/UISlider")
registerModule( "Framework/UI/Widget/UIListView")
registerModule( "Framework/UI/Widget/UIWidget")
registerModule( "Framework/UI/Widget/UIInput")
registerModule( "Framework/UI/Widget/UIModel")
registerModule( "Framework/UI/Widget/UICanvasGroup")
--Component
registerModule( "Framework/UI/Common/UIContain")
registerModule( "Framework/UI/Common/UIBaseView")
registerModule( "Framework/UI/Common/UIBaseItem")
registerModule( "Framework/UI/Common/UIBaseTips")
registerModule( "Framework/UI/Common/UITabView")


---@type EventManager
EventManager = registerGlobalModule("Framework/Event/EventManager")
---@type SceneManager
SceneManager = registerGlobalModule("Framework/Scene/SceneManager")
---@type UIManager
UIManager = registerGlobalModule("Framework/UI/UIManager")
---@type TipsManager
TipsManager = registerGlobalModule("Framework/UI/TipsManager")
---@type NetworkManager
NetworkManager = registerGlobalModule("Framework/Network/NetworkManager")