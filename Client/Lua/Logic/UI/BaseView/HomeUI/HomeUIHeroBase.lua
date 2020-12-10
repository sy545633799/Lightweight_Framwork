----------------------- auto generate code --------------------------
local base = UIBaseItem
---@class HomeUIHeroBase:UIBaseItem
local HomeUIHeroBase = BaseClass("HomeUIHeroBase", base)

function HomeUIHeroBase:ctor(container)
	self.img_quality = self:AddImage(container.img_quality)
	self.text_name = self:AddText(container.text_name)
	self.text_level = self:AddText(container.text_level)
	self.item_Equip1 = container.item_Equip1
	self.item_Equip2 = container.item_Equip2
	self.item_Equip3 = container.item_Equip3
	self.item_Equip4 = container.item_Equip4
	self.item_Equip5 = container.item_Equip5
	self.item_Equip6 = container.item_Equip6
	self.text_power = self:AddText(container.text_power)
	self.text_intelligence = self:AddText(container.text_intelligence)
	self.text_live = self:AddText(container.text_live)
	self.text_power = self:AddText(container.text_power)
	self.text_intelligence = self:AddText(container.text_intelligence)
	self.text_live = self:AddText(container.text_live)
	self.item_Skill1 = container.item_Skill1
	self.item_Skill2 = container.item_Skill2
	self.text_fight = self:AddText(container.text_fight)
	self.item_Skill3 = container.item_Skill3
	self.item_Skill4 = container.item_Skill4
	self.btn_takeoff = self:AddButton(container.btn_takeoff)
	self.btn_up = self:AddButton(container.btn_up)
	self.text_up = self:AddText(container.text_up)
	self.btn_takeoff = self:AddButton(container.btn_takeoff)
	self.btn_order = self:AddButton(container.btn_order)
	self.list_cards = self:AddListView(container.list_cards)
	self.item_Card = container.item_Card
end

return HomeUIHeroBase
