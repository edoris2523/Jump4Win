using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

	public Text m_messageText;

	static GameManager instance;

	public GameObject[] m_allPlayers;

	public List<Text> m_playerNameText;
	public List<Text> m_playerScoreText;

	[SerializeField]
	private int survivorNum = 0;

	public int m_maxScore = 3;
	private bool m_gameEnd = false;

	public static int m_selectedMap = 0;

	public static GameManager Instance
	{
		get
		{
			if (instance ==  null)
			{
				instance = GameObject.FindObjectOfType<GameManager>();

				if (instance == null)
				{
					instance = new GameObject().AddComponent<GameManager>();
				}
			}
			return instance;
		}
	}

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	[Client]
	public override void OnStartClient()
	{
		base.OnStartClient();
	}

	[Server]
	void Start()
	{
		if (isServer) {
			StartCoroutine ("GameLoopRoutine");
		} 
	}
		
	IEnumerator GameLoopRoutine()
	{
		Debug.Log ("GameLoopRoutine Begin");
		Prototype.NetworkLobby.LobbyManager lobbyManager = Prototype.NetworkLobby.LobbyManager.s_Singleton;

		if (lobbyManager != null)
		{
			m_allPlayers = GameObject.FindGameObjectsWithTag ("Player");

			while (m_allPlayers.GetLength(0) < lobbyManager._playerNumber)
			{
				Debug.Log ("PlayerNumber " + lobbyManager._playerNumber.ToString());
				m_allPlayers = GameObject.FindGameObjectsWithTag ("Player");
				yield return null;
			}
				
			yield return new WaitForSeconds (0.5f);
			// Map Setting
			//yield return new WaitForSeconds(1.0f);
			yield return StartCoroutine("StartGame");
			yield return StartCoroutine("PlayGame");
			yield return StartCoroutine("EndGame");
			//StartCoroutine("GameLoopRoutine");
		}
		else
		{
			Debug.LogWarning("========= GAMEMANAGER WARNING!  Launch game from Lobby scene only! =========");
		}
	}

	[ClientRpc]
	void RpcStartGame()
	{
		UpdateMessage ("FIGHT");
	}

	IEnumerator StartGame()
	{
		Debug.Log ("StartGame Begin");

		RpcUpdateMessage ("3");
		yield return new WaitForSeconds (1f);
		RpcUpdateMessage ("2");
		yield return new WaitForSeconds (1f);
		RpcUpdateMessage ("1");
		yield return new WaitForSeconds (1f);

		RpcStartGame();
		UpdateScoreboard();
	}

	void deactivatePlayers()
	{
		
	}

	[ClientRpc]
	void RpcPlayGame()
	{
		UpdateMessage("");
	}

	IEnumerator PlayGame()
	{
		Debug.Log ("PlayGame Begin");
		yield return new WaitForSeconds (1f);

		RpcPlayGame();

		while (m_gameEnd == false)
		{
			CheckSurvivor ();
			yield return null;
		}

	}

	[ClientRpc]
	void RpcEndGame()
	{
		FindObjectOfType<NetworkLobbyManager> ().ServerReturnToLobby ();
	}

	IEnumerator EndGame()
	{
		Debug.Log ("EndGame Begin");

		RpcUpdateMessage ("Game End");
		yield return new WaitForSeconds (2f);
		RpcEndGame ();
	}
		
	[ClientRpc]
	void RpcUpdateScoreboard(string[] playerNames, int[] playerScores)
	{
		for (int i = 0; i < m_allPlayers.GetLength(0); i++)
		{
			/*if (playerNames[i] != null)
			{
				m_playerNameText[i].text = playerNames[i];
			}

			if (playerScores[i] != null)
			{
				m_playerScoreText[i].text = playerScores[i].ToString();
			}*/
		}

	}

	void CheckSurvivor()
	{
		survivorNum = 0;

		for(int i = 0; i < m_allPlayers.GetLength(0); ++i)
		{
			HealthPoint_NET hp_net = m_allPlayers [i].GetComponent<HealthPoint_NET> ();
			if (hp_net.isDead == false)
				++survivorNum;
		}

		if(survivorNum == 1)
		{
			m_gameEnd = true;
		}
	}

	[Server]
	public void UpdateScoreboard()
	{
		string[] pNames = new string[m_allPlayers.GetLength(0)];
		int[] pScores = new int[m_allPlayers.GetLength(0)];

		for (int i = 0; i < m_allPlayers.GetLength(0); i++)
		{
			if (m_allPlayers[i] != null)
			{
				// Things to Update
			}
		}
		RpcUpdateScoreboard(pNames, pScores);
	}

	[ClientRpc]
	void RpcUpdateMessage (string msg)
	{
		UpdateMessage(msg);
	}

	public void UpdateMessage(string msg)
	{
		if (m_messageText != null) 
		{
			m_messageText.gameObject.SetActive (true);
			m_messageText.text = msg;
		}
	}
}
	
