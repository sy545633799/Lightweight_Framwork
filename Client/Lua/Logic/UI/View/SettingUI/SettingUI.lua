
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/SettingUI/SettingUIBase')
--- @class SettingUI: SettingUIBase
local SettingUI = BaseClass('SettingUI', base)
local SettingUITabsetting = require('Logic/UI/View/SettingUI/SettingUITabsetting')
local SettingUITabindividual = require('Logic/UI/View/SettingUI/SettingUITabindividual')
-------------------------------------------------------------

local subViewInfo = {
    [1] = {
        Tab = "tab_setting",
        Title = "设置",
    },
    [2] = {
        Tab = "tab_individual",
        Title = "个性",
    },
}

function SettingUI:ctor()
    self.tab_setting = self:AddComponent(SettingUITabsetting, self.item_Tabsetting)
    self.tab_individual = self:AddComponent(SettingUITabindividual, self.item_Tabindividual)
    self:InitTabs(subViewInfo)
end

function SettingUI:OnLoad(...)
    self:SelectTab(1)
end

function SettingUI:OnUnLoad()

end

return SettingUI