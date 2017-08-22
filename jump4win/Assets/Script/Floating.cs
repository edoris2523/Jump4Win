using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour {

	Transform tr;
	float a = 0f;

	public float spd = 0.05f;
	public float range = 10f;

	// Use this for initialization
	void Start () {
		tr = GetComponent<Transform> ();
		spd = Random.Range (0.1f, 0.3f);
		range = 10f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		a += spd;
		tr.Translate (0, Mathf.Sin(a) * range * Time.deltaTime, 0);
	}
}
