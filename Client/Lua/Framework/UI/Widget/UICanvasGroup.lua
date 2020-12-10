---@class UICanvasGroup:UIContent
UICanvasGroup = BaseClass("UICanvasGroup", UIContent)
-- 创建
function UICanvasGroup:ctor(component)
    ---@type UnityEngine.CanvasGroup
    self.component = component
end

function UICanvasGroup:DoFade(value, duration, ease, callback, handle)
    if not IsNull(self.component) then
        local tweener = self.component:DOFade(value, duration)
        self:AddTweener(tweener, ease, callback, handle)
    end
end

function UICanvasGroup:SetActive(bVal)
    if bVal then
        self.component.alpha = 1
        self.component.blocksRaycasts = true
    else
        self.component.alpha = 0
        self.component.blocksRaycasts = false
    end
end


function UICanvasGroup:SetAlpha(val)
    self.component.alpha = val
end
