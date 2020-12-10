--[[
-- added by wsh @ 2017-12-18
-- string扩展工具类，对string不支持的功能执行扩展
--]]

local unpack = unpack or table.unpack
local string_len = string.len
local string_byte = string.byte

-- 字符串分割
-- @split_string：被分割的字符串
-- @pattern：分隔符，可以为模式匹配
-- @init：起始位置
-- @plain：为true禁用pattern模式匹配；为false则开启模式匹配
function string.split(split_string, pattern, search_pos_begin, plain)
	assert(type(split_string) == "string")
	assert(type(pattern) == "string" and #pattern > 0)
	search_pos_begin = search_pos_begin or 1
	plain = plain or true
	local split_result = {}

	while true do
		local find_pos_begin, find_pos_end = string.find(split_string, pattern, search_pos_begin, plain)
		if not find_pos_begin then
			break
		end
		local cur_str = ""
		if find_pos_begin > search_pos_begin then
			cur_str = string.sub(split_string, search_pos_begin, find_pos_begin - 1)
		end
		split_result[#split_result + 1] = cur_str
		search_pos_begin = find_pos_end + 1
	end

	if search_pos_begin < string.len(split_string) then
		split_result[#split_result + 1] = string.sub(split_string, search_pos_begin)
	else
		split_result[#split_result + 1] = ""
	end

	return split_result
end

-- 字符串连接
function string.join(join_table, joiner)
	if #join_table == 0 then
		return ""
	end

	local fmt = "%s"
	for i = 2, #join_table do
		fmt = fmt .. joiner .. "%s"
	end

	return string.format(fmt, unpack(join_table))
end

-- 是否包含
-- 注意：plain为true时，关闭模式匹配机制，此时函数仅做直接的 “查找子串”的操作
function string.contains(target_string, pattern, plain)
	plain = plain or true
	local find_pos_begin, find_pos_end = string.find(target_string, pattern, 1, plain)
	return find_pos_begin ~= nil
end

-- 以某个字符串开始
function string.startswith(target_string, start_pattern, plain)
	plain = plain or true
	local find_pos_begin, find_pos_end = string.find(target_string, start_pattern, 1, plain)
	return find_pos_begin == 1
end

-- 以某个字符串结尾
function string.endswith(target_string, start_pattern, plain)
	plain = plain or true
	local find_pos_begin, find_pos_end = string.find(target_string, start_pattern, -#start_pattern, plain)
	return find_pos_end == #target_string
end

-------------------------------------------------------------------------
--字符串切割
function string.splitex(szFullString, szSeparator)
	local nFindStartIndex = 1
	local nSplitIndex = 1
	local nSplitArray = {}
	while true do
		local nFindLastIndex = string.find(szFullString, szSeparator, nFindStartIndex)
		if not nFindLastIndex then
			nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, string.len(szFullString))
			break
		end
		nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, nFindLastIndex - 1)
		nFindStartIndex = nFindLastIndex + string.len(szSeparator)
		nSplitIndex = nSplitIndex + 1
	end
	return nSplitArray
end


function string.trim(s)
	return (string.gsub(s, "^%s*(.-)%s*$", "%1"))
end

function string.utf8len(input)
	local len  = string_len(input)
	local left = len
	local cnt  = 0
	local arr  = {0, 0xc0, 0xe0, 0xf0, 0xf8, 0xfc}
	while left ~= 0 do
		local tmp = string_byte(input, -left)
		local i   = #arr
		while arr[i] do
			if tmp >= arr[i] then
				left = left - i
				break
			end
			i = i - 1
		end
		cnt = cnt + 1
	end
	return cnt
end

-- 计算utf8字符串字符数, 各种字符都按一个字符计算
-- 例如utf8len("1你好") => 3
function string.utf8lenex(str)
	local len = 0
	local aNum = 0 --字母个数
	local hNum = 0 --汉字个数
	local currentIndex = 1
	while currentIndex <= #str do
		local char = string.byte(str, currentIndex)
		local cs = string.charsize(char)
		currentIndex = currentIndex + cs
		len = len +1
		if cs == 1 then
			aNum = aNum + 1
		elseif cs >= 2 then
			hNum = hNum + 1
		end
	end
	return len, aNum, hNum
end

function string.haschinese(str)
	for i=1, string.len(str) do
		local curByte = string.byte(str, i)
		if curByte > 127 then
			return true
		end
	end
	return false
end

function string.charsize(ch)
	if not ch then return 0
	elseif ch >=252 then return 6
	elseif ch >= 248 and ch < 252 then return 5
	elseif ch >= 240 and ch < 248 then return 4
	elseif ch >= 224 and ch < 240 then return 3
	elseif ch >= 192 and ch < 224 then return 2
	elseif ch < 192 then return 1
	end
end


-- 截取utf8 字符串
-- str:            要截取的字符串
-- startChar:    开始字符下标,从1开始
-- numChars:    要截取的字符长度
function string.utf8sub(str, startChar, numChars)
	local startIndex = 1
	while startChar > 1 do
		local char = string.byte(str, startIndex)
		startIndex = startIndex + string.charsize(char)
		startChar = startChar - 1
	end

	local currentIndex = startIndex

	while numChars > 0 and currentIndex <= #str do
		local char = string.byte(str, currentIndex)
		currentIndex = currentIndex + string.charsize(char)
		numChars = numChars -1
	end
	return str:sub(startIndex, currentIndex - 1)
end