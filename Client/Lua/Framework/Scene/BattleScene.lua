---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by shenyi.
--- DateTime: 2020/3/20 15:43
local base = require "Framework.Scene.BaseScene"
local BattleScene = BaseClass("BattleScene", base)

function BattleScene:OnCreate()

end

--预加载资源
function BattleScene:OnPrepare(co)

end

function BattleScene:OnEnter(co)
    print("enterbattle")
end

function BattleScene:OnLeave()

end

return BattleScene