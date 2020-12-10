
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/HomeUI/HomeUITitleBase')
--- @class HomeUITitle: HomeUITitleBase
local HomeUITitle = BaseClass('HomeUITitle', base)
-------------------------------------------------------------

function HomeUITitle:ctor()

end

function HomeUITitle:OnRefresh(id)
    self.text_name:SetTextID(id)
end

return HomeUITitle