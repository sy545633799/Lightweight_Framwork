----------------------- auto generate code --------------------------
local base = UIBaseView
---@class LoginUI:UIBaseView
local view = BaseClass("LoginUI", base)

function view:OnCreate()
	---@type UIButton
	self.btn_login = self:AddComponent(UIButton, UIUtil.FindButton(self.gameObject,  "(Button)(Widget)login"))
	---@type UIWidget
	self.widget_login = self:AddComponent(UIWidget, UIUtil.FindRectTransform(self.gameObject,  "(Button)(Widget)login"))
end

return view
