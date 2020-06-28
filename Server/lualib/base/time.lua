--[[
    author:shenyi
    time:2018-12-27 17:36:56
]]

local timestamp = require "timestamp"
local string = string
local table = table
local math = math
local pairs = pairs
local tonumber = tonumber
time = {}

--以东12区为准
mTime_Zone = 12
--常用时间定义，秒数
mONE_MINUTE = 60
mONE_HOUR = 3600   -- 60 * 60
mONE_DAY = 86400   -- 3600 * 24
mONE_WEEK = 604800 -- 3600 * 24 * 7
mONE_YEAR = 3600 * 24 * 360
mONE_FIVEDAY = 18000

-- local a = os.date('!*t',os.time()) --0时区(格林尼治)时间
-- local b = os.date('*t',os.time())  --本地市区时间
--东12区时间与本地时间之差
local timediff = os.time(os.date("!*t", os.time())) + mTime_Zone * mONE_HOUR - os.time()

-- 2004年1月1日 0:00 星期四
local TIME_BASE = os.time({year = 2004, month = 1, day = 1, hour = 0, min = 0, second = 0}) + timediff

--秒时间戳
function time.now()
	return os.time() + timediff
end

--毫秒时间戳
function time.millisecond()
	return math.tointeger(timestamp.getmillisecond()) / 1000 + timediff
end

--微秒时间戳
function time.microsecond()
	return math.tointeger(timestamp.getmicrosecond()) / 1000000 + timediff
end

-------------------------------------------------格式化时间----------------------------------------------------
---------------------------------------------------------------------------------------------------------------
-- 获取unix格式的当前日期"2006-06-01"
function time.dayformat(Sec)
	return os.date("%Y-%m-%d", Sec)
end

-- 将秒数转成字符串 "2009-01-03 22:10:53"
function time.totalformat(Sec)
	return os.date("%Y-%m-%d %H:%M:%S", Sec)
end

--英文格式时间"Sat Dec 29 15:25:59 2018"
function time.formaten(Sec)
	return os.date("%c", Sec)
end

--中文格式时间
function time.formatcn(Sec)
	local tbl = os.date("*t", Sec)
	return string.format("%d年%d月%d日%02d:%02d:%02d", tbl.year, tbl.month, tbl.day, tbl.hour, tbl.min, tbl.sec)
end

-------------------------------------------根据给定时间按戳的特殊计算-------------------------------------------
---------------------------------------------------------------------------------------------------------------
--返回这个整点的时间戳
function time.hour()
	local t = os.date("*t", time.now())
	t.min = 0
	t.sec = 0
	return os.time(t)
end

--返回下一个整点
function time.nexhour()
	local t = os.date("*t", time.now())
	t.min = 0
	t.sec = 0
	return os.time(t) + 3600
end

--根据给点的时间戳，计算某一天开始时间的时间戳
function time.daybegintime(Time)
	local t = os.date("*t", Time)
	t.hour = 0
	t.min = 0
	t.sec = 0
	return os.time(t)
end

-- 根据给定的时间从0点已经消耗了多少秒
function time.dayelapsetime(Time)
	local timeBeginTime = time.todaybegintime(Time)
	return Time - timeBeginTime
end

-- 根据给点的时间戳，计算今天结束时间的时间戳
function time.dayendtime(Time)
	local t = os.date("*t", Time)
	t.hour = 23
	t.min = 59
	t.sec = 59
	return os.time(t)
end

-- 根据给定的时间计算离下一天还有几秒
function time.timetonextday(Time)
	local timeEndTime = time.todayendtime(Time)
	return timeEndTime - Time
end

-- 本周日的0点，周日作为一周的第一天
function time.weekbegintime(Time) 
	Time = Time or os.time()
	local t = os.date("*t", Time)
	local diff = t.sec + t.min * 60 + t.hour * 60 * 60 + (t.wday-1) * 60 * 60 * 24
	return Time - diff
end

-- 返回time是一个星期的第几天, 周日作为一周的第一天
function time.dayofweek(time) 
	local curTime = os.date("*t", time)
	return curTime.wday
