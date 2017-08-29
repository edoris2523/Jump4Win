using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HealthPoint_NET : NetworkBehaviour {

	public int hp = 2;
	public bool isDead;

	private Renderer m_renderer;
	private BoxCollider m_collider;

	void Start()
	{
		m_renderer = GetComponent<Renderer> ();
		m_collider = GetComponent<BoxCollider> ();
	}

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
		}
	}

	[ClientRpc]
	void RpcDied()
	{
		Debug.Log (gameObject.transform.name + " Dead");
		isDead = true;
		m_collider.enabled = false;
		m_renderer.enabled = false;

		/*
		if(isLocalPlayer){
			infoText.text = "Game Over!";
		}
		else{
			infoText.text = "You Won!";
		}

		Invoke("BackToLobby", 3f);*/
	}

	void BackToLobby()
	{
		FindObjectOfType<NetworkLobbyManager> ().ServerReturnToLobby ();
	}
}
