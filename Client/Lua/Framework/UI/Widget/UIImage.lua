---@class UIImage:UIContent
UIImage = BaseClass("UIImage", UIContent)
-- 创建
function UIImage:OnCreate(img)
	self.unity_img = img
end


-- 获取Sprite名称
function UIImage:GetSpriteName(self)
	return self.sprite_name
end

-- 设置Sprite名称
function UIImage:SetSpriteName(sprite_name)

end

-- 销毁
function UIImage:OnDestroy()

end