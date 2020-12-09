using System.Collections;
using System.Collections.Generic;
using GPUAnim;
using UnityEngine;
using UnityEngine.UI;
public class test : MonoBehaviour {
	public GameObject boneAnimPrefab;
	public GameObject vertexAnimPrefab;
	// public float a;
	// public float b;
	GameObject prefab;
	// public int spawnNumber = 10000;
	public Text fpsText;
	public Toggle animType;
	public Toggle supportsInstancing;
	public Button changeAnim;
	public Button addNum;
	public Button subNum;
	List<IGPUAnim> objs = new List<IGPUAnim> ();
	string[] animName;
	float deltaTime;
	float posX = 0;
	float posZ = 0;

	void Awake () {
		changeAnim.onClick.AddListener (OnchangeAnimClick);
		addNum.onClick.AddListener (OnaddNumClick);
		subNum.onClick.AddListener (OnsubNumClick);
		supportsInstancing.onValueChanged.AddListener ((bool isOn) => {
			GPUAnimManager.Instance.supportsInstancing = isOn;
		});
		animType.onValueChanged.AddListener ((bool isOn) => {
			if (isOn) animType.GetComponentInChildren<Text> ().text = "骨骼动画";
			else animType.GetComponentInChildren<Text> ().text = "顶点动画";
		});
	}

	// [ContextMenu ("test")]
	// void test04 () {

	// 	Debug.Log (a % b);

	// }
	void Start () {
		// GPUAnimManager.Instance.supportsInstancing = true;
		// Debug.Log (SystemInfo.supportsInstancing);
		// Debug.Log (SystemInfo.graphicsShaderLevel);
		// // GPUAnimManager.Instance.supportsInstancing = false;
		// for (int i = 0; i < spawnNumber; i++) {
		// 	Vector2 p = Random.insideUnitCircle * 50;
		// 	var o = GPUAnimManager.Instance.Instantiate (prefab, new Vector3 (p.x, 0, p.y), Quaternion.identity);
		// 	// var o = GameObject.Instantiate (prefab, new Vector3 (p.x, 0, p.y), Quaternion.identity);
		// 	o.Play ("run");
		// 	objs.Add (o);
		// 	if (animName == null) animName = o.GetAllAnimClipNmae ();
		// }
	}

	void OnchangeAnimClick () {
		for (int i = 0; i < objs.Count; i++) {
			objs[i].Play (animName[Random.Range (0, animName.Length - 1)], WrapMode.Loop, Random.Range (0.1f, 5f));
			// objs[i].speed = Random.Range (0.1f, 5f);
		}
	}
	void OnaddNumClick () {
		for (int i = 0; i < 100; i++) {
			Vector2 p = Random.insideUnitCircle * 50;
			// Vector2 p = new Vector2 (0, i * 4);
			if (animType.isOn) prefab = boneAnimPrefab;
			else prefab = vertexAnimPrefab;
			var o = GPUAnimManager.Instance.Instantiate (prefab, new Vector3 (p.x, 0, p.y), Quaternion.identity);
			if (animName == null) animName = o.GetAllAnimClipNmae ();
			o.Play ("run");
			objs.Add (o);
		}
	}
	void OnsubNumClick () {
		int n = Mathf.Min (objs.Count, 100);
		for (int i = 0; i < n; i++) objs[i].Destory ();
		objs.RemoveRange (0, n);
	}
	void Update () {
		// transform.Translate (new Vector3 (0, 0, 0.2f), Space.World);
		// if (objs.Count > 0) objs[0].position += new Vector3 (0, 0, 0.1f);
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		float fps = 1.0f / deltaTime;
		fpsText.text = string.Format ("帧率:{0}  小兵数量:{1}  instance:{2}  level:{3}", Mathf.Ceil (fps), objs.Count, SystemInfo.supportsInstancing, SystemInfo.graphicsShaderLevel);
		// if (Input.GetKeyDown (KeyCode.Keypad0)) {
		// 	if (objs[0].isActive) objs[0].HideSelf ();
		// 	else objs[0].ShowSelf ();
		// }
		// if (Input.GetKeyDown (KeyCode.Keypad1)) {
		// 	if (objs[1].isActive) objs[1].HideSelf ();
		// 	else objs[1].ShowSelf ();
		// }
		// if (Input.GetKeyDown (KeyCode.Keypad2)) {
		// 	if (objs[2].isActive) objs[2].HideSelf ();
		// 	else objs[2].ShowSelf ();
		// }
		// if (Input.GetKeyDown (KeyCode.Keypad3)) {
		// 	if (objs[3].isActive) objs[3].HideSelf ();
		// 	else objs[3].ShowSelf ();
		// }
		// if (Input.GetKeyDown (KeyCode.Keypad4)) {
		// 	if (objs[4].isActive) objs[4].HideSelf ();
		// 	else objs[4].ShowSelf ();
		// }
		// if (Input.GetKeyDown (KeyCode.Keypad5)) {
		// 	if (objs[5].isActive) objs[5].HideSelf ();
		// 	else objs[5].ShowSelf ();
		// }
		// if (Input.GetKeyDown (KeyCode.Keypad6)) {
		// 	if (objs[6].isActive) objs[6].HideSelf ();
		// 	else objs[6].ShowSelf ();
		// }
		// if (Input.GetKeyDown (KeyCode.Keypad7)) {
		// 	if (objs[7].isActive) objs[7].HideSelf ();
		// 	else objs[7].ShowSelf ();
		// }
		// if (Input.GetKeyDown (KeyCode.Keypad8)) {
		// 	if (objs[8].isActive) objs[8].HideSelf ();
		// 	else objs[8].ShowSelf ();
		// }
		// if (Input.GetKeyDown (KeyCode.Keypad9)) {
		// 	if (objs[9].isActive) objs[9].HideSelf ();
		// 	else objs[9].ShowSelf ();
		// }
	}
}