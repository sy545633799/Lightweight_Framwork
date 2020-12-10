---------------------------------------------------
--- tips基类
--- Created by shenyi
--- DateTime: 2019.9.26
---------------------------------------------------
local base = UIContain
---@class UIBaseTips:UIContain
UIBaseTips = BaseClass("UIBaseTips", base)
---动画播放完成即加入TipsManager中的对象池
-------------------------------------------------------------------------------
local resourceManager = ResourceManager
local commonUtil = CommonUtil

function UIBaseTips:ctor(container, config)
    self.config = config
end

function UIBaseTips:OnShowTips(...) end
function UIBaseTips:ShowTips(...)
    self:OnShowTips(...)
    self:SetActive(true)
    self:SetAsLastSibling()
end

---@protected
function UIBaseTips:OnHideTips() end
---@public
function UIBaseTips:HideTips()
    self:SetActive(false)
    self:Reset()
    self:OnHideTips()
end


function UIBaseTips:OnDestroy()
    resourceManager.UnloadPrefab(self.gameObject)
end