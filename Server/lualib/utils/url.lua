--[[
    author:shenyi
    time:2018-12-27 17:44:11
]]
local url = {}

local function encode_char(char)
    return "%" .. string.format("%02X", string.byte(char))
end

function url.url_encode(input)
    -- convert line endings
    input = string.gsub(tostring(input), "\n", "\r\n")
    -- escape all characters but alphanumeric, '.' and '-'
    input = string.gsub(input, "([^%w%.%- ])", encode_char)
    -- convert spaces to "+" symbols
    return string.gsub(input, " ", "+")
end

local function decode_char(h)
    return string.char(tonumber(h, 16))
end

function url.url_decode(input)
    input = string.gsub(input, "+", " ")
    input = string.gsub(input, "%%(%x%x)", decode_char)
    return string.gsub(input, "\r\n", "\n")
end

function util.url_query(q)
    local t = {}
    for k, v in pairs(q) do
        t[#t+1] = string.format("%s=%s", util.url_encode(k), util.url_encode(v))
    end
    return table.concat(t, "&")
end

return url