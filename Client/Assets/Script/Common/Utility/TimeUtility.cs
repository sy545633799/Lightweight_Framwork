// ========================================================
// des：
// author: 
// time：2020-09-29 11:14:05
// version：1.0
// ========================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class TimeUtility
	{
		private static DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
		private static TimeSpan beijingOffset = TimeSpan.FromHours(8);

		public static int GetTimeStampEx()
		{
			TimeSpan ts = DateTime.UtcNow - dt_1970;
			return Convert.ToInt32(ts.TotalSeconds);
		}

		public static double GetTimeStamp()
		{
			TimeSpan ts = DateTime.UtcNow - dt_1970;
			return ts.TotalSeconds;
		}

		public static double GetTimeStampByDt(DateTime dt)
		{
			TimeSpan ts = dt - dt_1970;
			return ts.TotalSeconds;
		}

		public static DateTime GetDateTime(double timestamp)
		{
			DateTime dt = dt_1970.AddTicks((long)timestamp * 10000000);
			return dt;
		}
		/// <summary>
		/// 获取北京时间信息
		/// </summary>
		/// <param name="timestamp">Utc时间戳</param>
		/// <returns></returns>
		public static DateTime GetBeiJingDateTime(double timestamp)
		{
			long time_tricks = dt_1970.Ticks + beijingOffset.Ticks + (long)timestamp * 10000000;
			DateTime dt = new DateTime(time_tricks);
			return dt;
		}

		/// <summary>
		/// 按照一个月中的第几周的第几天的某个时刻获取时间
		/// </summary>
		public static DateTime GetDateTime(int year, int month, int week, int day, int hour, int min, int second)
		{
			//本周第一个星期的第一天，有可能在上周
			DateTime firsDay = new DateTime(year, month, 1, hour, min, second, 0);
			DateTime firsSunday = firsDay.AddDays(-(int)firsDay.DayOfWeek - 1 + day);

			if (firsSunday.Month != firsDay.Month) //说明第一个周几不在这个月
				firsSunday = firsSunday.AddDays(7);
			DateTime targetDtM = firsSunday.AddDays((week - 1) * 7);
			return targetDtM;
		}


	}
}
