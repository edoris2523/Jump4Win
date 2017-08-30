using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadDetection : MonoBehaviour {

	private smoothPlayerController charCntrlr;
	private smoothPlayerController_NET charCntrlr_Net;
	//private HealthPoint hp;
	private HealthPoint_NET hp_Net;

	void Start()
	{
		//hp = GetComponentInParent<HealthPoint> ();
		hp_Net = GetComponentInParent<HealthPoint_NET> ();
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Player_Foot" || col.gameObject.tag == "Enemy_Foot"){
			Debug.Log ("I'm " + gameObject.transform.parent.name + "I'm get damaged");
			hp_Net.RpcGetDamaged (1);

			if (charCntrlr = col.gameObject.GetComponentInParent<smoothPlayerController> ()) 
			{
				if (hp_Net.hp < 0) 
					charCntrlr.forcedHighJump ();
				else
					charCntrlr.forcedJump ();
			}
			else
			{
				Debug.Log ("Can't find attached charactercontroller");
			}	

			if(charCntrlr_Net = col.gameObject.GetComponentInParent<smoothPlayerController_NET> ())
			{
				if (hp_Net.hp < 0) 
					charCntrlr_Net.forcedHighJump ();
			
				else
					charCntrlr_Net.forcedJump ();
			}

			else
			{
				Debug.Log ("Can't find attached charactercontroller_NET");
			}	
		}

		// Head is collided with Platform or Something else.
		else
		{
			if(gameObject.CompareTag("Player_Head"))
				gameObject.GetComponentInParent<smoothPlayerController_NET> ().reachedApex = true;
		}
	}
}
