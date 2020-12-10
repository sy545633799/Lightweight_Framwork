---@class UIButton:UIContent
UIButton = BaseClass("UIButton", UIContent)

---@public 创建
function UIButton:ctor(component)
	---@type Game.UIButton
	self.component = component
	self.click_map = {}
end

---@public 虚拟点击
function UIButton:Click()
	for _, func in pairs(self.click_map) do func() end
end

---@public 设置回调
function UIButton:AddClick(callback, handle, ...)
	local args = LuaUtil.SafePack(...)
	local func = function()
		coroutine.start(function()
			if handle then
				callback(handle, LuaUtil.SafeUnpack(args))
			else
				callback(LuaUtil.SafeUnpack(args))
			end
		end)
	end
	self.click_map[callback] = func
	self.component.onClick:AddListener(func)
end

---@public 设置回调
function UIButton:AddDown(callback, handle, ...)
	local args = LuaUtil.SafePack(...)
	local func = function()
		coroutine.start(function()
			if handle then
				callback(handle, LuaUtil.SafeUnpack(args))
			else
				callback(LuaUtil.SafeUnpack(args))
			end
		end)
	end
	self.click_map[callback] = func
	self.component.onDown:AddListener(func)
end

---@public 设置回调
function UIButton:AddPress(callback, handle, ...)
	local args = LuaUtil.SafePack(...)
	local func = function()
		coroutine.start(function()
			if handle then
				callback(handle, LuaUtil.SafeUnpack(args))
			else
				callback(LuaUtil.SafeUnpack(args))
			end

		end)
	end
	self.click_map[callback] = func
	self.component.onPress:AddListener(func)
end

---@public 设置回调
function UIButton:AddUP(callback, handle, ...)
	local args = LuaUtil.SafePack(...)
	local func = function()
		coroutine.start(function()
			if handle then
				callback(handle, LuaUtil.SafeUnpack(args))
			else
				callback(LuaUtil.SafeUnpack(args))
			end

		end)
	end
	self.click_map[callback] = func
	self.component.onUp:AddListener(func)
end

function UIButton:RemoveClick(callback)
	local func = self.click_map[callback]
	if func then
		self.component.onClick:RemoveListener(func)
		self.click_map[callback] = nil
	end
end

function UIButton:RemoveAllClick()
	self.click_map = {}
	self.component.onClick:RemoveAllListeners()
end

function UIButton:SetInteractable(enabled)
	--uiUtil.EnableOrDisableClick(go, enabled)
	--local img = go:GetComponent("Image")
	--if not img then
	--	return
	--end
	--if enabled then
	--	--img.material = nil
	--	uiUtil.AddButtonEffect(go)
	--else
	--	--img.material = ResourceManager.GetMaterial("Gray")
	--	uiUtil.RemoveButtonEffect(go)
	--end
	--
	--MDUIUtil:SetPicGray(img, not enabled)
end

function UIButton:dtor()
	self:RemoveAllClick()
end
