--[[
    author:shenyi
    time:2018-12-27 17:04:01
]]

function IsString(Elem)
	return (type(Elem) == "string")
end

function IsFunc(Elem)
	return (type(Elem) == "function")
end

function IsTable(Elem)
	return (type(Elem) == "table")
end

--判断table是否是数组
function IsArray(Elem)
	if not IsTable(Elem) then
		return false
	end

	local Map = {}
	local Size = 0
	for k, _ in pairs(Elem) do
		if not IsPosInt(k) then
			return false
		end

		Map[k] = true
		Size = Size + 1
	end

	if Size <= 0 then
		return true
	end

	for idx = 1, Size do
		if not Map[idx] then
			return false
		end
	end

	return true
end

function IsNumber(Elem)
	return (type(Elem) == "number")
end

function IsBoolean(Elem)
	return (type(Elem) == "boolean")
end

function IsFunction(Elem)
	return (type(Elem) == "function")
end

function IsCo(Elem)
	return (type(Elem) == "thread")
end

function IsInt(Num)
	if (not Num) or (not IsNumber(Num)) then
		return false
	end

	return (math.floor(Num) == Num)
end

-- bug: 3.0 也返回true
function IsPosInt(Num)
	if (not Num) or (not IsNumber(Num)) then
		return false
	end

	if Num <= 0 then
		return false
	end

	return (math.floor(Num) == Num)
end

function ToBoolean(value)
	return not not value
end