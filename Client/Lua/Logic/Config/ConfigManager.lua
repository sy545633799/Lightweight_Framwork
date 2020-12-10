ConfigManger = {}

---@return HeroInfoItem
ConfigManger.GetHeroConfig = function() return require("Logic.Config.HeroConfig") end

---@return table<number, HeroInfoItem>
ConfigManger.GetHeroInfo = function() return require("Logic.Config.HeroInfo") end

---@return Common_Item
ConfigManger.GetCommonItem = function() return require("Logic.Config.common_item") end