--- 通用积累，统一管理计时器，动画，事件
--- Created by shenyi
--- DateTime: 2019.6.7

---@public
---@class Updatable
Updatable = BaseClass("Updatable")

function Updatable:ctor()
    self.timer_callback = {}
    self.tweener_callabck = {}
    self.event_callback = {}

    if self.Update then
        self.__update_handle = UpdateBeat:CreateListener(self.Update, self)
        UpdateBeat:AddListener(self.__update_handle)
    end

    if self.LateUpdate then
        self.__lateupdate_handle = LateUpdateBeat:CreateListener(self.LateUpdate, self)
        LateUpdateBeat:AddListener(self.__lateupdate_handle)
    end

    if self.FixedUpdate then
        self.__fixedupdate_handle = FixedUpdateBeat:CreateListener(self.FixedUpdate, self)
        FixedUpdateBeat:AddListener(self.__fixedupdate_handle)
    end
end

---------------------------------------------------------计时器---------------------------------------------------
function Updatable:CallDelay(delay, callback, ...)
    --local timer_map = {}
    --local delayFunc = function()
    --    self.timer_callback[timer_map] = nil
    --    callback(timer_map.param)
    --end
    --local timer = Timer.Delay(delay, delayFunc, ...)
    --timer_map.timer = timer
    --timer_map.param = {...}
    --self.timer_callback[timer_map] = true
    --return timer_map
end

function Updatable:CallRepeat(delay, callback, ...)
    --local timer_map = {}
    --local timer = Timer.Repeat(delay, callback, ...)
    --timer_map.timer = timer
    --timer_map.param = {...}
    --self.timer_callback[timer_map] = true
    --return timer_map
end

function Updatable:RemoveTimer(timer_map)
    --timer_map.timer:Stop()
    --self.timer_callback[timer_map] = nil
end

function Updatable:RemoveAllTimer()
    --if not self.timer_callback then return end
    --for timer_map, _ in pairs(self.timer_callback) do
    --    timer_map.timer:Stop()
    --end
    --self.timer_callback = {}
end
--------------------------------------------------------动画部分(dotween)-----------------------------------------------------------
function Updatable:AddTweener(tweener, callback)
    --local onComplete = function()
    --    if callback then
    --        callback()
    --    end
    --
    --    self.tweener_callabck[tweener] = nil
    --end
    --if callback then
    --    tweener.onComplete = onComplete
    --end
end

function Updatable:RemoveTweener(tweener)
    --tweener:Kill()
    --self.tweener_callabck[tweener] = nil
end

function Updatable:RemoveAllTweener()
    --if not self.tweener_callabck then return end
    --for tweener, _ in pairs(self.tweener_callabck) do
    --    tweener:Kill()
    --end
    --self.tweener_callabck = {}
end

------------------------------------------------------事件部分-------------------------------------------------------------
function Updatable:AddEventListener(key_name, listener, handle)
    if self.event_callback[key_name] then
        print("重复添加事件名:", key_name)
        return
    end

    EventManager:AddListener(key_name, listener, handle)

    self.event_callback[key_name] = listener
end

function Updatable:RemoveEventListener(key_name)
    local info = self.event_callback[key_name]
    if not info then
        logError(string.format("没有注册名字为%s的事件", key_name))
        return
    end

    EventManager:RemoveListener(key_name, self.event_callback[key_name])
    self.event_callback[key_name] = nil
end

function Updatable:RemoveAllEventListener()
    if not self.event_callback then return end
    for key_name, listener in pairs(self.event_callback) do
        EventManager:RemoveListener(key_name, listener)
    end
    self.event_callback = {}
end

function Updatable:Broadcast(key_name, ...)
    EventManager:Broadcast(key_name, ...)
end

---暴力清空(个人辨别使用)
function Updatable:Clear()
    for k, v in pairs(self) do
        self[k] = nil;
    end
    setmetatable(self, nil)
end
---------------------------------------------------------------------------------------------------------------------------
function Updatable:dtor()
    self:RemoveAllTimer()
    self.timer_callback = nil
    self:RemoveAllTweener()
    self.tweener_callabck = nil
    self:RemoveAllEventListener()
    self.event_callback = nil
    if self.__update_handle then
        if self.__update_handle then
            UpdateBeat:RemoveListener(self.__update_handle)
        end
        if self.__lateupdate_handle then
            LateUpdateBeat:RemoveListener(self.__lateupdate_handle)
        end
        if self.__fixedupdate_handle then
            FixedUpdateBeat:RemoveListener(self.__fixedupdate_handle)
        end
    end
    self:Clear()
end