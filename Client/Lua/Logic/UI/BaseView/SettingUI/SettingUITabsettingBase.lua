----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class SettingUITabsettingBase:UIBaseItem
local SettingUITabsettingBase = BaseClass("SettingUITabsettingBase", base)

function SettingUITabsettingBase:ctor(container)
	self.img_kuang = self:AddImage(container.img_kuang)
	self.img_head = self:AddImage(container.img_head)
	self.text_name = self:AddText(container.text_name)
	self.btn_rename = self:AddButton(container.btn_rename)
	self.text_Id = self:AddText(container.text_Id)
	self.slider_sound = self:AddSlider(container.slider_sound)
	self.Tog_sound = self:AddToggle(container.Tog_sound)
	self.slider_effect = self:AddSlider(container.slider_effect)
	self.Tog_effect = self:AddToggle(container.Tog_effect)
	self.btn_return = self:AddButton(container.btn_return)
end

return SettingUITabsettingBase
