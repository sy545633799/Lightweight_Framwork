---@class UIWidget:UIContent
UIWidget = BaseClass("UIWidget", UIContent)

function UIText:OnCreate(rectTrans)
    self.unity_rectTrans = rectTrans
end