---------------------------------------------------
--- view基类
--- Created by shenyi
--- DateTime: 2019.7.15
---------------------------------------------------
local base = UIContain
---@class UIBaseItem:UIContain
UIBaseItem = BaseClass("UIBaseItem", base)

---@protected
function UIBaseItem:OnRefresh(...) end
---@public
function UIContent:Refresh(...)
    self:Dispose()
    self:SetActive(true)
    self:OnRefresh(...)
end

---@protected
function UIBaseItem:OnDispose() end
---@public
function UIContent:Dispose()
    self:Reset()
    self:OnDispose()
end