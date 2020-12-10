----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class HomeUIGroupBase:UIBaseItem
local HomeUIGroupBase = BaseClass("HomeUIGroupBase", base)

function HomeUIGroupBase:ctor(container)
	self.btn_group = self:AddButton(container.btn_group)
	self.text_name = self:AddText(container.text_name)
end

return HomeUIGroupBase
