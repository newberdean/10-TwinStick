using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour {

	private GameObject child;

	void Start (){
		child = transform.GetChild (0).gameObject;
	}

	void Update (){
		child.SetActive (Mathf.Approximately(Time.timeScale, 0f));
	}

	public void TogglePause (){
		if (!GameManager.LEVEL_DONE)
			GameManager.TogglePause ();
	}
}
