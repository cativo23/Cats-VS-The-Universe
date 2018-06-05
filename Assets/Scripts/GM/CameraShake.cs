using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
	public Vector3 pos;
	Camera mainCam;
	public float shakeAmount;
	void Awake(){
		if (mainCam == null)
			mainCam = Camera.main;
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Space)){
			Shake (0.5f, 0.5f);
		}

	}

	public void Shake(float amt, float lenght){
		pos = mainCam.transform.position;
		shakeAmount = amt;
		InvokeRepeating("DoShake", 0, 0.001f);
		Invoke ("StopShake",lenght);
	}

	void DoShake(){
		if (shakeAmount > 0) {
			print ("Shake!");
			Vector3 camPos = mainCam.transform.position;

			float shakeAmtx = Random.value * shakeAmount * 2 - shakeAmount;
			float shakeAmtY = Random.value * shakeAmount * 2 - shakeAmount;

			camPos.x = shakeAmtx;
			camPos.y = shakeAmtY;

			mainCam.transform.position = camPos;
		}
	}
	void StopShake(){ 
		CancelInvoke ("DoShake");
		mainCam.transform.position = pos;
	}
}
