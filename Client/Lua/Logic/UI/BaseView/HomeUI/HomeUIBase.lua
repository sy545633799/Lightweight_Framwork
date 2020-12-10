----------------------- auto generate code --------------------------
local base = UITabView
---@class HomeUIBase:UITabView
local HomeUIBase = BaseClass("HomeUIBase", base)

function HomeUIBase:ctor(container)
	self.widget_root = self:AddWidget(container.widget_root)
	self.item_Manor = container.item_Manor
	self.item_Package = container.item_Package
	self.item_Story = container.item_Story
	self.item_Hero = container.item_Hero
	self.item_Society = container.item_Society
	self.btn_kuang = self:AddButton(container.btn_kuang)
	self.img_kuang = self:AddImage(container.img_kuang)
	self.img_head = self:AddImage(container.img_head)
	self.text_name = self:AddText(container.text_name)
	self.text_level = self:AddText(container.text_level)
	self.btn_fight = self:AddButton(container.btn_fight)
	self.btn_addCrystal = self:AddButton(container.btn_addCrystal)
	self.text_crystal = self:AddText(container.text_crystal)
	self.btn_addGold = self:AddButton(container.btn_addGold)
	self.text_gold = self:AddText(container.text_gold)
	self.canvasGroup_tab01 = self:AddCanvasGroup(container.canvasGroup_tab01)
	self.btn_unselect01 = self:AddButton(container.btn_unselect01)
	self.img_unselect01 = self:AddImage(container.img_unselect01)
	self.text_unselect01 = self:AddText(container.text_unselect01)
	self.canvasGroup_select01 = self:AddCanvasGroup(container.canvasGroup_select01)
	self.img_select01 = self:AddImage(container.img_select01)
	self.text_select01 = self:AddText(container.text_select01)
	self.canvasGroup_tab02 = self:AddCanvasGroup(container.canvasGroup_tab02)
	self.btn_unselect02 = self:AddButton(container.btn_unselect02)
	self.img_unselect02 = self:AddImage(container.img_unselect02)
	self.text_unselect02 = self:AddText(container.text_unselect02)
	self.canvasGroup_select02 = self:AddCanvasGroup(container.canvasGroup_select02)
	self.img_select02 = self:AddImage(container.img_select02)
	self.text_select02 = self:AddText(container.text_select02)
	self.canvasGroup_tab03 = self:AddCanvasGroup(container.canvasGroup_tab03)
	self.btn_unselect03 = self:AddButton(container.btn_unselect03)
	self.img_unselect03 = self:AddImage(container.img_unselect03)
	self.text_unselect03 = self:AddText(container.text_unselect03)
	self.canvasGroup_select03 = self:AddCanvasGroup(container.canvasGroup_select03)
	self.img_select03 = self:AddImage(container.img_select03)
	self.text_select03 = self:AddText(container.text_select03)
	self.canvasGroup_tab04 = self:AddCanvasGroup(container.canvasGroup_tab04)
	self.btn_unselect04 = self:AddButton(container.btn_unselect04)
	self.img_unselect04 = self:AddImage(container.img_unselect04)
	self.text_unselect04 = self:AddText(container.text_unselect04)
	self.canvasGroup_select04 = self:AddCanvasGroup(container.canvasGroup_select04)
	self.img_select04 = self:AddImage(container.img_select04)
	self.text_select04 = self:AddText(container.text_select04)
	self.canvasGroup_tab05 = self:AddCanvasGroup(container.canvasGroup_tab05)
	self.btn_unselect05 = self:AddButton(container.btn_unselect05)
	self.img_unselect05 = self:AddImage(container.img_unselect05)
	self.text_unselect05 = self:AddText(container.text_unselect05)
	self.canvasGroup_select05 = self:AddCanvasGroup(container.canvasGroup_select05)
	self.img_select05 = self:AddImage(container.img_select05)
	self.text_select05 = self:AddText(container.text_select05)
end

return HomeUIBase
