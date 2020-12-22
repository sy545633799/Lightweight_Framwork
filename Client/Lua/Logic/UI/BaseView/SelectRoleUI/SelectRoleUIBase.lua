----------------------- auto generate code --------------------------
local base = UIBaseView
---@class SelectRoleUIBase:UIBaseView
local SelectRoleUIBase = BaseClass("SelectRoleUIBase", base)

function SelectRoleUIBase:ctor(container)
	self.btn_role01 = self:AddButton(container.btn_role01)
	self.text_role01 = self:AddText(container.text_role01)
	self.btn_role02 = self:AddButton(container.btn_role02)
	self.text_role02 = self:AddText(container.text_role02)
	self.btn_role03 = self:AddButton(container.btn_role03)
	self.text_role03 = self:AddText(container.text_role03)
end

return SelectRoleUIBase
