---@class UIWidget:UIContent
UIWidget = BaseClass("UIWidget", UIContent)
local uiUtil = UIUtil

function UIWidget:ctor(component)
    ---@type UnityEngine.RectTransform
    self.component = component
end

function UIWidget:SetAsSceneSize()
    self.component:SetAsSceneSize()
end

function UIWidget:SetRectSize(x, y)
    self.component:SetRectSize(x, y)
end

function UIWidget:SetRectAnchor(x1, y1, x2, y2)
    self.component:SetRectAnchor(x1, y1, x2, y2)
end

function UIWidget:SetRectPivot(x, y)
    self.component:SetRectPivot(x, y)
end

function UIWidget:SetRectAttachment(string)
    self.component:SetRectAttachment(string)
end

function UIWidget:SetAnchoredPosition(x, y)
    self.component:SetAnchoredPosition(x, y)
end

function UIWidget:SetAsFirstSibling()
    self.component:SetAsFirstSibling()
end

function UIWidget:SetAsLastSibling()
    self.component:SetAsLastSibling()
end

function UIWidget:SetSiblingIndex(index)
    self.component:SetSiblingIndex(index)
end

function UIWidget:DOAnchorMax(x, y, duration, ease, callback, handle)
    local tweener = self.component:DOAnchorMax({x, y}, duration)
    self:AddTweener(tweener, ease, callback, handle)
    return tweener
end

function UIWidget:DOAnchorMin(x, y, duration, ease, callback, handle)
    local tweener = self.component:DOAnchorMin({x, y}, duration)
    self:AddTweener(tweener, ease, callback, handle)
    return tweener
end

function UIWidget:DOAnchorPos(x, y, duration, ease, callback, handle)
    local tweener = self.component:DOAnchorPos({x, y}, duration)
    self:AddTweener(tweener, ease, callback, handle)
    return tweener
end

function UIWidget:DOAnchorPosX(x, duration, ease, callback, handle)
    local tweener = self.component:DOAnchorPosX(x, duration)
    self:AddTweener(tweener, ease, callback, handle)
    return tweener
end

function UIWidget:DOAnchorPosY(y, duration, ease, callback, handle)
    local tweener = self.component:DOAnchorPosY(y, duration)
    self:AddTweener(tweener, ease, callback, handle)
    return tweener
end

function UIWidget:DOLocalRotate(x, y, z, duration, ease, callback, handle)
    local tweener = self.component:DOLocalRotate({x, y, z}, duration)
    self:AddTweener(tweener, ease, callback, handle)
    return tweener
end

function UIWidget:DOScale(x, y, z, duration, ease, callback, handle)
    local tweener = self.component:DOScale({x, y, z}, duration)
    self:AddTweener(tweener, ease, callback, handle)
    return tweener
end

function UIWidget:DOScaleX(x, duration, ease, callback, handle)
    local tweener = self.component:DOScaleX(x, duration)
    self:AddTweener(tweener, ease, callback, handle)
    return tweener
end

function UIWidget:DOScaleY(y, duration, ease, callback, handle)
    local tweener = self.component:DOScaleY(y, duration)
    self:AddTweener(tweener, ease, callback, handle)
    return tweener
end

function UIWidget:DOScaleZ(z, duration, ease, callback, handle)
    local tweener = self.component:DOScaleZ(z, duration)
    self:AddTweener(tweener, ease, callback, handle)
    return tweener
end









