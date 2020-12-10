----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class TestTabUITabBBase:UIBaseItem
local TestTabUITabBBase = BaseClass("TestTabUITabBBase", base)

function TestTabUITabBBase:ctor(container)
	self.btn_test01 = self:AddButton(container.btn_test01)
	self.btn_test02 = self:AddButton(container.btn_test02)
	self.btn_test03 = self:AddButton(container.btn_test03)
	self.item_ListBB = container.item_ListBB
end

return TestTabUITabBBase
