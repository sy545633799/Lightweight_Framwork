
-------------------------------------------------------------
local base = require('Logic/UI/BaseView/HomeUI/HomeUIBase')
--- @class HomeUI: HomeUIBase
local HomeUI = BaseClass('HomeUI', base)
local HomeUIManor = require('Logic/UI/View/HomeUI/HomeUIManor')
local HomeUIPackage = require('Logic/UI/View/HomeUI/HomeUIPackage')
local HomeUIStory = require('Logic/UI/View/HomeUI/HomeUIStory')
local HomeUIHero = require('Logic/UI/View/HomeUI/HomeUIHero')
local HomeUISociety = require('Logic/UI/View/HomeUI/HomeUISociety')
-------------------------------------------------------------

local roleModel = RoleModel
local frameConfig = GetFrameConfig()

local subViewInfo = {
    [1] = {
        Tab = "tab_manor",
        TextID = 1003004,
    },
    [2] = {
        Tab = "tab_package",
        TextID = 1003005,
    },
    [3] = {
        Tab = "tab_story",
        TextID = 1003003,
    },
    [4] = {
        Tab = "tab_hero",
        TextID = 1003001,
    },
    [5] = {
        Tab = "tab_society",
        TextID = 1003002,
    },
}

function HomeUI:ctor()
    self.tab_manor = self:AddComponent(HomeUIManor, self.item_Manor)
    self.tab_package = self:AddComponent(HomeUIPackage, self.item_Package)
    self.tab_story = self:AddComponent(HomeUIStory, self.item_Story)
    self.tab_hero = self:AddComponent(HomeUIHero, self.item_Hero)
    self.tab_society = self:AddComponent(HomeUISociety, self.item_Society)
    self.btn_kuang:AddClick(function () UIManager:LoadView(UIConfig.SettingUI) end)
    self:InitTabs(subViewInfo)
end

function HomeUI:OnLoad(...)
    self:SelectTab(3)
    self:AddModelListener(roleModel.RoleData, PropertyNames.name, function (_, new) self.text_name:SetText(new) end)
    self:AddModelListener(roleModel.RoleData, PropertyNames.level, function (_, new) self.text_level:SetText(new) end)
    self:AddModelListener(roleModel.RoleData, PropertyNames.crystal, function (_, new) self.text_crystal:SetText(new) end)
    self:AddModelListener(roleModel.RoleData, PropertyNames.gold, function (_, new) self.text_gold:SetText(new) end)
    self:AddModelListener(roleModel.RoleData, PropertyNames.headFrameId, function (_, new) self.img_kuang:SetAtlasSprite(AtlasNames.HeadFrame, "Frame_"..new) end)
    self:AddModelListener(roleModel.RoleData, PropertyNames.headIconId, function (_, new)
        local num = tostring(new)
        while  #num < 3 do
            num = "0" .. num
        end
        self.img_head:SetAtlasSprite(AtlasNames.Head, num) end)
end

function HomeUI:OnUnLoad()

end

return HomeUI