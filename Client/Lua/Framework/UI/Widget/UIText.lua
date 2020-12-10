---@class UIText:UIContent
UIText = BaseClass("UIText", UIContent)
-- 创建
function UIText:ctor(component)
	---@type Game.UIText
	self.component = component
end

-- 获取文本
function UIText:GetText()
	if not IsNull(self.component) then
		return self.component.text
	end
end

-- 设置文本
function UIText:SetText(text)
	if not IsNull(self.component) then
		self.component.text = tostring(text)
	end
end

function UIText:SetTextID(id)
	if not IsNull(self.component) then
		self.component.ID = id
	end
end
-- 逐字打印
function UIText:DoText(text, duration, ease, callback, handle)
	self:RemoveAllTweener()
	if not IsNull(self.component) then
		local tweener = self.component:DOText(text,duration)
		self:AddTweener(tweener, ease, callback, handle)
	end
end


-- 透明度渐变动画
function UIText:DoFade(value, duration, ease, callback, handle)
	if not IsNull(self.component) then
		local tweener = self.component:DOFade(value, duration)
		self:AddTweener(tweener, ease, callback, handle)
	end
end

-- 销毁
function UIText:dtor()

end