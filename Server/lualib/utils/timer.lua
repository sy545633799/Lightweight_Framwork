--[[
    author:shenyi
    time:2018-12-27 17:53:02
]]

local skynet = require "skynet"

local assert = assert
local string = string

local timer = {}

local routine
local routine_list = {}
local once_routine_list = {}
local day_routine_list = {}
local second_routine_list = {}

skynet.init(function()
    routine = skynet.queryservice("routine")
end)

local function gen_key(key)
    return string.format("%d_%s", skynet.self(), key)
end

function timer.add_routine(key, func, interval)
    key = gen_key(key)
    assert(not routine_list[key], string.format("Already has routine %s.", key))
    routine_list[key] = func
    skynet.call(routine, "lua", "add", skynet.self(), key, interval)
end

function timer.del_routine(key)
    key = gen_key(key)
    if routine_list[key] then
        routine_list[key] = nil
        skynet.call(routine, "lua", "del", key)
    end
end

function timer.call_routine(key)
    assert(routine_list[key], string.format("No routine %s.", key))()
end

function timer.add_once_routine(key, func, interval)
    key = gen_key(key)
    assert(not once_routine_list[key], string.format("Already has once routine %s.", key))
    once_routine_list[key] = func
    skynet.call(routine, "lua", "add_once", skynet.self(), key, interval)
end

function timer.del_once_routine(key)
    key = gen_key(key)
    if once_routine_list[key] then
        once_routine_list[key] = nil
        skynet.call(routine, "lua", "del_once", key)
    end
end

function timer.call_once_routine(key)
    local func = assert(once_routine_list[key], string.format("No once routine %s.", key))
    once_routine_list[key] = nil
    func()
end

function timer.add_day_routine(key, func)
    key = gen_key(key)
    assert(not day_routine_list[key], string.format("Already has day routine %s.", key))
    day_routine_list[key] = func
    skynet.call(routine, "lua", "add_day", skynet.self(), key)
end

function timer.del_day_routine(key)
    key = gen_key(key)
    if day_routine_list[key] then
        day_routine_list[key] = nil
        skynet.call(routine, "lua", "del_day", key)
    end
end

function timer.call_day_routine(key, od, nd, owd, nwd)
    assert(day_routine_list[key], string.format("No day routine %s.", key))(od, nd, owd, nwd)
end

function timer.add_second_routine(key, func)
    key = gen_key(key)
    assert(not second_routine_list[key], string.format("Already has second routine %s.", key))
    second_routine_list[key] = func
    skynet.call(routine, "lua", "add_second", skynet.self(), key)
end

function timer.del_second_routine(key)
    key = gen_key(key)
    if second_routine_list[key] then
        second_routine_list[key] = nil
        skynet.call(routine, "lua", "del_second", key)
    end
end

function timer.call_second_routine(key)
    assert(second_routine_list[key], string.format("No second routine %s.", key))()
end

return timer