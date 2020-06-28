---------------------------------------------------
--- view基类
--- Created by shenyi
--- DateTime: 2019.6.5
---------------------------------------------------
local base = UIContent
---@class UIBaseView:UIContent
UIBaseView = BaseClass("UIBaseView", base)
local resourceManager = ResourceManager
local commonUtil = CommonUtil
local uiUtil = UIUtil

function UIBaseView:SetActive(bVal)
    base.SetActive(self, bVal)
    if bVal then
        commonUtil.SetAsLastSibling(self.gameObject)
        uiUtil.SetAsSceneSize(self.gameObject)
    end
end

function UIBaseView:PlayUIAnimation(name,reverse,speed,during)
    if self.Animation and type(name) == 'string' then
        local rate = 1
        if speed and type(speed) == 'number' then
            rate = speed
        end
        local time = 0
        if during and type(during) == 'number' then
            time = during
        end
        --UIUtil.PlayAnimation(self.Animation,name,rate,time,reverse)
    end
end

function UIBaseView:OnDestroy()
    resourceManager.UnloadAssets(self.gameObject)
end