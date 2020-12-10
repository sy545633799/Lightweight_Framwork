----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class TestTabUITabABase:UIBaseItem
local TestTabUITabABase = BaseClass("TestTabUITabABase", base)

function TestTabUITabABase:ctor(container)
	self.btn_test01 = self:AddButton(container.btn_test01)
	self.btn_test02 = self:AddButton(container.btn_test02)
	self.btn_test03 = self:AddButton(container.btn_test03)
	self.item_AA = container.item_AA
end

return TestTabUITabABase
