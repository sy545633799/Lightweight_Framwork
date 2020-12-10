----------------------- auto generate code --------------------------
local base = UITabView
---@class SettingUIBase:UITabView
local SettingUIBase = BaseClass("SettingUIBase", base)

function SettingUIBase:ctor(container)
	self.btn_close = self:AddButton(container.btn_close)
	self.canvasGroup_tab02 = self:AddCanvasGroup(container.canvasGroup_tab02)
	self.btn_unselect02 = self:AddButton(container.btn_unselect02)
	self.img_unselect02 = self:AddImage(container.img_unselect02)
	self.text_unselect02 = self:AddText(container.text_unselect02)
	self.canvasGroup_select02 = self:AddCanvasGroup(container.canvasGroup_select02)
	self.img_select02 = self:AddImage(container.img_select02)
	self.text_select02 = self:AddText(container.text_select02)
	self.canvasGroup_tab01 = self:AddCanvasGroup(container.canvasGroup_tab01)
	self.btn_unselect01 = self:AddButton(container.btn_unselect01)
	self.img_unselect01 = self:AddImage(container.img_unselect01)
	self.text_unselect01 = self:AddText(container.text_unselect01)
	self.canvasGroup_select01 = self:AddCanvasGroup(container.canvasGroup_select01)
	self.img_select01 = self:AddImage(container.img_select01)
	self.text_select01 = self:AddText(container.text_select01)
	self.item_Tabsetting = container.item_Tabsetting
	self.item_Tabindividual = container.item_Tabindividual
	self.text_title = self:AddText(container.text_title)
end

return SettingUIBase
