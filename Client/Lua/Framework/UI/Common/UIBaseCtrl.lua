---------------------------------------------------
--- uictrl基类
--- Created by shenyi
--- DateTime: 2019.6.5
---------------------------------------------------
---@class UIBaseCtrl:Updatable
UIBaseCtrl = BaseClass("UIBaseCtrl", Updatable)

function UIBaseCtrl:OnCreate(...) end
function UIBaseCtrl:ctor(view)
    self:OnCreate(view)
end

function UIBaseCtrl:OnLoad(...) end
function UIBaseCtrl:Load(...)
    self:OnLoad(...)
    self.view:SetActive(true)
    self.view:PlayUIAnimation("Open")
end

function UIBaseCtrl:OnUnLoad() end

function UIBaseCtrl:UnLoad()
    self:RemoveAllTimer()
    self:RemoveAllTweener()
    self:RemoveAllEventListener()
    self.view:SetActive(false)
    self:OnUnLoad()
end

function UIBaseCtrl:OnDestroy() end
function UIBaseCtrl:dtor()
    --self:UnLoad()
    self.view:Delete()
    self.view = nil
    self:OnDestroy()
end