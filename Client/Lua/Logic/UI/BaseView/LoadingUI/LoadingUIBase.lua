----------------------- auto generate code --------------------------
local base = UIBaseView
---@class LoadingUIBase:UIBaseView
local LoadingUIBase = BaseClass("LoadingUIBase", base)

function LoadingUIBase:ctor(container)
	self.slider_progress = self:AddSlider(container.slider_progress)
end

return LoadingUIBase
