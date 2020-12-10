---------------------------------------------------
--- Created by shenyi
--- DateTime: 2020.7.15
---------------------------------------------------
---@class ChapterModel:Model
local ChapterModel = BaseClass("ChapterModel", Model)

function ChapterModel:ctor()
    local chapters = GetChapterConfig()
    self.Chapters = {}
    for id, config in pairs(chapters) do
        local chapterId =  (math.floor(id / 1000)) % 1000
        if not self.Chapters[chapterId] then
            self.Chapters[chapterId] = {}
        end
        local index = id % 1000
        self.Chapters[chapterId][index] = { id = id, config = config}
    end
end

function ChapterModel:OnLogin(data)

end

function ChapterModel:OnLogout()

end

return ChapterModel