﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class smoothPlayerController_NET : NetworkBehaviour {

	public float walkSpeed = 2;
	public float runSpeed = 6;
	public float gravity = -12;
	public float jumpHeight = 1;
	[Range(0, 1)]
	public float airControlPercent;

	public float turnSmoothTime = 0.2f;
	float turnSmoothVelocity;

	public float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	float currentSpeed;
	float velocityY;

	public bool reachedApex;

	private float platformSpd = 5f;

	AudioPlayer_NET audioPlayer;

	//Transform cameraT;
	CharacterController controller;

	void Awake()
	{
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		if(!isLocalPlayer){
			Destroy (this);
			return;
		}
		//cameraT = Camera.main.transform;
		controller = GetComponent<CharacterController> ();
		audioPlayer = GetComponent<AudioPlayer_NET> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector2 input = new Vector2(0,0);
		float xPos = Input.GetAxis ("Mouse X");
		float yPos = Input.GetAxis ("Mouse Y");
		float TouchTime;
		float TouchTime2;

		// Using Touch
		if (Input.touchCount > 0) {
			TouchTime = Time.time;


			if (Input.touches[0].phase == TouchPhase.Began) {
				TouchTime = Time.time;

			}

			if(Input.touches[0].phase == TouchPhase.Moved)
			{
				xPos = Input.touches [0].deltaPosition.x;
				yPos = Input.touches [0].deltaPosition.y;
				input = new Vector2 (xPos, yPos);
			}

			if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
			{
				if (Time.time - TouchTime <= 0.2f)
				{
					Jump();
				}
			}
		} 

		else {

			input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		}
		Vector2 inputDir = input.normalized;
		bool running = Input.GetKey (KeyCode.LeftShift);

		if(velocityY <= 0.0f){
			reachedApex = true;
		}

		Move(inputDir, running);

		if(Input.GetKeyDown(KeyCode.Space)){
			Jump ();
		}
	}

	void Move(Vector2 inputDir, bool running){
		if (inputDir != Vector2.zero) {
			//float targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
			float targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
		}

		float targetSpeed = ((running && controller.isGrounded) ? runSpeed : walkSpeed) * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);


		// Reached Apex, increase gravity
		if (reachedApex) {
			velocityY += Time.deltaTime * gravity * 3;
		} else {
			velocityY += Time.deltaTime * gravity;
		}

		Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

		controller.Move (velocity * Time.deltaTime);
		currentSpeed = new Vector2 (controller.velocity.x, controller.velocity.z).magnitude;


		// If is on the ground, set velocityY to 0
		if(controller.isGrounded){
			velocityY = 0;
			reachedApex = false;
			turnSmoothTime = 0.01f;
		} else{
			turnSmoothTime = 0.1f;
		}

		Vector3 tureVelocity = transform.forward * targetSpeed * Time.deltaTime;

		//		if (dragged) {
		//			tureVelocity += new Vector3(9f, 0f, 0f) * Time.deltaTime;
		//		} 

		transform.Translate (tureVelocity, Space.World);
	}

	void Jump(){
		if(controller.isGrounded){
			audioPlayer.CmdPlayJumpSound ();
			float jumpVelocity = Mathf.Sqrt (-2 * gravity * jumpHeight);
			velocityY = jumpVelocity;
		}
	}

	public void forcedJump(){
		float jumpVelocity = Mathf.Sqrt (-2 * gravity * jumpHeight * 3);
		velocityY = jumpVelocity;
	}

	public void forcedHighJump(){
		Debug.Log ("Try HighJump");
		float jumpVelocity = Mathf.Sqrt (-2 * gravity * jumpHeight * 5);
		velocityY = jumpVelocity;
	}

	void Dragged(){
		controller.Move (new Vector3(1f, 0f, 0f) * 1.5f * Time.deltaTime);
	}

	public void forcedMove(Vector3 dir)
	{
		Debug.Log ("ForcedMove");
		controller.Move (dir);
	}

	float GetModifiedSmoothTime(float smoothTime){
		if(controller.isGrounded){
			return smoothTime;
		}

		if(airControlPercent == 0){
			return float.MaxValue;
		}
		return smoothTime / airControlPercent;
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Belt"){
			Dragged ();
		}
		if(col.gameObject.tag == "BangBang"){
			forcedHighJump ();
		}

	}

	void OnTriggerStay(Collider col){
		if(col.gameObject.tag == "Belt"){
			Dragged ();
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.CompareTag("Platform"))
		{
			if(col.gameObject.GetComponent<PlatformLoop> ().isRight)
			{
				Debug.Log ("Playercollided, MoveRight");
				forcedMove (new Vector3(1, 0, 0) * platformSpd * Time.deltaTime);
			}
			else if(col.gameObject.GetComponent<PlatformLoop> ().isLeft)
			{
				Debug.Log ("Playercollided, MoveLeft");
				forcedMove (new Vector3(-1, 0, 0) * platformSpd * Time.deltaTime);
			}
		}
	}

	void OnCollisionStay(Collision col)
	{
		if(col.gameObject.CompareTag("Platform"))
		{
			if(col.gameObject.GetComponent<PlatformLoop> ().isRight)
			{
				Debug.Log ("Playercollided, MoveRight");
				forcedMove (new Vector3(1, 0, 0) * platformSpd * Time.deltaTime);
			}
			else if(col.gameObject.GetComponent<PlatformLoop> ().isLeft)
			{
				Debug.Log ("Playercollided, MoveLeft");
				forcedMove (new Vector3(-1, 0, 0) * platformSpd * Time.deltaTime);
			}
		}
	}

	void OnCollisionExit(Collision col)
	{
		if(col.gameObject.CompareTag("Platform"))
		{
			if(col.gameObject.GetComponent<PlatformLoop> ().isRight)
			{
				Debug.Log ("Playercollided, MoveRight");
				forcedMove (new Vector3(1, 0, 0) * platformSpd * Time.deltaTime);
			}
			else if(col.gameObject.GetComponent<PlatformLoop> ().isLeft)
			{
				Debug.Log ("Playercollided, MoveLeft");
				forcedMove (new Vector3(-1, 0, 0) * platformSpd * Time.deltaTime);
			}
		}
	}
}

