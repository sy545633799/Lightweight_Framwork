---@class UIToggle:UIContent
UIToggle = BaseClass("UIToggle", UIContent)
-- 创建
function UIToggle:OnCreate(uitoggle)
	self.unity_uitoggle = uitoggle
end


function UIToggle:SetValue(bVal)
	assert(type(bVal) == "boolean")
	self.unity_uitoggle.isOn = bVal
end

function UIToggle:GetValueValue()
	return self.unity_uitoggle.isOn
end

function UIToggle:SetToggleGroup(group)
	self.unity_uitoggle.group = group.unity_uitoggleGroup
end

function UIToggle:ClearToggleGroup()
	self.unity_uitoggle.group = nil
end

function UIToggle:GetTouchEnabled()
	return self.unity_uitoggle.interactable
end

function UIToggle:SetTouchEnabled(bVal)
	assert(type(bVal) == "boolean")
	self.ui_component.unity_uitoggle = bVal
end

function UIToggle:SetCheckOldValue(bVal)
	assert(type(bVal) == "boolean")
	self.__checkOldValue = bVal
end

function UIToggle:SetOnValueChange(callback, bCover)
	if bCover then
		self.__callback = {}
		self.unity_uitoggle.onValueChanged:RemoveAllListeners()
	end
	local func = function (status)
		if not self.__checkOldValue or not self.__oldValue == status then
			callback(status)
			self.__oldValue = status
		end
	end
	table.insert(self.__callback, func)
	self.unity_uitoggle.onValueChanged:AddListener(func)
end

-- 资源释放
function UIToggle:OnDestroy()
	self.unity_uitoggle.onValueChanged:RemoveAllListeners()
end