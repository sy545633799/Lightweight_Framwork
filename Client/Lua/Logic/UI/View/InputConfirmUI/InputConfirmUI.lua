-------------------------------------------------------------
local base = require('Logic/UI/BaseView/InputConfirmUI/InputConfirmUIBase')
--- @class InputConfirmUI: InputConfirmUIBase
local InputConfirmUI = BaseClass('InputConfirmUI', base)
-------------------------------------------------------------

function InputConfirmUI:ctor()
    self.btn_Register:AddClick(self.OnRegister, self)
end

function InputConfirmUI:OnLoad(callback, handle)
    assert(callback and type(callback) == "function")
    self.callback = callback
    self.handle = handle
end

function InputConfirmUI:OnRegister()
    if self.handle then
        self.callback(self.handle, self.input_Name:GetText())
    else
        self.callback(self.input_Name:GetText())
    end
end

return InputConfirmUI