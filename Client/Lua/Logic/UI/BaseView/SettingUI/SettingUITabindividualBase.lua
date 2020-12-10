----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class SettingUITabindividualBase:UIBaseItem
local SettingUITabindividualBase = BaseClass("SettingUITabindividualBase", base)

function SettingUITabindividualBase:ctor(container)
	self.Tog_head = self:AddToggle(container.Tog_head)
	self.Tog_frame = self:AddToggle(container.Tog_frame)
end

return SettingUITabindividualBase
