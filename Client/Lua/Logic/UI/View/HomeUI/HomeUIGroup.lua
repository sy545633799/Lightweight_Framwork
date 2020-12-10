
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/HomeUI/HomeUIGroupBase')
--- @class HomeUIGroup: HomeUIGroupBase
local HomeUIGroup = BaseClass('HomeUIGroup', base)
-------------------------------------------------------------

function HomeUIGroup:ctor()
    self.btn_group:AddClick(function () logError("暂未开放") end)
end

function HomeUIGroup:OnRefresh(index)
    if index == 1 then
        self.text_name:SetTextID(1004020)
    elseif index == 2 then
        self.text_name:SetTextID(1005006)
    end
end

return HomeUIGroup