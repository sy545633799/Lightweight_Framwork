// ========================================================
// des: 
// author: shenyi
// time：2020-12-29 14:46:57
// version：1.0
// ========================================================

using UnityEngine;
using UnityEditor;
using System.IO;

namespace Game.Editor {
	public class TimeLinePlayableCreateEditor : EditorWindow
	{
		[MenuItem("Tools/TimeLine/Create Playable Template")]
		private static void OpenWindow()
			=> GetWindow<TimeLinePlayableCreateEditor>("创建新的TimeLineTrack模板");
		public string m_trackName = "TrackName";
		public string m_bindingType = "BindingType";
		public Color m_trackColor = Color.white;
		public bool m_haveBindingType = true;
		private void OnGUI()
		{
			GUILayout.BeginVertical("box");
			GUILayout.BeginHorizontal("box");
			GUILayout.Label("Track Name: ");
			m_trackName = GUILayout.TextField(m_trackName);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal("box");
			GUILayout.Label("Have Binding Type :");
			m_haveBindingType = EditorGUILayout.Toggle(m_haveBindingType);
			GUILayout.EndHorizontal();
			if (m_haveBindingType)
			{
				GUILayout.BeginHorizontal("box");
				GUILayout.Label("Binding Type: ");
				m_bindingType = GUILayout.TextField(m_bindingType);
				GUILayout.EndHorizontal();
			}
			GUILayout.BeginHorizontal("box");
			GUILayout.Label("Track Color: ");
			m_trackColor = EditorGUILayout.ColorField(m_trackColor);
			GUILayout.EndHorizontal();
			if (GUILayout.Button("Create Template"))
				ScriptCreater.CreateTrackTemplate(m_trackName, m_bindingType, m_trackColor, m_haveBindingType);
			GUILayout.EndVertical();
		}
	}

	public class ScriptCreater
	{
		public static void CreateTrackTemplate(string trackName, string bindingType, Color color, bool haveBindingType)
		{
			string _folderPath = $"{Application.dataPath}/Script/Timeline/{trackName}Track";
			if (!Directory.Exists(_folderPath))
				Directory.CreateDirectory(_folderPath);
			using (StreamWriter _steam = File.CreateText($"{_folderPath}/{trackName}Track.cs"))
			{
				_steam.WriteLine("using System;");
				_steam.WriteLine("using UnityEngine;");
				_steam.WriteLine("using UnityEngine.Playables;");
				_steam.WriteLine("using UnityEngine.Timeline;");
				_steam.WriteLine("");
				_steam.WriteLine("namespace Game");
				_steam.WriteLine("{");
				_steam.WriteLine($"    [Serializable]");
				_steam.WriteLine($"    [TrackClipType(typeof({trackName}Shot))]");
				if (haveBindingType)
					_steam.WriteLine($"    [TrackBindingType(typeof({bindingType}))]");
				_steam.WriteLine($"    [TrackColor({color.r}f, {color.g}f, {color.b}f)]");
				_steam.WriteLine($"    public class {trackName}Track : TrackAsset");
				_steam.WriteLine("    {");
				_steam.WriteLine($"        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject owner, int inputCount)");
				_steam.WriteLine("        {");
				_steam.WriteLine($"            ScriptPlayable<{trackName}Mixer> behaviour = ScriptPlayable<{trackName}Mixer>.Create(graph, inputCount);");
				_steam.WriteLine($"            return behaviour;");
				_steam.WriteLine("        }");
				_steam.WriteLine("    }");
				_steam.WriteLine("}");
				_steam.Close();
			}
			using (StreamWriter _steam = File.CreateText($"{_folderPath}/{trackName}Shot.cs"))
			{
				_steam.WriteLine("using System;");
				_steam.WriteLine("using UnityEngine;");
				_steam.WriteLine("using UnityEngine.Playables;");
				_steam.WriteLine("");
				_steam.WriteLine("namespace Game");
				_steam.WriteLine("{");
				_steam.WriteLine($"    [Serializable]");
				_steam.WriteLine($"    public class {trackName}Shot : PlayableAsset");
				_steam.WriteLine("    {");
				_steam.WriteLine($"        public {trackName}ShotPlayable template = new {trackName}ShotPlayable();");
				_steam.WriteLine($"        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)");
				_steam.WriteLine("        {");
				_steam.WriteLine($"            var playable = ScriptPlayable<{trackName}ShotPlayable>.Create(graph, template);");
				_steam.WriteLine($"            return playable;");
				_steam.WriteLine("        }");
				_steam.WriteLine("    }");
				_steam.WriteLine("}");
				_steam.Close();
			}
			using (StreamWriter _steam = File.CreateText($"{_folderPath}/{trackName}ShotPlayable.cs"))
			{
				_steam.WriteLine("using System;");
				_steam.WriteLine("using UnityEngine;");
				_steam.WriteLine("using UnityEngine.Playables;");
				_steam.WriteLine("");
				_steam.WriteLine("namespace Game");
				_steam.WriteLine("{");
				_steam.WriteLine($"    [Serializable]");
				_steam.WriteLine($"    public class {trackName}ShotPlayable : PlayableBehaviour");
				_steam.WriteLine("    {");
				_steam.WriteLine("    }");
				_steam.WriteLine("}");
				_steam.Close();
			}
			using (StreamWriter _steam = File.CreateText($"{_folderPath}/{trackName}Mixer.cs"))
			{
				_steam.WriteLine("using System;");
				_steam.WriteLine("using UnityEngine;");
				_steam.WriteLine("using UnityEngine.Playables;");
				_steam.WriteLine("");
				_steam.WriteLine("namespace Game");
				_steam.WriteLine("{");
				_steam.WriteLine($"    [Serializable]");
				_steam.WriteLine($"    public class {trackName}Mixer : PlayableBehaviour");
				_steam.WriteLine("    {");
				if (haveBindingType)
					_steam.WriteLine($"        public {bindingType} m_{bindingType};");
				_steam.WriteLine($"        public override void ProcessFrame(Playable playable, FrameData info, object playerData)");
				_steam.WriteLine("        {");
				if (haveBindingType)
				{
					_steam.WriteLine($"            if (m_{bindingType} == null)");
					_steam.WriteLine($"                m_{bindingType} = ({bindingType})playerData;;");
					_steam.WriteLine($"            if (m_{bindingType} == null)");
					_steam.WriteLine($"                return;");
				}
				_steam.WriteLine($"            int inputCount = playable.GetInputCount();");
				_steam.WriteLine($"            for (int i = 0; i < inputCount; i++)");
				_steam.WriteLine("            {");
				_steam.WriteLine($"                float _weight = playable.GetInputWeight(i);");
				_steam.WriteLine($"                ScriptPlayable<{trackName}ShotPlayable> _shotPlayable = (ScriptPlayable<{trackName}ShotPlayable>)playable.GetInput(i);");
				_steam.WriteLine($"                {trackName}ShotPlayable _shotbehaviour = _shotPlayable.GetBehaviour();");
				_steam.WriteLine($"                float normalizedTime = (float)(_shotPlayable.GetTime() / _shotPlayable.GetDuration());");
				_steam.WriteLine("            }");
				_steam.WriteLine("        }");
				_steam.WriteLine("    }");
				_steam.WriteLine("}");
				_steam.Close();
			}
			AssetDatabase.Refresh();
		}
	}
}
