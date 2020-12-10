----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class HomeUIPackageBase:UIBaseItem
local HomeUIPackageBase = BaseClass("HomeUIPackageBase", base)

function HomeUIPackageBase:ctor(container)
	self.list_Items = self:AddListView(container.list_Items)
	self.item_Items = container.item_Items
end

return HomeUIPackageBase
