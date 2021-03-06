﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(Transform))]

public class PlatformLoop : NetworkBehaviour {

	public Transform BottomLeft;
	public Transform TopRight;
	public float spd = 5f;
	public bool isRight;
	public bool isUp;
	public bool isLeft;
	public bool isDown;

	Transform tr;

	void Start(){
		tr = GetComponent<Transform> ();
	}

	void Update(){
		if(TopRight.transform.position.x >= gameObject.transform.position.x && BottomLeft.transform.position.y >= gameObject.transform.position.y){
			isRight = true;
			isLeft = false;
			isDown = false;
			isUp = false;
		}
		// Go Up
		else if(BottomLeft.transform.position.x <= gameObject.transform.position.x && TopRight.position.y >= gameObject.transform.position.y){
			isUp = true;
			isLeft = false;
			isDown = false;
			isRight = false;
		}
		// Go Left
		else if(BottomLeft.position.x <= gameObject.transform.position.x && TopRight.position.y <=  gameObject.transform.position.y){
			isLeft = true;
			isRight = false;
			isDown = false;
			isUp = false;
		}
		// Go Down
		else if(BottomLeft.transform.position.x >= gameObject.transform.position.x && TopRight.transform.position.y <= gameObject.transform.position.y){
			isDown = true;
			isLeft = false;
			isRight = false;
			isUp = false;
		}
	}

	void FixedUpdate () {

		// Go Right
		if(isRight){
			MoveRight ();
		}
		// Go Up
		else if(isUp){
			MoveUp ();
		}
		// Go Left
		else if(isLeft){
			MoveLeft ();
		}
		// Go Down
		else if(isDown){
			MoveDown ();
		}
	}


	void MoveRight(){
		tr.Translate (spd * Time.deltaTime, 0f, 0f);	
		//tr.position = Vector3.MoveTowards(tr.position, TopRight.position, spd * Time.deltaTime);
	}

	void MoveUp(){
		tr.Translate (0f, spd * Time.deltaTime, 0f);	
	}

	void MoveLeft(){
		tr.Translate (-spd * Time.deltaTime, 0f, 0f);	
	}

	void MoveDown(){
		tr.Translate (0f, -spd * Time.deltaTime, 0f);	
	}

//	void OnCollisionEnter(Collision col)
//	{
//		if(col.gameObject.CompareTag("Player"))
//		{
//			if(isRight)
//			{
//				Debug.Log ("Playercollided, MoveRight");
//				col.gameObject.GetComponent<smoothPlayerController_NET> ().forcedMove (new Vector3(1, 0, 0) * spd * Time.deltaTime);
//			}
//			else if(isLeft)
//			{
//				Debug.Log ("Playercollided, MoveLeft");
//				col.gameObject.GetComponent<smoothPlayerController_NET> ().forcedMove (new Vector3(-1, 0, 0) * spd * Time.deltaTime);
//			}
//		}
//	}
//
//	void OnCollisionStay(Collision col)
//	{
//		if(col.gameObject.CompareTag("Player"))
//		{
//			if(isRight)
//			{
//				Debug.Log ("Playercollided, MoveRight");
//				col.gameObject.GetComponent<smoothPlayerController_NET> ().forcedMove (new Vector3(1, 0, 0) * spd * Time.deltaTime);
//			}
//			else if(isLeft)
//			{
//				Debug.Log ("Playercollided, MoveLeft");
//				col.gameObject.GetComponent<smoothPlayerController_NET> ().forcedMove (new Vector3(-1, 0, 0) * spd * Time.deltaTime);
//			}
//		}
//	}
//
//	void OnCollisionExit(Collision col)
//	{
//		if(col.gameObject.CompareTag("Player"))
//		{
//			if(isRight)
//			{
//				Debug.Log ("Playercollided, MoveRight");
//				col.gameObject.GetComponent<smoothPlayerController_NET> ().forcedMove (new Vector3(1, 0, 0) * spd * Time.deltaTime);
//			}
//			else if(isLeft)
//			{
//				Debug.Log ("Playercollided, MoveLeft");
//				col.gameObject.GetComponent<smoothPlayerController_NET> ().forcedMove (new Vector3(-1, 0, 0) * spd * Time.deltaTime);
//			}
//		}
//	}
}
