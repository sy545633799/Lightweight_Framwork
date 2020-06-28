--[[
-- added by wsh @ 2017-12-01
-- Lua全局工具类，全部定义为全局函数、变量
-- TODO:
-- 1、SafePack和SafeUnpack会被大量使用，到时候看需要需要做记忆表降低GC
--]]
local unpack = unpack or table.unpack

LuaUtil = {}

-- 解决原生pack的nil截断问题，SafePack与SafeUnpack要成对使用
function LuaUtil.SafePack(...)
	local params = {...}
	params.n = select('#', ...)
	return params
end

-- 解决原生unpack的nil截断问题，SafePack与SafeUnpack要成对使用
function LuaUtil.SafeUnpack(safe_pack_tb)
	return unpack(safe_pack_tb, 1, safe_pack_tb.n)
end

-- 对两个SafePack的表执行连接
function LuaUtil.ConcatSafePack(safe_pack_l, safe_pack_r)
	local concat = {}
	for i = 1,safe_pack_l.n do
		concat[i] = safe_pack_l[i]
	end
	for i = 1,safe_pack_r.n do
		concat[safe_pack_l.n + i] = safe_pack_r[i]
	end
	concat.n = safe_pack_l.n + safe_pack_r.n
	return concat
end

-- 闭包绑定
function LuaUtil.Bind(self, func, ...)
	assert(self == nil or type(self) == "table")
	assert(func ~= nil and type(func) == "function")
	local params = nil
	if self == nil then
		params = SafePack(...)
	else
		params = SafePack(self, ...)
	end
	return function(...)
		local args = ConcatSafePack(params, SafePack(...))
		func(SafeUnpack(args))
	end
end

-- 回调绑定
-- 重载形式：
-- 1、成员函数、私有函数绑定：BindCallback(obj, callback, ...)
-- 2、闭包绑定：BindCallback(callback, ...)
function LuaUtil.BindCallback(...)
	local bindFunc = nil
	local params = SafePack(...)
	assert(params.n >= 1, "BindCallback : error params count!")
	if type(params[1]) == "table" and type(params[2]) == "function" then
		bindFunc = Bind(...)
	elseif type(params[1]) == "function" then
		bindFunc = Bind(nil, ...)
	else
		error("BindCallback : error params list!")
	end
	return bindFunc
end

-- 将字符串转换为boolean值
function LuaUtil.ToBoolean(s)
	local transform_map = {
		["true"] = true,
		["false"] = false,
	}

	return transform_map[s]
end

-- 深拷贝对象
function LuaUtil.DeepCopy(object)
	local lookup_table = {}
	
	local function _copy(object)
		if type(object) ~= "table" then
			return object
		elseif lookup_table[object] then
			return lookup_table[object]
		end

		local new_table = {}
		lookup_table[object] = new_table
		for index, value in pairs(object) do
			new_table[_copy(index)] = _copy(value)
		end

		return setmetatable(new_table, getmetatable(object))
	end

	return _copy(object)
end

-- 序列化表
function LuaUtil.Serialize(tb, flag)
	local result = ""
	result = string.format("%s{", result)

	local filter = function(str)
		str = string.gsub(str, "%[", " ")
		str = string.gsub(str, "%]", " ")
		str = string.gsub(str, '\"', " ")
		str	= string.gsub(str, "%'", " ")
		str	= string.gsub(str, "\\", " ")
		str	= string.gsub(str, "%%", " ")
		return str
	end

	for k,v in pairs(tb) do
		if type(k) == "number" then
			if type(v) == "table" then
				result = string.format("%s[%d]=%s,", result, k, Serialize(v))
			elseif type(v) == "number" then
				result = string.format("%s[%d]=%d,", result, k, v)
			elseif type(v) == "string" then
				result = string.format("%s[%d]=%q,", result, k, v)
			elseif type(v) == "boolean" then
				result = string.format("%s[%d]=%s,", result, k, tostring(v))
			else
				if flag then
					result = string.format("%s[%d]=%q,", result, k, type(v))
				else
					error("the type of value is a function or userdata")
				end
			end
		else
			if type(v) == "table" then
				result = string.format("%s%s=%s,", result, k, Serialize(v, flag))
			elseif type(v) == "number" then
				result = string.format("%s%s=%d,", result, k, v)
			elseif type(v) == "string" then
				result = string.format("%s%s=%q,", result, k, v)
			elseif type(v) == "boolean" then
				result = string.format("%s%s=%s,", result, k, tostring(v))
			else
				if flag then
					result = string.format("%s[%s]=%q,", result, k, type(v))
				else
					error("the type of value is a function or userdata")
				end
			end
		end
	end
	result = string.format("%s}", result)
	return result
end

function LuaUtil.NumberToString(szNum)
	---阿拉伯数字转中文大写
	local szChMoney = ""
	local iLen = 0
	local iNum = 0
	local iAddZero = 0
	local hzUnit = {"", "十", "百", "千", "万", "十", "百", "千", "亿","十", "百", "千", "万", "十", "百", "千"}
	local hzNum = {"零", "一", "二", "三", "四", "五", "六", "七", "八", "九"}
	if nil == tonumber(szNum) then
		return tostring(szNum)
	end
	iLen =string.len(szNum)
	if iLen > 10 or iLen == 0 or tonumber(szNum) < 0 then
		return tostring(szNum)
	end
	for i = 1, iLen  do
		iNum = string.sub(szNum,i,i)
		if iNum == 0 and i ~= iLen then
			iAddZero = iAddZero + 1
		else
			if iAddZero > 0 then
				szChMoney = szChMoney..hzNum[1]
			end
			szChMoney = szChMoney..hzNum[iNum + 1] --//转换为相应的数字
			iAddZero = 0
		end
		if (iAddZero < 4) and (0 == (iLen - i) % 4 or 0 ~= tonumber(iNum)) then
			szChMoney = szChMoney..hzUnit[iLen-i+1]
		end
	end
	local function removeZero(num)
		--去掉末尾多余的 零
		num = tostring(num)
		local szLen = string.len(num)
		local zero_num = 0
		for i = szLen, 1, -3 do
			szNum = string.sub(num,i-2,i)
			if szNum == hzNum[1] then
				zero_num = zero_num + 1
			else
				break
			end
		end
		num = string.sub(num, 1,szLen - zero_num * 3)
		szNum = string.sub(num, 1,6)
		--- 开头的 "一十" 转成 "十" , 贴近人的读法
		if szNum == hzNum[2]..hzUnit[2] then
			num = string.sub(num, 4, string.len(num))
		end
		return num
	end
	return removeZero(szChMoney)
end

function LuaUtil.GetRandomBornPos(scene, pos, random_size)
	local new_x = pos.x + (math.random()*2-1) * random_size
	local new_z = pos.z + (math.random()*2-1) * random_size
	local legal_pos, res = scene:GetNearestPolyOfPoint(
			{
				x = new_x,
				y = pos.y,
				z = new_z,
			})
	if res then
		return legal_pos
	else
		return pos
	end
end

-- 生成一个只读类型的 table
function LuaUtil.GenerateReadOnlyTable(table)
	local original_table = {}
	setmetatable(original_table,
			{ __index = table,
			  __newindex = function()
				  error('attempt to update a read-only table!')
			  end
			})
	return original_table
end