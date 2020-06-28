---@class UISlider:UIContent
UISlider = BaseClass("UISlider", UIContent)
-- 创建
function UISlider:OnCreate(slider)
	---@type UnityEngine.UI.Slider
	self.unity_uislider = slider
end

-- 获取进度
function UISlider:GetValue()
	if not IsNull(self.unity_uislider) then
		return self.unity_uislider.normalizedValue
	end
end

-- 设置进度
function UISlider:SetValue(value)
	if not IsNull(self.unity_uislider) then
		self.unity_uislider.normalizedValue = value
	end
end

-- 销毁
function UISlider:OnDestroy()


end