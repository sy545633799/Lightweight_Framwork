// ========================================================
// des：
// author: 
// time：2020-07-15 14:54:40
// version：1.0
// ========================================================
using System.Text.RegularExpressions;

namespace Game
{
	public static class StringExtension
	{
		public static string GetEnglishName(this string str)
		{
			string pattern = "[A-Za-z]";
			string itemClassName = "";
			MatchCollection results = Regex.Matches(str, pattern);
			foreach (var v in results)
			{
				itemClassName += v.ToString();
			}

			return itemClassName;
		}

		public static string GetNameNoBrackets(this string str)
		{
			return Regex.Replace(str, @"\([^\(]*\)", "").Trim();
		}

		/// <summary>
		/// 获取路径中最后一部分的名称（文件名或文件夹名）。
		/// </summary>
		/// <param name="path">文件路径或文件夹路径</param>
		/// <returns></returns>
		public static string GetLastPartNameOfPath(this string path)
		{
			// 截取最后一部分名称，名称的末尾可能带有多个斜杠(/)或反斜杠(\)
			var pattern = @"[^/\\]+[/\\]*$";
			var match = System.Text.RegularExpressions.Regex.Match(path, pattern);
			var name = match.Value;

			// 截取名称中不带斜杠(/)或反斜杠(\)的部分
			pattern = @"[^/\\]+";
			match = System.Text.RegularExpressions.Regex.Match(name, pattern);
			name = match.Value;

			return name;
		}
	}
}