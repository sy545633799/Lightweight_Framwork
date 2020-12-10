----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class HomeUIEquipBase:UIBaseItem
local HomeUIEquipBase = BaseClass("HomeUIEquipBase", base)

function HomeUIEquipBase:ctor(container)
	self.btn_equip = self:AddButton(container.btn_equip)
	self.img_icon = self:AddImage(container.img_icon)
	self.widget_star = self:AddWidget(container.widget_star)
	self.text_level = self:AddText(container.text_level)
end

return HomeUIEquipBase
