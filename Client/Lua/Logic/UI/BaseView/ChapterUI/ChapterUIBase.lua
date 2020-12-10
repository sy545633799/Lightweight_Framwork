----------------------- auto generate code --------------------------
local base = UIBaseView
---@class ChapterUIBase:UIBaseView
local ChapterUIBase = BaseClass("ChapterUIBase", base)

function ChapterUIBase:ctor(container)
	self.btn_close = self:AddButton(container.btn_close)
	self.text_title = self:AddText(container.text_title)
	self.list_chapter = self:AddListView(container.list_chapter)
	self.item_Chapter = container.item_Chapter
end

return ChapterUIBase
