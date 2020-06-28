using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public static class Extensions
{
	public static Vector2 ToVector2(this Vector3 vec)
	{
		return new Vector2(vec.x, vec.y);
	}

	public static byte ToByte(this string val)
	{
		byte ret = 0;
		try
		{
			if (!String.IsNullOrEmpty(val))
			{
				ret = Convert.ToByte(val);
			}
		}
		catch (Exception)
		{
		}
		return ret;
	}

	public static long ToInt64(this string val)
	{
		long ret = 0;
		try
		{
			if (!String.IsNullOrEmpty(val))
			{
				ret = Convert.ToInt64(val);
			}
		}
		catch (Exception)
		{
		}
		return ret;
	}

	public static float ToFloat(this string val)
	{
		float ret = 0;
		try
		{
			if (!String.IsNullOrEmpty(val))
			{
				ret = Convert.ToSingle(val);
			}
		}
		catch (Exception)
		{
		}
		return ret;
	}

	static public Int32 ToInt32(this string str)
	{
		Int32 ret = 0;

		try
		{
			if (!String.IsNullOrEmpty(str))
			{
				ret = Convert.ToInt32(str);
			}
		}
		catch (Exception)
		{
		}
		return ret;
	}

	public static Int32 ToInt32(this object obj)
	{
		Int32 ret = 0;
		try
		{
			if (obj != null)
			{
				ret = Convert.ToInt32(obj);
			}
		}
		catch (Exception)
		{
		}

		return ret;
	}

	static public string ToString(this object obj)
	{
		var ret = "";
		if (obj != null)
			ret = obj.ToString();
		return ret;
	}
}