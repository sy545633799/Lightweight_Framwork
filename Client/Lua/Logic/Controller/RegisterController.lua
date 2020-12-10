------@class RegisterController:Controller
local  RegisterController = BaseClass("RegisterController", Controller)

function RegisterController:ctor()

end

---@public 握手成功
function RegisterController:Register(name)
    local roleInfo = NetworkManager:SendRequest("req_register", { nickname = name })
    if roleInfo.error > 0 then
        logError("注册失败")
    else
        Model:Login(roleInfo)
        Controller:Login()
        SceneManager:SwitchScene(SceneConfig.HomeScene)
    end
end

return RegisterController