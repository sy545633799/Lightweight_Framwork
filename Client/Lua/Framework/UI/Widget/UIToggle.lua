---@class UIToggle:UIContent
UIToggle = BaseClass("UIToggle", UIContent)
-- 创建
function UIToggle:ctor(component)
	self.component = component
	self.__callback = {}
	self.__oldValue = false
end

function UIToggle:SetValue(bVal)
	assert(type(bVal) == "boolean")
	self.component.isOn = bVal
end

function UIToggle:GetValue()
	return self.component.isOn
end

function UIToggle:SetToggleGroup(group)
	self.component.group = group.componentGroup
end

function UIToggle:ClearToggleGroup()
	self.component.group = nil
end

function UIToggle:GetTouchEnabled()
	return self.component.interactable
end

function UIToggle:SetTouchEnabled(bVal)
	assert(type(bVal) == "boolean")
	self.component.interactable = bVal
end

function UIToggle:SetCheckOldValue(bVal)
	assert(type(bVal) == "boolean")
	self.__checkOldValue = bVal
end

function UIToggle:SetOnValueChange(callback, handle, ...)
	local args = LuaUtil.SafePack(...)
	local func = function(bVal)
		if not self.__checkOldValue or not self.__oldValue == bVal then
			coroutine.start(function()
				if handle then
					callback(handle, bVal, LuaUtil.SafeUnpack(args))
				else
					callback(bVal, LuaUtil.SafeUnpack(args))
				end
			end)
			self.__oldValue = bVal
		end
	end
	self.__callback[callback] = func
	self.component.onValueChanged:AddListener(func)
end

function UIToggle:RemoveAllListeners()
	self.__callback = {}
	self.component.onValueChanged:RemoveAllListeners()
end

-- 资源释放
function UIToggle:dtor()
	self.__callback = {}
	self.component.onValueChanged:RemoveAllListeners()
end