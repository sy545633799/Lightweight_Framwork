// ========================================================
// des：
// author: 
// time：2021-03-04 16:28:39
// version：1.0
// ========================================================


public enum ShaderLOD
{
	None,
	Low,
	Middle,
	High,
}

public enum EffectType
{
	None,
	Liner,
	Dissolve
}

public class ShaderGUIHelper
{
	public static string[] LodNames =
			{ "(0)使用全局LOD", "(100)Low", "(200)Middel", "(300)High" };
}
