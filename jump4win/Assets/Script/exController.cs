using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exController : MonoBehaviour {

	private bool onGround;
	private float jumpPressure;
	private float minJump;
	private float maxJumpPressure;

	private Rigidbody rb;
	private Animator anim;
	private bool isJump = false;

	// Use this for initialization
	void Start () {
		onGround = true;
		jumpPressure = 0f;
		minJump = 2f;
		maxJumpPressure = 10f;

		rb = GetComponentInParent<Rigidbody> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(onGround)
		{
			// Hold Jump Button
			if(Input.GetButton("Jump"))
			{
				if(jumpPressure < maxJumpPressure)
				{
					jumpPressure += Time.deltaTime * 10f;	
				}
				else
				{
					jumpPressure = maxJumpPressure;
				}
				anim.SetFloat ("jumpPressure", jumpPressure + minJump);
				anim.speed = 1f + (jumpPressure/10f);
			}
			// Not Holding Jump Button
			else
			{
				// Jump
				if(jumpPressure > 0f)
				{
					jumpPressure = jumpPressure + minJump;
					rb.velocity = new Vector3 (jumpPressure / 10f, jumpPressure, 0f);
					jumpPressure = 0f;
					onGround = false;
					anim.SetFloat ("jumpPressure", 0f);
					anim.SetBool ("onGround", onGround);
					anim.speed = 1f;
	
				}
			}
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.CompareTag("Ground"))
		{
			onGround = true;
			anim.SetBool ("onGround", onGround);
		}
	}
		
}
