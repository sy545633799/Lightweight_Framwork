
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/BattleUI/BattleUIBase')
--- @class BattleUI: BattleUIBase
local BattleUI = BaseClass('BattleUI', base)
-------------------------------------------------------------
function BattleUI:ctor()
    self.btn_Back:AddClick(function()
        SceneManager:SwitchScene(SceneConfig.HomeScene)
    end)
end

function BattleUI:OnLoad(...)

end

function BattleUI:OnUnLoad()

end

return BattleUI