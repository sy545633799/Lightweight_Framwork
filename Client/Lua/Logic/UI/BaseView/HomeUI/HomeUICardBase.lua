----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class HomeUICardBase:UIBaseItem
local HomeUICardBase = BaseClass("HomeUICardBase", base)

function HomeUICardBase:ctor(container)
	self.btn_card = self:AddButton(container.btn_card)
	self.img_icon = self:AddImage(container.img_icon)
	self.widget_star = self:AddWidget(container.widget_star)
	self.text_level = self:AddText(container.text_level)
end

return HomeUICardBase
