----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class HomeUIContentBase:UIBaseItem
local HomeUIContentBase = BaseClass("HomeUIContentBase", base)

function HomeUIContentBase:ctor(container)
	self.btn_content = self:AddButton(container.btn_content)
	self.img_kuang = self:AddImage(container.img_kuang)
	self.img_head = self:AddImage(container.img_head)
	self.text_name = self:AddText(container.text_name)
end

return HomeUIContentBase
