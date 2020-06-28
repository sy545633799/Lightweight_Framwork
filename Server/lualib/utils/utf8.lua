--[[
    author:shenyi
    time:2018-12-27 16:46:03
]]
local utf8 = {}
local pattern = "[%z\1\194-\244][\128-\191]*"
local function posrelat(start, len)
	if start < 0 then
		start = len + start + 1
	end
	return start
end

function utf8.map(str, yield, no_subs)
	local i = 0
	if no_subs then
		for b, e in str:gmatch("()" .. pattern .. "()") do
			i = i + 1
			local c = e - b
			yield(i, c, b)
		end
	else
		for b, c in str:gmatch("()(" .. pattern .. ")") do
			i = i + 1
			yield(i, c, b)
		end
	end
end

function utf8.chars(s, no_subs)
	return coroutine.wrap(function()
		return Map(s, coroutine.yield, no_subs)
	 end)
end

function utf8.len(str)
	return select(2, str:gsub("[^\128-\193]", ""))
end

function utf8.replace(str, new)
	return str:gsub(pattern, new)
end

function utf8.reverse(str)
	str = str:gsub(pattern, function(substr)
		return (#substr > 1 and substr:reverse())
	end)
	return str:reverse()
end

function utf8.strip(str)
	return str:gsub(pattern, function(substr)
		return (#substr > 1 and "")
	end)
end

function utf8.sub(str, start, finish)
	local l = utf8.len(str)
	start = posrelat(start, l)
	finish = finish and posrelat(finish, l) or l
	if start < 1 then
		start = 1
	end
	if l < finish then
		finish = l
	end
	if finish < start then
		return ""
	end
	local diff = finish - start
	local iter = Chars(str, true)
	for _ = 1, start - 1 do
		iter()
	end
	local c, b = select(2, iter())
	if diff == 0 then
		return string.sub(str, b, b + c - 1)
	end
	start = b
	for _ = 1, diff - 1 do
		iter()
	end
	c, b = select(2, iter())
	return string.sub(str, start, b + c - 1)
end

--中文, 英文， 数字
-- 0x4E00 用二进制表示为  100111000000000
-- 换成UTF-8码就是 11100100 10111000 10000000，即 228, 184, 128
-- 同理，0x9FA5为  11101001 10111110 10100101，即 233, 190, 165
function utf8.filterCN(s, cn, abc, number)
	local ss = {}
	local k = 1
	while true do
		if k > #s then break end
		local c = string.byte(s,k)
		if not c then break end
		if c<192 then
			if (c>=48 and c<=57) then  
                if number then
                  table.insert(ss, string.char(c))    
                end
            elseif (c>= 65 and c<=90) or (c>=97 and c<=122) then  
                if abc then
                  table.insert(ss, string.char(c))    
                end 
            end
			k = k + 1
		elseif c<224 then
			k = k + 2
		elseif c<240 then
			if cn then
				if c>=228 and c<=233 then
					local c1 = string.byte(s,k+1)
					local c2 = string.byte(s,k+2)
					if c1 and c2 then
						local a1,a2,a3,a4 = 128,191,128,191
						if c == 228 then a1 = 184
						elseif c == 233 then a2,a4 = 190,c1 ~= 190 and 191 or 165
						end
						if c1>=a1 and c1<=a2 and c2>=a3 and c2<=a4 then
							table.insert(ss, string.char(c,c1,c2))
						end
					end
				end
			end
			k = k + 3
		elseif c<248 then
			k = k + 4
		elseif c<252 then
			k = k + 5
		elseif c<254 then
			k = k + 6
		end
	end
	return table.concat(ss), ss
end


local function chsize(ch)
	if not ch then return 0
	elseif ch >=252 then return 6
	elseif ch >= 248 and ch < 252 then return 5
	elseif ch >= 240 and ch < 248 then return 4
	elseif ch >= 224 and ch < 240 then return 3
	elseif ch >= 192 and ch < 224 then return 2
	elseif ch < 192 then return 1
	end
end

---- 截取utf8 字符串
-- str:			要截取的字符串
-- startChar:	开始字符下标,从1开始
-- numChars:	要截取的字符长度
function utf8.utf8sub(str, startChar, numChars)
	local startIndex = 1
	while startChar > 1 do
		local char = string.byte(str, startIndex)
		startIndex = startIndex + chsize(char)
		startChar = startChar - 1
	end

	local currentIndex = startIndex

	while numChars > 0 and currentIndex <= #str do
		local char = string.byte(str, currentIndex)
		currentIndex = currentIndex + chsize(char)
		numChars = numChars -1
	end
	return str:sub(startIndex, currentIndex - 1)
end

return utf8