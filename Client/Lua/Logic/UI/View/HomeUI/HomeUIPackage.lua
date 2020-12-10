
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/HomeUI/HomeUIPackageBase')
--- @class HomeUIPackage: HomeUIPackageBase
local HomeUIPackage = BaseClass('HomeUIPackage', base)
local HomeUIItems = require('Logic/UI/View/HomeUI/HomeUIItems')
-------------------------------------------------------------

function HomeUIPackage:ctor()
    self.list_Items:InitListView(0, self.InitListItems, self)
end

function HomeUIPackage:OnRefresh(...)
    self.list_Items:SetListItemCount(20, true)
    self.list_Items:RefreshAllShownItem()


end

function HomeUIPackage:InitListItems(listView, index)
    if index < 1 or index >self.list_Items.TotalItemCount then
        return nil
    end
    local item = listView:NewListViewItem("(Item)Items")
    --local chapterItem = self.listItem[item]
    --if not chapterItem then
    --    chapterItem = self:AddComponent(ChapterUIChapter, item:GetUIView())
    --    self.listItem[item] = chapterItem
    --end
    --
    --chapterItem:Refresh(self.chapters[index], index)
    return item
end

return HomeUIPackage