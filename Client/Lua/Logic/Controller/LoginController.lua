------@class LoginController:Controller
local  LoginController = BaseClass("LoginController", Controller)

function LoginController:ctor()

end

---@public 握手成功, 请求玩家列表
function LoginController:HandSucess(account)
    local ret = NetworkManager:SendRequest(NetMsgId.req_role_list, { account = account })
    RoleModel:SetRoleAttribList(ret.list)
    UIManager:LoadView(UIConfig.SelectRoleUI)
end

---@public 根据职业创建角色, 直接返回角色信息
function LoginController:Req_Create_Role(job, nickname)
    local ret = NetworkManager:SendRequest(NetMsgId.req_create_role, { job = job, nickname = nickname })
    if ret and next(ret) then
        Model:Login(ret.roleInfo)
        Controller:Login()
        SceneManager:SwitchScene(SceneConfig.MainScene)
    else
        logError("创角失败")
    end
end

---@public 请求登录角色, 根据角色id返回角色信息
function LoginController:Req_Login(roleId)
    local ret = NetworkManager:SendRequest(NetMsgId.req_login, { roleId = roleId })
    if ret and next(ret) then
        Model:Login(ret.roleInfo)
        Controller:Login()
        SceneManager:SwitchScene(SceneConfig.MainScene)
    else
        logError("登录失败")
    end
end

function LoginController:Disconnect()
    Model:Logout()
    Controller:Logout()
    SceneManager:SwitchScene(SceneConfig.LoginScene)
end



return LoginController