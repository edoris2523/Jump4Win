using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchxx : MonoBehaviour {

	float xPos;
	float yPos;
	
	// Update is called once per frame
	void Update () {
		
		float xPos = Input.GetAxis("Mouse X");
		float yPos = Input.GetAxis("Mouse Y");
		if(Input.touchCount > 0)
		{
			xPos = Input.touches [0].deltaPosition.x;
			yPos = Input.touches [0].deltaPosition.x;
		}
	}
}
