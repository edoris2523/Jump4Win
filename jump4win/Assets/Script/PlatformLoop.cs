using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLoop : MonoBehaviour {

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
}
