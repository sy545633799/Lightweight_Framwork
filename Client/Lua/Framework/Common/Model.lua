--- Model基础类 TODO
--- Created by shenyi
--- DateTime: 2019.6.8

---@class Model
Model = BaseClass("Model")

local modelMap = {}

function Model:ctor()
    table.insert(modelMap, self)
    self.Login = false
    self.Logout = false
end

function Model:Login(data)
    for _, mgr in ipairs(modelMap) do
        mgr:OnLogin(data)
    end
end

function Model:Logout()
    for _, mgr in ipairs(modelMap) do
        mgr:OnLogout()
    end
end

function Model:OnLogin() end

function Model:OnLogout() end
