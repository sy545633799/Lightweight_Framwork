// ========================================================
// des：
// author: shenyi
// time：2020-05-01 14:04:46
// version：1.0
// ========================================================

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game {

	public class LaunchUI
	{
		private static GameObject mLauchUI;
		private static Slider mProcess;
		public static void Init()
		{
			mLauchUI = GameObject.Find("UIRoot/LaunchUI");
			mProcess = mLauchUI.transform.Find("Root/Process").GetComponent<Slider>();
			mLauchUI.SetActive(true);
		}

		public static void ShowProcess(float process)
		{
			mProcess.value = process;
		}
		
		public static void Dispose()
		{
			if (mLauchUI)
				GameObject.Destroy(mLauchUI);

		}
	}
}
