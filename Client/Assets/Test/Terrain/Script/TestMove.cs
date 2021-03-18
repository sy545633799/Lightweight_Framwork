using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using System;

public class TestMove : MonoBehaviour
{
	[Range(0, 10)]
	public float MoveSpeed = 3;
	[Range(0, 100)]
	public float RotateSpeed = 10f;
	[Range(0, 10)]
	public float GravitySpeed = 1;

	//public Vector2 JoySticDir { get; private set; } = Vector2.zero;
	private float interval = 0;
	private Vector3 motion = Vector3.zero;
	private CharacterController controller;
	private Animator animator;
	void Start()
    {
		controller = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		UIJoyStick.OnJoyStickTouchMove += OnJoyStickTouchMove;
		UIJoyStick.OnJoyStickTouchEnd += OnJoyStickTouchEnd;
	}

	private void OnJoyStickTouchEnd()
	{
		
	}

	private void OnJoyStickTouchMove(Vector2 tragetDir)
	{
		Move(tragetDir);
	}

	void Update()
    {
		InputManager.Update();

		float x = 0, y = 0;
		bool isGetKey = false;
		if (Input.GetKey(KeyCode.W))
		{
			y = 50;
			isGetKey = true;
		}
		if (Input.GetKey(KeyCode.S))
		{
			y = -50;
			isGetKey = true;
		}
		if (Input.GetKey(KeyCode.A))
		{
			x = -50;
			isGetKey = true;
		}
		if (Input.GetKey(KeyCode.D))
		{
			x = 50;
			isGetKey = true;
		}

		if (isGetKey)
		{
			Move(new Vector2(x, y));
		}
		else
		{
			interval -= 0.1f;
		}


		animator.SetFloat("Velocity X", motion.sqrMagnitude * interval);

	}

	private void Move(Vector2 tragetDir)
	{
		interval = 2;
		motion = MainCamera.Instance.ConvertDirByCam(tragetDir);

		float angle = Mathf.Atan2(motion.x, motion.z) * Mathf.Rad2Deg;
		Quaternion euler = Quaternion.Euler(Vector3.up * angle);
		euler = Quaternion.RotateTowards(transform.rotation, euler, RotateSpeed);
		transform.rotation = euler;

		controller.Move((motion + Vector3.down * GravitySpeed) * Time.deltaTime * MoveSpeed);

		animator.SetFloat("Velocity X", motion.sqrMagnitude);
		Shader.SetGlobalVector("_PlayerPos", transform.position);
	}
}
