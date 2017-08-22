using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioController : MonoBehaviour {

	public float velocity = 5;
	public float turnSpeed = 10;
	public float height = 0.5f;
	public float heightPadding = 0.05f;
	public LayerMask ground;
	public float maxGroundAngle = 120;
	public bool debug;

	Vector2 input;
	float angle;
	float groundAngle;

	Quaternion targetRotation;
	Transform cam;

	Vector3 forward;
	RaycastHit hitInfo;
	bool grounded;

	void Start(){
		cam = Camera.main.transform;
	}

	void Update(){
		GetInput ();
		CalculateDirection();
		CalculateForward ();
		CalculateGroundAngle ();
		CheckGround ();
		ApplyGravity ();
		DrawDebugLines ();

		if(Mathf.Abs(input.x) < 1 && Mathf.Abs(input.y) < 1) return;

		Rotate();
		Move();
	}

	void GetInput(){
		input.x = Input.GetAxisRaw ("Horizontal");
		input.y = Input.GetAxisRaw ("Vertical");
	}

	void CalculateDirection(){
		angle = Mathf.Atan2 (input.x, input.y);
		angle = Mathf.Rad2Deg * angle;
		angle += cam.eulerAngles.y;
	}

	void Rotate(){
		targetRotation = Quaternion.Euler (0, angle, 0);
		transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
	}

	void Move(){
		if (groundAngle >= maxGroundAngle) return;
		transform.position += forward * velocity * Time.deltaTime;
	}

	void CalculateForward(){
		if(!grounded){
			forward = transform.forward;
			return;
		}

		forward = Vector3.Cross (transform.right, hitInfo.normal);
	}

	void CalculateGroundAngle(){
		if(!grounded){
			groundAngle = 90;
			return;
		}

		groundAngle = Vector3.Angle (hitInfo.normal, transform.forward);
	}

	void CheckGround(){
		if(Physics.Raycast(transform.position, -Vector3.up, out hitInfo, height + heightPadding, ground)){
			if(Vector3.Distance(transform.position, hitInfo.point) < height){
				transform.position = Vector3.Lerp (transform.position, transform.position + Vector3.up * height, 5 * Time.deltaTime);
			}
			grounded = true;
		}
		else{
			grounded = false;
		}
	}

	void ApplyGravity(){
		if(!grounded){
			transform.position += Physics.gravity * Time.deltaTime;
		}
	}

	void DrawDebugLines(){
		if (!debug)
			return;

		Debug.DrawLine (transform.position, transform.position + forward * height * 2, Color.blue);
		Debug.DrawLine (transform.position, transform.position - Vector3.up * height, Color.green);
	}

}
