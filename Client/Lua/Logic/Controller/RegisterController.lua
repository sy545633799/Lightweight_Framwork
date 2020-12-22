------@class RegisterController:Controller
local  RegisterController = BaseClass("RegisterController", Controller)

function RegisterController:ctor()

end

---@public 握手成功
function RegisterController:Register(name)

    --local ret = NetworkManager:SendRequest(NetMsgId.req_register, { nickname = name })
    --
    --if ret.error > 0 then
    --    logError("注册失败")
    --else
    --    Model:Login(ret.roleInfo)
    --    Controller:Login()
    --    SceneManager:SwitchScene(SceneConfig.MainScene)
    --end
end

return RegisterController