// ========================================================
// des：
// author: 
// time：2020-10-23 10:43:18
// version：1.0
// ========================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
	/// <summary>  
	/// 计时器
	/// </summary>  
	public class Timer
	{
		public bool IsActive { get; private set; }
		public bool IsPause { get; private set; }
		public bool IsFrame { get; private set; }
		public bool IsRepeat { get; private set; }
		public Action Action { get; private set; }
		private float m_DelayTime;
		private float m_RecordTime;

		public void SetTimer(float timeDelay, bool frame, bool repeat, Action callback)
		{
			IsFrame = frame;
			if (frame)
				m_DelayTime = (int)timeDelay;
			else
				m_DelayTime = timeDelay;
			IsRepeat = repeat;
			Action = callback;
		}

		public void Start()
		{
			m_RecordTime = 0;
			IsActive = true;
			IsPause = false;
		}

		public void Stop()
		{
			Action = null;
			IsPause = false;
			IsActive = false;
		}

		public void Pause() => IsPause = true;
		public void Resume() => IsPause = false;

		public void Tick()
		{
			if (!IsActive || IsPause || Action == null) return;
			if (IsFrame)
				m_RecordTime += 1;
			else
				m_RecordTime += Time.fixedDeltaTime;

			if (m_RecordTime >= m_DelayTime)
			{
				Action?.Invoke();
				if (IsRepeat)
					m_RecordTime = 0;
				else
					Stop();
			}
		}
	}

	public enum TaskType
	{
		//某个时间戳执行，执行完之后销毁
		None = 0,
		//每小时某个时间点执行
		Hourly,
		//每天某个时间点执行
		Dayly,
		//每星期某个时间点执行
		Weekly,
		//每个月某个时间点执行
		Mouthly,
		//每年某个时间点执行
		Yearly,
	}

	/// <summary>
	/// 定时器
	/// </summary>
	public class TaskInfo : RecycleObject<TaskInfo>
	{
		public bool Active;
		public TaskType TaskTaskType { get; private set; } = TaskType.None;
		public Action Action { get; private set; }
		public int Year { get; private set; }
		public int Mounth { get; private set; }
		public int Week { get; private set; }
		public int Day { get; private set; }
		public int Hour { get; private set; }
		public int Min { get; private set; }
		public int Second { get; private set; }
		public double NextStamp { get; private set; }
		public DateTime Date { get; private set; }
		public DateTime TimeInZone => Date + TimeZone.CurrentTimeZone.GetUtcOffset(Date);

		public void SetInfo(TaskType type, Action action, int year, int mounth, int week, int day, int hour, int min, int second)
		{
			TaskTaskType = type;
			Action = action;
			Year = year;
			Mounth = mounth;
			Week = week;
			Day = day;
			Hour = hour;
			Min = min;
			Second = second;
		}

		public void AddToNextLoop()
		{
			Active = true;

			//统一用北京时间时间计算
			DateTime now = TimeUtility.GetDateTime(TcpManager.TimeStamp / 1000).AddHours(8);
			switch (TaskTaskType)
			{
				case TaskType.None:
					Date = new DateTime(Year, Mounth, Day, Hour, Min, Second, 0);
					break;
				case TaskType.Hourly:
					Date = new DateTime(now.Year, now.Month, now.Day, now.Hour, Min, Second, 0);
					if (now >= Date) Date = Date.AddHours(1);
					break;
				case TaskType.Dayly:
					Date = new DateTime(now.Year, now.Month, now.Day, Hour, Min, Second, 0);
					if (now >= Date) Date = Date.AddDays(1);
					break;
				case TaskType.Weekly:
					int day = now.Day - ((int)now.DayOfWeek + 1) + Day;
					Date = new DateTime(now.Year, now.Month, day, Hour, Min, Second, 0);
					if (now >= Date) Date = Date.AddDays(7);
					break;
				case TaskType.Mouthly:
					//Week为O表示一个月的第几天
					//Week大于0表示一个月的第几周的第几天

					if (Week == 0)
						Date = new DateTime(now.Year, now.Month, Day, Hour, Min, Second, 0);
					else
						Date = TimeUtility.GetDateTime(now.Year, now.Month, Week, Day, Hour, Min, Second);
					if (now >= Date)
					{
						DateTime nextMouth = now.AddMonths(1);
						Date = TimeUtility.GetDateTime(nextMouth.Year, nextMouth.Month, Week, Day, Hour, Min, Second);
					}
					break;
				case TaskType.Yearly:
					//Week为O表示一个月的第几天
					//Week大于0表示一个月的第几周的第几天
					if (Week == 0)
						Date = new DateTime(now.Year, Mounth, Day, Hour, Min, Second, 0);
					else
						Date = TimeUtility.GetDateTime(now.Year, Mounth, Week, Day, Hour, Min, Second);
					if (now >= Date)
					{
						DateTime nextYear = now.AddYears(1);
						Date = TimeUtility.GetDateTime(nextYear.Year, Mounth, Week, Day, Hour, Min, Second);
					}
					break;
			}
			//北京时间转为格林尼治时间
			Date = Date.AddHours(-8);
			NextStamp = TimeUtility.GetTimeStampByDt(Date);
			TimeManager.AddTask((int)NextStamp, this);
		}

		public override void Downcast()
		{
			Action = null;
			Active = false;
			NextStamp = 0;
			TaskTaskType = TaskType.None;
		}
	}


	/// <summary>  
	/// 定时计时任务管理器  
	/// </summary>  
	public class TimeManager
	{
		private static ObjectPool<Timer> TimerPool = new ObjectPool<Timer>(() => new Timer(), 32);
		private static List<Timer> m_TimerList = new List<Timer>(32);
		private static List<Timer> m_FrameTimerList = new List<Timer>(32);
		private static RecyclePool<TaskInfo> m_TaskPool = new RecyclePool<TaskInfo>(() => new TaskInfo(), 32);
		private static Dictionary<int, List<TaskInfo>> m_Tasks = new Dictionary<int, List<TaskInfo>>(32);
		//延迟回收列表
		private static Dictionary<int, List<IRecycle>> m_RecycleMap = new Dictionary<int, List<IRecycle>>(32);
		/****************************************************计时器相关**************************************************/
		private static void AddTimer(float timeDelay, bool frame, bool repeat, Action callback)
		{
			Timer timer = TimerPool.Alloc();
			timer.SetTimer(timeDelay, frame, repeat, callback);
			timer.Start();
			m_TimerList.Add(timer);
		}



		public static void RemoveTimer(Timer timer)
		{
			timer.Stop();
		}

		public static void Update()
		{
			//处理帧计时器
			for (int i = m_FrameTimerList.Count - 1; i >= 0; i--)
			{
				Timer timer = m_FrameTimerList[i];
				timer.Tick();
				if (!timer.IsActive)
				{
					m_FrameTimerList.Remove(timer);
					TimerPool.Recycle(timer);
				}
			}

			//处理延迟回收
			int key = Time.frameCount;
			List<IRecycle> recycles;
			if (m_RecycleMap.TryGetValue(key, out recycles))
			{
				for (int i = 0; i < recycles.Count; i++)
				{
					recycles[i].Recycle();
				}
				m_RecycleMap.Remove(key);
				recycles.Clear();
				ListPool<IRecycle>.Recycle(recycles);
			}
		}


		public static void DelayRecycle<T>(T t, int frame)
			where T : IRecycle
		{
			List<IRecycle> recycles;
			int key = Time.frameCount + (frame < 0 ? 0 : frame);
			if (!m_RecycleMap.TryGetValue(key, out recycles))
			{
				recycles = ListPool<IRecycle>.Get();
				m_RecycleMap.Add(key, recycles);
			}
			recycles.Add(t);
		}

		/****************************************************定时器相关**************************************************/

		public static void FixedUpdate()
		{
			//处理计时器
			for (int i = m_TimerList.Count - 1; i >= 0; i--)
			{
				Timer timer = m_TimerList[i];
				timer.Tick();
				if (!timer.IsActive)
				{
					m_TimerList.Remove(timer);
					TimerPool.Recycle(timer);
				}
			}

			//处理定时器
			int time = (int)(TcpManager.TimeStamp / 1000);
			List<TaskInfo> callbacks = null;
			if (m_Tasks.TryGetValue(time, out callbacks))
			{
				for (int i = 0; i < callbacks.Count; i++)
				{
					TaskInfo task = callbacks[i];
					if (task.Active)
					{
						task.Action?.Invoke();
						if (task.TaskTaskType == TaskType.None)
							task.Recycle();
						else
							task.AddToNextLoop();
					}
					else
						task.Recycle();
				}
				callbacks.Clear();
				ListPool<TaskInfo>.Recycle(callbacks);
			}
		}

		/****************************************************************************************/
		/// <summary>
		/// 按照时间戳添加定时器
		/// </summary>
		public static void AddTask(int timeStamp, TaskInfo callback)
		{
			List<TaskInfo> actions = null;
			if (!m_Tasks.TryGetValue(timeStamp, out actions))
				actions = ListPool<TaskInfo>.Get();
			actions.Add(callback);
		}



		public static void RmoveTask(TaskInfo task)
		{
			task.Active = false;
			//不能直接回收，一定要在update里回收，防止多次触发
			//task.Recycle();
		}

		/// <summary>
		/// 每小时(的第几分第几秒), 有些需求要求每个小时的0分0秒, 这种需求不多
		/// </summary>
		public static TaskInfo AddTaskHourly(int min, int sec, Action callback)
		{
			TaskInfo task = m_TaskPool.Alloc();
			task.SetInfo(TaskType.Hourly, callback, 0, 0, 0, 0, 0, min, sec);
			task.AddToNextLoop();
			return task;
		}


		/*************************************正常时间************************************/

		/// <summary>
		/// 日常定时器(每天的第几时第几分第几秒)
		/// </summary>
		public static TaskInfo AddTaskDayly(int hour, int min, int sec, Action callback)
		{
			TaskInfo task = m_TaskPool.Alloc();
			task.SetInfo(TaskType.Dayly, callback, 0, 0, 0, 0, hour, min, sec);
			task.AddToNextLoop();
			return task;
		}

		/// <summary>
		/// 周常定时器(每周的第几天第几时第几分第几秒)
		/// </summary>
		public static TaskInfo AddTaskWeekly(int day, int hour, int min, int sec, Action callback)
		{
			TaskInfo task = m_TaskPool.Alloc();
			task.SetInfo(TaskType.Weekly, callback, 0, 0, 0, day, hour, min, sec);
			task.AddToNextLoop();
			return task;
		}

		/// <summary>
		/// 月常定时器(每月的第几天第几时第几分第几秒)
		/// </summary>
		public static TaskInfo AddTaskMounthly(int week, int day, int hour, int min, int sec, Action callback)
		{
			TaskInfo task = m_TaskPool.Alloc();
			task.SetInfo(TaskType.Mouthly, callback, 0, 0, week, day, hour, min, sec);
			task.AddToNextLoop();
			return task;
		}

		/// <summary>
		/// 年常定时器(每年第几月的第几天第几时第几分第几秒)
		/// </summary>
		public static TaskInfo AddTaskYearly(int mounth, int day, int hour, int min, int sec, Action callback)
		{
			TaskInfo task = m_TaskPool.Alloc();
			task.SetInfo(TaskType.Yearly, callback, 0, mounth, 0, day, hour, min, sec);
			task.AddToNextLoop();
			return task;
		}

		/// <summary>
		/// 年常定时器(每年第几月的第几周的第几天第几时第几分第几秒)
		/// </summary>
		public static TaskInfo AddTaskYearly(int mounth, int week, int day, int hour, int min, int sec, Action callback)
		{
			TaskInfo task = m_TaskPool.Alloc();
			task.SetInfo(TaskType.Yearly, callback, 0, mounth, week, day, hour, min, sec);
			task.AddToNextLoop();
			return task;
		}

		/// <summary>
		/// 单次触发定时器
		/// </summary>
		public static TaskInfo AddTaskOnce(int year, int mounth, int week, int day, int hour, int min, int sec, Action callback)
		{
			TaskInfo task = m_TaskPool.Alloc();
			task.SetInfo(TaskType.None, callback, year, mounth, week, day, hour, min, sec);
			task.AddToNextLoop();
			return task;
		}

		public static void Dispose()
		{
			foreach (var tasks in m_Tasks)
			{
				List<TaskInfo> taskList = tasks.Value;
				for (int i = 0; i < taskList.Count; i++)
					taskList[i].Recycle();
				ListPool<TaskInfo>.Recycle(taskList);
			}
			m_Tasks.Clear();
		}


	}
}
