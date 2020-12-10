// ========================================================
// des：
// author: 
// time：2020-12-10 09:58:19
// version：1.0
// ========================================================

using System.IO;

namespace Game.Editor
{
    public class Generate : UnityEditor.AssetModificationProcessor
    {
        /// <summary>
        /// 在资源创建时调用
        /// </summary>
        /// <param name="path">自动传入资源路径</param>
        public static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            if (!path.Contains("Assets/Script") || !path.EndsWith(".cs")) return;
            {
                string allText = "// ========================================================\r\n"
                             + "// des：\r\n"
                             + "// author: \r\n"
                             + $"// time：{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\r\n"
                             + "// version：1.0\r\n"
                             + "// ========================================================\r\n\r\n";
                             
                string[] allLines = File.ReadAllLines(path);
                bool isMono = false;
                foreach (var text in allLines)
                {
					{
	                    if (text.Contains("MonoBehaviour"))
	                    {
	                        isMono = true;
	                        if(path.Contains("Editor"))
	                            allText += "namespace Game.Editor {\r\n";
	                        else
	                            allText += "namespace Game {\r\n";
	                    }
	                    allText = allText + (isMono? "\t" : "") + text + "\r\n";
	                }
	                if(isMono)
	                    allText        += "}\r\n";
	                File.WriteAllText(path, allText);
	            }
	        }
	    }
	}
}
