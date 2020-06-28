--- config util
--- Created by shenyi
--- DateTime: 2019.6.5
---

local DataClass = {}
--更新数据，同时通知view层
function DataClass:AddListener(key, callback)
    if not self.__data_callback[key] then
        self.__data_callback[key] = {}
    end
    self.__data_callback[key][callback] = true
end

--更新数据，同时通知view层
function DataClass:Update(key, value)
    local oldValue = self[key]
    --if type(value) == "table" then
    --    table.update(self[key], value)
    --else
        self[key] = value
    --end

    if self.__data_callback[key] then
        for func,_ in pairs(self.__data_callback[key]) do
            func(oldValue, self[key])
        end
    end
end

--更新数据，同时通知view层
function DataClass:RemoveListener(key, callback)
    if self.__data_callback[key] then
        self.__data_callback[key][callback] = nil
    end
end

--固定字段表
function DataItemClass(classname, data_tb, readOnly)
    assert(type(classname) == "string" and #classname > 0, "数据类型名称不能为空")

    --构造原型
    local cls = table.copy(data_tb or {})
    cls.__cname = classname
    cls.readOnly = readOnly and true or false

    setmetatable(cls, { __index = DataClass })

    function cls.New(data)
        local vtbl = {}
        vtbl.__data_callback = {}
        if data then
            assert(type(data) == "table", "data必须为table类型")
            for k,v in pairs(data) do
                if cls[k] then
                    vtbl[k] = v
                else
                    error(cls.__cname.."write err: No key named : "..tostring(k), 2)
                end
            end
        end
        setmetatable(vtbl, { __index = cls })

        local ret_data = setmetatable({}, {
            __index = function(tb, key)
                if vtbl[key] then
                    return vtbl[key]
                else
                    error(cls.__cname.." read err: no key named : "..tostring(key), 2)
                end
            end,
            __newindex = function(tb, key, value)
                if vtbl.readOnly then
                    error(cls.__cname.." write err: No key named : "..tostring(key), 2)
                else
                    if cls[key]then
                        vtbl[key] = value
                    else
                        error(cls.__cname.." write err: No key named : "..tostring(key), 2)
                    end
                end
            end
        })
        return ret_data
    end

    return cls
end