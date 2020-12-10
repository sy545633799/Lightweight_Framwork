
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/SettingUI/SettingUITabsettingBase')
--- @class SettingUITabsetting: SettingUITabsettingBase
local SettingUITabsetting = BaseClass('SettingUITabsetting', base)
-------------------------------------------------------------

function SettingUITabsetting:ctor()
    --TODO 发起logout请求
    self.btn_return:AddClick(function () NetworkManager:Close() end)
end

function SettingUITabsetting:OnRefresh(...)
    
end

return SettingUITabsetting