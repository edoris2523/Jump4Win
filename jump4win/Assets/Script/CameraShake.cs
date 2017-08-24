using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour{

	public Transform camTransform;

	public float shake = 1f;

	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	Vector3 originPos;

	void Awake(){
		if(camTransform == null){
			camTransform = GetComponent<Transform> ();
			shake = 1f;
		}
	}

	void OnEnable(){
		originPos = camTransform.localPosition;
	}

	void Update(){
		if(shake > 0){
			camTransform.localPosition = originPos + Random.insideUnitSphere * shakeAmount;
			shake -= Time.deltaTime * decreaseFactor;
		} else{
			shake = 0f;
			camTransform.localPosition = originPos;
		}
	}
}