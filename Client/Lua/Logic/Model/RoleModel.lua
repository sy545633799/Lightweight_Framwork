---------------------------------------------------
--- Created by shenyi
--- DateTime: 2020.7.15
---------------------------------------------------
---@class RoleModel:Model
local RoleModel = BaseClass("RoleModel", Model)

function RoleModel:ctor()
    self.RoleData = PropertyData()
    
end

function RoleModel:OnLogin(data)
    self.RoleData:UpdateData(data.roleInfo.attrib)
end

function RoleModel:OnLogout()

end

return RoleModel