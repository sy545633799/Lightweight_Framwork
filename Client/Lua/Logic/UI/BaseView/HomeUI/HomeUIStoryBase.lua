----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class HomeUIStoryBase:UIBaseItem
local HomeUIStoryBase = BaseClass("HomeUIStoryBase", base)

function HomeUIStoryBase:ctor(container)
	self.btn_friend = self:AddButton(container.btn_friend)
	self.btn_mail = self:AddButton(container.btn_mail)
	self.btn_boss = self:AddButton(container.btn_boss)
	self.btn_task = self:AddButton(container.btn_task)
	self.btn_activity = self:AddButton(container.btn_activity)
end

return HomeUIStoryBase
