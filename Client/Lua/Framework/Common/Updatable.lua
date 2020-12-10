--- 通用积累，统一管理计时器，动画，事件
--- Created by shenyi
--- DateTime: 2019.6.7

---@public
---@class Updatable:BaseClass
Updatable = BaseClass("Updatable")
local eventManager = EventManager

function Updatable:ctor()
    self.timer_callback = {}
    ---@type table<DG.Tweening.Tweener, table>
    self.tweener_callabck = {}
    self.event_callback = {}
end

---------------------------------------------------------计时器---------------------------------------------------

---CallDelay 一次性延迟函数
---@param delay number 延迟几秒
---@param callback function 延迟回调
---@param handle table 持有者
function Updatable:CallDelay(delay, callback, handle)
    local timer_map = {}
    local delayFunc = function()
        self.timer_callback[timer_map] = nil
        coroutine.start(function ()
            if handle then
                callback(handle)
            else
                callback()
            end
        end)
    end
    local timer = TimerUtil.Delay(delay, delayFunc, handle)
    timer_map.timer = timer
    self.timer_callback[timer_map] = true
    return timer_map
end

---CallRepeat 重复性延迟函数
---@param delay number 延迟几秒
---@param callback function 延迟回调
---@param handle table 持有者
---@param excuteImmediately boolean 是否立即执行
function Updatable:CallRepeat(delay, callback, handle, excuteImmediately)
    if excuteImmediately then callback(handle) end
    local timer_map = {}
    local delayFunc = function()
        ---repeat里不要开协程
        --coroutine.start(function ()
        if handle then
            callback(handle)
        else
            callback()
        end
        --end)
    end
    local timer = TimerUtil.Repeat(delay, delayFunc, handle)
    timer_map.timer = timer
    self.timer_callback[timer_map] = true
    return timer_map
end

function Updatable:CallUntil(delay, condition, callback, handle)
    local timer_map = {}
    local delayFunc = function()
        if condition() then
            self:RemoveTimer(timer_map)
            coroutine.start(function()
                if handle then
                    callback(handle)
                else
                    callback()
                end
            end)
        end
    end
    local timer = TimerUtil.Repeat(delay, delayFunc)
    timer_map.timer = timer
    self.timer_callback[timer_map] = true
    return timer_map
end

---CallDelay 一次性延迟函数
---@param delay number 延迟几秒
---@param callback function 延迟回调
---@param handle table 持有者
function Updatable:CallDelayFrame(delay, callback, handle)
    local timer_map = {}
    local delayFunc = function()
        self.timer_callback[timer_map] = nil
        coroutine.start(function ()
            if handle then
                callback(handle)
            else
                callback()
            end
        end)
    end
    local timer = TimerUtil.DelayFrame(delay, delayFunc, handle)
    timer_map.timer = timer
    self.timer_callback[timer_map] = true
    return timer_map
end

---CallRepeat 重复性延迟函数
---@param delay number 延迟几秒
---@param callback function 延迟回调
---@param handle table 持有者
---@param excuteImmediately boolean 是否立即执行
function Updatable:CallRepeatFrame(delay, callback, handle, excuteImmediately)
    if excuteImmediately then callback(handle) end
    local timer_map = {}
    local delayFunc = function()
        ---repeat里不要开协程
        --coroutine.start(function ()
        if handle then
            callback(handle)
        else
            callback()
        end
        --end)
    end
    local timer = TimerUtil.RepeatFrame(delay, delayFunc, handle)
    timer_map.timer = timer
    self.timer_callback[timer_map] = true
    return timer_map
end

function Updatable:RemoveTimer(timer_map)
    timer_map.timer:Stop()
    self.timer_callback[timer_map] = nil
end

function Updatable:RemoveAllTimer()
    if not self.timer_callback then return end
    for timer_map, _ in pairs(self.timer_callback) do
        timer_map.timer:Stop()
    end
    self.timer_callback = {}
end
--------------------------------------------------------动画部分(dotween)-----------------------------------------------------------
---@protected
---@param tweener DG.Tweening.Tweener
function Updatable:AddTweener(tweener, ease, callback, handle)
    if not tweener then
        return
    end
    local onComplete = function()
        if callback then
            callback(self.tweener_callabck[tweener].handle)
        end
        self.tweener_callabck[tweener] = nil
    end
    if ease then tweener:SetEase(ease) end
    if callback then tweener.onComplete = onComplete end

    self.tweener_callabck[tweener] = { callback = callback , handle = handle }
end

---@param tweener DG.Tweening.Tweener
function Updatable:RemoveTweener(tweener)
    if tweener then
        tweener:Kill()
        self.tweener_callabck[tweener] = nil
    end
end

function Updatable:PauseAllTweener()
    if not self.tweener_callabck then return end
    for tweener, _ in pairs(self.tweener_callabck) do
        tweener:Pause()
    end
end

function Updatable:ResumeAllTweener()
    if not self.tweener_callabck then return end
    for tweener, _ in pairs(self.tweener_callabck) do
        tweener:Play()
    end
end

function Updatable:RemoveAllTweener()
    if not self.tweener_callabck then return end
    for tweener, _ in pairs(self.tweener_callabck) do
        tweener:Kill()
    end
    self.tweener_callabck = {}
end

------------------------------------------------------事件部分-------------------------------------------------------------
function Updatable:AddEventListener(key_name, listener, handle)
    if self.event_callback[key_name] then
        logError("重复添加事件名:", key_name)
        return
    end
    eventManager:AddListener(key_name, listener, handle)

    self.event_callback[key_name] = listener
end

function Updatable:RemoveEventListener(key_name)
    local info = self.event_callback[key_name]
    if not info then
        logError(string.format("没有注册名字为%s的事件", key_name))
        return
    end

    eventManager:RemoveListener(key_name, self.event_callback[key_name])
    self.event_callback[key_name] = nil
end

function Updatable:RemoveAllEventListener()
    if not self.event_callback then return end
    for key_name, listener in pairs(self.event_callback) do
        eventManager:RemoveListener(key_name, listener)
    end
    self.event_callback = {}
end

function Updatable:Broadcast(key_name, ...)
    eventManager:Broadcast(key_name, ...)
end

---暴力清空(个人辨别使用)
function Updatable:Clear()
    for k, v in pairs(self) do
        self[k] = nil;
    end
    setmetatable(self, nil)
    --log(tostring(self))
    self.release = true
end
---------------------------------------------------------------------------------------------------------------------------
function Updatable:dtor()
    self:RemoveAllTimer()
    self.timer_callback = nil
    self:RemoveAllTweener()
    self.tweener_callabck = nil
    self:RemoveAllEventListener()
    self.event_callback = nil
    self:Clear()
end