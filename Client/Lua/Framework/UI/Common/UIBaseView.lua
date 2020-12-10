---------------------------------------------------
--- view基类
--- Created by shenyi
--- DateTime: 2019.6.5
---------------------------------------------------
local base = UIContain
---@class UIBaseView:UIContain
UIBaseView = BaseClass("UIBaseView", base)
local resourceManager = ResourceManager

function UIBaseView:ctor(container, config)
    self.config = config
end

---@public 拉回到Canvas让相机渲染(只能让UIManager使用)
function UIBaseView:Show(bVal)
    if bVal then
        self:SetAnchoredPosition(0,0)
        self:SetAsLastSibling()
        self:SetAsSceneSize()
    else
        self:SetAnchoredPosition(0,5000)
    end
end

function UIBaseView:OnLoad(...) end
---@public(只能让UIManager使用)
function UIBaseView:Load(...)
    self:OnLoad(...)
    self:SetActive(true)
    self:Show(true)
    self:PlayUIAnimation("Open")
end

function UIBaseView:OnUnLoad() end
---@public(只能让UIManager使用)
function UIBaseView:UnLoad()
    self:Reset()
    self:SetActive(false)
    self:OnUnLoad()
end

---@protected 自身关闭
function UIBaseView:Close()
    UIManager:UnLoadView(self.config)
end

function UIBaseView:OnDestroy()
    resourceManager.UnloadPrefab(self.component)
end