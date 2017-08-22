using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadDetection : MonoBehaviour {

	private smoothPlayerController charCntrlr;
	private HealthPoint hp;

	void Start()
	{
		hp = GetComponentInParent<HealthPoint> ();
	}

	void OnTriggerEnter(Collider col){
		Debug.Log (col.gameObject.tag);
		if(col.gameObject.tag == "Player_Foot" || col.gameObject.tag == "Enemy_Foot"){
			Debug.Log ("I'm " + gameObject.transform.parent.name + "I'm get damaged");
			hp.getDamaged (1);
			charCntrlr = col.gameObject.GetComponentInParent<smoothPlayerController> ();

			if (charCntrlr != null) {
				if (hp.hp < 0) {
					charCntrlr.forcedHighJump ();
				} else{
					charCntrlr.forcedJump ();
				}
			} else{
				Debug.Log ("Can't find attached charactercontroller");
			}
		}
	}
}
