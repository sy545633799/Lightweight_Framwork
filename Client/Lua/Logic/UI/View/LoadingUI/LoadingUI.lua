
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/LoadingUI/LoadingUIBase')
--- @class LoadingUI: LoadingUIBase
local LoadingUI = BaseClass('LoadingUI', base)
-------------------------------------------------------------

function LoadingUI:ctor()
    self:AddEventListener(EventNames.Scene.LoadProcess, function (process)
        self.slider_progress:SetValue(process)
    end)
end

function LoadingUI:OnLoad(...)

end

function LoadingUI:OnUnLoad()

end

return LoadingUI