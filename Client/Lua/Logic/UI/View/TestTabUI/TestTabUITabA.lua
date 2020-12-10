
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/TestTabUI/TestTabUITabABase')
--- @class TestTabUITabA: TestTabUITabABase
local TestTabUITabA = BaseClass('TestTabUITabA', base)
local TestTabUIAA = require('Logic/UI/View/TestTabUI/TestTabUIAA')
-------------------------------------------------------------

function TestTabUITabA:ctor()

end

function TestTabUITabA:OnRefresh(...)
    
end

return TestTabUITabA