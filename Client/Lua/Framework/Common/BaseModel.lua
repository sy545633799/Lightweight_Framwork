--- Model基础类 TODO
--- Created by shenyi
--- DateTime: 2019.6.8

---@class BaseModel
BaseModel = BaseClass("BaseModel")

local modelMap = {}

function BaseModel:OnCreate() end

function BaseModel:ctor()
    table.insert(modelMap, self)
    self:OnCreate()
end

function BaseModel:Login(data)
    for _, mgr in ipairs(modelMap) do
        mgr:OnLogin(data)
    end
end

function BaseModel:Logout()
    for _, mgr in ipairs(modelMap) do
        mgr:OnLogout()
    end
end

function BaseModel:OnLogin() end

function BaseModel:OnLogout() end

--暂时没有删除机制， 勿用
function BaseModel:OnDestroy() end
function BaseModel:dtor()
    self:OnDestroy()
end