using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform target;
	public Vector3 offsetPos;
	public float moveSpeed = 5;
	public float turnSpeed = 10;
	public float smoothSpeed = 0.5f;

	Quaternion targetRotation;
	Vector3 targetPos;
	bool smoothRotating = false;

	void Update(){
		MoveWithTarget ();
		LookAtTarget ();

		if(Input.GetKeyDown(KeyCode.G) && !smoothRotating){
			StartCoroutine ("RotateAroundTarget", 45f);
		}

		if(Input.GetKeyDown(KeyCode.H) && !smoothRotating){
			StartCoroutine ("RotateAroundTarget", 45f);
		}

	}

	void MoveWithTarget(){
		targetPos = target.position + offsetPos;
		transform.position = Vector3.Lerp (transform.position, targetPos, moveSpeed * Time.deltaTime);
	}

	void LookAtTarget(){
		targetRotation = Quaternion.LookRotation (target.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
	}

	IEnumerator RotateAroundTarget(float angle){
		Vector3 vel = Vector3.zero;
		Vector3 targetOffsetPos = Quaternion.Euler (0, angle, 0) * offsetPos;
		float dist = Vector3.Distance (offsetPos, targetOffsetPos);
		smoothRotating = true;

		while(dist > 0.02f){
			offsetPos = Vector3.SmoothDamp(offsetPos, targetOffsetPos, ref vel, smoothSpeed);
			dist = Vector3.Distance (offsetPos, targetOffsetPos);
			yield return null;
		}

		smoothRotating = false;
		offsetPos = targetOffsetPos;
	}

}
