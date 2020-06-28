using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System;
using System.Reflection;

namespace Game.Editor
{
    public class ExportLocation
    {
        public static string ExcelPath = "Config/Location/Location.xlsx";
        public static string AssetPath = "Assets/Art/Assets/Location/Location.asset";

        private static object GetCellValue(ICell cell)
        {
            if (cell == null) return "";

            object value = "";
            if (cell.CellType != CellType.Blank)
            {

                switch (cell.CellType)
                {
                    case CellType.Numeric:
                        if (DateUtil.IsCellDateFormatted(cell))
                            value = cell.DateCellValue;
                        else
                            value = cell.NumericCellValue;
                        break;
                    //case CellType.Boolean:
                    //    // Boolean type
                    //    value = cell.BooleanCellValue;
                    //    break;
                    //case CellType.Formula:
                    //    value = cell.NumericCellValue;
                    //    break;
                    default:
                        value = cell.StringCellValue;
                        break;
                }

            }
            return value;
        }


        [MenuItem("Tools/本地化/导出本地化资源", priority = 200)]
        public static void ExportLocationEx()
        {
            LocationAsset holder = ScriptableObject.CreateInstance<LocationAsset>();
            holder.List = new List<LocationTable>();
            string path = Application.dataPath + "/" + ExcelPath;
            if (!File.Exists(path))
            {
                Debug.LogError("沒有找到本地化excel文件");
                return;
            }
            IWorkbook wk = null;
            var extension = Path.GetExtension(path);
            string fileName = Path.GetFileName(path);

            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
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
                    Console.WriteLine(e.Message);
                }
            }

            ISheet sheet = null;
            if (wk != null)
            {
                for (var i = 0; i < wk.NumberOfSheets; i++)
                {
                    var sheet1 = wk.GetSheetAt(i);
                    if (sheet1.SheetName == "Location")
                        sheet = sheet1;
                }
            }

            if (sheet == null)
            {
                Debug.LogError("没有找到名为Location的sheet");
                return;
            }

            FieldInfo[] infos = typeof(LocationTable).GetFields();
            Dictionary<string, FieldInfo> name2Property = new Dictionary<string, FieldInfo>();
            for (int i = 0; i < infos.Length; i++)
                name2Property.Add(infos[i].Name, infos[i]);

            List<string> cellNames = new List<string>();
            for (int i = 0; i < sheet.LastRowNum + 1; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null)
                    Debug.LogError($"第{i + 1}行出错");
                else
                {
                    if (i == 0)
                        continue;
                    else if (i == 1)
                    {
                        for (int j = 0; j < row.LastCellNum; j++)
                        {
                            ICell cell = row.GetCell(j);
                            string cellName = GetCellValue(cell).ToString();
                            cellNames.Add(cellName);
                        }
                    }
                    else
                    {
                        LocationTable table = new LocationTable();
                        for (int j = 0; j < cellNames.Count; j++)
                        {
                            ICell cell = row.GetCell(j);
                            string name = cellNames[j];
                            FieldInfo info;
                            if (name2Property.TryGetValue(name, out info))
                            {
                                string cellValue = GetCellValue(cell).ToString();
                                if (j == 0)
                                {
                                    int id;
                                    if (int.TryParse(cellValue, out id))
                                        info.SetValue(table, id);
                                    else
                                        Debug.LogError($"第{i}行ID填写有误");
                                }
                                else
                                    info.SetValue(table, cellValue);
                            }
                            else
                            {
                                Debug.LogError($"代码中不包含名为{name}的字段");
                                return;
                            }
                        }
                        holder.List.Add(table);
                    }
                }
            }

            AssetDatabase.CreateAsset(holder, AssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("导出完成");
        }

        [MenuItem("Tools/本地化/切换为简体中文", priority = 201)]
        public static void ChangeSimpleChinese()
        {
            EditorPrefs.SetInt("Language", (int)SystemLanguage.ChineseSimplified);
        }

        [MenuItem("Tools/本地化/切换为繁体中文", priority = 202)]
        public static void ChangeTraditionalChinese()
        {
            EditorPrefs.SetInt("Language", (int)SystemLanguage.ChineseTraditional);
        }

        [MenuItem("Tools/本地化/切换为日语", priority = 203)]
        public static void ChangeAsJapanese()
        {
            EditorPrefs.SetInt("Language", (int)SystemLanguage.Japanese);
        }

        [MenuItem("Tools/本地化/切换为韩语", priority = 204)]
        public static void ChangeAsKorean()
        {
            EditorPrefs.SetInt("Language", (int)SystemLanguage.Korean);
        }

        [MenuItem("Tools/本地化/切换为英语", priority = 205)]
        public static void ChangeAsEnglish()
        {
            EditorPrefs.SetInt("Language", (int)SystemLanguage.English);
        }

        [MenuItem("Tools/本地化/切换为越南文", priority = 206)]
        public static void ChangeAsVietnamese()
        {
            EditorPrefs.SetInt("Language", (int)SystemLanguage.Vietnamese);
        }
    }
}