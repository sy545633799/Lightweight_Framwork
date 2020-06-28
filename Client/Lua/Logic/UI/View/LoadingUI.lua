----------------------- auto generate code --------------------------
local base = UIBaseView
---@class LoadingUI:UIBaseView
local view = BaseClass("LoadingUI", base)

function view:OnCreate()
	---@type UISlider
	self.slider_progress = self:AddComponent(UISlider, UIUtil.FindSlider(self.gameObject,  "(Slider)progress"))
end

return view
