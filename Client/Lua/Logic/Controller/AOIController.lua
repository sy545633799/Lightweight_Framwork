---@class AOIController:Controller
local AOIController = BaseClass("AOIController", Controller)
------------------------------------------------------------------------------
local EntityType = EntityType
local Player = require("Logic/Entity/Behaviour/Player")
local Hero = require("Logic/Entity/Behaviour/Hero")
------------------------------------------------------------------------------
function AOIController:ctor()
    ---@type table<string, Entity> @private
    self.entites_map = {}
    self:AddMessageListener(NetMsgId.sync_create_entities, self.CreateEntities, self)
    self:AddMessageListener(NetMsgId.sync_delete_entities, self.RemoveEntities, self)
end

---@param args table<number, AOIData>
function AOIController:CreateEntities(args)
    local roleId  = RoleModel.RoleData.roleId
    for aoidId, aoiData in pairs(args) do
        local entity
        if aoiData.paramId == roleId then
            aoiData.type = EntityType.hero
            entity = Hero.New(aoiData)
            MainCamera:SetTarget(entity.behavior.transform)
        else
            logError("创建其他单位")
        end
        self.entites_map[aoidId] = entity
    end
end

---@param args table<number, Entity>
function AOIController:RemoveEntities(args)
    for aoidId, entity in pairs(args) do
        if self.entites_map[aoidId] then
            entity:Delete()
            self.entites_map[aoidId] = nil
        end
    end
end

---@return Entity
function AOIController:GetEntityByAoiId(uid)
    return self.entites_map[uid]
end

function AOIController:DestroyEntityByAoiId(uid)
    local entity = self.entites_map[uid]
    if entity then
        entity:Delete()
        self.entites_map[uid] = nil
    end
end

--自动寻路到场景中elementID的元素
function AOIController:SimplePosToTarget(elementID,callBack)

end

return AOIController