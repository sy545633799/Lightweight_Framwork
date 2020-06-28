--- 管理基类， Todo
--- Created by shenyi
--- DateTime: 2019.6.9

---@class Manager:MessageListener
Manager = BaseClass("Manager", MessageListener)

local managerMap = {}

function Manager:OnCreate() end

function Manager:ctor()
    table.insert(managerMap, self)
    self.Login = false
    self.Logout = false
    self.event = CreateEvent()
    self:OnCreate()

end



function Manager:Login(data)
    for _, mgr in ipairs(managerMap) do
        mgr:OnLogin(data)
    end
end

function Manager:AddListener(eventName, func, owner)
    self.event.AddListener(eventName, func, owner)
end

function Manager:RemoveListener(eventName, func, owner)
    self.event.RemoveListener(eventName, func, owner)
end

function Manager:Brocast(key_name, data)
    self.event.Brocast(key_name, data)
end

local function InnerLogout(self)
    self:RemoveAllTimer()
    self:OnLogout()
end

function Manager:Logout()
    for _, mgr in ipairs(managerMap) do
        InnerLogout(mgr)
    end
end

function Manager:OnLogin() end


--跳转场景
function Manager:OnLogout() end