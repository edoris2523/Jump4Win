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
	private GameObject foot;
	private GameObject head;

	void Start()
	{
		m_renderer = GetComponent<Renderer> ();
		m_collider = GetComponent<BoxCollider> ();
	}

	void Update()
	{
		if (gameObject.transform.position.y < -5f)
			RpcDied ();
	}

	[ClientRpc]
	public void RpcGetDamaged(int damage){
		if (!isServer || hp <= 0)
			return;

		hp -= damage;
		if(hp <= 0)
		{
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

		// head, foot deactive
		transform.GetChild (1).gameObject.SetActive (false);
		transform.GetChild (2).gameObject.SetActive (false);

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
