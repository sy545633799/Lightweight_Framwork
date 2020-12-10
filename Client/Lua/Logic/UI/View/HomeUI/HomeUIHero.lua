
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/HomeUI/HomeUIHeroBase')
--- @class HomeUIHero: HomeUIHeroBase
local HomeUIHero = BaseClass('HomeUIHero', base)
-------------------------------------------------------------
local HomeUICard = require('Logic/UI/View/HomeUI/HomeUICard')

function HomeUIHero:ctor()
    self.listItems = {}
    self.list_cards:InitListView(0, self.InitListHeros, self)
end

function HomeUIHero:OnRefresh(...)
    self.list_cards:SetListItemCount(20, true)
    self.list_cards:RefreshAllShownItem()
end

function HomeUIHero:InitListHeros(listView, index)
    if index < 1 or index >self.list_cards.TotalItemCount then
        return nil
    end
    local item = listView:NewListViewItem("(Item)Card")
    --local chapterItem = self.listItem[item]
    --if not chapterItem then
    --    chapterItem = self:AddComponent(ChapterUIChapter, item:GetUIView())
    --    self.listItem[item] = chapterItem
    --end
    --
    --chapterItem:Refresh(self.chapters[index], index)
    return item
end

return HomeUIHero