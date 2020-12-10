---@class UIInput:UIContent
UIInput = BaseClass("UIInput", UIContent)

function UIInput:ctor(component)
    self.component = component
end

function UIInput:GetText()
    return self.component.text
end

function UIInput:SetText(text)
    self.component.text = text
end