----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class HomeUIManorBase:UIBaseItem
local HomeUIManorBase = BaseClass("HomeUIManorBase", base)

function HomeUIManorBase:ctor(container)
	self.btn_guild = self:AddButton(container.btn_guild)
	self.btn_museum = self:AddButton(container.btn_museum)
	self.btn_tower = self:AddButton(container.btn_tower)
	self.btn_tavern = self:AddButton(container.btn_tavern)
end

return HomeUIManorBase
