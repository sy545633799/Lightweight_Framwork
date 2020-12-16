
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/LoginUI/LoginUIBase')
--- @class LoginUI: LoginUIBase
local LoginUI = BaseClass('LoginUI', base)
-------------------------------------------------------------

local networkManager = NetworkManager


function LoginUI:ctor()
    self.btn_login:AddClick(self.DoLogin, self)

end

function LoginUI:DoLogin(...)
    local account = self.input_Account:GetText()
    if string.haschinese(account) then
        logError("包含汉字")
        return
    end

    if string.len(account) > 20 or string.len(account) < 1 then
        logError("name length err!")
        return
    end

    networkManager:Login("127.0.0.1",8890)
end

function LoginUI:OnLoad(...)
    local account = PlayerPrefs.GetString("Account")
    if account and #account > 0 then
        self.input_Account:SetText(account)
    end
    self:AddEventListener(EventNames.Network.NetworkEvent, self.NetworkEvent, self)
end

function LoginUI:NetworkEvent(event)
    if event == NetStateEvent.ConnectSucess then
        local account = self.input_Account:GetText()
        PlayerPrefs.SetString("Account", account)
        LoginController:HandSucess(account)
    end
end


return LoginUI