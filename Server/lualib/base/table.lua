--[[
    author:shenyi
    time:2018-12-27 16:16:42
]]

-- 返回table大小
function table.size(t)
	local count = 0
	for _ in pairs(t) do
		count = count + 1
	end
	return count
end

-- 判断table是否为空
function table.empty(t)
    return not next(t)
end

function table.keys(hashtable)
    local keys = {}
    for k, v in pairs(hashtable) do
        keys[#keys + 1] = k
    end
    return keys
end

function table.values(hashtable)
    local values = {}
    for k, v in pairs(hashtable) do
        values[#values + 1] = v
    end
    return values
end

function table.reverse(Array)
	local size = #Array
	for i = 1, math.floor(size / 2) do
		local tmp = Array[i]
		Array[i] = Array[size + 1 - i]
		Array[size + 1 - i] = tmp
	end
	return Array
end

function table.removebyvalue(array, value, removeall)
    local c, i, max = 0, 1, #array
    while i <= max do
        if array[i] == value then
            table.remove(array, i)
            c = c + 1
            i = i - 1
            max = max - 1
            if not removeall then break end
        end
        i = i + 1
    end
    return c
end

function table.insertto(dest, src, begin)
    begin = checkint(begin)
    if begin <= 0 then
        begin = #dest + 1
    end

    local len = #src
    for i = 0, len - 1 do
        dest[i + begin] = src[i + 1]
    end
end

function table.indexof(array, value, begin)
    for i = begin or 1, #array do
        if array[i] == value then return i end
    end
    return false
end

function table.keyof(hashtable, value)
    for k, v in pairs(hashtable) do
        if v == value then return k end
    end
    return nil
end

function table.map(t, fn)
    for k, v in pairs(t) do
        t[k] = fn(v, k)
    end
end

function table.walk(t, fn)
    for k,v in pairs(t) do
        fn(v, k)
    end
end

function table.filter(t, fn)
    for k, v in pairs(t) do
        if not fn(v, k) then t[k] = nil end
    end
end

--总和
function table.sum(tbl)
	local Total = 0
	for _, v in pairs(tbl) do
		Total = Total + v
	end
	return Total
end

--返回number型数组的平均值(float)
function table.avg(Array)
	if #Array == 0 then
		return 0
	end
	local All = 0
	local i = 0
	for _, Data in ipairs(Array) do
		assert(type(Data) == "number")
		All = All + Data
		i = i + 1
	end
	return All/i
end

--返回Array中的最大值
--注意:不是Hash-table
function table.max(Array)
	return math.max (unpack(Array))
end

--返回Array中的最小值
--注意:不是Hash-table
function table.min(Array)
	return math.min (unpack(Array))
end

--从table中随机返回1个value
function table.random_value(Table)
	local Values = table.values(Table)
	local n = #Values
	if n <= 0 then
		return nil
	end
	return Values[math.random(1, n)]
end

-- 浅拷贝
function table.clone(t)
    local result = {}
    for k, v in pairs (t) do
        result[k] = v
    end
    return result
end

-- 深拷贝
function table.copy(t, nometa)   
    local result = {}

    if not nometa then
        setmetatable(result, getmetatable(t))
    end

    for k, v in pairs(t) do
        if type(v) == "table" then
            result[k] = table.copy(v)
        else
            result[k] = v
        end
    end
    return result
end

function table.merge(t, t1, pairs_func, deepcopy)
	pairs_func = pairs_func or pairs
	for k, v1 in pairs_func(t1) do
		local vt1 = type(v1)
		if not t[k] then
			if deepcopy then
				t[k] = table.deepcopy(v1)
			else
				t[k] = v1
			end
		else
			local v = t[k]
			local vt = type(v)
			if vt == vt1 then
				if vt == 'table' then
					table.merge(t[k], t1[k], pairs_func, deepcopy)
				else
					t[k] = t1[k]
				end
			end
		end
	end
	return t
end

-- 以加的方式将表t1合并到表t中。
function table.addmerge(t, t1, pairs_func)
	pairs_func = pairs_func or pairs
	for k, v1 in pairs_func(t1) do
		local vt1 = type(v1)
		if not t[k] then
			t[k] = table.deepcopy(v1)
		else
			local v = t[k]
			local vt = type(v)
			if vt == vt1 then
				if vt == 'table' then
					table.addmerge(t[k], t1[k], pairs_func)
				elseif vt == 'number' then
					t[k] = t[k] + t1[k]
				elseif vt == 'string' then
					t[k] = t[k] .. t1[k]
				end
			end
		end
	end
end

function table.submerge(t, t1, pairs_func)
	pairs_func = pairs_func or pairs
	for k, v1 in pairs_func(t1) do
		local vt1 = type(v1)
		if t[k] then
			local v = t[k]
			local vt = type(v)
			if vt == vt1 then
				if vt == 'table' then
					table.submerge(t[k], t1[k], pairs_func)
				elseif vt == 'number' then
					t[k] = t[k] - t1[k]
				end
			end
		end
	end
end

function table.has_key(Tbl, key)
	if not Tbl then return false end
	for k, _ in pairs(Tbl) do
		if k == key then return true end
	end
	return false
end

function table.has_value(Tbl, value)
	if not Tbl then return false end
	for _, v in pairs(Tbl) do
		if v == value then return true end
	end
	return false
end

function table.has_key_or_value(Tbl, keyOrValue)
	if not Tbl then return false end
	for k, v in pairs(Tbl) do
		if v == keyOrValue or k == keyOrValue then return true end
	end
	return false
end

function table.same_key(Tbl1, Tbl2)
	assert(type(Tbl1) == 'table')
	assert(type(Tbl2) == 'table')
	if table.size(Tbl1) ~= table.size(Tbl2) then
		return false
	end

	for k, v in pairs(Tbl2) do
		if not Tbl1[k]	then
			return false
		end
	end
	return true
end

--判断包含关系(hash table only)
function table.same_table(Tbl1, Tbl2)
	assert(type(Tbl1) == 'table')
	assert(type(Tbl2) == 'table')
	if table.size(Tbl1) ~= table.size(Tbl2) then
		return false
	end

	for k, v in pairs(Tbl2) do
		if Tbl1[k] ~= v then
			return false
		end
	end
	return true
end


local function dump(obj)
    local getIndent, quoteStr, wrapKey, wrapVal, dumpObj
    getIndent = function(level)
        return string.rep("\t", level)
    end
    quoteStr = function(str)
        return '"' .. string.gsub(str, '"', '\\"') .. '"'
    end
    wrapKey = function(val)
        if type(val) == "number" then
            return "[" .. val .. "]"
        elseif type(val) == "string" then
            return "[" .. quoteStr(val) .. "]"
        else
            return "[" .. tostring(val) .. "]"
        end
    end
    wrapVal = function(val, level)
        if type(val) == "table" then
            return dumpObj(val, level)
        elseif type(val) == "number" then
            return val
        elseif type(val) == "string" then
            return quoteStr(val)
        else
            return tostring(val)
        end
    end
    dumpObj = function(obj, level)
        if type(obj) ~= "table" then
            return wrapVal(obj)
        end
        level = level + 1
        local tokens = {}
        tokens[#tokens + 1] = "{"
        for k, v in pairs(obj) do
            tokens[#tokens + 1] = getIndent(level) .. wrapKey(k) .. " = " .. wrapVal(v, level) .. ","
        end
        tokens[#tokens + 1] = getIndent(level - 1) .. "}"
        return table.concat(tokens, "\n")
    end
    return dumpObj(obj, 0)
end

do
    local _tostring = tostring
    tostring = function(v)
        if type(v) == 'table' then
            return dump(v)
        else
            return _tostring(v)
        end
    end
end