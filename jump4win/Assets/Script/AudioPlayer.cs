using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

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
		
	public void PlayJumpSound()
	{
		int ranNum = (int)Random.Range (0, 4);
		audioSource.PlayOneShot (jumpSound[ranNum]);
	}
}
