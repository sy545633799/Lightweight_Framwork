
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/TestTabUI/TestTabUIBase')
--- @class TestTabUI: TestTabUIBase
local TestTabUI = BaseClass('TestTabUI', base)
local TestTabUITabA = require('Logic/UI/View/TestTabUI/TestTabUITabA')
local TestTabUITabB = require('Logic/UI/View/TestTabUI/TestTabUITabB')
-------------------------------------------------------------

function TestTabUI:ctor()

end

function TestTabUI:OnLoad(...)

end

function TestTabUI:OnUnLoad()

end

return TestTabUI