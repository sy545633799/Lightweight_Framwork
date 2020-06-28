--- Created by shenyi
--- DateTime: 2019.12.30

local Log = CS.UnityEngine.Debug

--输出日志--
function log(str)
    Log.Log(tostring(str) .. '\n' .. debug.traceback());
end

--错误日志--
function logError(str)
    Log.LogError(tostring(str) .. '\n' .. debug.traceback());
end

--警告日志--
function logWarn(str)
    Log.LogWarning(tostring(str) .. '\n' .. debug.traceback());
end