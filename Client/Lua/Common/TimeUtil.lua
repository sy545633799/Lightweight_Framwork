--- Created by shenyi
--- DateTime: 2019.12.30

--以东12区为准
mTime_Zone = 12
--常用时间定义，秒数
mONE_MINUTE = 60
mONE_HOUR = 3600   -- 60 * 60
mONE_DAY = 86400   -- 3600 * 24
mONE_WEEK = 604800 -- 3600 * 24 * 7
mONE_YEAR = 3600 * 24 * 360
mONE_FIVEDAY = 18000

TimeUtil = {}
-------------------------------------------------格式化时间----------------------------------------------------
---------------------------------------------------------------------------------------------------------------
-- 获取unix格式的当前日期"2006-06-01"
function TimeUtil.dayformat(Sec)
	return os.date("%Y-%m-%d", math.floor(Sec))
end

-- 将秒数转成字符串 "2009-01-03 22:10:53"
function TimeUtil.totalformat(Sec)
	return os.date("%Y-%m-%d %H:%M:%S", math.floor(Sec))
end

--英文格式时间"Sat Dec 29 15:25:59 2018"
function TimeUtil.formaten(Sec)
	return os.date("%c", math.floor(Sec))
end

--中文格式时间
function TimeUtil.formatcn(Sec)
	local tbl = os.date("*t", math.floor(Sec))
	return string.format("%d年%d月%d日%02d:%02d:%02d", tbl.year, tbl.month, tbl.day, tbl.hour, tbl.min, tbl.sec)
end

-------------------------------------------------时间转化----------------------------------------------------
---------------------------------------------------------------------------------------------------------------
-- 毫秒级时间戳转成时间
function TimeUtil.timestampToDatetime(t, format)
	if not format then
		format = "%Y%m%d%H"
	end
	return os.date(format, t)
end

function TimeUtil.timestampToSecond(t)
	return os.date('%Y-%m-%d %H:%M:%S', t)
end

function TimeUtil.timeFromSeconds(seconds)
	local t = seconds
	local day = math.floor(t/86400)
	t = t - day * 86400

	local hou = math.floor(t/3600)
	t = t - 3600 * hou

	local min = math.floor(t/60)
	t = t - 60 * min

	local sec = math.floor(t)
	return { d = day, h = hou, m = min, s = sec}
end

-- 格式如: 12:06:45，不到1小时则如：06:45
function TimeUtil.TimeToStr(seconds)
	local t = TimeUtil.timeFromSeconds(seconds)

	local timeStr = ''
	if t.h > 0 then
		timeStr = t.h + t.d * 24 .. ":"
	elseif t.d > 0 then
		timeStr = t.d * 24 .. ":"
	end
	timeStr = timeStr .. string.format("%02d", t.m) .. ":" .. string.format("%02d", t.s)
	return timeStr
end

--类似TimeToStr，大于1天返回天，小于1天大于1小时返回小时，小于小时返回分钟
function TimeUtil.TimeTooStr(seconds)
	local t = TimeUtil.timeFromSeconds(seconds)
	if t.d > 0 then
		return t.d..'天'..t.h..'小时'
	elseif t.h > 0 then
		return t.h..'小时'..t.m..'分钟'
	else
		return TimeUtil.TimeToStr(seconds)
	end
end

--类似TimeToStr，精确到分钟
function TimeUtil.TimeToStrMin(seconds,endtime)
	local t = TimeUtil.timeFromSeconds(seconds)

	local timeStr = ''
	if t.h > 0 then
		timeStr = t.h .. ":"
	elseif endtime and t.m == 0 then
		timeStr = "24:"
	else
		timeStr = "00:"
	end
	timeStr = timeStr .. string.format("%02d", t.m)
	return timeStr
end


function TimeUtil.TimeToStrCH(seconds)
	local t = t_value or TimeUtil.timeFromSeconds(seconds)
	local timeStr = ''
	if t.d > 0 then
		timeStr = t.d .. '天'
	end

	if t.h > 0 then
		timeStr = timeStr .. t.h .. '小时'
	end

	if t.m > 0 then
		timeStr = timeStr .. t.m .. '分钟'
	end

	if t.s > 0 then
		timeStr = timeStr .. t.s .. '秒'
	end
	return timeStr
end


