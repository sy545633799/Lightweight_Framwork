--- Created by shenyi.
--- DateTime: 2020/6/25 15:43
--- Modified from https://github.com/smilehao/xlua-framework

local co_pool = {}
local co_Timer = {}

--- 回收协程
local function __RecycleCoroutine(co)
    if not coroutine.status(co) == "suspended" then
        error("Try to recycle coroutine not suspended : "..coroutine.status(co))
    end

    table.insert(co_pool, co)
end

--- 可复用协程
local function __Coroutine(func, ...)
    local args = LuaUtil.SafePack(...)
    while func do
        func(LuaUtil.SafeUnpack(args))
        __RecycleCoroutine(coroutine.running())
        args = LuaUtil.SafePack(coroutine.yield())
        func = args[1]
        table.remove(args, 1)
    end
end

--- 获取协程
local function __GetCoroutine()
    local co = nil
    if table.length(co_pool) > 0 then
        co = table.remove(co_pool)
    else
        co = coroutine.create(__Coroutine)
    end
    return co
end

local function __PResume(co, ...)
    --local resume_ret = nil
    --if func ~= nil then
    --    resume_ret = LuaUtil.SafePack(coroutine.resume(co, ...))
    --else
    --    resume_ret = LuaUtil.SafePack(coroutine.resume(co, ...))
    --end
    local resume_ret = LuaUtil.SafePack(coroutine.resume(co, ...))
    local flag, msg = resume_ret[1], resume_ret[2]
    if not flag then
        logError(msg.."\n"..debug.traceback(co))
    elseif resume_ret.n > 1 then
        table.remove(resume_ret, 1)
    else
        resume_ret = nil
    end
    return flag, resume_ret
end

--- @func：协程函数体
--- @...：传入协程的可变参数
function coroutine.start(func, ...)
    local co = __GetCoroutine()
    __PResume(co, func, ...)
    return co
end

function coroutine.resumeex(co, ...)
    if co_Timer[co] then
        TimerUtil.RemoveTimer(co_Timer[co])
        co_Timer[co] = nil
    end
    return __PResume(co, ...)
end

--- 等待帧数，并在Update执行完毕后resume
function coroutine.waitforframes(frames)
    local co = coroutine.running()
    assert(co and type(frames) == "number" and frames >= 1 and math.floor(frames) == frames)
    co_Timer[co] = TimerUtil.DelayFrame(frames, function () coroutine.resumeex(co) end)
    return coroutine.yield()
end

--- 等待秒数，并在Update执行完毕后resume
--- 等同于Unity侧的yield return new WaitForSeconds
function coroutine.waitforseconds(seconds)
    local co = coroutine.running()
    assert(co and type(seconds) == "number" and seconds >= 0)
    co_Timer[co] = TimerUtil.Delay(seconds, function () coroutine.resumeex(co) end)
    return coroutine.yield()
end

--- 等待异步操作完成，并在Update执行完毕resume
--- 等同于Unity侧的yield return AsyncOperation
--- 注意：yield return WWW也是这种情况之一
--- @async_operation：异步句柄---或者任何带有isDone、progress成员属性的异步对象
--- @callback：每帧回调，传入参数为异步操作进度progress
function coroutine.waitforasync(async_operation, callback)
    local co = coroutine.running()
    assert(co and async_operation)
    co_Timer[co] = TimerUtil.RepeatFrame(1, function ()
        if callback then callback(async_operation.process) end
        if async_operation.isDone then coroutine.resumeex(co) end
    end)
    return coroutine.yield()
end

function coroutine.waituntil(func, ...)
    local co = coroutine.running()
    assert(co and func)
    local args = LuaUtil.SafePack(...)
    co_Timer[co] = TimerUtil.RepeatFrame(1, function () if func(LuaUtil.SafeUnpack(args)) then coroutine.resumeex(co) end end)
    return coroutine.yield()
end

--- 等待条件为假，并在Update执行完毕resume
--- 等同于Unity侧的yield return new WaitWhile
function coroutine.waitwhile(func, ...)
    local co = coroutine.running()
    assert(co and func)
    local args = LuaUtil.SafePack(...)
    co_Timer[co] = TimerUtil.RepeatFrame(1, function () if not func(LuaUtil.SafeUnpack(args)) then coroutine.resumeex(co) end end)
    return coroutine.yield()
end

function coroutine.Do(func, process, ...)
    local co = coroutine.running()
    assert(co and func)
    local operation = func(...)
    co_Timer[co] = TimerUtil.RepeatFrame(1, function ()
        if operation.IsDone or operation.isDone then
            if process then process(1) end
            coroutine.resumeex(co, operation.Result)
        else
            if process then process(operation.process) end
        end
    end)
    return coroutine.yield()
end