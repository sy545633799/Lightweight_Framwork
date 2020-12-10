----------------------- auto generate code --------------------------
local base = UIBaseView
---@class LoginUIBase:UIBaseView
local LoginUIBase = BaseClass("LoginUIBase", base)

function LoginUIBase:ctor(container)
	self.btn_login = self:AddButton(container.btn_login)
	self.text_login = self:AddText(container.text_login)
	self.input_Account = self:AddInput(container.input_Account)
end

return LoginUIBase
