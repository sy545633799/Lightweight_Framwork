---------------------------------------------------
--- Created by shenyi
--- DateTime: 2020.7.15
---------------------------------------------------
---@class RoleModel:Model
local RoleModel = BaseClass("RoleModel", Model)

---@class RoleData
local RoleData =
{
    roleId         = 1,
    name           = 1,
    level          = 1,
    exp            = 0,
    vip            = 0,
    totalFight     = 0,
    progress       = 0,
    pVPScore       = 0,
    headIconId     = 1,
    headFrameId    = 1,
    crystal        = 0,
    gold           = 0,
    silver         = 0,
    energy         = 0,
    achive         = 0,
    guide          = 0,
    vipExp         = 0,
    vipGift        = 0,
    mouthCard      = 0,
    emotion        = 0,
    guildId        = 0,
    daySign        = 0,
    sceneId        = 1,
}



function RoleModel:ctor()
    ---@type RoleData
    self.RoleData = PropertyData()
end

function RoleModel:OnLogin(data)
    self.RoleData:UpdateData(data.attrib)
end

function RoleModel:OnLogout()

end

return RoleModel