end

-- 返回time是一个月的第几天
function time.dayofyear(Time)
	local t = os.date("*t", Time)
	return t.day
end

-- 返回time是一个月的第几周
function time.weekofmouth(time) 
	local curTime = os.date("*t", time)
	local tmp = curTime.day - curTime.wday
	local tmp2 = math.floor(tmp / 7)
	local tmp3 = tmp % 7
	if tmp3 == 0 then
		return tmp2 + 1
	else
		return tmp2 + 2
	end
end

-- 返回time是一年的第几个月
function time.mouthofyear(time) 
	local curTime = os.date("*t", time)
	return curTime.mouth
end

-- 是否是闰年
function time.isleapyear(year)
	return ((year % 100 == 0 and year % 400 == 0) or (year % 100 ~= 0 and year % 4 == 0))
end
--------------------------------------计算当前时间相对于2014年1月1日0：00的年月日--------------------------------
---------------------------------------------------------------------------------------------------------------

-- 返回所给时间是相对于2004年1月1日为第一天开始算起的的第几天
function time.daysince2004(Time)
	local TotalDay = 0
	local Standard = TIME_BASE		--2004年1月1日 00:00
	if not Time then
		Time = time.now()
	end
	if Time > Standard then
		TotalDay = (Time - Standard) / mONE_DAY
	else
		TotalDay = (Standard - Time) / mONE_DAY
	end
	return math.floor(TotalDay) + 1
end

-- 返回所给时间是相对于2004年1月1日为第一星期开始算起的的第几周，分割点是周一凌晨0:00
function time.weeksince2004(Time)
	local TotalWeek = 0
	local Standard = TIME_BASE + 3600*24*4		--因为2004年1月1日 0:00是星期四早上0:00，要改成星期日24:00
	Time = Time or time.now()
	if Time > Standard then
		TotalWeek = (Time - Standard) / mONE_WEEK
	else
		TotalWeek = (Standard - Time) / mONE_WEEK
	end
	return math.floor(TotalWeek + 1)
end

-- 返回所给时间是相对于2004年1月的第几个月，调用者保证 Time > TIME_BASE
function time.monthsince2004(Time)
	Time = Time or time.now()
	local Base = os.date("*t", TIME_BASE)
	local BaseYear, BaseMonth = Base.year, Base.month
	local Current = os.date("*t", Time)
	local CurYear, CurMonth = Current.year, Current.month

	return ((CurYear-BaseYear) * 12 + CurMonth)
end

----------------------------------------------根据日期获取总时间------------------------------------------------
---------------------------------------------------------------------------------------------------------------
--根据日期获得某年某月总天数
function time.daysoftime(year, mouth)
	local _curDate = {}
	_curDate.year = 2018
	_curDate.month = 13
	_curDate.day = 0
	local _maxDay = os.date("%d",os.time(_curDate))
	return _maxDay
end

----------------------------------------------根据日期获取时间戳------------------------------------------------
---------------------------------------------------------------------------------------------------------------

-- 返回下一个星期几，时，分，秒的timestamp
-- 星期天是d=1
function time.getnextdhms(d, h, m, s)
	local timetbl = os.date("*t", time.now())
	timetbl.hour = h
	timetbl.min = m
	timetbl.sec = s
	local retTime
	for i = 1, 7 do
		retTime = os.time(timetbl) + i * mONE_DAY
		if os.date("*t", retTime).wday == d then
			return retTime
		end
	end
end

--返回某年某月第几个星期几开始的时间
function time.getday(year, month, week, wday)
	local timetbl = {} -- 
	timetbl.year = year
	timetbl.month = month
	timetbl.hour = 0
	timetbl.min = 0
	timetbl.sec = 0

	local retTime
	for i = 1, 7 do
		timetbl.day = i 
		retTime = os.time(timetbl)
		if os.date("*t", retTime).wday == wday then
			break
		end
	end
	return retTime + (week - 1) * mONE_WEEK
end

