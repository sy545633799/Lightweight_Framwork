---@class AOIController:Controller
local AOIController = BaseClass("AOIController", Controller)
------------------------------------------------------------------------------
local EntityType = EntityType
local Player = require("Logic/Entity/Behaviour/Player")
local Hero = require("Logic/Entity/Behaviour/Hero")
------------------------------------------------------------------------------
---@type common_scene
function AOIController:ctor()
    ---@type table<string, Entity> @private
    self.entites = {}
end

---@param aoi_map table<number, AOIData>
function AOIController:EnterMap(aoi_map)
    local roleId  = RoleModel.RoleData.roleId
    for aoidId, aoiData in pairs(aoi_map) do
        local entity
        if aoiData.paramId == roleId then
            aoiData.type = EntityType.hero
            entity = Hero.New(aoiData)
            MainCamera:SetTarget(entity.behavior.transform)
        else

        end
        self.entites[aoidId] = entity
    end

end

function AOIController:GetBornPosition()
    if self.EnterBattleMap then
        return self.bornPosition
    end
    return { x = 0, y = 0, z = 0}
end

---@return Entity
function AOIController:GetEntityByAoiId(uid)
    return self.entites[uid]
end

function AOIController:DestroyEntityByAoiId(uid)
    local entity = self.entites[uid]
    if entity then
        entity:Delete()
        self.entites[uid] = nil
    end
end

--自动寻路到场景中elementID的元素
function AOIController:SimplePosToTarget(elementID,callBack)

end

return AOIController