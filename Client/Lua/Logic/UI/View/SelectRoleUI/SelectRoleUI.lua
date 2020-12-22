
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/SelectRoleUI/SelectRoleUIBase')
--- @class SelectRoleUI: SelectRoleUIBase
local SelectRoleUI = BaseClass('SelectRoleUI', base)
-------------------------------------------------------------
local job_config = require "Logic/Config/Job"

function SelectRoleUI:ctor()
    for i = 1, 3 do
        self["btn_role0" .. i]:AddClick(function () self:OnSelect(i) end)
    end
end

function SelectRoleUI:OnSelect(k)
    local job_list = RoleModel.RoleAttribList

    if job_list and job_list[k] then
        local roleId = job_list[k]
        LoginController:Req_Login(roleId)
    else
        UIManager:LoadView(UIConfig.InputConfirmUI, function(nick_name)
            LoginController:Req_Create_Role(k, nick_name)
        end)
    end
end

return SelectRoleUI