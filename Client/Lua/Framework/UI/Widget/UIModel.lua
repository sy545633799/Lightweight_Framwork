---@class UIModel:UIContent
UIModel = BaseClass("UIModel", UIContent)
local modelManager = ModelManager
local commonUtil = CommonUtil

--function UIModel:ctor() end

function UIModel:SetModel(model_id, callback)
    --if model_id == self.model_id then return end
    --self.model_id = model_id
    --modelManager.SetModel(self.gameObject, model_id, function (go)
    --    coroutine.start(function ()
    --        if self.model then
    --            modelManager.ReleaseModel(self.model)
    --            commonUtil.SetLayerRecursively(self.model, "Default")
    --        end
    --        commonUtil.SetLayerRecursively(go, "UI")
    --        self.model = go
    --        if callback then callback(go) end
    --    end)
    --end)
end

function UIModel:ReleaseModel()
    --if self.model then
    --    modelManager.ReleaseModel(self.model)
    --    commonUtil.SetLayerRecursively(self.model, "Default")
    --    self.model = nil
    --    self.model_id = nil
    --end
end

function UIModel:dtor()
    self:ReleaseModel()
end