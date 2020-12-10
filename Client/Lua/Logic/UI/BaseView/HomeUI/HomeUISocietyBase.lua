----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class HomeUISocietyBase:UIBaseItem
local HomeUISocietyBase = BaseClass("HomeUISocietyBase", base)

function HomeUISocietyBase:ctor(container)
	self.list_society = self:AddListView(container.list_society)
	self.item_Title = container.item_Title
	self.item_Group = container.item_Group
	self.item_Content = container.item_Content
end

return HomeUISocietyBase
