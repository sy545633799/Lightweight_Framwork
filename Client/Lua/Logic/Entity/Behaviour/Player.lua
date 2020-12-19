local base = Entity
---@class Player:Entity
local Player = BaseClass("Player", base)

function Player:ctor(aoiData)

end

function Player:OnBodyCreate(componets)
    base.OnBodyCreate(self, componets)
end

function Player:MoveByNavimesh(pos, stopDis, onArrived, onMoving)
    --if self.moveComp then
    --    self.moveComp:MoveByNavimesh(pos, (stopDis or 0), onArrived, onMoving)
    --
    --end
end

function Player:StopMoveByNavimesh()
    --if self.moveComp then
    --    self.moveComp:StopMoveByNavimesh()
    --end
end

return Player