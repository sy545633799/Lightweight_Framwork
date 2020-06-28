---@type Game.CommonUtil
CommonUtil = CS.Game.CommonUtil
---@type Game.UIUtil
UIUtil = CS.Game.UIUtil
---@type Game.UIListener
UIListener = CS.Game.UIListener
--@type Game.InputManager
InputManager = CS.Game.InputManager
---@type Game.LoadManager
LoadManager = CS.Game.LoadManager
---@type Game.ResourceManager
ResourceManager = CS.Game.ResourceManager
---@type Game.TcpManager
TcpManager = CS.Game.TcpManager

require "Common.Main"
require "Framework.Common.BaseClass"
require "Framework.Common.ConstClass"
require "Framework.Common.DataClass"
require "Framework.Common.Updatable"
require "Framework.Common.MessageListener"
require "Framework.Common.BaseModel"
require "Framework.Common.ModelListener"
require "Framework.Util.TimerUtil"
require "Framework.Util.CoroutineUtil"
--UI
require "Framework.UI.Common.UIContent"
require "Framework.UI.Common.UIBaseView"
require "Framework.UI.Common.UIBaseCtrl"
require "Framework.UI.Common.UITabCtrl"
--Widget
require "Framework.UI.Widget.UIText"
require "Framework.UI.Widget.UIImage"
require "Framework.UI.Widget.UIButton"
require "Framework.UI.Widget.UIToggle"
require "Framework.UI.Widget.UISlider"
require "Framework.UI.Widget.UIListView"
require "Framework.UI.Widget.UIWidget"
--Event
require "Framework.Event.EventNames"


--Manager(TODO)
---@type EventManager
EventManager = require("Framework.Event.EventManager").New()
---@type SceneManager
SceneManager = require("Framework.Scene.SceneManager").New()
---@type UIManager
UIManager = require("Framework.UI.UIManager").New()
---@type NetworkManager
NetworkManager = require("Framework.Network.NetworkManager").New()