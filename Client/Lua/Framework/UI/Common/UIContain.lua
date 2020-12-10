---------------------------------------------------
--- view基类
--- Created by shenyi
--- DateTime: 2019.7.15
---------------------------------------------------
local base = UIWidget
---@class UIContain:UIWidget
UIContain = BaseClass("UIContain", base)

function UIContain:ctor(container)
    ---@type UnityEngine.RectTransform
    self.component = container.transform
    ---@type Game.UIConTainer
    self.container = container.container
end

function UIContain:DoFade(value, duration, ease, callback, handle)
    self:RemoveAllTweener()
    local tweener = self.container:DOFade(value, duration)
    self:AddTweener(tweener, ease, callback, handle)
end

function UIContain:SetActive(bVal)
    ---TODO
    self.container:SetActive(bVal)
end

function UIContain:SetAlpha(val)
    self.container:SetAlpha(val)
end
