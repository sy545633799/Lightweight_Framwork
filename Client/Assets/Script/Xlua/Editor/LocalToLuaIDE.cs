//// ========================================================
//// des：
//// author: 
//// time：2020-04-01 14:09:26
//// version：1.0
//// ========================================================

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System.IO;
//using System.Reflection;
//using UnityEditor.Callbacks;
//using System;

//public class LocalToLuaIDE : Editor
//{
//    private static readonly string[] LUA_IGNORE_LINE = { "[C]:", "[string \"PLoop/Core\"]:", "[string \"Common/Debug\"]:" };
//    private static readonly string[] CS_IGNORE_LINE = { "UnityEngine.Debug:", "UEEngine.UELog:", "UEEngine.UELogMan:" };

//    private const string EXTERNAL_EDITOR_PATH_KEY = "mTv8";
//    private const string LUA_PROJECT_ROOT_FOLDER_PATH_KEY = "obUd";
//    private static int msDefaultOpenInstanceID = -1;
//    private static int msDefaultOpenLine = -1;

//    /// <summary>
//    /// 双击console的回调
//    /// </summary>
//    /// <param name="instanceID"></param>
//    /// <param name="line"></param>
//    /// <returns></returns>
//    [OnOpenAssetAttribute(2)]
//    public static bool OnOpen(int instanceID, int line)
//    {
//        if (!GetConsoleWindowListView() || (object)EditorWindow.focusedWindow != consoleWindow)
//        {
//            return false;
//        }
//        string fileName = GetListViewRowCount(ref line);

//        if (fileName == null)
//        {
//            if (OnOpenByDefault(instanceID, line))
//            {
//                return true;
//            }
//            return false;
//        }
//        OnOpenAsset(fileName, line);

//        return true;
//    }

//    public static bool OnOpenAsset(string file, int line)
//    {
//        string filePath = file;

//        string luaFolderRoot = EditorUserSettings.GetConfigValue(LUA_PROJECT_ROOT_FOLDER_PATH_KEY);
//        if (string.IsNullOrEmpty(luaFolderRoot))
//        {
//            SetLuaProjectRoot();
//            luaFolderRoot = EditorUserSettings.GetConfigValue(LUA_PROJECT_ROOT_FOLDER_PATH_KEY);
//        }
//        filePath = luaFolderRoot.Trim() + "/" + filePath.Trim();//+ ".lua";

//        return OpenFileAtLineExternal(filePath, line);
//    }

//    static bool OpenFileAtLineExternal(string fileName, int line)
//    {
//        string editorPath = EditorUserSettings.GetConfigValue(EXTERNAL_EDITOR_PATH_KEY);
//        if (string.IsNullOrEmpty(editorPath) || !File.Exists(editorPath))
//        {   // 没有path就弹出面板设置
//            SetExternalEditorPath();
//        }
//        OpenFileWith(fileName, line);
//        return true;
//    }

//    static void OpenFileWith(string fileName, int line)
//    {
//        string editorPath = EditorUserSettings.GetConfigValue(EXTERNAL_EDITOR_PATH_KEY);
//        string projectRootPath = EditorUserSettings.GetConfigValue(LUA_PROJECT_ROOT_FOLDER_PATH_KEY);
//        System.Diagnostics.Process proc = new System.Diagnostics.Process();
//        proc.StartInfo.FileName = editorPath;
//        string procArgument = "";
//        if (editorPath.IndexOf("idea") != -1)
//        {
//            procArgument = string.Format("{0} --line {1} {2}", projectRootPath, line, fileName);
//        }
//        else
//        {
//            procArgument = string.Format("{0}:{1}:0", fileName, line);
//        }
//        proc.StartInfo.Arguments = procArgument;
//        proc.Start();
//    }

//    [MenuItem("Tools/Set Your IDE Path")]
//    static void SetExternalEditorPath()
//    {
//        string path = EditorUserSettings.GetConfigValue(EXTERNAL_EDITOR_PATH_KEY);
//        path = EditorUtility.OpenFilePanel("Select Your IDE", path, "exe");

//        if (path != "")
//        {
//            EditorUserSettings.SetConfigValue(EXTERNAL_EDITOR_PATH_KEY, path);
//            Debug.Log("Set Your IDE Path: " + path);
//        }
//    }

//    [MenuItem("Tools/Set Your Luaproject Root")]
//    static void SetLuaProjectRoot()
//    {
//        string path = EditorUserSettings.GetConfigValue(LUA_PROJECT_ROOT_FOLDER_PATH_KEY);
//        path = EditorUtility.OpenFolderPanel("Select Your Luaproject Root", path, "");

//        if (path != "")
//        {
//            EditorUserSettings.SetConfigValue(LUA_PROJECT_ROOT_FOLDER_PATH_KEY, path);
//            Debug.Log("Set Luaproject Root Path: " + path);
//        }
//    }

//    private static object consoleWindow;
//    private static object logListView;
//    private static FieldInfo logListViewCurrentRow;
//    private static MethodInfo LogEntriesGetEntry;
//    private static object logEntry;
//    private static FieldInfo logEntryCondition;
//    private static bool GetConsoleWindowListView()
//    {
//        if (logListView == null)
//        {
//            Assembly unityEditorAssembly = Assembly.GetAssembly(typeof(EditorWindow));
//            Type consoleWindowType = unityEditorAssembly.GetType("UnityEditor.ConsoleWindow");
//            FieldInfo fieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
//            consoleWindow = fieldInfo.GetValue(null);

//            if (consoleWindow == null)
//            {
//                logListView = null;
//                return false;
//            }

