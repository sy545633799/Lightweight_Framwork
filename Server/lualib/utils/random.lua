--[[
    author:shenyi
    time:2018-12-27 17:58:46
]]
local rand = require "rand"
local r = rand.new(19650218)
local random = {}

function random.init(seed)
    rand.init(r, seed)
end

local function rand_num(fn, min, max)
    if not min and not max then
        return rand.randf(r)
    end
    if not max then
        min, max = 1, min
    end
    return fn(r, min, max)
end

function random.randi(min, max)
    return rand_num(rand.randi, min, max)
end

function random.randx(min, max)
    return rand_num(rand.randx, min, max)
end

return random