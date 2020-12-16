------@class LoginController:Controller
local  LoginController = BaseClass("LoginController", Controller)

function LoginController:ctor()

end

---@public 握手成功
function LoginController:HandSucess()
    local roleInfo = NetworkManager:SendRequest(NetMsgId.req_login)
    if not roleInfo or not next(roleInfo) then
        UIManager:LoadView(UIConfig.RegisterUI)
    else
        Model:Login(roleInfo)
        Controller:Login()
        SceneManager:SwitchScene(SceneConfig.MainScene)
    end
end

function LoginController:Disconnect()
    logError("Disconnect")
    Model:Logout()
    Controller:Logout()
    SceneManager:SwitchScene(SceneConfig.LoginScene)
end



return LoginController