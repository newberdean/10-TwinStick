using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterEntranceScript : MonoBehaviour {

	public GameObject teleporterExit;

	public void OnTriggerEnter (Collider other){
		if (other.CompareTag ("Player") && ReplaySystemManager.Recording && 
			!ReplaySystemManager.Replaying && !ReplaySystemManager.Rewinding) {
			other.transform.position = teleporterExit.transform.position + Vector3.up*0.49f;
		}
	}
}
