// ========================================================
// des：
// author: 
// time：2020-09-21 20:34:31
// version：1.0
// ========================================================

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Game.Editor {
	
	public class ExcelReader
	{

		public static Dictionary<string, List<Dictionary<string, ExcelObjectElement>>> ReadExcel(string excelPath)
		{
			if (!File.Exists(excelPath))
			{
				Debug.LogError("沒有找到本地化excel文件");
				return null;
			}
			IWorkbook wk = null;
			var extension = Path.GetExtension(excelPath);
			string fileName = Path.GetFileName(excelPath);

			using (FileStream fs = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				try
				{
					if (extension != null)
					{
						if (extension.Equals(".xls"))
							wk = new HSSFWorkbook(fs);
						else
							wk = new XSSFWorkbook(fs);
					}
				}
				catch (Exception e)
				{
					Debug.LogError(e.Message);
				}
			}
			Dictionary<string, List<Dictionary<string, ExcelObjectElement>>> result = new Dictionary<string, List<Dictionary<string, ExcelObjectElement>>>();
			for (var k = 0; k < wk.NumberOfSheets; k++)
			{
				ISheet sheet = wk.GetSheetAt(k);
				result[sheet.SheetName] = new List<Dictionary<string, ExcelObjectElement>>();
				IRow nameRow = sheet.GetRow(1);
				IRow typeRow = sheet.GetRow(2);
				for (int i = 3; i < sheet.LastRowNum + 1; i++)
				{
					IRow row = sheet.GetRow(i);
					if (row == null)
						Debug.LogError($"{sheet.SheetName} 第{i + 1}行出错");
					else
					{
						Dictionary<string, ExcelObjectElement> dic = new Dictionary<string, ExcelObjectElement>();
						for (int j = 0; j < nameRow.Cells.Count; j++)
						{
							ExcelObjectElement element = new ExcelObjectElement();
							if (typeRow.GetCell(j) == null || nameRow.GetCell(j) == null)
							{
								
								continue;
							}
							element.FieldType = typeRow.GetCell(j).ToString();
							if (row.GetCell(j) != null)
								element.FieldValue = row.GetCell(j).ToString();
							else
								element.FieldValue = string.Empty;
							dic.Add(nameRow.GetCell(j).ToString(), element);
						}
						result[sheet.SheetName].Add(dic);
					}
				}
			}

			return result;
		}
	}
}
