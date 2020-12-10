----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class HomeUIItemBase:UIBaseItem
local HomeUIItemBase = BaseClass("HomeUIItemBase", base)

function HomeUIItemBase:ctor(container)
	self.btn_Item = self:AddButton(container.btn_Item)
	self.img_Icon = self:AddImage(container.img_Icon)
	self.text_Num = self:AddText(container.text_Num)
	self.widget_Num = self:AddWidget(container.widget_Num)
end

return HomeUIItemBase
