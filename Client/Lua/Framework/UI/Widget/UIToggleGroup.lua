---@class UIToggleGroup:UIContent
UIToggleGroup = BaseClass("UIToggleGroup", UIContent)
-- 创建
function UIToggleGroup:ctor(component)
    self.component = component
end
