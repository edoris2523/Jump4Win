using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class Portal : MonoBehaviour {
	public Transform destination;

	void OnTriggerEnter(Collider col){
		if(col.gameObject.CompareTag("Player")){
			col.gameObject.transform.position = destination.position;
		}
	}
}
