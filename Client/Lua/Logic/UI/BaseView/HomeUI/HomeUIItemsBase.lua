----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class HomeUIItemsBase:UIBaseItem
local HomeUIItemsBase = BaseClass("HomeUIItemsBase", base)

function HomeUIItemsBase:ctor(container)
	self.item_Item1 = container.item_Item1
	self.item_Item2 = container.item_Item2
	self.item_Item3 = container.item_Item3
	self.item_Item4 = container.item_Item4
	self.item_Item5 = container.item_Item5
end

return HomeUIItemsBase
