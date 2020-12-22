---------------------------------------------------
--- Created by shenyi
--- DateTime: 2020.7.15
---------------------------------------------------
---@class RoleModel:Model
local RoleModel = BaseClass("RoleModel", Model)

----@class RoleAttrib
local RoleAttrib =
{
    name           = "",
    job            = 1,
    level          = 1,
    exp            = 0,
    vip            = 0,
    crystal        = 0,
    gold           = 0,
    silver         = 0,
    daySign        = 0,
    headIconId     = 0,
    headFrameId    = 0,
    sceneId        = 0,
    achive         = 0,
    vipExp         = 0,
    vipGift        = 0,
    mouthCard      = 0,
    modelId        = 0,
}

----@class RoleInfo
local RoleInfo =
{
    roleId = "",
    ---@type RoleAttrib
    attrib = {}
}

function RoleModel:ctor()
    ---@type RoleAttrib
    self.RoleAttrib = PropertyData()
end

---@param data RoleInfo
function RoleModel:OnLogin(data)
    self.roleId = data.roleId
    self.RoleAttrib:UpdateData(data.attrib)
end

---@param data table<number, RoleInfo>
function RoleModel:SetRoleAttribList(data)
    if not data then
        ---@type table<number, string>
        self.RoleAttribList = nil
        return
    end
    self.RoleAttribList = {}
    for _, v in pairs(data) do
        self.RoleAttribList[v.attrib.job] = v.roleId
    end
end

function RoleModel:OnLogout()


end

return RoleModel