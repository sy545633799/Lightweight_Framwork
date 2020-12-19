---@class AOIController:Controller
local AOIController = BaseClass("AOIController", Controller)
------------------------------------------------------------------------------
local EntityType = EntityType
local Player = require("Logic/Entity/Behaviour/Player")
local Hero = require("Logic/Entity/Behaviour/Hero")
------------------------------------------------------------------------------
function AOIController:ctor()
    ---@type table<string, Entity> @private
    self.entites = {}
    self:AddMessageListener(NetMsgId.sync_create_entities, self.CreateEntities, self)
    self:AddMessageListener(NetMsgId.sync_delete_entities, self.RemoveEntities, self)
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

---@return Entity
function AOIController:CreateEntities(args)
    logError(args)
end

---@return Entity
function AOIController:RemoveEntities(args)

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