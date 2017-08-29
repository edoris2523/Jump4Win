using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoint : MonoBehaviour {

	public int hp = 2;
	public bool isDead;

	// Update is called once per frame
	void Update () {
		deadChecking ();
	}

	public void getDamaged(int damage){
		hp -= damage;
	}

	private void deadChecking(){
		if(hp <= 0){
			Debug.Log (gameObject.transform.name + " Dead");
			isDead = true;
			//gameObject.transform.gameObject.SetActive (false);
		}
	}
}

