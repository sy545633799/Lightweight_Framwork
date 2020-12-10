---@class UIImage:UIContent
UIImage = BaseClass("UIImage", UIContent)
-- 创建
function UIImage:ctor(component)
	self.component = component
end


-- 获取Sprite名称
function UIImage:GetSpriteName()
	return self.component.sprite.name
end

---@param atlas_name string 图集名
---@param sprite_name string 图片名, 不带后缀
function UIImage:SetAtlasSprite(atlas_name, sprite_name)
	assert(atlas_name and sprite_name, "empty atlas or sprite name")
	self.sprite_name = sprite_name or ""
	ResourceManager.LoadSprite(atlas_name, self.sprite_name, function (obj)
        if obj and not self.Deleted and self.sprite_name == sprite_name then
            self.component.sprite = obj
        end
	end)
end

function UIImage:SetTextureName(sprite_name)
	ResourceManager.LoadTexture(sprite_name, function (obj)
		if not self.Deleted then
			self.component.sprite = obj
		end
	end)
end

-- 透明度渐变动画
function UIImage:DoFade(value, duration)
	local tweener = nil
	if not IsNull(self.component) then
		tweener = self.component:DOFade(value, duration)
	end
	return tweener
end

-- 销毁
function UIImage:dtor()
	
end