//            FieldInfo listViewFieldInfo = consoleWindowType.GetField("m_ListView", BindingFlags.Instance | BindingFlags.NonPublic);
//            logListView = listViewFieldInfo.GetValue(consoleWindow);
//            logListViewCurrentRow = listViewFieldInfo.FieldType.GetField("row", BindingFlags.Instance | BindingFlags.Public);

//            Type logEntriesType = unityEditorAssembly.GetType("UnityEditorInternal.LogEntries");
//            LogEntriesGetEntry = logEntriesType.GetMethod("GetEntryInternal", BindingFlags.Static | BindingFlags.Public);
//            Type logEntryType = unityEditorAssembly.GetType("UnityEditorInternal.LogEntry");
//            logEntry = Activator.CreateInstance(logEntryType);
//            logEntryCondition = logEntryType.GetField("condition", BindingFlags.Instance | BindingFlags.Public);
//        }

//        return true;
//    }
//    private static string GetListViewRowCount(ref int line)
//    {
//        string oriCondition = GetLog();
//        string condition = oriCondition;
//        condition = condition.Substring(0, condition.IndexOf('\n'));
//        int index = condition.IndexOf(".lua:");

//        if (index >= 0)
//        {
//            int start = condition.IndexOf("[");
//            int end = condition.IndexOf("]:");
//            string _line = condition.Substring(index + 5, end - index - 5); ///这里取的行数只适配tolua
//            Int32.TryParse(_line, out line);
//            return condition.Substring(start + 1, index + 3 - start);
//        }

//        index = condition.IndexOf("LuaException: [");
//        if (index >= 0)
//        {
//            return ParseLuaRowCount(condition, out line);
//        }

//        index = oriCondition.IndexOf("stack traceback:");
//        if (index >= 0)
//        {
//            string traceback = oriCondition.Substring(index);
//            string[] stracebacks = traceback.Split(new char[] { '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
//            for (int i = 1; i < stracebacks.Length; i++)
//            {
//                if (!StartWithIgnoreLine(stracebacks[i], false))
//                {
//                    return ParseLuaRowCount(stracebacks[i], out line);
//                }
//            }
//        }

//        index = condition.IndexOf(".cs:");
//        if (index >= 0)
//        {
//            int start = condition.IndexOf("[");
//            int end = condition.IndexOf("]:");
//            string _line = condition.Substring(index + 5, end - index - 5);///这里取的行数只适配tolua
//            Int32.TryParse(_line, out line);
//            return condition.Substring(start + 1, index + 2 - start);
//        }

//        return null;
//    }

//    private static bool StartWithIgnoreLine(string text, bool isDefalut)
//    {
//        string[] ignoreLines = isDefalut ? CS_IGNORE_LINE : LUA_IGNORE_LINE;
//        for (int i = 0; i < ignoreLines.Length; i++)
//        {
//            if (text.Trim().StartsWith(ignoreLines[i]))
//            {
//                return true;
//            }
//        }
//        return false;
//    }

//    private static string ParseLuaRowCount(string text, out int line)
//    {
//        int startIndex = text.IndexOf("[");
//        int endIndex = text.IndexOf("]:");
//        string fileNameStr = text.Substring(startIndex + 9, endIndex - 1 - (startIndex + 9)) + ".lua";
//        int lineTxtStartIndex = endIndex;
//        int lineTxtEndIndex = text.IndexOf(":", lineTxtStartIndex + 2);
//        string _line = text.Substring(lineTxtStartIndex + 2, lineTxtEndIndex - (lineTxtStartIndex + 2)); ///这里取的行数只适配tolua
//        Int32.TryParse(_line, out line);
//        return fileNameStr;
//    }

//    private static bool OnOpenByDefault(int instanceID, int line)
//    {
//        string fileName = GetListViewRowCountByDefault(ref line);
//        if (fileName == null)
//        {
//            return false;
//        }

//        if (msDefaultOpenInstanceID == instanceID && msDefaultOpenLine == line)
//        {
//            msDefaultOpenInstanceID = -1;
//            msDefaultOpenLine = -1;
//            return false;
//        }
//        msDefaultOpenInstanceID = instanceID;
//        msDefaultOpenLine = line;

//        OpenFileByDefault(fileName, line);
//        return true;
//    }

//    private static string GetLog()
//    {
//        int row = (int)logListViewCurrentRow.GetValue(logListView);
//        LogEntriesGetEntry.Invoke(null, new object[] { row, logEntry });
//        return logEntryCondition.GetValue(logEntry) as string;
//    }

//    private static string GetListViewRowCountByDefault(ref int line)
//    {
//        string oriCondition = GetLog();
//        int index = oriCondition.IndexOf("UnityEngine.Debug:");
//        if (index >= 0)
//        {
//            string traceback = oriCondition.Substring(index);
//            string[] stracebacks = traceback.Split(new char[] { '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
//            for (int i = 0; i < stracebacks.Length; i++)
//            {
//                if (!StartWithIgnoreLine(stracebacks[i], true))
//                {
//                    return ParseCSRowCount(stracebacks[i], out line);
//                }
//            }
//        }
//        return null;
//    }

//    private static string ParseCSRowCount(string text, out int line)
//    {
//        int startIndex = text.LastIndexOf("(at ") + 4;
//        int endIndex = text.LastIndexOf(")");
//        string fileNameAndLine = text.Substring(startIndex, endIndex - startIndex);
//        string[] splitResults = fileNameAndLine.Split(':');
//        Int32.TryParse(splitResults[1].Trim(), out line);
//        string fileName = splitResults[0];
//        return fileName;
//    }

//    public static void OpenFileByDefault(string filePath, int line)
//    {
//        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(filePath, typeof(MonoScript));
//        AssetDatabase.OpenAsset(obj, line);
//    }
//}
