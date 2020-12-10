---@class UISlider:UIContent
UISlider = BaseClass("UISlider", UIContent)
-- 创建
function UISlider:ctor(component)
	---@type UnityEngine.UI.Slider
	self.component = component
end

-- 获取进度
function UISlider:GetValue()
	if not IsNull(self.component) then
		return self.component.normalizedValue
	end
end

---@public 直接绑定C#函数
function UISlider:AddValueCSChangedListener(callback)
	self.component.onValueChanged:AddListener(callback)
end


---@public 不支持协程!!!!!!!!!!
function UISlider:AddValueChangedListener(callback, handle)
	local func = function(bVal)
		if handle then
			callback(handle, bVal)
		else
			callback(bVal)
		end
	end
	self.component.onValueChanged:AddListener(func)
end

-- 设置进度
function UISlider:SetValue(value)
	if not IsNull(self.component) then
		self.component.value = value
	end
end

-- 销毁
function UISlider:dtor()


end