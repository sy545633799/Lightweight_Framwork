
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/HomeUI/HomeUISocietyBase')
--- @class HomeUISociety: HomeUISocietyBase
local HomeUISociety = BaseClass('HomeUISociety', base)
-------------------------------------------------------------
local HomeUIContent = require('Logic/UI/View/HomeUI/HomeUIContent')
local HomeUIGroup = require('Logic/UI/View/HomeUI/HomeUIGroup')
local HomeUITitle = require('Logic/UI/View/HomeUI/HomeUITitle')

function HomeUISociety:ctor()
    self.listItems = {}
    self.list_society:InitListView(0, self.InitListItems, self)
end

function HomeUISociety:OnRefresh(...)
    self.list_society:SetListItemCount(20, true)
    self.list_society:RefreshAllShownItem()
end

function HomeUISociety:InitListItems(listView, index)
    if index < 1 or index >self.list_society.TotalItemCount then
        return nil
    end
    local item
    if index == 1 then
        item = listView:NewListViewItem("(Item)Title")
    elseif index <= 3 then
        item = listView:NewListViewItem("(Item)Group")
    elseif index <= 4 then
        item = listView:NewListViewItem("(Item)Title")
    else
        item = listView:NewListViewItem("(Item)Content")
    end

    local luaItem = self.listItems[item]
    if not luaItem then
        if index == 1 then
            luaItem = self:AddComponent(HomeUITitle, item:GetUIView())
        elseif index <= 3 then
            luaItem = self:AddComponent(HomeUIGroup, item:GetUIView())
        elseif index <= 4 then
            luaItem = self:AddComponent(HomeUITitle, item:GetUIView())
        else
            luaItem = self:AddComponent(HomeUIContent, item:GetUIView())
        end
        self.listItems[item] = luaItem
    end

    if index == 1 then
        luaItem:Refresh(1004019)
    elseif index <= 3 then
        luaItem:Refresh(index - 1)
    elseif index <= 4 then
        luaItem:Refresh(1004018)
    else
        luaItem:Refresh(index - 4)
    end
    return item
end

return HomeUISociety