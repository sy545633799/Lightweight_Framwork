--[[
    author:shenyi
    time:2018-12-27 17:00:27
]]

local function nonReturnFunc()
end

function pairs_anyway(tbl, comp)
	if not tbl then
		return nonReturnFunc
	end
	return pairs(tbl, comp)
end

function ipairs_anyway(tbl)
	if not tbl then
		return nonReturnFunc
	end
	return ipairs(tbl)
end

function pairs_sort(tbl, comp)
	local keys = {}
	for k, v in pairs(tbl) do
		table.insert(keys, k)
	end
	table.sort(keys, comp)
	return keys

	--[[
	local ret = {}
	for _, k in pairs(keys) do
		ret[k] = tbl[k]
	end
	return ret
	--]]
end

-- 有序遍历
function pairs_orderly(tbl, comp)
	local keys = {}
	for k, v in pairs(tbl) do
		table.insert(keys, k)
	end
	table.sort(keys, comp)
	local index = 0
	local keys_count = #keys
	local next_orderly = function()
		index = index + 1
		if index > keys_count then return end
		return keys[index], tbl[keys[index]]
	end
	return next_orderly
end

----------
function copy_keys(tbl)
	local keys = {}
	for k, v in pairs(tbl) do
		keys[k] = v
	end
	return keys
end

function pairs_orderly_safe(tbl, comp)
	local keys = {}
	local newTbl = {}
	local keys_count = 0
	for k, v in pairs(tbl) do
		table.insert(keys, k)
		newTbl[k] = v
		keys_count = keys_count + 1
	end
	table.sort(keys, comp)
	local index = 0
	local next_orderly = function()
		index = index + 1
		if index > keys_count then return end
		return keys[index], newTbl[keys[index]]
	end
	return next_orderly
end


local function random_sort (Array)
	local n = #Array

	local k = {}
	for i = 1, n do
		k[i] = i
	end

	local o = {}
	local s = {}
	for i = 1, n do
		local j = math.random (n - i + 1)
		s[k[j]] = i
		table.insert(o, Array[k[j]])
		table.remove (k, j)
	end

	return o, s
end

-- 乱序遍历
function pairs_randomly(tbl)
	local keys = {}
	for k, v in pairs(tbl) do
		table.insert(keys, k)
	end

	keys = random_sort(keys)
	local keys_count = #keys
	local index = 0
	local next_randomly = function(tbl)
		index = index + 1
		if index > keys_count then return end
		return keys[index], tbl[keys[index]]
	end
	return next_randomly, tbl
end

-- 按照指定关键字列表遍历
function pairs_by_keys(tbl, keys)
	local index = 0
	local keys_count = #keys
	local next_data = function()
		index = index + 1
		if index > keys_count then return end
		local key = keys[index]
		return key, tbl[key]
	end
	return next_data
end