--天
function TimeUtil.TimeToStrTian(seconds, t_value)
	local t = t_value or TimeUtil.timeFromSeconds(seconds)
	local timeStr = ''
	if t.d > 0 then
		timeStr = t.d .. '天'
	end

	return timeStr
end

--小时
function TimeUtil.TimeToStrXiaoShi(seconds)
	local t = TimeUtil.timeFromSeconds(seconds)
	local timeStr = ''
	timeStr = TimeUtil.TimeToStrTian(seconds, t)

	if t.h > 0 then
		timeStr = timeStr .. t.h .. '小时'
	end

	return timeStr
end

--分
function TimeUtil.TimeToStrFen(seconds, t_value)
	local t = t_value or TimeUtil.timeFromSeconds(seconds)
	local timeStr = ''
	timeStr = TimeUtil.TimeToStrXiaoShi(seconds, t)

	if t.m > 0 then
		timeStr = timeStr .. t.m .. '分'
	end

	return timeStr
end

--秒
function TimeUtil.TimeToStrMiao(seconds, t_value)
	local t = t_value or TimeUtil.timeFromSeconds(seconds)
	local timeStr = ''
	timeStr = TimeUtil.TimeToStrFen(seconds, t)

	if t.s > 0 then
		timeStr = timeStr .. t.s .. '秒'
	end

	return timeStr
end

--每周固定时间刷新,大于一天显示天数，小于一天显示倒计时，格式如: 12:06:45;如：每周日晚上九点刷新
function TimeUtil.FixedTimeRefresh ( weekday,targethour)
	local curTime = ConnectionManager:GetCurConnection().ServerTimestampSec
	local curDate = TimeUtil.timestampToDatetime(curTime,'*t')
	local NowMonthDayNum = tonumber(os.date("%d",os.time({year=os.date("%Y"),month=os.date("%m")+1,day=0})))
	local targetDay
	local targetMonth
	local targetYear
	local targetTime
	local NeedTime

	if curDate.wday == weekday then
		if curDate.hour < targethour then
			targetDay = curDate.day
		else
			if curDate.hour == targethour and curDate.min == 0 and curDate.sec == 0 then
				targetDay = curDate.day
			else
				targetDay = curDate.day + 7
			end
		end
	else
		if weekday < curDate.wday then
			targetDay = curDate.day + 8 - curDate.wday
		else
			targetDay = curDate.day + weekday - curDate.wday
		end
	end

	if targetDay > NowMonthDayNum then
		targetMonth = curDate.month + 1
		targetDay = targetDay - NowMonthDayNum
	else
		targetMonth = curDate.month
	end

	if targetMonth > 12 then
		targetYear = curDate.year + 1
		targetMonth = targetMonth - 12
	else
		targetYear = curDate.year
	end

	targetTime = os.time{year = targetYear,month = targetMonth,day = targetDay,hour = targethour}
	NeedTime = targetTime - curTime

	local t = TimeUtil.timeFromSeconds(NeedTime)
	if t.d >= 1 then
		return t.d
	else
		if NeedTime == 0 then
			return TimeUtil.TimeToStr(NeedTime) ,true
		else
			return TimeUtil.TimeToStr(NeedTime)
		end
	end
end

-- 每天X点刷新倒计时
function TimeUtil.FixedTimeEveryRefresh(targethour)

	local curTime = ConnectionManager:GetCurConnection().ServerTimestampSec
	local curDate = TimeUtil.timestampToDatetime(curTime,'*t')
	local targetDay
	local targetTime
	local targetMonth
	local targetYear


	if curDate.hour < targethour  then
		targetDay   = curDate.day
		targetMonth = curDate.month
		targetYear  = curDate.year
	else
		local NowMonthDayNum = tonumber(os.date("%d",os.time({year=os.date("%Y"),month=os.date("%m")+1,day=0})))
		targetDay = curDate.day + 1
		if targetDay > NowMonthDayNum then
			targetMonth = curDate.month + 1
			targetDay = targetDay - NowMonthDayNum
		else
			targetMonth = curDate.month
		end

		if targetMonth > 12 then
			targetYear = curDate.year + 1
			targetMonth = targetMonth - 12
		else
			targetYear = curDate.year
		end
	end
	targetTime = os.time{year = targetYear,month = targetMonth,day = targetDay,hour = targethour}
	local NeedTime = targetTime - curTime
	return TimeUtil.TimeToStr(NeedTime)
end