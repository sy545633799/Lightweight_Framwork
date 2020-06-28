--- Created by shenyi.
--- DateTime: 2020/6/25 15:43
--- Modified from https://github.com/smilehao/xlua-framework

local Timer = BaseClass("Timer")

-- 构造函数
function Timer:ctor(delay, func, handle, one_shot, use_frame, unscaled)
    self.target = {}
    if delay and func then
        self:Init(delay, func, handle, one_shot, use_frame, unscaled)
    end
end

-- Init
function Timer:Init(delay, func, handle, one_shot, use_frame, unscaled)
    assert(type(delay) == "number" and delay >= 0)
    assert(func ~= nil)
    -- 时长，秒或者帧
    self.delay = delay
    -- 回调函数
    self.target.func = func
    -- 回传对象，一般作为回调函数第一个self参数
    self.target.handle = handle
    -- 是否是一次性计时
    self.one_shot = one_shot
    -- 是否是帧定时器，否则为秒定时器
    self.use_frame = use_frame
    -- 使用deltaTime计时，还是采用unscaledDeltaTime计时
    self.unscaled = unscaled
    -- 是否已经启用
    self.started = false
    -- 倒计时
    self.left = delay
    -- 是否已经结束
    self.over = false
    -- 传入对象是否为空
    self.obj_not_nil = handle and true or false
    -- 启动定时器时的帧数
    self.start_frame_count = Time.frameCount
end

-- Update
function Timer:Update(is_fixed)
    if not self.started or self.over then
        return
    end
    local timeup = false
    if self.use_frame then
        -- TODO：这里有个经常会落后一帧的问题，一般出现在协程当中--当协程启用另外的协程时
        -- 协程不做精确定时，一般用于异步等待或者分帧操作，所以这里暂时没有什么影响，后面看是否需要修改
        timeup = (Time.frameCount >= self.start_frame_count + self.delay)
    else
        local delta = nil
        if is_fixed then
            delta = Time.fixedDeltaTime
        else
            delta = not self.unscaled and Time.deltaTime or Time.unscaledDeltaTime
        end
        self.left = self.left - delta
        timeup = (self.left <= 0)
    end

    if timeup then
        if self.target.func ~= nil then
            -- 说明：这里一定要先改状态，后回调
            -- 如果回调手动删除定时器又马上再次获取，则可能得到的是同一个定时器，再修改状态就不对了
            -- added by wsh @ 2018-01-09：TimerManager已经被重构，不存在以上问题，但是这里的代码不再做调整
            if not self.one_shot then
                if not self.use_frame then
                    -- 说明：必须把上次计时“欠下”的时间考虑进来，否则会有误差
                    self.left = self.delay + self.left
                end
                self.start_frame_count = Time.frameCount
            else
                self.over = true
            end
            -- 说明：运行在保护模式，有错误也只是停掉定时器，不要让客户端挂掉
            local status, err
            if self.obj_not_nil then
                status, err = pcall(self.target.func, self.target.handle)
            else
                status, err = pcall(self.target.func)
            end
            if not status then
                self.over = true
                logError(err)
            end
        else
            self.over = true
        end
    end
end

-- 启动计时
function Timer:Start()
    if self.over then
        logError("You can't start a overed timer, try add a new one!")
    end
    if not self.started then
        self.left = self.delay
        self.started = true
        self.start_frame_count = Time.frameCount
    end
end

-- 暂停计时
function Timer:Pause()
    self.started = false
end

-- 恢复计时
function Timer:Resume()
    self.started = true
end

-- 停止计时
function Timer:Stop()
    self.left = 0
    self.one_shot = false
    self.target.func = nil
    self.target.handle = nil
    self.use_frame = false
    self.unscaled = false
    self.started = false
    self.over = true
end

-- 复位：如果计时器是启动的，并不会停止，只是刷新倒计时
function Timer:Reset()
    self.left = self.delay
    self.start_frame_count = Time.frameCount
end

-- 是否已经完成计时
function Timer:IsOver()
    if self.target.func == nil then
        return true
    end
    return self.over
end

-----------------------------------------------------------------------------------------------------------------------
local update_handle = nil
local update_timer = {}
local pool = {}

TimerUtil = {}

-- 延后回收定时器，必须全部更新完毕再回收，否则会有问题
local function DelayRecycle(timers)
    for timer,_ in pairs(timers) do
        if timer:IsOver() then
            timer:Stop()
            table.insert(pool, timer)
            timers[timer] = nil
        end
    end
end

-- 获取定时器
local function InnerGetTimer(delay, func, handle, one_shot, use_frame, unscaled)
    local timer = nil
    if table.length(pool) > 0 then
        timer = table.remove(pool)
        if delay and func then
            timer:Init(delay, func, handle, one_shot, use_frame, unscaled)
        end
    else
        timer = Timer.New(delay, func, handle, one_shot, use_frame, unscaled)
    end
    return timer
end

-- 延迟执行
function TimerUtil.Delay(seconds, func, handle)
    local timer = InnerGetTimer(seconds, func, handle, true, false, false)
    update_timer[timer] = true
    timer:Start()
    return timer
end

function TimerUtil.DelayFrame(seconds, func, handle)
    local timer = InnerGetTimer(seconds, func, handle, true, true, false)
    update_timer[timer] = true
    timer:Start()
    return timer
end

-- 重复执行
function TimerUtil.Repeat(seconds, func, handle)
    local timer = InnerGetTimer(seconds, func, handle, false, false, false)
    update_timer[timer] = true
    timer:Start()
    return timer
end

function TimerUtil.RepeatFrame(seconds, func, handle)
    local timer = InnerGetTimer(seconds, func, handle, false, true, false)
    update_timer[timer] = true
    timer:Start()
    return timer
end

function TimerUtil.RemoveTimer(timer)
    timer:Stop()
end
---------------------------------------------------------UpdateHandle---------------------------------------------------
-- Update回调
local function UpdateHandle()
    for timer,_ in pairs(update_timer) do
        timer:Update(false)
    end
    DelayRecycle(update_timer)
end

if not update_handle then
    update_handle = UpdateBeat:CreateListener(UpdateHandle)
    UpdateBeat:AddListener(update_handle)
end
------------------------------------------------------------------------------------------------------------------------

-- 清理：可用在场景切换前，不清理关系也不大，只是缓存池不会下降
function TimerUtil.Cleanup()
    for timer,_ in pairs(update_timer) do
        timer:Stop()
        update_timer[timer] = nil
    end
    update_timer = {}
    pool = {}
end
