---@class UIButton:UIContent
UIButton = BaseClass("UIButton", UIContent)
-- 创建
function UIButton:OnCreate(btn)
	---@type Game.UIButton
	self.unity_uibutton = btn
end

-- 虚拟点击
function UIButton:Click(self)
	if self.__onclick  ~= nil then
		self.__onclick()
	end
end

-- 设置回调
function UIButton:AddClick(callback)
	self.__onclick = callback
	self.unity_uibutton.onClick:AddListener(self.__onclick)
end

function UIButton:RmoveClick(callback)
	self.unity_uibutton.onClick:RemoveListener(self.__onclick)
end

function UIButton:RmoveAllClick()
	self.unity_uibutton.onClick:RemoveAllListeners()
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

-- 资源释放
function UIButton:OnDestroy()
	self.__onclick = nil
	self:RmoveAllClick()
end
