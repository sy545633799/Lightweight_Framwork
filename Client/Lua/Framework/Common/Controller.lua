--- 管理基类， Todo
--- Created by shenyi
--- DateTime: 2019.6.9

---@class Controller:MessageListener
Controller = BaseClass("Controller", MessageListener)

local controllerMap = {}

function Controller:OnCreate() end

function Controller:ctor()
    table.insert(controllerMap, self)
    self.Login = false
    self.Logout = false
end

function Controller:Login()
    for _, mgr in ipairs(controllerMap) do
        mgr:OnLogin()
    end
end


function Controller:Logout()
    for _, mgr in ipairs(controllerMap) do
        mgr:RemoveAllTimer()
        mgr:OnLogout()
    end
end

function Controller:OnLogin() end


--跳转场景
function Controller:OnLogout() end