
-------------------------------------------------------------
local base = UIBaseCtrl
---@type UIBaseCtrl
local LoadingUICtrl = BaseClass('LoadingUICtrl', base)
-------------------------------------------------------------

function LoadingUICtrl: OnCreate(view)
    ---@type LoadingUI
    self.view = view
    self:AddEventListener(EventNames.Scene.LoadProcess, function (process)
        self.view.slider_progress:SetValue(process)
    end)
end

function LoadingUICtrl: OnLoad(pram)
    
end


function LoadingUICtrl: OnUnLoad()

end

function LoadingUICtrl: OnDestroy()

end

return LoadingUICtrl