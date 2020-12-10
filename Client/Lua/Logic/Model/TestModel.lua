---------------------------------------------------
--- Model模板
--- Created by shenyi
--- DateTime: 2020.7.15
---------------------------------------------------
---@class TestModel:Model
local  TestModel = BaseClass("TestModel", Model)

function TestModel:ctor()

end

function TestModel:OnLogin()

end

function TestModel:TestCall()
    log("testCallModel")
end

function TestModel:OnLogout()

end

return TestModel