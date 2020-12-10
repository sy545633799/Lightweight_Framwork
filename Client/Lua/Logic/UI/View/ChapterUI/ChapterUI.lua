
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/ChapterUI/ChapterUIBase')
--- @class ChapterUI: ChapterUIBase
local ChapterUI = BaseClass('ChapterUI', base)
local ChapterUIChapter = require('Logic/UI/View/ChapterUI/ChapterUIChapter')
-------------------------------------------------------------

function ChapterUI:ctor()
    self.btn_close:AddClick(function () self:Close() end)
    self.list_chapter:InitListView(0, self.InitListChapter, self)
    self.listItem = {}
end

function ChapterUI:OnLoad(index)
    self.chapters = ChapterModel.Chapters[index]
    self.list_chapter:SetListItemCount(#self.chapters, true)
    self.list_chapter:RefreshAllShownItem()
end

function ChapterUI:InitListChapter(listView, index)
    if index < 1 or index >self.list_chapter.TotalItemCount then
        return nil
    end
    local item = listView:NewListViewItem("(Item)Chapter")
    local chapterItem = self.listItem[item]
    if not chapterItem then
        chapterItem = self:AddComponent(ChapterUIChapter, item:GetUIView())
        self.listItem[item] = chapterItem
    end

    chapterItem:Refresh(self.chapters[index], index)
    return item
end

function ChapterUI:OnUnLoad()
    self.chapters = nil
end

return ChapterUI