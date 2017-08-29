using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AudioPlayer_NET : NetworkBehaviour {

	/*
 *  Custom AudioPlayer for Playing SFX randomly
 *  Need Audiosource On Gameobject to be Attached
*/

	AudioSource audioSource;
	public AudioClip[] jumpSound;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}

	[Command]
	public void CmdPlayJumpSound()
	{
		Debug.Log ("PlaySound");
		int ranNum = (int)Random.Range (0, 4);
		audioSource.PlayOneShot (jumpSound[ranNum]);
	}
}
