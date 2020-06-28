--- entity(TODO), UI基类，可以监听Model层数据
--- Created by shenyi
--- DateTime: 2019.6.8

---@class ModelListener:Updatable
ModelListener = BaseClass("ModelListener", Updatable)

function ModelListener:ctor()
    self.model_callback = {}
end
------------------------------------------------------数据更新部分-------------------------------------------------------------
local function InnerAddModelListener(self, key_name, handle)

end

function Updatable:OnAddModelListener() end
function Updatable:AddModelListener()
    self:OnAddEventListener(InnerAddModelListener)
end

function Updatable:RemoveModelListener(key_name)

end

function Updatable:RemoveAllModelListener()
    
end
--------------------------------------------------------------------------------------------------------------------------------
function ModelListener:dtor()
    self:RemoveAllModelListener()
    self.model_callback = nil
end