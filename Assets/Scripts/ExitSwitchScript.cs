using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSwitchScript : MonoBehaviour {

	public bool active = false;
	public Material activeMat, inActiveMat;
	public GameObject[] linkedObjects;

	private ExitScript[] exitScripts;
	private ExitSwitchScript[] exitSwitchScripts;
	private Vector3 originalPosition;
	private Renderer childRenderer;

	// Use this for initialization
	void Start () {
		originalPosition = transform.GetChild(0).position;
		childRenderer = GetComponentInChildren<Renderer> ();
		SetUpButton ();
		int exits = 0;
		int switches = 0;
		foreach (GameObject g in linkedObjects) {
			if (g.GetComponent<ExitScript> () != null)
				exits++;
			else if (g.GetComponent<ExitSwitchScript> () != null)
				switches++;
		}
		exitScripts = new ExitScript[exits];
		exits = 0;
		for (int i = 0; i < linkedObjects.Length; i++) {
			if (linkedObjects [i].GetComponent<ExitScript> () != null)
				exitScripts [i - exits] = linkedObjects [i].GetComponent<ExitScript> ();
			else
				exits++;
		}
		exitSwitchScripts = new ExitSwitchScript[switches];
		switches = 0;
		for (int i = 0; i < linkedObjects.Length; i++) {
			if (linkedObjects [i].GetComponent<ExitSwitchScript> () != null)
				exitSwitchScripts [i - switches] = linkedObjects [i].GetComponent<ExitSwitchScript> ();
			else
				switches++;
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
			foreach (ExitSwitchScript exit in exitSwitchScripts) {
				exit.SetButtonState (active);
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

	public void ToggleButton (){
		active = !active;
		SetUpButton ();
	}

	public void SetButtonState (bool state){
		active = state;
		SetUpButton ();
	}
}
