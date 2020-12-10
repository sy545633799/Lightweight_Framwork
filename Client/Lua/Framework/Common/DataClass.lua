--- config util
--- Created by shenyi
--- DateTime: 2019.6.5

---@class DataClass
local DataClass = {}

function DataClass:UpdateField(field_key, new_field_value)
    local old_value = self[field_key]
    if new_field_value then
        self[field_key] = new_field_value
    end
    if self.__data_callback[field_key] then
        for callback, _ in pairs(self.__data_callback[field_key]) do
            callback(old_value, self[field_key])
        end
    end
end

function DataClass:UpdateData(data)
    for key, val in pairs(data) do
        self:UpdateField(key, val)
    end
end

function DataClass:AddListener(field_key, callback, updateNow)
    if not self.__data_callback[field_key] then
        self.__data_callback[field_key] = {}
    end
    self.__data_callback[field_key][callback] = true
    if updateNow then
        self:UpdateField(field_key, self[field_key])
    end
end

function DataClass:RemoveListener(field_key, callback)
    if self.__data_callback[field_key] then
        self.__data_callback[field_key][callback] = nil
    end
end

---@return DataClass
function PropertyData()
    local data = {}
    data.__data_callback = {}
    setmetatable(data, {
        __index = function(table, key)
            if DataClass[key] then
                return DataClass[key]
            else
                return ""
            end
        end,
    })
    return data
end