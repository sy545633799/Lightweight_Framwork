
-------------------------------------------------------------
local base = UIBaseCtrl
---@type UIBaseCtrl
local LoginUICtrl = BaseClass('LoginUICtrl', base)
-------------------------------------------------------------

function LoginUICtrl: OnCreate(view)
    ---@type LoginUI
    self.view = view
    self.view.btn_login:AddClick(function ()
        NetworkManager:Login("127.0.0.1", 8003, 8890, "DevelopServer", 123)
    end)
end

function LoginUICtrl: OnLoad(...)

end

function LoginUICtrl: OnUnLoad()

end

function LoginUICtrl: OnDestroy()

end

return LoginUICtrl