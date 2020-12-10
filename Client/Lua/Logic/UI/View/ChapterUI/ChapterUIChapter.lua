
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/ChapterUI/ChapterUIChapterBase')
--- @class ChapterUIChapter: ChapterUIChapterBase
local ChapterUIChapter = BaseClass('ChapterUIChapter', base)
-------------------------------------------------------------

function ChapterUIChapter:ctor()
    self.btn_chapter:AddClick(function () SceneManager:SwitchScene(SceneConfig.BattleScene,  self.chapterId) end)
end

function ChapterUIChapter:OnRefresh(data, index)
    self.chapterId = data.id
    self.text_info:SetText(data.config.Name)
end

return ChapterUIChapter