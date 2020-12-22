----------------------- auto generate code --------------------------
local base = UIBaseView
---@class InputConfirmUIBase:UIBaseView
local InputConfirmUIBase = BaseClass("InputConfirmUIBase", base)

function InputConfirmUIBase:ctor(container)
	self.input_Name = self:AddInput(container.input_Name)
	self.btn_Register = self:AddButton(container.btn_Register)
end

return InputConfirmUIBase
