----------------------- auto generate code --------------------------
local base = UIBaseView
---@class RegisterUIBase:UIBaseView
local RegisterUIBase = BaseClass("RegisterUIBase", base)

function RegisterUIBase:ctor(container)
	self.widget_Root = self:AddWidget(container.widget_Root)
	self.input_Name = self:AddInput(container.input_Name)
	self.btn_Register = self:AddButton(container.btn_Register)
end

return RegisterUIBase
