using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGPUAnim {

	Vector3 position { get; set; }
	Quaternion rotation { get; set; }
	Vector3 scale { get; set; }
	float speed { get; set; }
	int _index { get; set; }
	bool isActive { get; set; }
	void Play (string animName, WrapMode wrap = WrapMode.Loop, float speed = 1f, float startTime = 0f);
	void Stop ();
	void Destory ();
	void HideSelf ();
	void ShowSelf ();
	bool IsPlaying (string name);
	string[] GetAllAnimClipNmae ();

}