----------------------- auto generate code --------------------------
local base = UIBaseView
---@class BattleUIBase:UIBaseView
local BattleUIBase = BaseClass("BattleUIBase", base)

function BattleUIBase:ctor(container)
	self.btn_Back = self:AddButton(container.btn_Back)
	self.widget_Back = self:AddWidget(container.widget_Back)
end

return BattleUIBase
