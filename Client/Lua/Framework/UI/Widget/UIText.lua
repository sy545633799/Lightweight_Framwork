---@class UIText:UIContent
UIText = BaseClass("UIText", UIContent)
-- 创建
function UIText:OnCreate(uiText)
	self.unity_uitext = uiText
end

-- 获取文本
function UIText:GetText()
	if not IsNull(self.unity_uitext) then
		return self.unity_uitext.text
	end
end

-- 设置文本
function UIText:SetText(text)
	if not IsNull(self.unity_uitext) then
		self.unity_uitext.text = tostring(text)
	end
end

-- 销毁
function UIText:OnDestroy()

end