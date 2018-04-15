using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoosterScript : MonoBehaviour {

	public float forceToUse = 100f;

	public void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player") && ReplaySystemManager.Recording && 
			!ReplaySystemManager.Replaying && !ReplaySystemManager.Rewinding) {
			Rigidbody rb = other.GetComponent<Rigidbody> ();
			if (Mathf.Approximately (0f, forceToUse)) {
				rb.velocity = rb.velocity * -1f;
			} else {
				rb.AddForce (transform.up * forceToUse, ForceMode.Impulse);
			}
		}
	}
}