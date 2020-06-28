--[[
    author:shenyi
    time:2018-12-27 16:15:00
]]
local UTF8_ZH_CN_PAT = "[%z\1-\127\192-\247][\128-\191]*"

string._htmlspecialchars_set = {}
string._htmlspecialchars_set["&"] = "&amp;"
string._htmlspecialchars_set["\""] = "&quot;"
string._htmlspecialchars_set["'"] = "&#039;"
string._htmlspecialchars_set["<"] = "&lt;"
string._htmlspecialchars_set[">"] = "&gt;"

function string.htmlspecialchars(input)
    for k, v in pairs(string._htmlspecialchars_set) do
        input = string.gsub(input, k, v)
    end
    return input
end

function string.restorehtmlspecialchars(input)
    for k, v in pairs(string._htmlspecialchars_set) do
        input = string.gsub(input, v, k)
    end
    return input
end

function string.nl2br(input)
    return string.gsub(input, "\n", "<br />")
end

function string.text2html(input)
    input = string.gsub(input, "\t", "    ")
    input = string.htmlspecialchars(input)
    input = string.gsub(input, " ", "&nbsp;")
    input = string.nl2br(input)
    return input
end

function string.split(input, delimiter)
    input = tostring(input)
    delimiter = tostring(delimiter)
    if (delimiter=='') then return false end
    local pos,arr = 0, {}
    -- for each divider found
    for st,sp in function() return string.find(input, delimiter, pos, true) end do
        table.insert(arr, string.sub(input, pos, st - 1))
        pos = sp + 1
    end
    table.insert(arr, string.sub(input, pos))
    return arr
end

function string.ltrim(input)
    return string.gsub(input, "^[ \t\n\r]+", "")
end

function string.rtrim(input)
    return string.gsub(input, "[ \t\n\r]+$", "")
end

function string.trim(input)
    input = string.gsub(input, "^[ \t\n\r]+", "")
    return string.gsub(input, "[ \t\n\r]+$", "")
end

function string.ucfirst(input)
    return string.upper(string.sub(input, 1, 1)) .. string.sub(input, 2)
end

-- utf8编码中，单字的首字节，表示该字包含了多少个字节
local function checkcount(curByte)
	local byteCount = 1
	if curByte > 0 and curByte <= 127 then
		byteCount = 1
	elseif curByte >= 192 and curByte <= 223 then
		byteCount = 2
	elseif curByte >= 224 and curByte <= 239 then
		byteCount = 3
	elseif curByte >= 240 and curByte <= 247 then
		byteCount = 4
	end

	return byteCount
end

-- 计算字数（英文字母算1个字 中文算2个字）
function string.utf8lenex(str)
	local len = 0
	for uchar in string.gmatch(str, UTF8_ZH_CN_PAT) do
		local curByte = string.byte(uchar)

		local byteCount = checkcount(curByte)
		if byteCount == 1 then
			len = len + 1
		else
			len = len + 2
		end
	end

	return len
end

--计算utf8长度(中英文都为1))
function string.utf8len(input)
    local len  = string.len(input)
    local left = len
    local cnt  = 0
    local arr  = {0, 0xc0, 0xe0, 0xf0, 0xf8, 0xfc}
    while left ~= 0 do
        local tmp = string.byte(input, -left)
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

-- 获得字符串str前len个字符，这里的每一个中文和每一个英文都是一个字符
function string.substring(str, len)
	local charList = {}
	for uchar in string.gmatch(str, UTF8_ZH_CN_PAT) do
		table.insert(charList, uchar)
	end
	if #charList <= len then
		return str
	end

	return table.concat(charList, "", 1, len)
end

-- 获得字符串str前len个字符，中文算两个，英文算一个
function string.substringex(str, maxLen)
	local retStr = ""
	local len = 0
	for uchar in string.gmatch(str, UTF8_ZH_CN_PAT) do
		local tempStr = string.format("%s%s", retStr, uchar)
		local curByte = string.byte(uchar)

		local byteCount = checkcount(curByte)
		local lenTemp = len
		if byteCount == 1 then
			lenTemp = lenTemp + 1
		else
			lenTemp = lenTemp + 2
		end

		if lenTemp > maxLen then
			return retStr, lenTemp
		end
		retStr = tempStr
		len = lenTemp
	end

	return str, len
end

--是否只有ASCII码
function string.onlyASCII(str)
for i=1, string.len(account) do
		local curByte = string.byte(account, i)
	    if curByte > 127 then
			return false
	    end
	end
    return true
end