----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class HomeUISkillBase:UIBaseItem
local HomeUISkillBase = BaseClass("HomeUISkillBase", base)

function HomeUISkillBase:ctor(container)
	self.text_level = self:AddText(container.text_level)
end

return HomeUISkillBase
