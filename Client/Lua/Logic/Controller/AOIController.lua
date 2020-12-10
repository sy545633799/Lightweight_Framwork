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

function AOIController:EnterMap(sceneId)

    ---@class AOIData
    local aoiData =
    {
        sceneId = 1,
        uid = "123",
        element_id = 10001,
        pos = { x = -3.323405, y = -1.17, z = -8.666541 },
        forward = { x = 0, y = 0, z = 0 },
        entityType = 1,
        modelId = 10001,
    }

    local entity
    aoiData.entityType = EntityType.player
    local entity = Hero.New(aoiData)
    MainCamera:SetTarget(entity.behavior.transform)

    self.entites["123"] = entity

end

function AOIController:GetBornPosition()
    if self.EnterBattleMap then
        return self.bornPosition
    end
    return { x = 0, y = 0, z = 0}
end

---@return Entity
function AOIController:GetEntityByUID(uid)
    return self.entites[uid]
end

function AOIController:DestroyEntityByUID(uid)
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