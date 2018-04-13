using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSwitchScript : MonoBehaviour {

	public bool active = false;
	public Material activeMat, inActiveMat;
	public GameObject[] linkedObjects;

	private ExitScript[] exitScripts;
	private Vector3 originalPosition;
	private Renderer childRenderer;

	// Use this for initialization
	void Start () {
		originalPosition = transform.GetChild(0).position;
		childRenderer = GetComponentInChildren<Renderer> ();
		SetUpButton ();
		int exits = 0;
		foreach (GameObject g in linkedObjects) {
			if (g.GetComponent<ExitScript> () != null)
				exits++;
		}
		exitScripts = new ExitScript[exits];
		exits = 0;
		for (int i = 0; i < exitScripts.Length; i++) {
			if (linkedObjects [i + exits].GetComponent<ExitScript> () != null)
				exitScripts [i] = linkedObjects [i + exits].GetComponent<ExitScript> ();
			else
				exits++;
		}
	}
	
	public void OnTriggerEnter (Collider other){
		if (other.CompareTag ("Player")) {
			active = !active;
			SetUpButton ();
			foreach (ExitScript exit in exitScripts) {
				if (active)
					exit.Enable ();
				else
					exit.Disable ();
			}
		}
	}

	void SetUpButton(){
		if (active)
			childRenderer.material = activeMat;
		else
			childRenderer.material = inActiveMat;
		transform.GetChild(0).position = active ? originalPosition + Vector3.down*0.12f : originalPosition;
	}
}
