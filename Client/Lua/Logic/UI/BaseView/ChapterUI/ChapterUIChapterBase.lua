----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class ChapterUIChapterBase:UIBaseItem
local ChapterUIChapterBase = BaseClass("ChapterUIChapterBase", base)

function ChapterUIChapterBase:ctor(container)
	self.btn_chapter = self:AddButton(container.btn_chapter)
	self.text_info = self:AddText(container.text_info)
end

return ChapterUIChapterBase
