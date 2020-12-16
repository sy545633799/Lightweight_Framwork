
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/RegisterUI/RegisterUIBase')
--- @class RegisterUI: RegisterUIBase
local RegisterUI = BaseClass('RegisterUI', base)
-------------------------------------------------------------

function RegisterUI:ctor()
    self.btn_Register:AddClick(self.Regiser, self)
end

function RegisterUI:Regiser()
    RegisterController:Register(self.input_Name:GetText())
end

return RegisterUI