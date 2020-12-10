----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class TestTabUIAABase:UIBaseItem
local TestTabUIAABase = BaseClass("TestTabUIAABase", base)

function TestTabUIAABase:ctor(container)
	self.btn_test01 = self:AddButton(container.btn_test01)
	self.btn_test02 = self:AddButton(container.btn_test02)
	self.btn_test03 = self:AddButton(container.btn_test03)
end

return TestTabUIAABase
