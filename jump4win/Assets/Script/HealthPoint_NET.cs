using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HealthPoint_NET : NetworkBehaviour {

	public int hp = 2;
	public bool isDead;

	Text infoText;

	public void getDamaged(int damage){
		if (!isServer || hp <= 0)
			return;

		hp -= damage;
		if(hp <= 0)
		{
			Debug.Log (gameObject.transform.name + " Dead");
			hp = 0;
			RpcDied ();
			isDead = true;
			gameObject.transform.gameObject.SetActive (false);
		}
	}

	[ClientRpc]
	void RpcDied()
	{
		Debug.Log (gameObject.transform.name + " Dead");
		infoText = GameObject.FindObjectOfType<Text> ();
		isDead = true;
		gameObject.transform.gameObject.SetActive (false);

		if(isLocalPlayer){
			infoText.text = "Game Over!";
		}
		else{
			infoText.text = "You Won!";
		}

		Invoke("BackToLobby", 3f);
	}

	void BackToLobby()
	{
		FindObjectOfType<NetworkLobbyManager> ().ServerReturnToLobby ();
	}
}
