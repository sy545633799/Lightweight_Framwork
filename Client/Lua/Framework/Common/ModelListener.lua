--- UI基类，可以监听Model层数据
--- Created by shenyi
--- DateTime: 2019.6.8

---@class ModelListener:Updatable
ModelListener = BaseClass("ModelListener", Updatable)

function ModelListener:ctor()
    self.model_callback = {}
end
------------------------------------------------------数据更新部分-------------------------------------------------------------

function ModelListener:AddModelListener(data, field_key, callback, handle, updateNow)
    assert(data and field_key and callback, "监听数据或者键名为空")
    if not self.model_callback[data] then
        self.model_callback[data] = {}
    end
    if self.model_callback[data][field_key] then
        logError("重复添加数据监听")
        return
    end
    local func = function(old, new)
        if handle then callback(handle, old, new)  else callback(old, new) end
    end

    self.model_callback[data][field_key] = func
    data:AddListener(field_key, func, updateNow == nil or updateNow)
end

function ModelListener:RemoveModelListener(data, field_key)
    assert(data and field_key, "监听数据或者键名为空")
    if not self.model_callback[data] then
        logError("没有监听该数据")
        return
    end
    data:RemoveListener(field_key, self.model_callback[data][field_key])
    self.model_callback[data][field_key] = nil
    if not next(self.model_callback[data]) then
        self.model_callback[data] = nil
    end
end

function ModelListener:RemoveAllModelListener()
    for data, tab in pairs(self.model_callback) do
        for field_key, func in pairs(tab) do
            data:RemoveListener(field_key, func)
        end
    end
    self.model_callback = {}
end
--------------------------------------------------------------------------------------------------------------------------------
function ModelListener:dtor()
    self:RemoveAllModelListener()
    self.model_callback = nil
end