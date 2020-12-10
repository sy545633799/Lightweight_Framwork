----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class HomeUITitleBase:UIBaseItem
local HomeUITitleBase = BaseClass("HomeUITitleBase", base)

function HomeUITitleBase:ctor(container)
	self.text_name = self:AddText(container.text_name)
end

return HomeUITitleBase
