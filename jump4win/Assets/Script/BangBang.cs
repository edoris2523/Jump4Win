using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BangBang : MonoBehaviour {

	public float targetSpeed;

	void Start(){
		targetSpeed = 10f;
	}

	void OnCollisionEnter(Collision col){
		//col.transform.position += transform.up * targetSpeed * Time.deltaTime;
		//col.transform.Translate (transform.up * targetSpeed * Time.deltaTime, Space.World);
		Debug.Log ("BangBang Collision_enter");
	}

	void OnCollisionStay(Collision col){
		//col.rigidbody.AddForce (new Vector3(0, 10000, 0), ForceMode.Impulse);
		//col.transform.Translate (transform.up * targetSpeed * Time.deltaTime, Space.World);
		Debug.Log ((transform.up * targetSpeed * Time.deltaTime));
		Debug.Log ("BangBang Collision_stay");
	}
}
